﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TestClient
{
    // State object for receiving data from remote device.
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
    class AsynchronousClient
    {
        // The port number for the remote device.
        private const int port = 11000;

        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        // The response from the remote device.
        public static String response = String.Empty;

        public static Socket clientSocket;

        // delegate 생성
        private Action StartClientDelegate;
        private Action<Socket> WaitingReceiveDelegate;

        public AsynchronousClient()
        {
            StartClientDelegate = StartClient;
            WaitingReceiveDelegate = WaitingReceive;
        }

        // Window Form ListBox 사용 메서드
        private static void WriteListBoxSafe(String text)
        {
            if (TestClientUI.testClientUI.lb_Result.InvokeRequired)
            {
                TestClientUI.testClientUI.lb_Result.Invoke((MethodInvoker)delegate ()
                {
                    WriteListBoxSafe(text);
                });
            } else
            {
                TestClientUI.testClientUI.lb_Result.Items.Add(text);
                TestClientUI.testClientUI.lb_Result.SetSelected(TestClientUI.testClientUI.lb_Result.Items.Count - 1, true);
            }
        }

        // Callback 메서드
        public void StartClientCallback(IAsyncResult ar)
        {
            var async = ar.AsyncState as AsynchronousClient;
            async.EndStartClient(ar);
        }

        // BeginInvoke 메서드
        public IAsyncResult BeginStartClient(AsyncCallback asyncCallback, object state)
        {
            return StartClientDelegate.BeginInvoke(asyncCallback, state);
        }

        // EndInvoke 메서드
        private void EndStartClient(IAsyncResult asyncResult)
        {
            StartClientDelegate.EndInvoke(asyncResult);
        }

        public (Socket, IPEndPoint) CreateSocket()
        {
            // Establish the remote endpoint for the socket.
            // The name of the remote device is "host.contoso.com".
            IPHostEntry ipHostInfo = Dns.GetHostEntry(IPAddress.Loopback);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.
            clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            return (clientSocket, remoteEP);
        }

        public void ReleaseSocket(Socket client)
        {
            // Release the socket.
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        // 작업 진행 메서드
        private static void StartClient()
        {
            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // The name of the remote device is "host.contoso.com".
                IPHostEntry ipHostInfo = Dns.GetHostEntry(IPAddress.Loopback);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                // Send test data to the remote device.
                Send(client, "This is a test<EOF>");
                sendDone.WaitOne();

                // Receive the response from the remote device.
                Receive(client);
                receiveDone.WaitOne();

                // Write the response to the console.
                Console.WriteLine("Response receivd : {0}", response);
                WriteListBoxSafe("Response receivd : " + response);
                

                // Release the socket.
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connect.
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
                WriteListBoxSafe("Socket connected to " + client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.
                connectDone.Set();
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void WaitingReceiveCallback(IAsyncResult ar)
        {
            var async = ar.AsyncState as AsynchronousClient;
            async.EndWaitingReceive(ar);
        }

        public IAsyncResult BeginWaitingReceive(Socket client, AsyncCallback asyncCallback, object state)
        {
            return WaitingReceiveDelegate.BeginInvoke(client, asyncCallback, state);
        }

        private void EndWaitingReceive(IAsyncResult asyncResult)
        {
            WaitingReceiveDelegate.EndInvoke(asyncResult);
        }

        private void WaitingReceive(Socket client)
        {
            Receive(client);
            receiveDone.Reset();
            receiveDone.WaitOne();

            WriteListBoxSafe(response);
        }

        public static void Receive(Socket client)
        {
            try
            {
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                } else
                {
                    // All the data has arrived; put it in response.
                    if(state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.
                    receiveDone.Set();
                }
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

        public static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);
                WriteListBoxSafe("Sent " + bytesSent + " bytes to server.");

                // Signal that all bytes have been sent.
                sendDone.Set();
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
