using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

namespace TestClient
{
    public partial class TestClientUI : Form
    {
        AsynchronousClient client = new AsynchronousClient();
        public static TestClientUI testClientUI;
        public TestClientUI()
        {
            InitializeComponent();
            testClientUI = this;
        }

        private void TestClient_Load(object sender, EventArgs e)
        {
            // Connection Test
            //client.BeginStartClient(client.StartClientCallback, client);

            (Socket socket, IPEndPoint remoteEP) = client.CreateSocket();
            AsynchronousClient.clientSocket.BeginConnect(remoteEP, new AsyncCallback(AsynchronousClient.ConnectCallback), AsynchronousClient.clientSocket);
            client.BeginWaitingReceive(AsynchronousClient.clientSocket, client.WaitingReceiveCallback, client);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            client.ReleaseSocket(AsynchronousClient.clientSocket);
            this.Close();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            string data = txt_Send.Text + "<EOF>";
            AsynchronousClient.Send(AsynchronousClient.clientSocket, data);
        }

        private void txt_Send_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btn_Send_Click(sender, e);
                    txt_Send.SelectAll();
                    break;
            }
        }
    }
}
