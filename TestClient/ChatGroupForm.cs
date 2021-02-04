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

namespace TestClient
{
    public partial class ChatGroupForm : Form
    {
        public NetworkStream stream = default(NetworkStream);
        // 열려있는 group의 name
        public string group = string.Empty;
        // 현재 클라이언트의 ID
        public string user_ID = string.Empty;

        public List<string> groupUserList = new List<string>();
        public List<string> userList = new List<string>();

        public ChatGroupForm()
        {
            InitializeComponent();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            if (txt_Send.Text != "")
            {
                string sendMsg = this.txt_Send.Text + "&" + group + "&" + user_ID + "&groupChat";

                byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
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
            string sendMsg = group + "&" + user_ID + "&LeaveGroup";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            this.Close();
        }

        private void btn_Invitation_Click(object sender, EventArgs e)
        {
            InvitationForm invitationForm = new InvitationForm
            {
                Location = new Point(this.Location.X + this.Width, this.Location.Y),
                stream = stream,
                group = group,
                user_ID = user_ID,
                userList = userList,
                groupUserList = groupUserList
            };
            invitationForm.ShowDialog();
        }

        private void btn_SendFile_Click(object sender, EventArgs e)
        {

        }

        public void RedrawUserList()
        {
            if (lb_UserList.InvokeRequired)
            {
                lb_UserList.BeginInvoke(new MethodInvoker(delegate
                {
                    lb_UserList.Items.Clear();
                    foreach (var temp in groupUserList)
                    {
                        lb_UserList.Items.Add(temp);
                    }
                }));
            }
            else
            {
                lb_UserList.Items.Clear();
                foreach (var temp in groupUserList)
                {
                    lb_UserList.Items.Add(temp);
                }
            }
            
        }
    }
}
