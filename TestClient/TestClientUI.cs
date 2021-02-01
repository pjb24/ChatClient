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

        public TestClientUI()
        {
            InitializeComponent();
        }

        // Form Load할 때 call
        private void TestClient_Load(object sender, EventArgs e)
        {
            string IP = ConfigurationManager.AppSettings["IP"];
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);

            // (serverIP, port) 연결 시도, 현재는 Loopback 사용중, Exception 처리 필요
            Initializer.clientSocket.Connect(IP, port);
            clientSocket = Initializer.clientSocket;
            // NetworkStream 정보 저장, NetworkStream?
            Initializer.stream = clientSocket.GetStream();
            stream = Initializer.stream;

            message = "Connected to Chat Server";
            DisplayText(message);

            // GetMessage Thread 생성
            Thread t_handler = new Thread(GetMessage);
            t_handler.IsBackground = true;
            // GetMessage Thread 시작
            t_handler.Start();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
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

                    DisplayText(message);

                    // received message 처리
                    // allow sign in message
                    if (message.Contains("allowSignin"))
                    {
                        // 정보 추출
                        string msg = message.Substring(0, message.LastIndexOf("allowSignin"));
                        string user_ID = msg.Substring(0, msg.LastIndexOf("&"));
                        DisplayText(user_ID);

                        // sign in 후, GroupForm Thread 생성, 매개변수를 전달하기 위해 ParameterizedThreadStart 사용
                        Thread thread = new Thread(new ParameterizedThreadStart(open_Group));
                        thread.Start(user_ID);

                    } // receive groupList, client용 groupList가 따로 있으니 groupList를 요청할 때는 client와 server간 동기화 할 때 뿐
                    else if (message.Contains("responseGroupList"))
                    {
                        // 지금은 여러번 받게 되는데 차후 한번만 받게 바꾸자
                        // 한번만 받게 변경 완료
                        string msg = message.Substring(0, message.LastIndexOf("&responseGroupList"));
                        string[] groups = msg.Split('&');

                        foreach (string g in groups)
                        {
                            if (!Initializer.groupList.Contains(g))
                            {
                                // groupList 추가
                                Initializer.groupList.Add(g);
                            }
                        }
                        // 화면 갱신
                        GroupRefresh();
                    } // receive userList 동기화
                    else if (message.Contains("responseUserList"))
                    {
                        string msg = message.Substring(0, message.LastIndexOf("&responseUserList"));
                        string[] users = msg.Split('&');

                        foreach (string user in users)
                        {
                            if (!Initializer.userList.Contains(user))
                            {
                                // userList에 추가
                                Initializer.userList.Add(user);
                            }
                        }
                    } // receive complete create group
                    else if (message.Contains("completeCreateGroup"))
                    {
                        string msg = message.Substring(0, message.LastIndexOf("completeCreateGroup"));
                        string user_ID = msg.Substring(0, msg.LastIndexOf("&"));

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

                        if (Initializer.groupList.Contains(group))
                        {
                            // 열려있는 ChatGroupForm 중에서 group이 일치하는 window에 출력
                            foreach (ChatGroupForm temp in Initializer.chatGroupForms)
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

        // 크로스스레드 문제 코드 정리해서 없앨 수 있을것 같음
        private void GroupRefresh()
        {
            // 크로스스레드가 발생할 때
            if (Initializer.groupForm.InvokeRequired)
            {
                // BeginInvoke - 비동기식 대리자 실행
                // 익명함수? 익명대리자?
                Initializer.groupForm.BeginInvoke(new MethodInvoker(delegate
                {
                    GroupRefresh();
                }));
            }
            else
                Initializer.groupForm.designGroup();
        }

        // ParameterizedThreadStart가 object만 받기 때문에 object 사용
        private void open_Group(object user_ID)
        {
            Initializer.user_ID = Convert.ToString(user_ID);
            Initializer.groupForm.Text = "사용중인 유저 : " + Convert.ToString(user_ID);
            // Show를 사용하면 바로 꺼짐, 이유 확인 필요
            Initializer.groupForm.ShowDialog();
        }

        // ChatGroupForm이 열렸을 때 Form 정보 저장
        public void open_ChatGroupForm(ChatGroupForm chatGroupForm)
        {
            Initializer.chatGroupForms.Add(chatGroupForm);
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

        private void btn_SignIn_Click(object sender, EventArgs e)
        {
            string user_ID = txt_UserID.Text;
            string user_PW = txt_UserPW.Text;

            string sendMsg = user_ID + "&" + user_PW + "signin";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            Initializer.stream.Write(buffer, 0, buffer.Length);
            Initializer.stream.Flush();

            txt_UserID.Clear();
            txt_UserPW.Clear();
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {

        }
    }
}

// 출처: https://it-jerryfamily.tistory.com/80 [IT 이야기]
// 출처: https://yeolco.tistory.com/53 [열코의 프로그래밍 일기]