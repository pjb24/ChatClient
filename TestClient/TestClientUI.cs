﻿using System;
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

        GroupForm groupForm = new GroupForm();
        List<ChatGroupForm> chatGroupForms = new List<ChatGroupForm>();

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
                    string msg = message.Substring(0, message.LastIndexOf("allowSignin"));
                    string user_ID = msg.Substring(0, message.LastIndexOf("&"));
                    DisplayText(user_ID);

                    Thread thread = new Thread(new ParameterizedThreadStart(open_Group));
                    thread.Start(user_ID);
                    
                } // receive groupList, client용 groupList가 따로 있으니 groupList를 요청할 때는 client와 server간 동기화 할 때 뿐
                else if (message.Contains("responseGroupList"))
                {
                    // 지금은 여러번 받게 되는데 차후 한번만 받게 바꾸자
                    string msg = message.Substring(0, message.LastIndexOf("&responseGroupList"));
                    string[] groups = msg.Split('&');
                    
                    foreach(string g in groups)
                    {
                        if (!groupList.Contains(g))
                        {
                            groupList.Add(g);

                            groupForm.groupList = groupList;
                        }
                    }
                    GroupRefresh();
                } // receive userList 동기화
                else if (message.Contains("responseUserList"))
                {
                    string msg = message.Substring(0, message.LastIndexOf("&responseUserList"));
                    string[] users = msg.Split('&');
                    foreach(string user in users)
                    {
                        if (!userList.Contains(user))
                        {
                            // userList에 추가
                            userList.Add(user);

                            groupForm.userList = userList;
                            GroupRefresh();
                        }
                    }
                } // receive complete create group
                else if (message.Contains("completeCreateGroup"))
                {
                    // 임시
                    string msg = message.Substring(0, message.LastIndexOf("completeCreateGroup"));
                    string user_ID = message.Substring(0, message.LastIndexOf("&"));

                    string sendMsg = user_ID + "&requestGroupList";
                    buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                } // default
                else if (message.Contains("&groupChat"))
                {
                    string msg = message.Substring(0, message.LastIndexOf("&groupChat"));

                    string user_ID = msg.Substring(msg.LastIndexOf("&") + 1);
                    msg = msg.Substring(0, msg.LastIndexOf("&"));

                    string group = msg.Substring(msg.LastIndexOf("&") + 1);
                    msg = msg.Substring(0, msg.LastIndexOf("&"));

                    string chat = msg;

                    foreach(string temp in groupList)
                    {
                        foreach(ChatGroupForm tmp in chatGroupForms)
                        {
                            if(tmp.group.Equals(temp))
                            {
                                tmp.DisplayText(user_ID + " : " + chat);
                            }
                        }
                    }
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

        private void GroupRefresh()
        {
            if (groupForm.InvokeRequired)
            {
                groupForm.BeginInvoke(new MethodInvoker(delegate
                {
                    GroupRefresh();
                }));
            }
            else
                groupForm.designGroup();
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
            groupForm.stream = stream;
            groupForm.user_ID = Convert.ToString(user_ID);
            groupForm.userList = userList;
            groupForm.groupList = groupList;
            groupForm.testClientUI = this;
            groupForm.ShowDialog();
        }

        public void open_GroupChatForm(ChatGroupForm chatGroupForm)
        {
            chatGroupForms.Add(chatGroupForm);
        }
    }
}

// 출처: https://it-jerryfamily.tistory.com/80 [IT 이야기]
// 출처: https://yeolco.tistory.com/53 [열코의 프로그래밍 일기]