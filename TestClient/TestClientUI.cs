using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace TestClient
{
    public partial class TestClientUI : Form
    {
        public List<string> userList = new List<string>();
        public List<string> groupList = new List<string>();

        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        string message = string.Empty;

        public TestClientUI()
        {
            InitializeComponent();
        }

        private void TestClient_Load(object sender, EventArgs e)
        {
            clientSocket.Connect(IPAddress.Loopback, 11000);
            stream = clientSocket.GetStream();

            message = "Connected to Chat Server";
            DisplayText(message);

            byte[] buffer = Encoding.Unicode.GetBytes("ㅁㄴㅇㄹ" + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            Thread t_handler = new Thread(GetMessage);
            t_handler.IsBackground = true;
            t_handler.Start();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(this.txt_Send.Text + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
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

        private void GetMessage()
        {
            while (true)
            {
                stream = clientSocket.GetStream();
                int BUFFERSIZE = clientSocket.ReceiveBufferSize;
                byte[] buffer = new byte[BUFFERSIZE];
                int bytes = stream.Read(buffer, 0, buffer.Length);

                string message = Encoding.Unicode.GetString(buffer, 0, bytes);

                DisplayText(message);

                // allow sign in message
                if (message.Contains("allowSignin"))
                {
                    string user_ID = message.Substring(0, message.IndexOf("allowSignin"));
                    DisplayText(user_ID);

                    Thread thread = new Thread(new ParameterizedThreadStart(open_Group));
                    thread.Start(user_ID);
                    
                } // receive groupList, client용 groupList가 따로 있으니 groupList를 요청할 때는 client와 server간 동기화 할 때 뿐
                else if (message.Contains("groupList"))
                {
                    // 지금은 여러번 받게 되는데 차후 한번만 받게 바꾸자
                    string group = message.Substring(0, message.IndexOf("groupList"));
                    if (!groupList.Contains(group))
                    {
                        groupList.Add(group);
                        GroupForm groupForm = new GroupForm();
                        groupForm.groupList = groupList;
                        groupForm.Refresh();
                    }
                } // receive userList 동기화
                else if (message.Contains("userID"))
                {
                    string msg = message.Substring(0, message.LastIndexOf("&"));
                    string[] users = msg.Split('&');
                    foreach(string user in users)
                    {
                        if (!userList.Contains(user))
                        {
                            // userList에 추가
                            userList.Add(user);
                            GroupForm groupForm = new GroupForm();
                            groupForm.userList = userList;
                            groupForm.Refresh();
                        }
                    }
                } // receive complete create group
                else if (message.Contains("completeCreateGroup"))
                {
                    // 임시
                    string user_ID = message.Substring(0, message.IndexOf("completeCreateGroup"));
                    string msg = user_ID + "requestGroupList";
                    buffer = Encoding.Unicode.GetBytes(msg + "$");
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                } // default
                else
                {
                    ChatGroupForm chatGroupForm = new ChatGroupForm();
                    chatGroupForm.DisplayText(message);
                }

            }
        }

        private void DisplayText(string text)
        {
            if (lb_Result.InvokeRequired)
            {
                lb_Result.BeginInvoke(new MethodInvoker(delegate
                {
                    lb_Result.Items.Add(text + Environment.NewLine);
                }));
            }
            else
                lb_Result.Items.Add(text + Environment.NewLine);
        }

        /*
        private void btn_Register_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.stream = stream;
            registerForm.Show();
        }*/

        
        private void btn_SignIn_Click(object sender, EventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.stream = stream;
            signInForm.Show();
        }

        private void open_Group(object user_ID)
        {
            GroupForm groupForm = new GroupForm();
            groupForm.stream = stream;
            groupForm.user_ID = Convert.ToString(user_ID);
            groupForm.userList = userList;
            groupForm.groupList = groupList;
            groupForm.ShowDialog();
        }
    }
}

// 출처: https://it-jerryfamily.tistory.com/80 [IT 이야기]
// 출처: https://yeolco.tistory.com/53 [열코의 프로그래밍 일기]