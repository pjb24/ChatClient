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
using System.Configuration;

using log4net;
using System.Security.Cryptography;

namespace TestClient
{
    public partial class TestClientUI : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TestClientUI));

        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        string message = string.Empty;

        string user_ID = null;
        List<string> userList = new List<string>();
        Dictionary<string, List<string>> groupList = new Dictionary<string, List<string>>();

        List<ChatGroupForm> chatGroupForms = new List<ChatGroupForm>();

        public TestClientUI()
        {
            InitializeComponent();
        }

        // Form Load할 때 call
        private void TestClient_Load(object sender, EventArgs e)
        {
            SettingControlLocationSignIn();

            string IP = ConfigurationManager.AppSettings["IP"];
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);

            try
            {
                // (serverIP, port) 연결 시도, 현재는 Loopback 사용중, Exception 처리 필요
                clientSocket.Connect(IP, port);
                // NetworkStream 정보 저장, NetworkStream?
                stream = clientSocket.GetStream();

                message = "Connected to Chat Server";
                DisplayText(message);

                // GetMessage Thread 생성
                Thread t_handler = new Thread(GetMessage);
                t_handler.IsBackground = true;
                // GetMessage Thread 시작
                log.Info("client start");
                t_handler.Start();
            }
            catch (SocketException se)
            {
                Console.WriteLine(string.Format("clientSocket.Connect - SocketException : {0}", se.StackTrace));
            }
        }

        // server message 대기
        private void GetMessage()
        {
            while (true)
            {
                try
                {
                    stream = clientSocket.GetStream();
                    int BUFFERSIZE = clientSocket.ReceiveBufferSize;
                    byte[] buffer = new byte[BUFFERSIZE];
                    int bytes = stream.Read(buffer, 0, buffer.Length);

                    message = Encoding.Unicode.GetString(buffer, 0, bytes);

                    // DisplayText(message);

                    // received message 처리
                    // allow sign in message
                    if (message.Contains("allowSignin"))
                    {
                        // 정보 추출
                        string msg = message.Substring(0, message.LastIndexOf("allowSignin"));
                        user_ID = msg.Substring(0, msg.LastIndexOf("&"));
                        DisplayText(user_ID);

                        SettingControlLocationGroup();
                    } // receive groupList, client용 groupList가 따로 있으니 groupList를 요청할 때는 client와 server간 동기화 할 때 뿐
                    else if (message.Contains("responseGroupList"))
                    {
                        try
                        {
                            string msg = message.Substring(0, message.LastIndexOf("&responseGroupList"));
                            string[] groups = msg.Split('&');



                            foreach (string group in groups)
                            {
                                string groupName = group.Substring(group.LastIndexOf("^")+1);

                                if (!groupList.ContainsKey(groupName))
                                {
                                    string temp = group.Substring(0, group.LastIndexOf("^"));
                                    List<string> tmp = temp.Split('^').ToList();
                                    // groupList 추가
                                    groupList.Add(groupName, tmp);
                                }
                            }
                            // 화면 갱신
                            GroupRefresh();
                        }
                        catch (ArgumentOutOfRangeException aore)
                        {
                            Console.WriteLine(aore.StackTrace);
                            Console.WriteLine("만들어진 채팅방이 없습니다. 새로운 채팅방을 만들어보세요.");
                            MessageBox.Show("만들어진 채팅방이 없습니다.\n새로운 채팅방을 만들어보세요.", "알림");
                        }                        
                    } // receive userList 동기화
                    else if (message.Contains("responseUserList"))
                    {
                        try
                        {
                            string msg = message.Substring(0, message.LastIndexOf("&responseUserList"));
                            string[] users = msg.Split('&');

                            foreach (string user in users)
                            {
                                if (!userList.Contains(user))
                                {
                                    // userList에 추가
                                    userList.Add(user);
                                }
                            }
                            GroupRefresh();
                        }
                        catch (ArgumentOutOfRangeException aore)
                        {
                            Console.WriteLine(aore.StackTrace);
                            Console.WriteLine("서버에 등록된 회원이 본인 밖에 없음");
                        }
                    } // receive complete create group
                    else if (message.Contains("completeCreateGroup"))
                    {
                        string msg = message.Substring(0, message.LastIndexOf("completeCreateGroup"));
                        user_ID = msg.Substring(0, msg.LastIndexOf("&"));

                        btn_PullGroup_Click();
                    } // default
                    else if (message.Contains("&groupChat"))
                    {
                        string msg = message.Substring(0, message.LastIndexOf("&groupChat"));

                        user_ID = msg.Substring(msg.LastIndexOf("&") + 1);
                        msg = msg.Substring(0, msg.LastIndexOf("&"));

                        string group = msg.Substring(msg.LastIndexOf("&") + 1);
                        msg = msg.Substring(0, msg.LastIndexOf("&"));

                        string chat = msg;

                        if (groupList.ContainsKey(group))
                        {
                            // 열려있는 ChatGroupForm 중에서 group이 일치하는 window에 출력
                            foreach (ChatGroupForm temp in chatGroupForms)
                            {
                                if (temp.group.Equals(group))
                                {
                                    temp.DisplayText(user_ID + " : " + chat);
                                }
                            }
                        }
                    } else if (message.Contains(" is aleady registered"))
                    {
                        MessageBox.Show("이미 등록된 회원입니다.", "알림");
                    } else if (message.Contains(" is not registered"))
                    {
                        MessageBox.Show("등록된 회원이 아닙니다.", "알림");
                    } else if (message.Contains("incorrect PW"))
                    {
                        MessageBox.Show("올바르지 못한 비밀번호입니다.", "알림");
                    } else if (message.Contains(" is register"))
                    {
                        MessageBox.Show("회원가입 되었습니다.", "알림");
                    } else if (message.Contains("&existGroup"))
                    {
                        string msg = message.Substring(0, message.LastIndexOf("&existGroup"));
                        MessageBox.Show(msg + "은 이미 존재하는 채팅방입니다.", "알림");
                    } else if (message.Contains("is already online"))
                    {
                        string msg = message.Substring(0, message.LastIndexOf("is already online"));
                        MessageBox.Show(msg + "는 이미 접속 중입니다.", "알림");
                    }
                    // 채팅방 나감
                    else if (message.Contains("&LeaveGroup"))
                    {
                        string msg = message.Substring(0, message.LastIndexOf("&LeaveGroup"));
                        string receivedID = msg.Substring(msg.LastIndexOf("&") + 1);
                        string group = msg.Substring(0, msg.LastIndexOf("&"));

                        if (user_ID.Equals(receivedID))
                        {
                            groupList.Remove(group);
                            SettingControlLocationGroup();
                        }
                        else
                        {
                            groupList[group].Remove(receivedID);
                            foreach (var chatGroupForm in chatGroupForms)
                            {
                                if (chatGroupForm.group.Equals(group))
                                {
                                    chatGroupForm.DisplayText(receivedID + "님이 채팅방에서 나가셨습니다.");
                                    chatGroupForm.groupUserList.Remove(receivedID);
                                    chatGroupForm.RedrawUserList(); 
                                }
                            }
                            GroupRefresh();
                        }
                    }
                    // 채팅방 초대
                    else if (message.Contains("&Invitation"))
                    {
                        string msg = message.Substring(0, message.LastIndexOf("&Invitation"));

                        // string user_ID = msg.Substring(msg.LastIndexOf("&") + 1);
                        msg = msg.Substring(0, msg.LastIndexOf("&"));

                        string group = msg.Substring(msg.LastIndexOf("&") + 1);
                        msg = msg.Substring(0, msg.LastIndexOf("&"));

                        List<string> InvitedUsers = msg.Split('&').ToList<string>();

                        foreach (var user in InvitedUsers)
                        {
                            if (!user_ID.Equals(user))
                            {
                                groupList[group].Add(user);
                                foreach (var chatGroupForm in chatGroupForms)
                                {
                                    if (chatGroupForm.group.Equals(group))
                                    {
                                        chatGroupForm.DisplayText(user + "님이 채팅방에 초대되셨습니다.");
                                        chatGroupForm.RedrawUserList();
                                    }
                                }
                            }
                            else
                            {
                                btn_PullGroup_Click();
                            }
                        }
                        GroupRefresh();
                    }
                }
                /*
                catch (SocketException se)
                {
                    Console.WriteLine(string.Format("GetMessage - SocketException : {0}", se.StackTrace));

                    if (clientSocket != null)
                    {
                        OnDisconnected(clientSocket);

                        clientSocket.Close();
                        stream.Close();
                        break;
                    }
                }*/
                catch (System.IO.IOException ioe)
                {
                    Console.WriteLine(string.Format("GetMessage - IOException : {0}", ioe.StackTrace));

                    /*
                    if (clientSocket != null)
                    {
                        OnDisconnected(clientSocket);

                        clientSocket.Close();
                        stream.Close();
                        break;
                    }
                    */
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine(string.Format("GetMessage - InvalidOperationException : {0}", ioe.StackTrace));
                    if (clientSocket != null)
                    {
                        clientSocket.Close();
                        stream.Close();
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    break;
                }
            }
        }

        private void OnDisconnected(TcpClient client)
        {
            MessageBox.Show("서버와의 연결이 끊겼습니다.", "알림");
        }

        // 크로스스레드 문제
        private void DisplayText(string text)
        {
            if (lb_Result.InvokeRequired)
            {
                lb_Result.BeginInvoke(new MethodInvoker(delegate
                {
                    lb_Result.Items.Add(text + Environment.NewLine);
                    lb_Result.SelectedIndex = lb_Result.Items.Count - 1;
                }));
            }
            else
            {
                lb_Result.Items.Add(text + Environment.NewLine);
                lb_Result.SelectedIndex = lb_Result.Items.Count - 1;
            }
        }

        private void GroupRefresh()
        {
            // 크로스스레드가 발생할 때
            if (lb_GroupList.InvokeRequired)
            {
                // BeginInvoke - 비동기식 대리자 실행
                // 익명함수? 익명대리자?
                lb_GroupList.BeginInvoke(new MethodInvoker(delegate
                {
                    DesignGroup();
                }));
            }
            else
                DesignGroup();
        }

        // ChatGroupForm이 열렸을 때 Form 정보 저장
        /*
        public void Open_ChatGroupForm(ChatGroupForm chatGroupForm)
        {
            chatGroupForms.Add(chatGroupForm);
        }*/

        private void txt_UserID_Enter(object sender, EventArgs e)
        {
            if (txt_UserID.Text == "ID")
            {
                txt_UserID.Text = "";

                txt_UserID.ForeColor = Color.Black;
            }
        }

        private void txt_UserID_Leave(object sender, EventArgs e)
        {
            if (txt_UserID.Text == "")
            {
                txt_UserID.Text = "ID";

                txt_UserID.ForeColor = Color.Silver;
            }
        }

        private void txt_UserPW_Enter(object sender, EventArgs e)
        {
            if (txt_UserPW.Text == "PW")
            {
                txt_UserPW.Text = "";

                txt_UserPW.ForeColor = Color.Black;

                txt_UserPW.PasswordChar = '*';
            }
        }

        private void txt_UserPW_Leave(object sender, EventArgs e)
        {
            if (txt_UserPW.Text == "")
            {
                txt_UserPW.Text = "PW";

                txt_UserPW.ForeColor = Color.Silver;

                txt_UserPW.PasswordChar = '\0';
            }
        }

        private void btn_SignInSubmit_Click(object sender, EventArgs e)
        {
            SHA256Managed sHA256Managed = new SHA256Managed();
            var temp = sHA256Managed.ComputeHash(Encoding.Unicode.GetBytes(txt_UserPW.Text));

            user_ID = txt_UserID.Text;
            string user_PW = temp.ToString();

            string sendMsg = user_ID + "&" + user_PW + "signin";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            txt_UserID.Clear();
            txt_UserPW.Clear();
        }

        private void SettingControlLocationReset()
        {
            // SignIn Form
            lbl_SignIn.Location = new Point(455, 63);
            txt_UserID.Location = new Point(430, 139);
            txt_UserPW.Location = new Point(430, 206);
            btn_SignInSubmit.Location = new Point(430, 266);
            btn_OpenRegister.Location = new Point(430, 339);

            // Register Form
            lbl_Register.Location = new Point(761, 63);
            btn_RegisterSubmit.Location = new Point(753, 266);
            btn_RegisterClose.Location = new Point(753, 339);

            // Group Form
            lb_UserList.Location = new Point(987, 12);
            lb_GroupList.Location = new Point(987, 204);
            btn_OpenCreateGroup.Location = new Point(1252, 479);
            btn_SignOut.Location = new Point(1252, 519);
            // btn_PullUser.Location = new Point(987, 478);
            // btn_PullGroup.Location = new Point(987, 519);

            // CreateGroup Form
            clb_GroupingUser.Location = new Point(1479, 12);
            btn_Create.Location = new Point(1479, 518);
            btn_CreateClose.Location = new Point(1704, 519);

            // Log
            lb_Result.Location = new Point(430, 631);
        }

        private void SettingControlLocationSignIn()
        {
            if (txt_UserID.InvokeRequired)
            {
                txt_UserID.BeginInvoke(new MethodInvoker(delegate
                {
                    SettingControlLocationReset();

                    this.Text = "로그인";
                    lbl_SignIn.Location = new Point(112, 112);
                    txt_UserID.Location = new Point(87, 256);
                    txt_UserPW.Location = new Point(87, 295);
                    btn_SignInSubmit.Location = new Point(87, 354);
                    btn_OpenRegister.Location = new Point(87, 404);
                }));
            }
            else
            {
                SettingControlLocationReset();

                this.Text = "로그인";
                lbl_SignIn.Location = new Point(112, 112);
                txt_UserID.Location = new Point(87, 256);
                txt_UserPW.Location = new Point(87, 295);
                btn_SignInSubmit.Location = new Point(87, 354);
                btn_OpenRegister.Location = new Point(87, 404);
            }
            
        }

        private void SettingControlLocationRegister()
        {
            if (txt_UserID.InvokeRequired)
            {
                txt_UserID.BeginInvoke(new MethodInvoker(delegate
                {
                    SettingControlLocationReset();

                    this.Text = "회원가입";
                    lbl_Register.Location = new Point(96, 112);
                    txt_UserID.Location = new Point(87, 256);
                    txt_UserPW.Location = new Point(87, 295);
                    btn_RegisterSubmit.Location = new Point(87, 354);
                    btn_RegisterClose.Location = new Point(87, 404);
                }));
            }
            else
            {
                SettingControlLocationReset();

                this.Text = "회원가입";
                lbl_Register.Location = new Point(96, 112);
                txt_UserID.Location = new Point(87, 256);
                txt_UserPW.Location = new Point(87, 295);
                btn_RegisterSubmit.Location = new Point(87, 354);
                btn_RegisterClose.Location = new Point(87, 404);
            }
        }

        private void SettingControlLocationGroup()
        {
            if (txt_UserID.InvokeRequired)
            {
                txt_UserID.BeginInvoke(new MethodInvoker(delegate
                {
                    btn_PullUser_Click();
                    btn_PullGroup_Click();

                    SettingControlLocationReset();

                    this.Text = "로비";
                    lb_UserList.Location = new Point(0, 0);
                    lb_GroupList.Location = new Point(0, 192);
                    btn_OpenCreateGroup.Location = new Point(12, 515);
                    btn_SignOut.Location = new Point(252, 515);

                    // btn_PullUser.Location = new Point(12, 474);
                    // btn_PullGroup.Location = new Point(12, 515);
                }));
            }
            else
            {
                btn_PullUser_Click();
                btn_PullGroup_Click();

                SettingControlLocationReset();

                this.Text = "로비";
                lb_UserList.Location = new Point(0, 0);
                lb_GroupList.Location = new Point(0, 192);
                btn_OpenCreateGroup.Location = new Point(12, 515);
                btn_SignOut.Location = new Point(252, 515);

                // btn_PullUser.Location = new Point(12, 474);
                // btn_PullGroup.Location = new Point(12, 515);
            }
        }

        private void SettingControlLocationCreateGroup()
        {
            if (txt_UserID.InvokeRequired)
            {
                txt_UserID.BeginInvoke(new MethodInvoker(delegate
                {
                    SettingControlLocationReset();

                    this.Text = "채팅방 생성";
                    clb_GroupingUser.Location = new Point(0, 0);
                    btn_Create.Location = new Point(12, 497);
                    btn_CreateClose.Location = new Point(210, 497);
                }));
            }
            else
            {
                SettingControlLocationReset();

                this.Text = "채팅방 생성";
                clb_GroupingUser.Location = new Point(0, 0);
                btn_Create.Location = new Point(12, 497);
                btn_CreateClose.Location = new Point(210, 497);
            }
        }

        private void btn_OpenRegister_Click(object sender, EventArgs e)
        {
            SettingControlLocationRegister();
        }

        private void btn_RegisterClose_Click(object sender, EventArgs e)
        {
            SettingControlLocationSignIn();
        }

        private void btn_OpenCreateGroup_Click(object sender, EventArgs e)
        {
            SettingControlLocationCreateGroup();
            foreach(string user in userList)
            {
                if (!clb_GroupingUser.Items.Contains(user))
                {
                    clb_GroupingUser.Items.Add(user);
                }
            }
        }

        private void btn_CreateClose_Click(object sender, EventArgs e)
        {
            SettingControlLocationGroup();
        }

        private void btn_RegisterSubmit_Click(object sender, EventArgs e)
        {
            SHA256Managed sHA256Managed = new SHA256Managed();
            var temp = sHA256Managed.ComputeHash(Encoding.Unicode.GetBytes(txt_UserPW.Text));

            user_ID = txt_UserID.Text;
            string user_PW = temp.ToString();

            string sendMsg = user_ID + "&" + user_PW + "register";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");

            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            txt_UserID.Clear();
            txt_UserPW.Clear();
        }

        private void btn_PullUser_Click(object sender, EventArgs e)
        {
            string sendMsg = user_ID + "&requestUserList";
            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void btn_PullUser_Click()
        {
            string sendMsg = user_ID + "&requestUserList";
            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void btn_PullGroup_Click(object sender, EventArgs e)
        {
            string sendMsg = user_ID + "&requestGroupList";
            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void btn_PullGroup_Click()
        {
            string sendMsg = user_ID + "&requestGroupList";
            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void btn_SignOut_Click(object sender, EventArgs e)
        {
            if (chatGroupForms.Count != 0)
            {
                foreach (ChatGroupForm room in chatGroupForms)
                {
                    room.Close();
                }
            }

            string sendMsg = user_ID + "&SignOut";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            user_ID = null;
            userList.Clear();
            groupList.Clear();
            chatGroupForms.Clear();
            lb_GroupList.Items.Clear();
            SettingControlLocationSignIn();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            string sendMsg = null;
            foreach (object checkeditem in clb_GroupingUser.CheckedItems)
            {
                sendMsg = sendMsg + (string)checkeditem + "&";
            }
            Console.WriteLine(sendMsg);

            // send group info to server
            sendMsg = sendMsg + user_ID + "&createGroup";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            for (int i = 0; i < clb_GroupingUser.Items.Count; i++)
            {
                clb_GroupingUser.SetItemChecked(i, false);
            }

            SettingControlLocationGroup();
        }

        private void DesignGroup()
        {
            // change design

            // lb_UserList
            foreach (var item in userList)
            {
                if (!lb_UserList.Items.Contains(item))
                {
                    lb_UserList.Items.Add(item);
                }
            }

            // lb_GroupList

            lb_GroupList.Items.Clear();
            foreach (var item in groupList)
            {
                //if (!lb_GroupList.Items.Contains(item.Key))
                //{
                    string tmp = null;
                    lb_GroupList.Items.Add(item.Key);
                    foreach (var temp in item.Value)
                    {
                        tmp = tmp + temp + ", ";  
                    }
                    tmp = tmp.Substring(0, tmp.LastIndexOf(", "));
                    lb_GroupList.Items.Add("채팅방 인원 : " + tmp);
                    lb_GroupList.Items.Add("");
                //}
            }
            // window를 비활성화하여 WM_PAINT call
            // true 배경을 지우고 다시 그린다
            // false 현 배경 위에 다시 그린다
            Invalidate(false);
        }

        private void lb_GroupList_DoubleClick(object sender, EventArgs e)
        {
            if (lb_GroupList.SelectedItems.Count == 1)
            {
                Open_ChatGroup(sender);
            }
        }

        private void Open_ChatGroup(object sender)
        {
            try
            {
                ListBox lb = sender as ListBox;

                int index = lb.SelectedIndex - (lb.SelectedIndex % 3);
                string item = lb.Items[index].ToString();

                // 해당 윈도우가 이미 열려있을 때 처리
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm.Name.Equals("chatGroupForm" + index))
                    {
                        if (openForm.WindowState == FormWindowState.Minimized)
                        {
                            openForm.WindowState = FormWindowState.Normal;
                            openForm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
                        }
                        openForm.Activate();
                        return;
                    }
                }
                ChatGroupForm chatGroupForm = new ChatGroupForm();
                chatGroupForm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
                chatGroupForm.stream = stream;
                chatGroupForm.group = item;
                chatGroupForm.user_ID = user_ID;
                chatGroupForm.Tag = index;
                chatGroupForm.Text = item;
                chatGroupForm.Name = "chatGroupForm" + index;
                chatGroupForm.groupUserList = groupList[item];
                chatGroupForm.userList = userList;

                // ChatGroupForm이 열렸을 때 TestClientUI에 Form 정보 저장
                // Open_ChatGroupForm(chatGroupForm);

                chatGroupForms.Add(chatGroupForm);

                chatGroupForm.Show();
            }
            catch (KeyNotFoundException)
            {

            }
        }

        private void clb_GroupingUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

// 출처: https://it-jerryfamily.tistory.com/80 [IT 이야기]
// 출처: https://yeolco.tistory.com/53 [열코의 프로그래밍 일기]