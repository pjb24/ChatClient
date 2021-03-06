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
using System.IO;
using System.Security.Cryptography;

using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using log4net;
using MyMessageProtocol;

namespace TestClient
{
    public partial class ChatRoomForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ChatRoomForm));

        const int CHUNK_SIZE = 4096;

        public NetworkStream stream = default(NetworkStream);
        // 열려있는 room의 No
        public int roomNo = 0;
        // 열려있는 room의 roomName
        public string roomName = string.Empty;
        // 열려있는 room의 accessRight
        public int accessRight = 0;
        // 현재 클라이언트의 ID
        public string user_ID = string.Empty;

        // 열려있는 room에 속한 회원목록
        public Dictionary<int, Tuple<int, int, int>> usersInRoom = new Dictionary<int, Tuple<int, int, int>>();
        // 전체 회원목록
        public Dictionary<int, string> userList = new Dictionary<int, string>();
        public int dataFormat = 0;

        public ChatRoomForm()
        {
            InitializeComponent();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            if (txt_Send.Text != "")
            {
                Chat chat = new Chat()
                {
                    RoomNo = roomNo,
                    UserID = user_ID,
                    ChatMsg = this.txt_Send.Text
                };

                string serialized = string.Empty;
                if (dataFormat == 1)
                {
                    serialized = JsonConvert.SerializeObject(chat);
                }
                else if (dataFormat == 2)
                {
                    ISerializer serializer = new SerializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
                    serialized = serializer.Serialize(chat);
                }

                byte[] Key = Cryption.KeyGenerator(TestClientUI.msgid.ToString());
                byte[] IV = Cryption.IVGenerator(CONSTANTS.REQ_CHAT.ToString());

                string encrypted = string.Empty;
                encrypted = Cryption.EncryptString_Aes(serialized, Key, IV);

                PacketMessage reqMsg = new PacketMessage();
                reqMsg.Body = new RequestChat() {
                    msg = encrypted
                };
                
                reqMsg.Header = new Header()
                {
                    MSGID = TestClientUI.msgid++,
                    MSGTYPE = CONSTANTS.REQ_CHAT,
                    BODYLEN = (uint)reqMsg.Body.GetSize(),
                    FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                    LASTMSG = CONSTANTS.LASTMSG,
                    SEQ = 0
                };
                MessageUtil.Send(stream, reqMsg);
            }
            txt_Send.Clear();
            txt_Send.Focus();
        }

        private void txt_Send_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (!((e.Modifiers & Keys.Shift) == Keys.Shift))
                    {
                        if (txt_Send.Text.Trim('\r', '\n') != "")
                        {
                            e.SuppressKeyPress = true;
                            btn_Send_Click(sender, e);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        // 크로스스레드 문제로 사용
        // TestClientUI에서 호출함, 호출 위치 변경 생각해볼것
        public void DisplayText(string text)
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

        private void ChatGroupForm_Load(object sender, EventArgs e)
        {
            RedrawUserList();
        }

        private void btn_Leave_Click(object sender, EventArgs e)
        {
            if (btn_ManagerConfig.Visible && userList.Count != 0)
            {
                MessageBox.Show("채팅방 생성자는 회원이 남아있을 경우 채팅방을 나갈 수 없습니다.", "알림");
                return;
            }
            if (MessageBox.Show("채팅방에서 나가시겠습니까?", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Relation relation = new Relation()
                {
                    RoomNo = roomNo,
                    UserNo = SearchUserNoByUserID(user_ID)
                };

                string serialized = string.Empty;
                if (dataFormat == 1)
                {
                    serialized = JsonConvert.SerializeObject(relation);
                }
                else if (dataFormat == 2)
                {
                    ISerializer serializer = new SerializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
                    serialized = serializer.Serialize(relation);
                }

                byte[] Key = Cryption.KeyGenerator(TestClientUI.msgid.ToString());
                byte[] IV = Cryption.IVGenerator(CONSTANTS.REQ_LEAVE_ROOM.ToString());

                string encrypted = string.Empty;
                encrypted = Cryption.EncryptString_Aes(serialized, Key, IV);

                PacketMessage reqMsg = new PacketMessage();
                reqMsg.Body = new RequestLeaveRoom()
                {
                    msg = encrypted
                };
                reqMsg.Header = new Header()
                {
                    MSGID = TestClientUI.msgid++,
                    MSGTYPE = CONSTANTS.REQ_LEAVE_ROOM,
                    BODYLEN = (uint)reqMsg.Body.GetSize(),
                    FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                    LASTMSG = CONSTANTS.LASTMSG,
                    SEQ = 0
                };
                MessageUtil.Send(stream, reqMsg);

                this.Close();
            }
        }

        private void btn_Invitation_Click(object sender, EventArgs e)
        {
            List<string> roomUserList = new List<string>();
            foreach(KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
            {
                if (temp.Value.Item1.Equals(roomNo))
                {
                    roomUserList.Add(userList[temp.Value.Item2]);
                }
            }
            InvitationForm invitationForm = new InvitationForm
            {
                Location = new Point(this.Location.X + this.Width, this.Location.Y),
                stream = stream,
                roomNo = roomNo,
                roomName = roomName,
                user_ID = user_ID,
                userList = userList,
                roomUserList = roomUserList,
                dataFormat = dataFormat
            };
            invitationForm.ShowDialog();
        }

        private void btn_SendFile_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            openFileDialog1.InitialDirectory = "C:\\";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;

                string[] p = filePath.Split('\\');
                long fileSize = new FileInfo(filePath).Length;
                string fileName = p[p.Count() - 1];

                if (openFileDialog1.FileName.Length == 0)
                {
                    MessageBox.Show("파일을 선택해주십시오.", "알림");
                    btn_SendFile_Click(sender, e);
                }

                if (MessageBox.Show(fileName + "파일을 전송하시겠습니까?", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Relation relation = new Relation()
                    {
                        RoomNo = roomNo,
                        UserNo = SearchUserNoByUserID(user_ID)
                    };
                    MyMessageProtocol.File file = new MyMessageProtocol.File()
                    {
                        Size = fileSize,
                        Name = fileName,
                        Path = filePath,
                        Relation = relation
                    };

                    string serialized = string.Empty;
                    if (dataFormat == 1)
                    {
                        serialized = JsonConvert.SerializeObject(file);
                    }
                    else if (dataFormat == 2)
                    {
                        ISerializer serializer = new SerializerBuilder()
                            .WithNamingConvention(CamelCaseNamingConvention.Instance)
                            .Build();
                        serialized = serializer.Serialize(file);
                    }

                    byte[] Key = Cryption.KeyGenerator(TestClientUI.msgid.ToString());
                    byte[] IV = Cryption.IVGenerator(CONSTANTS.REQ_SEND_FILE.ToString());

                    string encrypted = string.Empty;
                    encrypted = Cryption.EncryptString_Aes(serialized, Key, IV);

                    PacketMessage reqMsg = new PacketMessage();
                    reqMsg.Body = new RequestSendFile()
                    {
                        msg = encrypted
                    };
                    reqMsg.Header = new Header()
                    {
                        MSGID = TestClientUI.msgid++,
                        MSGTYPE = CONSTANTS.REQ_SEND_FILE,
                        BODYLEN = (uint)reqMsg.Body.GetSize(),
                        FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                        LASTMSG = CONSTANTS.LASTMSG,
                        SEQ = 0
                    };

                    MessageUtil.Send(stream, reqMsg);
                }
            }
        }

        public void RedrawUserList()
        {
            if (lb_UserList.InvokeRequired)
            {
                lb_UserList.BeginInvoke(new MethodInvoker(delegate
                {
                    lb_UserList.Items.Clear();
                    foreach (KeyValuePair<int, Tuple<int, int, int>> user in usersInRoom)
                    {
                        if (user.Value.Item1.Equals(roomNo))
                        {
                            lb_UserList.Items.Add(userList[user.Value.Item2]);
                        }
                    }
                    this.Text = roomName;
                }));
            }
            else
            {
                lb_UserList.Items.Clear();
                foreach (KeyValuePair<int, Tuple<int, int, int>> user in usersInRoom)
                {
                    if (user.Value.Item1.Equals(roomNo))
                    {
                        lb_UserList.Items.Add(userList[user.Value.Item2]);
                    }
                }
                this.Text = roomName;
            }
            
        }

        private void btn_banishUser_Click(object sender, EventArgs e)
        {
            bool isCommonUser = false;
            string banishedUser = string.Empty;
            banishedUser = lb_UserList.SelectedItem.ToString();
            
            if (banishedUser.Length == 0)
            {
                return;
            }

            foreach(KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
            {
                if (temp.Value.Item1.Equals(roomNo) && temp.Value.Item2.Equals(SearchUserNoByUserID(banishedUser)) && temp.Value.Item3.Equals(0))
                {
                    isCommonUser = true;
                }
            }

            if (isCommonUser)
            {
                if (MessageBox.Show(this, banishedUser + " 회원을 추방하시겠습니까?", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Relation relation = new Relation()
                    {
                        RoomNo = roomNo,
                        UserNo = SearchUserNoByUserID(banishedUser)
                    };

                    string serialized = string.Empty;
                    if (dataFormat == 1)
                    {
                        serialized = JsonConvert.SerializeObject(relation);
                    }
                    else if (dataFormat == 2)
                    {
                        ISerializer serializer = new SerializerBuilder()
                            .WithNamingConvention(CamelCaseNamingConvention.Instance)
                            .Build();
                        serialized = serializer.Serialize(relation);
                    }

                    byte[] Key = Cryption.KeyGenerator(TestClientUI.msgid.ToString());
                    byte[] IV = Cryption.IVGenerator(CONSTANTS.REQ_BANISH_USER.ToString());

                    string encrypted = string.Empty;
                    encrypted = Cryption.EncryptString_Aes(serialized, Key, IV);

                    PacketMessage reqMsg = new PacketMessage();
                    reqMsg.Body = new RequestBanishUser()
                    {
                        msg = encrypted
                    };
                    reqMsg.Header = new Header()
                    {
                        MSGID = TestClientUI.msgid++,
                        MSGTYPE = CONSTANTS.REQ_BANISH_USER,
                        BODYLEN = (uint)reqMsg.Body.GetSize(),
                        FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                        LASTMSG = CONSTANTS.LASTMSG,
                        SEQ = 0
                    };

                    MessageUtil.Send(stream, reqMsg);
                } 
            }
            else
            {
                MessageBox.Show(this, "일반 회원만 추방할 수 있습니다.", "알림");
            }
        }

        private void btn_changeRoomConfig_Click(object sender, EventArgs e)
        {
            RoomConfig roomConfig = new RoomConfig();
            roomConfig.roomName = roomName;
            roomConfig.txt_RoomName.Text = roomName;
            roomConfig.accessRight = accessRight;
            if (accessRight == 0)
            {
                roomConfig.rdo_PrivateRoom.Checked = true;
            }
            else
            {
                roomConfig.rdo_PublicRoom.Checked = true;
            }
            roomConfig.OnChangeRoomConfig += new RoomConfig.ChangeRoomConfigHandler(ChangeRoomConfig);
            roomConfig.ShowDialog();
        }

        private void ChangeRoomConfig(int accessRight, string roomName)
        {
            // 채팅방 설정 변경 메시지 전송
            if (!accessRight.Equals(this.accessRight) || !roomName.Equals(this.roomName))
            {
                Room room = new Room()
                {
                    No = roomNo,
                    AccessRight = accessRight,
                    Name = roomName
                };

                string serialized = string.Empty;
                if (dataFormat == 1)
                {
                    serialized = JsonConvert.SerializeObject(room);
                }
                else if (dataFormat == 2)
                {
                    ISerializer serializer = new SerializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
                    serialized = serializer.Serialize(room);
                }

                byte[] Key = Cryption.KeyGenerator(TestClientUI.msgid.ToString());
                byte[] IV = Cryption.IVGenerator(CONSTANTS.REQ_CHANGE_ROOM_CONFIG.ToString());

                string encrypted = string.Empty;
                encrypted = Cryption.EncryptString_Aes(serialized, Key, IV);

                PacketMessage reqMsg = new PacketMessage();
                reqMsg.Body = new RequestChangeRoomConfig()
                {
                    msg = encrypted
                };
                reqMsg.Header = new Header()
                {
                    MSGID = TestClientUI.msgid++,
                    MSGTYPE = CONSTANTS.REQ_CHANGE_ROOM_CONFIG,
                    BODYLEN = (uint)reqMsg.Body.GetSize(),
                    FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                    LASTMSG = CONSTANTS.LASTMSG,
                    SEQ = 0
                };

                MessageUtil.Send(stream, reqMsg);
            }
        }

        private void btn_managerConfig_Click(object sender, EventArgs e)
        {
            RoomManagerConfig roomManagerConfig = new RoomManagerConfig();
            roomManagerConfig.roomNo = roomNo;
            roomManagerConfig.usersInRoom = usersInRoom;
            roomManagerConfig.userList = userList;

            roomManagerConfig.OnChangeManagerConfig += new RoomManagerConfig.ChangeManagerConfigHandler(ChangeManagerConfig);

            roomManagerConfig.ShowDialog();
        }

        private void ChangeManagerConfig(List<int> changedUser)
        {
            // 관리자 권한 변경 메시지 전송
            PacketMessage reqMsg = new PacketMessage();

            Room room = new Room()
            {
                No = roomNo
            };
            List<Relation> relations = new List<Relation>();
            foreach (int temp in changedUser)
            {
                Relation relation = new Relation()
                {
                    UserNo = temp
                };
                relations.Add(relation);
            }
            room.Relation = relations;

            string serialized = string.Empty;
            if (dataFormat == 1)
            {
                serialized = JsonConvert.SerializeObject(room);
            }
            else if (dataFormat == 2)
            {
                ISerializer serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                serialized = serializer.Serialize(room);
            }

            byte[] Key = Cryption.KeyGenerator(TestClientUI.msgid.ToString());
            byte[] IV = Cryption.IVGenerator(CONSTANTS.REQ_CHANGE_MANAGEMENT_RIGHTS.ToString());

            string encrypted = string.Empty;
            encrypted = Cryption.EncryptString_Aes(serialized, Key, IV);

            reqMsg.Body = new RequestChangeManagementRights()
            {
                msg = encrypted
            };
            reqMsg.Header = new Header()
            {
                MSGID = TestClientUI.msgid++,
                MSGTYPE = CONSTANTS.REQ_CHANGE_MANAGEMENT_RIGHTS,
                BODYLEN = (uint)reqMsg.Body.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };

            MessageUtil.Send(stream, reqMsg);
        }
    }
}
