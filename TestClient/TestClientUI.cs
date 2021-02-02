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
using System.Configuration;

namespace TestClient
{
    public partial class TestClientUI : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        string message = string.Empty;

        string user_ID = null;
        List<string> userList = new List<string>();
        List<string> groupList = new List<string>();

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
            t_handler.Start();
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
                        // 지금은 여러번 받게 되는데 차후 한번만 받게 바꾸자
                        // 한번만 받게 변경 완료

                        try
                        {
                            string msg = message.Substring(0, message.LastIndexOf("&responseGroupList"));
                            string[] groups = msg.Split('&');

                            foreach (string g in groups)
                            {
                                if (!groupList.Contains(g))
                                {
                                    // groupList 추가
                                    groupList.Add(g);
                                }
                            }
                            // 화면 갱신
                            GroupRefresh();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("만들어진 채팅방이 없습니다.\n새로운 채팅방을 만들어보세요.", "알림");
                        }                        
                    } // receive userList 동기화
                    else if (message.Contains("responseUserList"))
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
                    } // receive complete create group
                    else if (message.Contains("completeCreateGroup"))
                    {
                        string msg = message.Substring(0, message.LastIndexOf("completeCreateGroup"));
                        user_ID = msg.Substring(0, msg.LastIndexOf("&"));

                        string sendMsg = user_ID + "&requestGroupList";
                        buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
                        stream.Write(buffer, 0, buffer.Length);
                        stream.Flush();
                    } // default
                    else if (message.Contains("&groupChat"))
                    {
                        string msg = message.Substring(0, message.LastIndexOf("&groupChat"));

                        user_ID = msg.Substring(msg.LastIndexOf("&") + 1);
                        msg = msg.Substring(0, msg.LastIndexOf("&"));

                        string group = msg.Substring(msg.LastIndexOf("&") + 1);
                        msg = msg.Substring(0, msg.LastIndexOf("&"));

                        string chat = msg;

                        if (groupList.Contains(group))
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
                    }
                }
                catch (Exception e)
                {
                    DisplayText(e.ToString());
                    Console.WriteLine(e.ToString());
                    break;
                }
            }
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
        public void Open_ChatGroupForm(ChatGroupForm chatGroupForm)
        {
            chatGroupForms.Add(chatGroupForm);
        }

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
            user_ID = txt_UserID.Text;
            string user_PW = txt_UserPW.Text;

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
            txt_UserID.Location = new Point(430, 139);
            txt_UserPW.Location = new Point(430, 206);
            btn_SignInSubmit.Location = new Point(430, 266);
            btn_OpenRegister.Location = new Point(430, 339);

            // Register Form
            btn_RegisterSubmit.Location = new Point(753, 266);
            btn_RegisterClose.Location = new Point(753, 339);

            // Group Form
            lb_GroupList.Location = new Point(987, 12);
            btn_OpenCreateGroup.Location = new Point(1202, 478);
            btn_SignOut.Location = new Point(1252, 519);
            btn_PullUser.Location = new Point(987, 478);
            btn_PullGroup.Location = new Point(987, 519);

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

                    txt_UserID.Location = new Point(87, 256);
                    txt_UserPW.Location = new Point(87, 295);
                    btn_SignInSubmit.Location = new Point(87, 354);
                    btn_OpenRegister.Location = new Point(87, 404);
                }));
            }
            else
            {
                SettingControlLocationReset();

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

                    txt_UserID.Location = new Point(87, 256);
                    txt_UserPW.Location = new Point(87, 295);
                    btn_RegisterSubmit.Location = new Point(87, 354);
                    btn_RegisterClose.Location = new Point(87, 404);
                }));
            }
            else
            {
                SettingControlLocationReset();

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
                    SettingControlLocationReset();

                    lb_GroupList.Location = new Point(0, 0);
                    btn_OpenCreateGroup.Location = new Point(202, 476);
                    btn_SignOut.Location = new Point(252, 515);

                    btn_PullUser.Location = new Point(12, 474);
                    btn_PullGroup.Location = new Point(12, 515);
                }));
            }
            else
            {
                SettingControlLocationReset();

                lb_GroupList.Location = new Point(0, 0);
                btn_OpenCreateGroup.Location = new Point(202, 476);
                btn_SignOut.Location = new Point(252, 515);

                btn_PullUser.Location = new Point(12, 474);
                btn_PullGroup.Location = new Point(12, 515);
            }
        }

        private void SettingControlLocationCreateGroup()
        {
            if (txt_UserID.InvokeRequired)
            {
                txt_UserID.BeginInvoke(new MethodInvoker(delegate
                {
                    SettingControlLocationReset();

                    clb_GroupingUser.Location = new Point(0, 0);
                    btn_Create.Location = new Point(12, 497);
                    btn_CreateClose.Location = new Point(210, 497);
                }));
            }
            else
            {
                SettingControlLocationReset();

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
            user_ID = txt_UserID.Text;
            string user_PW = txt_UserPW.Text;

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

        private void btn_PullGroup_Click(object sender, EventArgs e)
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
            foreach (var item in groupList)
            {
                if (!lb_GroupList.Items.Contains(item))
                {
                    lb_GroupList.Items.Add(item);
                }
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
            ListBox lb = sender as ListBox;
            // 해당 윈도우가 이미 열려있을 때 처리
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.Name.Equals("chatGroupForm" + lb.SelectedIndex))
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
            chatGroupForm.stream = stream;
            chatGroupForm.group = groupList[lb.SelectedIndex];
            chatGroupForm.user_ID = user_ID;
            chatGroupForm.Tag = lb.SelectedIndex;
            chatGroupForm.Text = groupList[lb.SelectedIndex];
            chatGroupForm.Name = "chatGroupForm" + lb.SelectedIndex;

            // ChatGroupForm이 열렸을 때 TestClientUI에 Form 정보 저장
            Open_ChatGroupForm(chatGroupForm);

            chatGroupForm.Show();
        }
    }
}

// 출처: https://it-jerryfamily.tistory.com/80 [IT 이야기]
// 출처: https://yeolco.tistory.com/53 [열코의 프로그래밍 일기]