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

using log4net;
using MyMessageProtocol;

namespace TestClient
{
    public partial class ChatGroupForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ChatGroupForm));

        const int CHUNK_SIZE = 4096;

        public NetworkStream stream = default(NetworkStream);
        // 열려있는 group의 id
        public long pid = 0;
        // 열려있는 group의 roomName
        public string roomName = string.Empty;
        // 현재 클라이언트의 ID
        public string user_ID = string.Empty;

        // 열려있는 group에 속한 회원목록
        public List<string> groupUserList = new List<string>();
        // 전체 회원목록
        public List<string> userList = new List<string>();

        public ChatGroupForm()
        {
            InitializeComponent();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            if (txt_Send.Text != "")
            {
                string msg = pid + "&" + user_ID + "&" + this.txt_Send.Text;
                PacketMessage reqMsg = new PacketMessage();
                reqMsg.Body = new RequestChat() {
                    msg = msg
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

        private void ChatGroupForm_Load(object sender, EventArgs e)
        {
            RedrawUserList();
        }

        private void btn_Leave_Click(object sender, EventArgs e)
        {
            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Body = new RequestLeaveGroup()
            {
                msg = pid + "&" + user_ID
            };
            reqMsg.Header = new Header()
            {
                MSGID = TestClientUI.msgid++,
                MSGTYPE = CONSTANTS.REQ_LEAVE_GROUP,
                BODYLEN = (uint)reqMsg.Body.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };
            MessageUtil.Send(stream, reqMsg);            

            this.Close();
        }

        private void btn_Invitation_Click(object sender, EventArgs e)
        {
            InvitationForm invitationForm = new InvitationForm
            {
                Location = new Point(this.Location.X + this.Width, this.Location.Y),
                stream = stream,
                pid = pid,
                roomName = roomName,
                user_ID = user_ID,
                userList = userList,
                groupUserList = groupUserList
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

                /*
                using (Stream fileStream = new FileStream(filePath, FileMode.Open))
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

                        fileMsg.Body = new SendFile()
                        {
                            msg = pid + "&^%$#&^%$&^%$" + user_ID + "&^%$#&^%$&^%$" + fileName + "&^%$#&^%$&^%$" + fileSize + "&^%$#&^%$&^%$" + Encoding.Unicode.GetString(sendBytes)
                        };
                        fileMsg.Header = new Header()
                        {
                            MSGID = TestClientUI.msgid,
                            MSGTYPE = CONSTANTS.SEND_FILE,
                            BODYLEN = (uint)fileMsg.Body.GetSize(),
                            FRAGMENTED = fragmented,
                            LASTMSG = (totalRead < fileStream.Length) ? CONSTANTS.NOT_LASTMSG : CONSTANTS.LASTMSG,
                            SEQ = msgSeq++
                        };

                        // 모든 파일의 내용이 전송될 때까지 파일 스트림을 0x03 메시지에 담아 서버로 보냄
                        MessageUtil.Send(stream, fileMsg);
                    }
                }
                */

                
                Console.WriteLine(fileSize);
                Console.WriteLine(fileName);

                PacketMessage reqMsg = new PacketMessage();
                reqMsg.Body = new RequestSendFile()
                {
                    msg = pid + "&" + user_ID + "&" + fileSize + "&" + fileName + "&" + filePath
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

        public void RedrawUserList()
        {
            if (lb_UserList.InvokeRequired)
            {
                lb_UserList.BeginInvoke(new MethodInvoker(delegate
                {
                    lb_UserList.Items.Clear();
                    foreach (string user in groupUserList)
                    {
                        lb_UserList.Items.Add(user);
                    }
                }));
            }
            else
            {
                lb_UserList.Items.Clear();
                foreach (string user in groupUserList)
                {
                    lb_UserList.Items.Add(user);
                }
            }
            
        }
    }
}
