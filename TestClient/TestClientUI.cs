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
        Dictionary<int, string> userList = new Dictionary<int, string>();
        Dictionary<int, Tuple<int, string>> roomList = new Dictionary<int, Tuple<int, string>>();
        Dictionary<int, Tuple<int, int, int>> usersInRoom = new Dictionary<int, Tuple<int, int, int>>();

        List<ChatRoomForm> chatGroupForms = new List<ChatRoomForm>();

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
                MessageBox.Show("서버에 접속할 수 없습니다.\n 잠시 후 프로그램을 다시 실행해주십시오.", "알림");
                ProgramClose();
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
                                    if (user_ID.Equals(resBody.userID))
                                    {
                                        MessageBox.Show("회원가입 되었습니다.", "회원가입 성공");
                                        SettingControlLocationSignIn();
                                    }
                                    // 회원가입한 사람이 아닐 때
                                    else
                                    {
                                        userList.Add(resBody.No, resBody.userID);
                                        GroupRefresh();
                                    }
                                    break;
                                }
                            // 회원가입 실패 - 이미 존재하는 사용자
                            case CONSTANTS.RES_REGISTER_FAIL_EXIST:
                                {
                                    MessageBox.Show("이미 등록된 회원입니다.", "회원가입 실패");
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
                                    MessageBox.Show("등록된 회원이 아닙니다.", "로그인 실패");
                                    break;
                                }
                            // 로그인 실패 - 잘못된 비밀번호
                            case CONSTANTS.RES_SIGNIN_FAIL_WRONG_PASSWORD:
                                {
                                    MessageBox.Show("잘못된 비밀번호 입니다.", "로그인 실패");
                                    break;
                                }
                            // 로그인 실패 - 이미 접속 중인 사용자
                            case CONSTANTS.RES_SIGNIN_FAIL_ONLINE_USER:
                                {
                                    MessageBox.Show("이미 접속 중인 사용자 입니다.", "로그인 실패");
                                    break;
                                }
                            // 회원목록 반환
                            case CONSTANTS.RES_USERLIST:
                                {
                                    ResponseUserList resBody = (ResponseUserList)message.Body;
                                    foreach (KeyValuePair<int, string> user in resBody.userList)
                                    {
                                        if (!userList.ContainsKey(user.Key))
                                        {
                                            userList.Add(user.Key, user.Value);
                                        }
                                    }
                                    GroupRefresh();
                                    break;
                                }
                            // 채팅방목록 반환
                            case CONSTANTS.RES_ROOMLIST:
                                {
                                    ResponseRoomList resBody = (ResponseRoomList)message.Body;
                                    if (resBody.GetSize() != 0)
                                    {
                                        foreach (KeyValuePair<int, Tuple<int, string>> room in resBody.roomList)
                                        {
                                            if (!roomList.ContainsKey(room.Key))
                                            {
                                                // roomList 추가
                                                roomList.Add(room.Key, new Tuple<int, string>(room.Value.Item1, room.Value.Item2));
                                            }
                                        }

                                        foreach (KeyValuePair<int, Tuple<int, int, int>> users in resBody.usersInRoom)
                                        {
                                            if (!usersInRoom.ContainsKey(users.Key))
                                            {
                                                // roomList 추가
                                                usersInRoom.Add(users.Key, new Tuple<int, int, int>(users.Value.Item1, users.Value.Item2, users.Value.Item3));
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
                            case CONSTANTS.RES_CREATE_ROOM_SUCCESS:
                                {
                                    ResponseCreateRoomSuccess resBody = (ResponseCreateRoomSuccess)message.Body;
                                    if (!roomList.ContainsKey(resBody.roomNo))
                                    {
                                        // roomList 추가
                                        roomList.Add(resBody.roomNo, new Tuple<int, string>(resBody.accessRight, resBody.roomName));

                                        // 채팅방 회원 추가
                                        foreach(KeyValuePair<int, Tuple<int, int, int>> temp in resBody.usersInRoom)
                                        {
                                            usersInRoom.Add(temp.Key, new Tuple<int, int, int>(temp.Value.Item1, temp.Value.Item2, temp.Value.Item3));
                                        }
                                        GroupRefresh();
                                    }
                                    break;
                                }
                            // 채팅 메시지 수신
                            case CONSTANTS.RES_CHAT:
                                {
                                    /*
                                    ResponseChat resBody = (ResponseChat)message.Body;

                                    if (roomList.ContainsKey(resBody.roomNo))
                                    {
                                        // 열려있는 ChatGroupForm 중에서 roomNo가 일치하는 window에 출력
                                        foreach (ChatRoomForm temp in chatGroupForms)
                                        {
                                            if (temp.roomNo == resBody.roomNo)
                                            {
                                                temp.DisplayText(resBody.userID + " : " + resBody.chatMsg);
                                            }
                                        }
                                    }
                                    */
                                    break;
                                }
                            // 채팅방 초대 완료
                            case CONSTANTS.RES_INVITATION_SUCCESS:
                                {
                                    ResponseInvitationSuccess resBody = (ResponseInvitationSuccess)message.Body;

                                    int userNo = 0;
                                    userNo = SearchUserNoByUserID(user_ID);

                                    bool invited = false;
                                    foreach(KeyValuePair<int, Tuple<int, int, int>> temp in resBody.usersInRoom)
                                    {
                                        if (temp.Value.Item2.Equals(userNo))
                                        {
                                            invited = true;
                                            break;
                                        }
                                    }

                                    string userName = string.Empty;
                                    // 초대된 회원이라면
                                    if (invited)
                                    {
                                        btn_PullGroup_Click();
                                    }
                                    // 채팅방에 원래 있던 회원이라면
                                    else
                                    {
                                        // usersInRoom 추가
                                        foreach (KeyValuePair<int, Tuple<int, int, int>> temp in resBody.usersInRoom)
                                        {
                                            usersInRoom.Add(temp.Key, new Tuple<int, int, int>(temp.Value.Item1, temp.Value.Item2, temp.Value.Item3));
                                            // 열려있는 채팅방 중 변경된 채팅방이 있다면
                                            foreach (ChatRoomForm chatGroupForm in chatGroupForms)
                                            {
                                                if (chatGroupForm.roomNo == resBody.roomNo)
                                                {
                                                    foreach(KeyValuePair<int, string> tmp in userList)
                                                    {
                                                        if (tmp.Key.Equals(temp.Value.Item2))
                                                        {
                                                            userName = tmp.Value;
                                                            break;
                                                        }
                                                    }
                                                    chatGroupForm.DisplayText(userName + "님이 채팅방에 초대되셨습니다.");
                                                    chatGroupForm.RedrawUserList();
                                                }
                                            }
                                        }
                                        GroupRefresh();
                                    }
                                    break;
                                }
                            // 채팅방 나가기 완료
                            case CONSTANTS.RES_LEAVE_ROOM_SUCCESS:
                                {
                                    ResponseLeaveRoomSuccess resBody = (ResponseLeaveRoomSuccess)message.Body;
                                    int userNo = 0;
                                    int usersInRoomNo = 0;
                                    // 나간 사람일 때
                                    if (user_ID.Equals(userList[resBody.userNo]))
                                    {
                                        // roomList 제거
                                        roomList.Remove(resBody.roomNo);

                                        // 회원 번호 검색
                                        userNo = resBody.userNo;

                                        // usersInRoom 제거
                                        foreach (KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
                                        {
                                            if (temp.Value.Item1.Equals(resBody.roomNo) && temp.Value.Item2.Equals(userNo))
                                            {
                                                usersInRoomNo = temp.Key;
                                                break;
                                            }
                                        }
                                        usersInRoom.Remove(usersInRoomNo);
                                    }
                                    else
                                    {
                                        // 회원 번호 검색
                                        userNo = resBody.userNo;

                                        // usersInRoom 제거
                                        foreach (KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
                                        {
                                            if (temp.Value.Item1.Equals(resBody.roomNo) && temp.Value.Item2.Equals(userNo))
                                            {
                                                usersInRoomNo = temp.Key;
                                                break;
                                            }
                                        }
                                        usersInRoom.Remove(usersInRoomNo);

                                        foreach (ChatRoomForm chatGroupForm in chatGroupForms)
                                        {
                                            if (chatGroupForm.roomNo == resBody.roomNo)
                                            {
                                                chatGroupForm.DisplayText(userList[resBody.userNo] + "님이 채팅방에서 나가셨습니다.");
                                                chatGroupForm.usersInRoom.Remove(usersInRoomNo);
                                                chatGroupForm.RedrawUserList();
                                            }
                                        }
                                    }
                                    GroupRefresh();
                                    break;
                                }
                            // 채팅방 추방
                            case CONSTANTS.RES_BANISH_USER_SUCCESS:
                                {
                                    ResponseBanishUserSuccess resBody = (ResponseBanishUserSuccess)message.Body;
                                    int userNo = 0;
                                    int usersInRoomNo = 0;
                                    // 추방된 사람일 때
                                    if (user_ID.Equals(userList[resBody.banishedUserNo]))
                                    {
                                        // roomList 제거
                                        roomList.Remove(resBody.roomNo);

                                        // 회원 번호 검색
                                        userNo = resBody.banishedUserNo;

                                        // usersInRoom 제거
                                        foreach (KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
                                        {
                                            if (temp.Value.Item1.Equals(resBody.roomNo) && temp.Value.Item2.Equals(userNo))
                                            {
                                                usersInRoomNo = temp.Key;
                                                break;
                                            }
                                        }
                                        usersInRoom.Remove(usersInRoomNo);
                                    }
                                    else
                                    {
                                        // 회원 번호 검색
                                        userNo = resBody.banishedUserNo;

                                        // usersInRoom 제거
                                        foreach (KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
                                        {
                                            if (temp.Value.Item1.Equals(resBody.roomNo) && temp.Value.Item2.Equals(userNo))
                                            {
                                                usersInRoomNo = temp.Key;
                                                break;
                                            }
                                        }
                                        usersInRoom.Remove(usersInRoomNo);

                                        foreach (ChatRoomForm chatGroupForm in chatGroupForms)
                                        {
                                            if (chatGroupForm.roomNo == resBody.roomNo)
                                            {
                                                chatGroupForm.DisplayText(userList[resBody.banishedUserNo] + "님이 채팅방에서 추방되었습니다.");
                                                chatGroupForm.RedrawUserList();
                                            }
                                        }
                                    }
                                    GroupRefresh();
                                    break;
                                }
                            // 채팅방 설정 변경 완료
                            case CONSTANTS.RES_CHANGE_ROOM_CONFIG_SUCCESS:
                                {
                                    ResponseChangeRoomConfigSuccess resBody = (ResponseChangeRoomConfigSuccess)message.Body;

                                    string accessRightToString = string.Empty;
                                    if (resBody.accessRight == 0)
                                    {
                                        accessRightToString = "비공개";
                                    }
                                    else
                                    {
                                        accessRightToString = "공개";
                                    }

                                    // 채팅방 열려있는지 확인
                                    foreach (ChatRoomForm chatGroupForm in chatGroupForms)
                                    {
                                        if (chatGroupForm.roomNo == resBody.roomNo)
                                        {
                                            // 변경점 확인
                                            // accessRight 와 roomName 변경
                                            if (!resBody.accessRight.Equals(roomList[resBody.roomNo].Item1) && !resBody.roomName.Equals(roomList[resBody.roomNo].Item2))
                                            {
                                                chatGroupForm.DisplayText(string.Format("{0}번 채팅방의 공개 여부가 {1}로, 채팅방 이름이 {2}로 변경됨", resBody.roomNo, accessRightToString, resBody.roomName));
                                            }
                                            // accessRight 변경
                                            else if (!resBody.roomName.Equals(roomList[resBody.roomNo].Item1))
                                            {
                                                chatGroupForm.DisplayText(string.Format("{0}번 채팅방의 공개 여부가 {1} 변경됨", resBody.roomNo, accessRightToString));
                                            }
                                            // roomName 변경
                                            else if (!resBody.roomName.Equals(roomList[resBody.roomNo].Item2))
                                            {
                                                chatGroupForm.DisplayText(string.Format("{0}번 채팅방의 이름이 {1}로 변경됨", resBody.roomNo, resBody.roomName));
                                            }
                                        }
                                    }
                                    // roomList 변경
                                    roomList[resBody.roomNo] = new Tuple<int, string>(resBody.accessRight, resBody.roomName);
                                    GroupRefresh();

                                    break;
                                }
                            // 채팅방 관리자 권한 변경 완료
                            case CONSTANTS.RES_CHANGE_MANAGEMENT_RIGHTS_SUCCESS:
                                {
                                    ResponseChangeManagementRightsSuccess resBody = (ResponseChangeManagementRightsSuccess)message.Body;

                                    List<int> tempKey = new List<int>();
                                    List<int> tempRoomNo = new List<int>();
                                    List<int> tempUserNo = new List<int>();
                                    List<int> tempRight = new List<int>();

                                    // usersInRoom 변경
                                    foreach (KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
                                    {
                                        foreach (int userNo in resBody.changedUsersNo)
                                        {
                                            if (temp.Value.Item1.Equals(resBody.roomNo) && temp.Value.Item2.Equals(userNo) && temp.Value.Item3.Equals(0))
                                            {
                                                // usersInRoom[temp.Key] = new Tuple<int, int, int>(resBody.roomNo, userNo, 1);
                                                tempKey.Add(temp.Key);
                                                tempRoomNo.Add(resBody.roomNo);
                                                tempUserNo.Add(userNo);
                                                tempRight.Add(1);
                                                // 열려있는 채팅방에 표시
                                                foreach (ChatRoomForm chatGroupForm in chatGroupForms)
                                                {
                                                    if (chatGroupForm.roomNo == resBody.roomNo)
                                                    {
                                                        chatGroupForm.DisplayText(string.Format("{0}님에게 관리자 권한이 부여되었습니다.", userList[userNo]));
                                                    }
                                                }
                                            }
                                            else if (temp.Value.Item1.Equals(resBody.roomNo) && temp.Value.Item2.Equals(userNo) && temp.Value.Item3.Equals(1))
                                            {
                                                // usersInRoom[temp.Key] = new Tuple<int, int, int>(resBody.roomNo, userNo, 0);
                                                tempKey.Add(temp.Key);
                                                tempRoomNo.Add(resBody.roomNo);
                                                tempUserNo.Add(userNo);
                                                tempRight.Add(0);
                                                // 열려있는 채팅방에 표시
                                                foreach (ChatRoomForm chatGroupForm in chatGroupForms)
                                                {
                                                    if (chatGroupForm.roomNo == resBody.roomNo)
                                                    {
                                                        chatGroupForm.DisplayText(string.Format("{0}님에게서 관리자 권한이 해제되었습니다.", userList[userNo]));
                                                    }
                                                }
                                            }
                                        }   
                                    }

                                    for (int i = 0; i < tempKey.Count; i++)
                                    {
                                        usersInRoom[tempKey[i]] = new Tuple<int, int, int>(tempRoomNo[i], tempUserNo[i], tempRight[i]);
                                    }
                                    break;
                                }
                            // 파일 전송 준비 완료
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
                            case CONSTANTS.REQ_SEND_FILE:
                                {
                                    RequestSendFile reqBody = (RequestSendFile)message.Body;

                                    string msg = message.Header.MSGID + "&" + reqBody.roomNo + "&" + reqBody.filePath + "&" + user_ID;

                                    PacketMessage resMsg = new PacketMessage();
                                    resMsg.Body = new ResponseSendFile()
                                    {
                                        msg = msg
                                        // MSGID = message.Header.MSGID,
                                        // RESPONSE = CONSTANTS.ACCEPTED,
                                        // pid = reqBody.pid
                                        // filePath = reqBody.filePath
                                    };
                                    resMsg.Header = new Header()
                                    {
                                        MSGID = msgid++,
                                        MSGTYPE = CONSTANTS.RES_SEND_FILE,
                                        BODYLEN = (uint)resMsg.Body.GetSize(),
                                        FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                                        LASTMSG = CONSTANTS.LASTMSG,
                                        SEQ = 0
                                    };

                                    MessageUtil.Send(stream, resMsg);

                                    long fileSize = reqBody.FILESIZE;
                                    string fileName = reqBody.FILENAME;

                                    string dir = System.Windows.Forms.Application.StartupPath + "\\file";
                                    if (Directory.Exists(dir) == false)
                                    {
                                        Directory.CreateDirectory(dir);
                                    }

                                    // 파일 스트림 생성
                                    FileStream file = new FileStream(dir + "\\" + fileName, FileMode.Create);
                                    uint? dataMsgId = null;
                                    ushort prevSeq = 0;
                                    while ((message = MessageUtil.Receive(stream)) != null)
                                    {
                                        Console.Write("#");
                                        if (message.Header.MSGTYPE != CONSTANTS.REQ_SEND_FILE_DATA)
                                            break;

                                        if (dataMsgId == null)
                                            dataMsgId = message.Header.MSGID;
                                        else
                                        {
                                            if (dataMsgId != message.Header.MSGID)
                                                break;
                                        }

                                        // 메시지 순서가 어긋나면 전송 중단
                                        if (prevSeq++ != message.Header.SEQ)
                                        {
                                            Console.WriteLine("{0}, {1}", prevSeq, message.Header.SEQ);
                                            break;
                                        }

                                        file.Write(message.Body.GetBytes(), 0, message.Body.GetSize());

                                        // 분할 메시지가 아니면 반복을 한번만하고 빠져나옴
                                        if (message.Header.FRAGMENTED == CONSTANTS.NOT_FRAGMENTED)
                                            break;
                                        //마지막 메시지면 반복문을 빠져나옴
                                        if (message.Header.LASTMSG == CONSTANTS.LASTMSG)
                                            break;
                                    }
                                    long recvFileSize = file.Length;
                                    file.Close();

                                    if (roomList.ContainsKey(reqBody.roomNo))
                                    {
                                        // 열려있는 ChatGroupForm 중에서 pid가 일치하는 window에 출력
                                        foreach (ChatRoomForm temp in chatGroupForms)
                                        {
                                            if (temp.roomNo == reqBody.roomNo)
                                            {
                                                temp.DisplayText(userList[reqBody.userNo] + " : " + fileName + " 파일을 전송했습니다.");
                                            }
                                        }
                                    }

                                    resMsg.Body = new ResponseFileSendComplete()
                                    {
                                        MSGID = message.Header.MSGID,
                                        RESULT = CONSTANTS.SUCCESS
                                    };
                                    resMsg.Header = new Header()
                                    {
                                        MSGID = msgid++,
                                        MSGTYPE = CONSTANTS.RES_FILE_SEND_COMPLETE,
                                        BODYLEN = (uint)resMsg.Body.GetSize(),
                                        FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                                        LASTMSG = CONSTANTS.LASTMSG,
                                        SEQ = 0
                                    };
                                    MessageUtil.Send(stream, resMsg);
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
                    MessageBox.Show("서버에서 연결을 종료했습니다.\n 프로그램을 다시 실행해주십시오.", "알림");
                    ProgramClose();
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

        /*
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
        */

        private void btn_SignInSubmit_Click(object sender, EventArgs e)
        {
            user_ID = txt_UserID.Text;
            if (user_ID.Equals("ID") || user_ID.Length == 0)
            {
                MessageBox.Show("회원 ID를 입력해주세요.", "알림");
                return;
            }
            if (user_ID.Length > 20)
            {
                MessageBox.Show("회원 ID는 20자를 초과할 수 없습니다.", "알림");
                txt_UserID.Clear();
                return;
            }
            if (txt_UserPW.Text.Equals("PW") || txt_UserPW.Text.Length == 0)
            {
                MessageBox.Show("회원 PW를 입력해주세요.", "알림");
                return;
            }

            SHA256Managed sHA256Managed = new SHA256Managed();
            byte[] temp = sHA256Managed.ComputeHash(Encoding.Unicode.GetBytes(txt_UserPW.Text));
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
        }

        private void SettingControlLocationReset()
        {
            // SignIn Form
            lbl_SignIn.Location = new Point(455, 63);
            txt_UserID.Location = new Point(430, 139);
            txt_UserPW.Location = new Point(430, 206);
            btn_SignInSubmit.Location = new Point(430, 266);
            btn_OpenRegister.Location = new Point(430, 339);
            lbl_UserID.Location = new Point(401, 141);
            lbl_UserPW.Location = new Point(389, 208);

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
            clb_GroupingUser.Location = new Point(1479, 112);
            btn_Create.Location = new Point(1479, 518);
            btn_CreateClose.Location = new Point(1704, 519);
            rdo_PublicRoom.Location = new Point(1645, 75);
            rdo_PrivateRoom.Location = new Point(1711, 75);
            lbl_RoomName.Location = new Point(1485, 9);
            txt_RoomName.Location = new Point(1489, 37);
            lbl_RoomAccess.Location = new Point(1485, 77);

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
                    lbl_UserID.Location = new Point(58, 258);
                    lbl_UserPW.Location = new Point(46, 297);
                    txt_UserID.Clear();
                    txt_UserPW.Clear();
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
                lbl_UserID.Location = new Point(58, 258);
                lbl_UserPW.Location = new Point(46, 297);
                txt_UserID.Clear();
                txt_UserPW.Clear();
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
                    lbl_UserID.Location = new Point(58, 258);
                    lbl_UserPW.Location = new Point(46, 297);
                    txt_UserID.Clear();
                    txt_UserPW.Clear();
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
                lbl_UserID.Location = new Point(58, 258);
                lbl_UserPW.Location = new Point(46, 297);
                txt_UserID.Clear();
                txt_UserPW.Clear();
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
                    clb_GroupingUser.Location = new Point(0, 112);
                    btn_Create.Location = new Point(12, 497);
                    btn_CreateClose.Location = new Point(210, 497);
                    rdo_PublicRoom.Location = new Point(167, 75);
                    rdo_PrivateRoom.Location = new Point(238, 75);
                    lbl_RoomName.Location = new Point(12, 9);
                    txt_RoomName.Location = new Point(16, 37);
                    lbl_RoomAccess.Location = new Point(12, 77);
                }));
            }
            else
            {
                SettingControlLocationReset();

                this.Text = "채팅방 생성";
                clb_GroupingUser.Location = new Point(0, 112);
                btn_Create.Location = new Point(12, 497);
                btn_CreateClose.Location = new Point(210, 497);
                rdo_PublicRoom.Location = new Point(167, 75);
                rdo_PrivateRoom.Location = new Point(238, 75);
                lbl_RoomName.Location = new Point(12, 9);
                txt_RoomName.Location = new Point(16, 37);
                lbl_RoomAccess.Location = new Point(12, 77);
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
            foreach(KeyValuePair<int, string> user in userList)
            {
                if (!clb_GroupingUser.Items.Contains(user.Value) && !user_ID.Equals(user.Value))
                {
                    clb_GroupingUser.Items.Add(user.Value);
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
            if (user_ID.Equals("ID") || user_ID.Length == 0)
            {
                MessageBox.Show("회원 ID를 입력해주세요.", "알림");
                return;
            }
            if (user_ID.Length > 20)
            {
                MessageBox.Show("회원 ID는 20자를 초과할 수 없습니다.", "알림");
                return;
            }
            if (txt_UserPW.Text.Equals("PW") || txt_UserPW.Text.Length == 0)
            {
                MessageBox.Show("회원 PW를 입력해주세요.", "알림");
                return;
            }

            SHA256Managed sHA256Managed = new SHA256Managed();
            byte[] temp = sHA256Managed.ComputeHash(Encoding.Unicode.GetBytes(txt_UserPW.Text));
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
            reqMsg.Body = new RequestRoomList()
            {
                userID = user_ID
            };
            reqMsg.Header = new Header()
            {
                MSGID = msgid++,
                MSGTYPE = CONSTANTS.REQ_ROOMLIST,
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
                foreach (ChatRoomForm room in chatGroupForms)
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
            roomList.Clear();
            chatGroupForms.Clear();
            lb_GroupList.Items.Clear();
            SettingControlLocationSignIn();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            int accessRight = 0;
            string roomName = string.Empty;
            string creator = user_ID;
            // 채팅방에 들어갈 회원들 수집
            List<string> tempUsers = new List<string>();

            foreach (object checkeditem in clb_GroupingUser.CheckedItems)
            {
                tempUsers.Add(checkeditem.ToString());
            }
            // 정렬
            tempUsers.Sort();
            string group = string.Join(", ", tempUsers);

            // 채팅방 이름 설정
            if (txt_RoomName.Text.Length == 0)
            {
                string groupName = string.Empty;
                groupName = creator + ", ";
                // 채팅방 이름이 20자가 넘어가면 20자로 자르기
                if ((groupName.Length + group.Length) > 15)
                {
                    groupName = groupName + group;
                    groupName = groupName.Substring(0, 15) + " Room";
                }
                else
                {
                    groupName = groupName + group + " Room";
                }
                roomName = groupName;
            }
            else
            {
                roomName = txt_RoomName.Text;
            }

            // 채팅방 접근 권한 설정
            if (rdo_PublicRoom.Checked)
            {
                accessRight = 1;
            }
            else
            {
                accessRight = 0;
            }

            string msg = accessRight + "&" + roomName + "&" + creator + "&" + group;

            // send room info to server
            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Body = new RequestCreateRoom()
            {
                msg = msg
            };
            reqMsg.Header = new Header()
            {
                MSGID = msgid++,
                MSGTYPE = CONSTANTS.REQ_CREATE_ROOM,
                BODYLEN = (uint)reqMsg.Body.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };
            MessageUtil.Send(stream, reqMsg);

            // 설정 값 초기화
            txt_RoomName.Clear();
            rdo_PublicRoom.Checked = true;
            rdo_PrivateRoom.Checked = false;
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
            lb_UserList.Items.Clear();
            foreach (KeyValuePair<int, string> item in userList)
            {
                if (!lb_UserList.Items.Contains(item.Value) && !user_ID.Equals(item.Value))
                {
                    lb_UserList.Items.Add(item.Value);
                }
            }

            // lb_GroupList

            lb_GroupList.Items.Clear();
            foreach (KeyValuePair<int, Tuple<int, string>> item in roomList)
            {
                // private 방 출력
                if (item.Value.Item1.Equals(0))
                {
                    foreach(KeyValuePair<int, Tuple<int, int, int>> tmp in usersInRoom)
                    {
                        if (tmp.Value.Item1.Equals(item.Key) && userList[tmp.Value.Item2].Equals(user_ID))
                        {
                            lb_GroupList.Items.Add(item.Key);
                            lb_GroupList.Items.Add("비공개");
                            lb_GroupList.Items.Add(item.Value.Item2);

                            string users = string.Empty;
                            foreach (KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
                            {
                                if (temp.Value.Item1.Equals(item.Key))
                                {
                                    users = users + userList[temp.Value.Item2] + ", ";
                                }
                            }
                            users = users.Substring(0, users.LastIndexOf(","));
                            lb_GroupList.Items.Add("채팅방 인원 : " + users);
                        }
                    }
                }
                // public 방 출력
                else
                {
                    lb_GroupList.Items.Add(item.Key);
                    lb_GroupList.Items.Add("공개");
                    lb_GroupList.Items.Add(item.Value.Item2);

                    string users = string.Empty;
                    foreach (KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
                    {
                        if (temp.Value.Item1.Equals(item.Key))
                        {
                            users = users + userList[temp.Value.Item2] + ", ";
                        }
                    }
                    users = users.Substring(0, users.LastIndexOf(","));
                    lb_GroupList.Items.Add("채팅방 인원 : " + users);
                }
            }
            // window를 비활성화하여 WM_PAINT call
            // true 배경을 지우고 다시 그린다
            // false 현 배경 위에 다시 그린다
            Invalidate(false);
        }

        private void lb_GroupList_DoubleClick(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            int index = lb.SelectedIndex - (lb.SelectedIndex % 4);
            int roomNo = int.Parse(lb.Items[index].ToString());
            int userNo = 0;
            int managerRight = 0;

            foreach (var temp in userList)
            {
                if (temp.Value.Equals(user_ID))
                {
                    userNo = temp.Key;
                    break;
                }
            }

            // usersInRoom에서 회원의 권한 검색 where roomNo, userNo
            foreach (KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
            {
                if (temp.Value.Item1.Equals(roomNo) && temp.Value.Item2.Equals(userNo))
                {
                    managerRight = temp.Value.Item3;
                }
            }

            if (lb_GroupList.SelectedItems.Count == 1)
            {
                // 일반 회원
                if (managerRight == 0)
                {
                    Open_ChatGroup(sender);
                }
                // 관리자
                else if (managerRight == 1)
                {
                    Open_ChatGroup_Manager(sender);
                }
                // 생성자
                else
                {
                    Open_ChatGroup_Creator(sender);
                }
            }
        }

        // 일반 회원용
        private void Open_ChatGroup(object sender)
        {
            ListBox lb = sender as ListBox;

            int index = lb.SelectedIndex - (lb.SelectedIndex % 4);
            int roomNo = int.Parse(lb.Items[index].ToString());
            string roomName = roomList[roomNo].Item2;

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
            ChatRoomForm chatGroupForm = new ChatRoomForm();
            chatGroupForm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            chatGroupForm.stream = stream;
            chatGroupForm.roomNo = roomNo;
            chatGroupForm.roomName = roomName;
            chatGroupForm.user_ID = user_ID;
            chatGroupForm.Tag = index;
            chatGroupForm.Text = roomName;
            chatGroupForm.Name = "chatGroupForm" + index;
            chatGroupForm.usersInRoom = usersInRoom;
            chatGroupForm.userList = userList;
            chatGroupForm.accessRight = roomList[roomNo].Item1;
            // 사이즈 변경
            chatGroupForm.Size = new Size(350, 600);
            // 버튼 visible 변경
            chatGroupForm.btn_BanishUser.Visible = false;
            chatGroupForm.btn_ChangeRoomConfig.Visible = false;
            chatGroupForm.btn_ManagerConfig.Visible = false;

            // ChatGroupForm이 열렸을 때 Form 정보 저장
            chatGroupForms.Add(chatGroupForm);

            chatGroupForm.Show();
        }

        // 관리자용
        private void Open_ChatGroup_Manager(object sender)
        {
            ListBox lb = sender as ListBox;

            int index = lb.SelectedIndex - (lb.SelectedIndex % 4);
            int roomNo = int.Parse(lb.Items[index].ToString());
            string roomName = roomList[roomNo].Item2;

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
            ChatRoomForm chatGroupForm = new ChatRoomForm();
            chatGroupForm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            chatGroupForm.stream = stream;
            chatGroupForm.roomNo = roomNo;
            chatGroupForm.roomName = roomName;
            chatGroupForm.user_ID = user_ID;
            chatGroupForm.Tag = index;
            chatGroupForm.Text = roomName;
            chatGroupForm.Name = "chatGroupForm" + index;
            chatGroupForm.usersInRoom = usersInRoom;
            chatGroupForm.userList = userList;
            chatGroupForm.accessRight = roomList[roomNo].Item1;

            // 사이즈 변경
            chatGroupForm.Size = new Size(450, 600);
            // 버튼 visible 변경
            chatGroupForm.btn_BanishUser.Visible = true;
            chatGroupForm.btn_ChangeRoomConfig.Visible = true;
            chatGroupForm.btn_ManagerConfig.Visible = false;

            // 회원 추방 기능을 위해 userList를 사용 가능하게 변경
            chatGroupForm.lb_UserList.SelectionMode = SelectionMode.One;

            // ChatGroupForm이 열렸을 때 Form 정보 저장
            chatGroupForms.Add(chatGroupForm);

            chatGroupForm.Show();
        }

        // 생성자용
        private void Open_ChatGroup_Creator(object sender)
        {
            ListBox lb = sender as ListBox;

            int index = lb.SelectedIndex - (lb.SelectedIndex % 4);
            int roomNo = int.Parse(lb.Items[index].ToString());
            string roomName = roomList[roomNo].Item2;

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
            ChatRoomForm chatGroupForm = new ChatRoomForm();
            chatGroupForm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            chatGroupForm.stream = stream;
            chatGroupForm.roomNo = roomNo;
            chatGroupForm.roomName = roomName;
            chatGroupForm.user_ID = user_ID;
            chatGroupForm.Tag = index;
            chatGroupForm.Text = roomName;
            chatGroupForm.Name = "chatGroupForm" + index;
            chatGroupForm.usersInRoom = usersInRoom;
            chatGroupForm.userList = userList;
            chatGroupForm.accessRight = roomList[roomNo].Item1;

            // 사이즈 변경
            chatGroupForm.Size = new Size(450, 600);
            // 버튼 visible 변경
            chatGroupForm.btn_BanishUser.Visible = true;
            chatGroupForm.btn_ChangeRoomConfig.Visible = true;
            chatGroupForm.btn_ManagerConfig.Visible = true;

            // 회원 추방 기능을 위해 userList를 사용 가능하게 변경
            chatGroupForm.lb_UserList.SelectionMode = SelectionMode.One;

            // ChatGroupForm이 열렸을 때 Form 정보 저장
            chatGroupForms.Add(chatGroupForm);

            chatGroupForm.Show();
        }

        private int SearchUserNoByUserID(string userID)
        {
            int userNo = 0;
            foreach (KeyValuePair<int, string> temp in userList)
            {
                if (temp.Value.Equals(userID))
                {
                    userNo = temp.Key;
                    break;
                }
            }
            return userNo;
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

        private void ProgramClose()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    this.Close();
                }));
            }
            else
            {
                this.Close();
            }
        }
    }
}