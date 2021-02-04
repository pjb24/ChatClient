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

namespace TestClient
{
    public partial class InvitationForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InvitationForm));

        public NetworkStream stream = default(NetworkStream);
        // 열려있는 group의 name
        public string group = string.Empty;
        // 현재 클라이언트의 ID
        public string user_ID = string.Empty;

        public List<string> userList = new List<string>();
        public List<string> groupUserList = new List<string>();

        public InvitationForm()
        {
            InitializeComponent();
        }

        private void btn_Invitation_Click(object sender, EventArgs e)
        {
            string sendMsg = null;
            foreach (object checkeditem in clb_InviteUser.CheckedItems)
            {
                sendMsg = sendMsg + (string)checkeditem + "&";
            }
            Console.WriteLine(sendMsg);

            // send group info to server
            sendMsg = sendMsg + group + "&" + user_ID + "&Invitation";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            this.Close();
        }

        private void btn_InvitationClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InvitationForm_Load(object sender, EventArgs e)
        {
            foreach (string user in userList)
            {
                if (!groupUserList.Contains(user))
                {
                    if (!clb_InviteUser.Items.Contains(user))
                    {
                        clb_InviteUser.Items.Add(user);
                    }
                }
            }
        }
    }
}
