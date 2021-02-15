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

using log4net;
using System.Security.Cryptography;
using System.IO;

using MyMessageProtocol;

namespace TestClient
{
    public partial class TestClientUI : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TestClientUI));

        const int CHUNK_SIZE = 4096;

        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        string message = string.Empty;

        string user_ID = string.Empty;
        List<string> userList = new List<string>();
        Dictionary<long, Tuple<string, string>> groupList = new Dictionary<long, Tuple<string, string>>();

        List<ChatGroupForm> chatGroupForms = new List<ChatGroupForm>();

        public static uint msgid = 0;

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

                    PacketMessage message = MessageUtil.Receive(stream);
                    if (message != null)
                    {
                        switch (message.Header.MSGTYPE)
                        {
                            // 회원가입 성공
                            case CONSTANTS.RES_REGISTER_SUCCESS:
                                {
                                    ResponseRegisterSuccess resBody = (ResponseRegisterSuccess)message.Body;

                                    // 회원가입한 사람일 때
                                    if (user_ID.Length == 0)
                                    {
                                        MessageBox.Show("회원가입 되었습니다.", "알림");
                                        SettingControlLocationSignIn();
                                    }
                                    // 회원가입한 사람이 아닐 때
                                    else
                                    {
                                        userList.Add(resBody.userID);
                                        GroupRefresh();
                                    }
                                    break;
                                }
                            // 회원가입 실패 - 이미 존재하는 사용자
                            case CONSTANTS.RES_REGISTER_FAIL_EXIST:
                                {
                                    MessageBox.Show("이미 등록된 회원입니다.", "알림");
                                    break;
                                }
                            // 로그인 성공
                            case CONSTANTS.RES_SIGNIN_SUCCESS:
                                {
                                    // 폼 컨트롤 위치 조정
                                    SettingControlLocationGroup();
                                    break;
                                }
                            // 로그인 실패 - 존재하지 않는 사용자
                            case CONSTANTS.RES_SIGNIN_FAIL_NOT_EXIST:
                                {
                                    MessageBox.Show("등록된 회원이 아닙니다.", "알림");
                                    break;
                                }
                            // 로그인 실패 - 잘못된 비밀번호
                            case CONSTANTS.RES_SIGNIN_FAIL_WRONG_PASSWORD:
                                {
                                    MessageBox.Show("잘못된 비밀번호 입니다.", "알림");
                                    break;
                                }
                            // 로그인 실패 - 이미 접속 중인 사용자
                            case CONSTANTS.RES_SIGNIN_FAIL_ONLINE_USER:
                                {
                                    MessageBox.Show("이미 접속 중인 사용자 입니다.", "알림");
                                    break;
                                }
                            // 회원목록 반환
                            case CONSTANTS.RES_USERLIST:
                                {
                                    ResponseUserList resBody = (ResponseUserList)message.Body;
                                    foreach (string user in resBody.users)
                                    {
                                        if (!userList.Contains(user) && !user_ID.Equals(user))
                                        {
                                            userList.Add(user);
                                        }
                                    }
                                    GroupRefresh();
                                    break;
                                }
                            // 채팅방목록 반환
                            case CONSTANTS.RES_GROUPLIST:
                                {
                                    ResponseGroupList resBody = (ResponseGroupList)message.Body;
                                    if (resBody.GetSize() != 0)
                                    {
                                        string[] delimiterChars = { "&" };
                                        List<string> groups = new List<string>(resBody.msg.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries));

                                        foreach (string group in groups)
                                        {
                                            string usersInGroup = group.Substring(group.LastIndexOf("^") + 1);
                                            string temp = group.Substring(0, group.LastIndexOf("^"));

                                            string groupName = temp.Substring(temp.LastIndexOf("^") + 1);

                                            long pid = long.Parse(temp.Substring(0, temp.LastIndexOf("^")));

                                            if (!groupList.ContainsKey(pid))
                                            {
                                                // groupList 추가
                                                groupList.Add(pid, new Tuple<string, string>(groupName, usersInGroup));
                                            }
                                        }
                                        // 화면 갱신
                                        GroupRefresh();
                                    }
                                    else
                                    {
                                        MessageBox.Show("만들어진 채팅방이 없습니다.\n새로운 채팅방을 만들어보세요.", "알림");
                                    }
                                    break;
                                }
                            // 채팅방 생성 완료
                            case CONSTANTS.RES_CREATE_GROUP_SUCCESS:
                                {
                                    ResponseCreateGroupSuccess resBody = (ResponseCreateGroupSuccess)message.Body;
                                    if (!groupList.ContainsKey(resBody.pid))
                                    {
                                        // groupList 추가
                                        groupList.Add(resBody.pid, new Tuple<string, string>(resBody.roomName, resBody.users));
                                        GroupRefresh();
                                    }
                                    break;
                                }
                            // 채팅 메시지 수신
                            case CONSTANTS.RES_CHAT:
                                {
                                    ResponseChat resBody = (ResponseChat)message.Body;

                                    if (groupList.ContainsKey(resBody.pid))
                                    {
                                        // 열려있는 ChatGroupForm 중에서 pid가 일치하는 window에 출력
                                        foreach (ChatGroupForm temp in chatGroupForms)
                                        {
                                            if (temp.pid == resBody.pid)
                                            {
                                                temp.DisplayText(resBody.userID + " : " + resBody.chatMsg);
                                            }
                                        }
                                    }
                                    break;
                                }
                            // 채팅방 초대 완료
                            case CONSTANTS.RES_INVITATION_SUCCESS:
                                {
                                    ResponseInvitationSuccess resBody = (ResponseInvitationSuccess)message.Body;

                                    // 초대된 회원이라면
                                    if (resBody.invitedUsers.Contains(user_ID))
                                    {
                                        btn_PullGroup_Click();
                                    }
                                    // 채팅방에 원래 있던 회원이라면
                                    else
                                    {
                                        string[] delimiterChars = { ", " };
                                        List<string> users = new List<string>(groupList[resBody.pid].Item2.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries));

                                        // 초대된 인원 추가
                                        users.AddRange(resBody.invitedUsers);
                                        // 정렬
                                        users.Sort();

                                        // 변환
                                        string usersInGroup = string.Join(", ", users);

                                        foreach (string user in resBody.invitedUsers)
                                        {
                                            // groupList 변경
                                            groupList[resBody.pid] = new Tuple<string, string>(groupList[resBody.pid].Item1, usersInGroup);
                                            // 열려있는 채팅방 중 변경된 채팅방이 있다면
                                            foreach (ChatGroupForm chatGroupForm in chatGroupForms)
                                            {
                                                if (chatGroupForm.pid == resBody.pid)
                                                {
                                                    chatGroupForm.DisplayText(user + "님이 채팅방에 초대되셨습니다.");
                                                    chatGroupForm.RedrawUserList();
                                                }
                                            }
                                        }
                                        GroupRefresh();
                                    }

                                    break;
                                }
                            // 채팅방 나가기 완료
                            case CONSTANTS.RES_LEAVE_GROUP_SUCCESS:
                                {
                                    ResponseLeaveGroupSuccess resBody = (ResponseLeaveGroupSuccess)message.Body;

                                    // 나간 사람일 때
                                    if (user_ID.Equals(resBody.receivedID))
                                    {
                                        groupList.Remove(resBody.pid);
                                    }
                                    else
                                    {
                                        string[] delimiterChars = { ", " };
                                        List<string> users = new List<string>(groupList[resBody.pid].Item2.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries));

                                        users.Remove(resBody.receivedID);
                                        string usersInGroup = string.Join(", ", users);

                                        groupList[resBody.pid] = new Tuple<string, string>(groupList[resBody.pid].Item1, usersInGroup);

                                        foreach (ChatGroupForm chatGroupForm in chatGroupForms)
                                        {
                                            if (chatGroupForm.pid == resBody.pid)
                                            {
                                                chatGroupForm.DisplayText(resBody.receivedID + "님이 채팅방에서 나가셨습니다.");
                                                chatGroupForm.groupUserList.Remove(resBody.receivedID);
                                                chatGroupForm.RedrawUserList();
                                            }
                                        }
                                    }
                                    GroupRefresh();
                                    break;
                                }
                            case CONSTANTS.RES_SEND_FILE:
                                {
                                    ResponseSendFile resBody = (ResponseSendFile)message.Body;

                                    using (Stream fileStream = new FileStream(resBody.filePath, FileMode.Open))
                                    {
                                        byte[] rbytes = new byte[CHUNK_SIZE];

                                        long readValue = BitConverter.ToInt64(rbytes, 0);

                                        int totalRead = 0;
                                        ushort msgSeq = 0;
                                        byte fragmented = (fileStream.Length < CHUNK_SIZE) ? CONSTANTS.NOT_FRAGMENTED : CONSTANTS.FRAGMENT;

                                        while (totalRead < fileStream.Length)
                                        {
                                            int read = fileStream.Read(rbytes, 0, CHUNK_SIZE);
                                            totalRead += read;
                                            PacketMessage fileMsg = new PacketMessage();

                                            byte[] sendBytes = new byte[read];
                                            Array.Copy(rbytes, 0, sendBytes, 0, read);

                                            fileMsg.Body = new RequestSendFileData(sendBytes);
                                            fileMsg.Header = new Header()
                                            {
                                                MSGID = msgid,
                                                MSGTYPE = CONSTANTS.REQ_SEND_FILE_DATA,
                                                BODYLEN = (uint)fileMsg.Body.GetSize(),
                                                FRAGMENTED = fragmented,
                                                LASTMSG = (totalRead < fileStream.Length) ? CONSTANTS.NOT_LASTMSG : CONSTANTS.LASTMSG,
                                                SEQ = msgSeq++
                                            };

                                            // 모든 파일의 내용이 전송될 때까지 파일 스트림을 0x03 메시지에 담아 서버로 보냄
                                            MessageUtil.Send(stream, fileMsg);
                                        }
                                    }
                                    break;
                                }
                            case CONSTANTS.RES_FILE_SEND_COMPLETE:
                                {
                                    // 서버에서 파일을 제대로 받았는지에 대한 응답을 받음
                                    ResponseFileSendComplete resBody = (ResponseFileSendComplete)message.Body;
                                    Console.WriteLine("파일 전송 성공");
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
                catch (IOException IOe)
                {
                    Console.WriteLine(IOe.StackTrace);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
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
            byte[] temp = sHA256Managed.ComputeHash(Encoding.Unicode.GetBytes(txt_UserPW.Text));

            user_ID = txt_UserID.Text;
            string user_PW = string.Join(string.Empty, Array.ConvertAll(temp, b => b.ToString("X2")));

            // 로그인 메시지 작성
            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Body = new RequestSignIn()
            {
                msg = user_ID + "&" + user_PW
            };
            reqMsg.Header = new Header()
            {
                MSGID = msgid++,
                MSGTYPE = CONSTANTS.REQ_SIGNIN,
                BODYLEN = (uint)reqMsg.Body.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };
            // 로그인 메시지 발송
            MessageUtil.Send(stream, reqMsg);

            /*
            string sendMsg = user_ID + "&" + user_PW + "signin";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            */

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
                    btn_SignOut.Location = new Point(202, 515);

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
                btn_SignOut.Location = new Point(202, 515);

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
            byte[] temp = sHA256Managed.ComputeHash(Encoding.Unicode.GetBytes(txt_UserPW.Text));

            user_ID = txt_UserID.Text;
            string user_PW = string.Join(string.Empty, Array.ConvertAll(temp, b => b.ToString("X2")));

            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Body = new RequestRegister()
            {
                msg = user_ID + "&" + user_PW
            };
            reqMsg.Header = new Header()
            {
                MSGID = msgid++,
                MSGTYPE = CONSTANTS.REQ_REGISTER,
                BODYLEN = (uint) reqMsg.Body.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };

            MessageUtil.Send(stream, reqMsg);

            // string sendMsg = user_ID + "&" + user_PW + "register";
            /*
            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            */

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
            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Header = new Header()
            {
                MSGID = msgid++,
                MSGTYPE = CONSTANTS.REQ_USERLIST,
                BODYLEN = 0,
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };

            MessageUtil.Send(stream, reqMsg);

            /*
            string sendMsg = user_ID + "&requestUserList";
            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            */
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
            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Body = new RequestGroupList()
            {
                userID = user_ID
            };
            reqMsg.Header = new Header()
            {
                MSGID = msgid++,
                MSGTYPE = CONSTANTS.REQ_GROUPLIST,
                BODYLEN = (uint)reqMsg.Body.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };

            MessageUtil.Send(stream, reqMsg);

            /*
            string sendMsg = user_ID + "&requestGroupList";
            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            */
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

            // 로그아웃 메시지 작성 및 발송
            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Body = new RequestSignOut()
            {
                userID = user_ID
            };
            reqMsg.Header = new Header()
            {
                MSGID = msgid++,
                MSGTYPE = CONSTANTS.REQ_SIGNOUT,
                BODYLEN = (uint)reqMsg.Body.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };
            MessageUtil.Send(stream, reqMsg);

            /*
            string sendMsg = user_ID + "&SignOut";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            */

            // 상태 초기화
            user_ID = null;
            userList.Clear();
            groupList.Clear();
            chatGroupForms.Clear();
            lb_GroupList.Items.Clear();
            SettingControlLocationSignIn();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            // 채팅방에 들어갈 회원들 수집
            List<string> usersInGroup = new List<string>();
            usersInGroup.Add(user_ID);
            foreach (object checkeditem in clb_GroupingUser.CheckedItems)
            {
                usersInGroup.Add(checkeditem.ToString());
            }
            // 정렬
            usersInGroup.Sort();

            string group = string.Join(", ", usersInGroup);
            string groupName = group + "Group";
            // 채팅방 이름이 20자가 넘어가면 20자로 자르기
            if (groupName.Length > 20)
            {
                groupName = groupName.Substring(0, 20);
            }

            string msg = groupName + "&" + group;

            // send group info to server
            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Body = new RequestCreateGroup()
            {
                msg = msg
            };
            reqMsg.Header = new Header()
            {
                MSGID = msgid++,
                MSGTYPE = CONSTANTS.REQ_CREATE_GROUP,
                BODYLEN = (uint)reqMsg.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };
            MessageUtil.Send(stream, reqMsg);

            /*
            string sendMsg = groupName + "&" + group + "&createGroup";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            */

            // 초대 회원 선택 초기화
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
            foreach (string item in userList)
            {
                if (!lb_UserList.Items.Contains(item))
                {
                    lb_UserList.Items.Add(item);
                }
            }

            // lb_GroupList

            lb_GroupList.Items.Clear();
            foreach (KeyValuePair<long, Tuple<string, string>> item in groupList)
            {
                //if (!lb_GroupList.Items.Contains(item.Key))
                //{
                    lb_GroupList.Items.Add(item.Key);
                    lb_GroupList.Items.Add(item.Value.Item1);
                    lb_GroupList.Items.Add("채팅방 인원 : " + item.Value.Item2);
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
            ListBox lb = sender as ListBox;

            int index = lb.SelectedIndex - (lb.SelectedIndex % 3);
            long pid = long.Parse(lb.Items[index].ToString());
            string roomName = lb.Items[index + 1].ToString();

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
            chatGroupForm.pid = pid;
            chatGroupForm.roomName = roomName;
            chatGroupForm.user_ID = user_ID;
            chatGroupForm.Tag = index;
            chatGroupForm.Text = roomName;
            chatGroupForm.Name = "chatGroupForm" + index;

            // List 변환
            string[] delimiterChars = { ", " };
            List<string> groupUserList = new List<string>(groupList[pid].Item2.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries));
            /*
            foreach(var tmp in groupUserList)
            {
                Console.WriteLine(tmp);
            }
            Console.WriteLine(groupList[pid].Item2);
            */

            chatGroupForm.groupUserList = groupUserList;
            chatGroupForm.userList = userList;

            // ChatGroupForm이 열렸을 때 TestClientUI에 Form 정보 저장
            // Open_ChatGroupForm(chatGroupForm);

            chatGroupForms.Add(chatGroupForm);

            chatGroupForm.Show();
        }

        private void clb_GroupingUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private string AESEncrypt256(string input, string key)
        {
            SHA256Managed sHA256Managed = new SHA256Managed();
            byte[] salt = sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(key.Length.ToString()));

            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            // aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Key = salt;
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

            ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            string Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        private string AESDecrypt256(string input, string key)
        {
            SHA256Managed sHA256Managed = new SHA256Managed();
            byte[] salt = sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(key.Length.ToString()));

            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            // aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Key = salt;
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            ICryptoTransform decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            string Output = Encoding.UTF8.GetString(xBuff);
            return Output;
        }
    }
}