using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Threading;
using System.Configuration;

using System.IO;

using MyMessageProtocol;

namespace TestClient
{
    public partial class LobbyForm : Form
    {
        public TcpClient clientSocket = new TcpClient();
        public NetworkStream stream = default(NetworkStream);

        const int CHUNK_SIZE = 4096;

        Dictionary<int, string> userList = new Dictionary<int, string>();
        Dictionary<int, Tuple<int, string>> roomList = new Dictionary<int, Tuple<int, string>>();
        Dictionary<int, Tuple<int, int, int>> usersInRoom = new Dictionary<int, Tuple<int, int, int>>();

        List<ChatRoomForm> chatGroupForms = new List<ChatRoomForm>();

        bool loop = true;

        public string user_ID = string.Empty;
        public uint msgid = 0;

        public LobbyForm()
        {
            InitializeComponent();
        }

        private void SignInForm_OnSignInSuccess()
        {
            loop = true;
            Thread t_handler = new Thread(() => GetMessage(stream, clientSocket));
            t_handler.IsBackground = true;
            t_handler.Start();

            Pull_UserList();
            Pull_RoomList();
        }

        private int Pull_UserList()
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
            return 0;
        }

        private int Pull_RoomList()
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
            return 0;
        }

        private void btn_CreateRoom_Click(object sender, EventArgs e)
        {
            this.Hide();
            CreateRoomForm createRoomForm = new CreateRoomForm();
            createRoomForm.user_ID = user_ID;
            createRoomForm.userList = userList;
            createRoomForm.OnSubmitCreateRoom += CreateRoomForm_OnSubmitCreateRoom;
            createRoomForm.Location = this.Location;
            createRoomForm.Show();
        }

        private void CreateRoomForm_OnSubmitCreateRoom(string msg)
        {
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

            loop = false;
            user_ID = string.Empty;
            userList.Clear();
            roomList.Clear();
            usersInRoom.Clear();
            chatGroupForms.Clear();
            this.Hide();
            GlobalClass.signInForm.OnSignInSuccess += SignInForm_OnSignInSuccess;
            GlobalClass.signInForm.Location = this.Location;
            GlobalClass.signInForm.Show();
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

            lb_RoomList.Items.Clear();
            foreach (KeyValuePair<int, Tuple<int, string>> item in roomList)
            {
                // private 방 출력
                if (item.Value.Item1.Equals(0))
                {
                    foreach (KeyValuePair<int, Tuple<int, int, int>> tmp in usersInRoom)
                    {
                        if (tmp.Value.Item1.Equals(item.Key) && userList[tmp.Value.Item2].Equals(user_ID))
                        {
                            lb_RoomList.Items.Add(item.Key);
                            lb_RoomList.Items.Add("비공개");
                            lb_RoomList.Items.Add(item.Value.Item2);

                            string users = string.Empty;
                            foreach (KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
                            {
                                if (temp.Value.Item1.Equals(item.Key))
                                {
                                    users = users + userList[temp.Value.Item2] + ", ";
                                }
                            }
                            users = users.Substring(0, users.LastIndexOf(","));
                            lb_RoomList.Items.Add("채팅방 인원 : " + users);
                        }
                    }
                }
                // public 방 출력
                else
                {
                    lb_RoomList.Items.Add(item.Key);
                    lb_RoomList.Items.Add("공개");
                    lb_RoomList.Items.Add(item.Value.Item2);

                    string users = string.Empty;
                    foreach (KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
                    {
                        if (temp.Value.Item1.Equals(item.Key))
                        {
                            users = users + userList[temp.Value.Item2] + ", ";
                        }
                    }
                    users = users.Substring(0, users.LastIndexOf(","));
                    lb_RoomList.Items.Add("채팅방 인원 : " + users);
                }
            }
            // window를 비활성화하여 WM_PAINT call
            // true 배경을 지우고 다시 그린다
            // false 현 배경 위에 다시 그린다
            Invalidate(false);
        }

        private void GroupRefresh()
        {
            // 크로스스레드가 발생할 때
            if (lb_RoomList.InvokeRequired)
            {
                // BeginInvoke - 비동기식 대리자 실행
                // 익명함수? 익명대리자?
                lb_RoomList.BeginInvoke(new MethodInvoker(delegate
                {
                    DesignGroup();
                }));
            }
            else
                DesignGroup();
        }

        private void GetMessage(NetworkStream stream, TcpClient clientSocket)
        {
            if (loop == false)
            {
                stream.Close();
                clientSocket.Close();
            }
            while (loop)
            {
                try
                {
                    if (stream == null)
                    {
                        return;
                    }
                    PacketMessage message = MessageUtil.Receive(stream);
                    if (message != null)
                    {
                        switch (message.Header.MSGTYPE)
                        {
                            // 회원가입 성공
                            case CONSTANTS.RES_REGISTER_SUCCESS:
                                {
                                    ResponseRegisterSuccess resBody = (ResponseRegisterSuccess)message.Body;

                                    userList.Add(resBody.No, resBody.userID);
                                    GroupRefresh();
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
                                        foreach (KeyValuePair<int, Tuple<int, int, int>> temp in resBody.usersInRoom)
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
                                    break;
                                }
                            // 채팅방 초대 완료
                            case CONSTANTS.RES_INVITATION_SUCCESS:
                                {
                                    ResponseInvitationSuccess resBody = (ResponseInvitationSuccess)message.Body;

                                    int userNo = 0;
                                    userNo = SearchUserNoByUserID(user_ID);

                                    bool invited = false;
                                    foreach (KeyValuePair<int, Tuple<int, int, int>> temp in resBody.usersInRoom)
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
                                        Pull_RoomList();
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
                                                    foreach (KeyValuePair<int, string> tmp in userList)
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
                                    if (user_ID.Equals(resBody.userID))
                                    {
                                        // roomList 제거
                                        roomList.Remove(resBody.roomNo);

                                        // 회원 번호 검색
                                        SearchUserNoByUserID(resBody.userID);

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
                                        SearchUserNoByUserID(resBody.userID);

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
                                                chatGroupForm.DisplayText(resBody.userID + "님이 채팅방에서 나가셨습니다.");
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
                                    if (user_ID.Equals(resBody.banishedUser))
                                    {
                                        // roomList 제거
                                        roomList.Remove(resBody.roomNo);

                                        // 회원 번호 검색
                                        userNo = SearchUserNoByUserID(resBody.banishedUser);

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
                                        userNo = SearchUserNoByUserID(resBody.banishedUser);

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
                                                chatGroupForm.DisplayText(resBody.banishedUser + "님이 채팅방에서 추방되었습니다.");
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
                                                temp.DisplayText(reqBody.userID + " : " + fileName + " 파일을 전송했습니다.");
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
                    loop = false;
                    MessageBox.Show("서버에서 연결을 종료했습니다.\n 프로그램을 다시 실행해주십시오.", "알림");
                    this.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    MessageBox.Show(e.StackTrace);
                }
            }
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

        private void LobbyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void lb_RoomList_DoubleClick(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            int index = lb.SelectedIndex - (lb.SelectedIndex % 4);
            int roomNo = int.Parse(lb.Items[index].ToString());
            int userNo = 0;
            int managerRight = 3;

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

            if (lb_RoomList.SelectedItems.Count == 1)
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
                else if (managerRight == 2)
                {
                    Open_ChatGroup_Creator(sender);
                }
                else if (roomList[roomNo].Item1 == 1)
                {
                    Open_ChatGroup_NonMember(sender);
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

        private void LobbyForm_Load(object sender, EventArgs e)
        {
            Thread t_handler = new Thread(() => GetMessage(stream, clientSocket));
            t_handler.IsBackground = true;
            t_handler.Start();

            Pull_UserList();
            Pull_RoomList();
        }

        // 공개 채팅방 외부 회원용
        private void Open_ChatGroup_NonMember(object sender)
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
            chatGroupForm.btn_Leave.Visible = false;
            chatGroupForm.btn_Invitation.Visible = false;
            chatGroupForm.btn_SendFile.Visible = false;

            // ChatGroupForm이 열렸을 때 Form 정보 저장
            chatGroupForms.Add(chatGroupForm);

            chatGroupForm.Show();
        }
    }
}
