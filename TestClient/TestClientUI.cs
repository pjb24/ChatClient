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
        // <ID>
        public List<string> userList = new List<string>();
        // <groupName>
        public List<string> groupList = new List<string>();

        // class에서 사용할 변수 초기화
        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        string message = string.Empty;

        // 사용되는 GroupForm이 1개이기 때문에 이곳에 선언
        GroupForm groupForm = new GroupForm();
        List<ChatGroupForm> chatGroupForms = new List<ChatGroupForm>();

        public TestClientUI()
        {
            InitializeComponent();
        }

        // Form Load할 때 call
        private void TestClient_Load(object sender, EventArgs e)
        {
            // (serverIP, port) 연결 시도, 현재는 Loopback 사용중, Exception 처리 필요
            clientSocket.Connect(IPAddress.Loopback, 11000);
            // NetworkStream 정보 저장, NetworkStream?
            stream = clientSocket.GetStream();

            message = "Connected to Chat Server";
            DisplayText(message);

            // Encoding해서 buffer에 저장, "asdf" 현재 사용하지 않음, 없어도 된다.
            byte[] buffer = Encoding.Unicode.GetBytes("asdf" + "$");
            // 송출
            stream.Write(buffer, 0, buffer.Length);
            // MSDN : 파생 클래스에서 재정의되면 이 스트림에 대해 모든 버퍼를 지우고 버퍼링된 데이터가 내부 디바이스에 쓰여지도록 합니다.
            // NetworkStream에서는 구현되지 않음
            stream.Flush();

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

        private void btn_Send_Click(object sender, EventArgs e)
        {
            // txt_Send의 Text를 읽어서 server에 전송
            byte[] buffer = Encoding.Unicode.GetBytes(this.txt_Send.Text + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void txt_Send_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Enter 키 입력할 때 발생
                case Keys.Enter:
                    btn_Send_Click(sender, e);
                    // txt_Send의 Text를 선택해줌
                    txt_Send.SelectAll();
                    // Text를 지움
                    // txt_Send.Clear();
                    break;
            }
        }

        // server message 대기
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

                // received message 처리
                // allow sign in message
                if (message.Contains("allowSignin"))
                {
                    // 정보 추출
                    string msg = message.Substring(0, message.LastIndexOf("allowSignin"));
                    string user_ID = msg.Substring(0, message.LastIndexOf("&"));
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
                    
                    foreach(string g in groups)
                    {
                        if (!groupList.Contains(g))
                        {
                            // groupList 추가
                            groupList.Add(g);
                            groupForm.groupList = groupList;
                        }
                    }
                    // 화면 갱신
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
                        }
                    }
                    GroupRefresh();
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

        // 크로스스레드 문제
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

        // 크로스스레드 문제 코드 정리해서 없앨 수 있을것 같음
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

        // 진입점 정리 필요
        private void btn_SignIn_Click(object sender, EventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.stream = stream;
            signInForm.Show();
        }

        // ParameterizedThreadStart가 object만 받기 때문에 object 사용
        private void open_Group(object user_ID)
        {
            groupForm.stream = stream;
            groupForm.user_ID = Convert.ToString(user_ID);
            groupForm.userList = userList;
            groupForm.groupList = groupList;
            groupForm.testClientUI = this;
            // Show를 사용하면 바로 꺼짐, 이유 확인 필요
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