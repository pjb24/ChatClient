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

using log4net;
using MyMessageProtocol;

namespace TestClient
{
    public partial class InvitationForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InvitationForm));

        public NetworkStream stream = default(NetworkStream);
        // 열려있는 room의 pid
        public int roomNo = 0;
        // 열려있는 room의 roomName
        public string roomName = string.Empty;
        // 현재 클라이언트의 ID
        public string user_ID = string.Empty;

        // 열려있는 room에 속한 회원 목록
        public List<string> roomUserList = new List<string>();
        // 전체 회원 목록
        public Dictionary<int, string> userList = new Dictionary<int, string>();

        public InvitationForm()
        {
            InitializeComponent();
        }

        private void btn_Invitation_Click(object sender, EventArgs e)
        {
            List<string> usersInGroup = new List<string>();
            // 채팅방에 들어갈 회원 수집
            foreach (object checkeditem in clb_InviteUser.CheckedItems)
            {
                usersInGroup.Add(checkeditem.ToString());
            }

            string group = string.Join(", ", usersInGroup);

            // send group info to server
            string msg = roomNo + "&" + group;

            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Body = new RequestInvitation()
            {
                msg = msg
            };
            reqMsg.Header = new Header()
            {
                MSGID = TestClientUI.msgid++,
                MSGTYPE = CONSTANTS.REQ_INVITATION,
                BODYLEN = (uint)reqMsg.Body.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };

            MessageUtil.Send(stream, reqMsg);

            this.Close();
        }

        private void btn_InvitationClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InvitationForm_Load(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, string> user in userList)
            {
                // 현재 room에 없고 checkedListBox에 표시되지 않음
                if (!roomUserList.Contains(user.Value))
                {
                    if (!clb_InviteUser.Items.Contains(user.Value))
                    {
                        clb_InviteUser.Items.Add(user.Value);
                    }
                }
            }
        }
    }
}
