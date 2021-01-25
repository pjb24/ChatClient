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
    public partial class GroupForm : Form
    {
        public NetworkStream stream = default(NetworkStream);
        public List<string> userList = new List<string>();
        public List<string> groupList = new List<string>();
        public string user_ID;

        private List<Button> btn_OpenGroup = new List<Button>();

        public GroupForm()
        {
            InitializeComponent();
        }
        
        private void btn_CreateGroup_Click(object sender, EventArgs e)
        {
            CreateGroupForm createGroupForm = new CreateGroupForm();
            createGroupForm.stream = stream;
            createGroupForm.userList = userList;
            createGroupForm.user_ID = user_ID;
            createGroupForm.Show();
        }

        private void btn_OpenGroup_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ChatGroupForm chatGroupForm = new ChatGroupForm();
            chatGroupForm.stream = stream;
            chatGroupForm.group = groupList[(int)btn.Tag];
            chatGroupForm.Show();
        }

        private void btn_SignOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void btn_PullGroup_Click(object sender, EventArgs e)
        {
            // group 동기화 요청
            string msg =  user_ID + "requestGroupList";
            byte[] buffer = Encoding.Unicode.GetBytes(msg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void btn_PullUser_Click(object sender, EventArgs e)
        {
            // user 동기화 요청
            string msg = user_ID + "requestUserList";
            byte[] buffer = Encoding.Unicode.GetBytes(msg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void GroupForm_Load(object sender, EventArgs e)
        {
            // change design
            for (int i = 0; i < groupList.Count; i++)
            {
                btn_OpenGroup.Add(new Button());
                btn_OpenGroup[i].Location = new Point(10 + 10 * i, 10 + 10 * i);
                btn_OpenGroup[i].Name = "btn_OpenGroup" + i.ToString();
                //btn_OpenGroup[i].Size = new Size(50, 50);
                btn_OpenGroup[i].Text = "Group" + i.ToString();
                btn_OpenGroup[i].UseVisualStyleBackColor = true;
                btn_OpenGroup[i].Tag = i;
                btn_OpenGroup[i].Click += new EventHandler(btn_OpenGroup_Click);
                Controls.Add(btn_OpenGroup[i]);
            }
        }
    }
}
