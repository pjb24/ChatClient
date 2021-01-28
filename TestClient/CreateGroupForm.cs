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
    public partial class CreateGroupForm : Form
    {
        public NetworkStream stream = default(NetworkStream);
        public List<string> userList = new List<string>();
        public string user_ID;

        private CheckedListBox chk_GroupUser = new CheckedListBox();

        public CreateGroupForm()
        {
            InitializeComponent();            
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            string sendMsg = null;
            foreach(var checkeditem in chk_GroupUser.CheckedItems)
            {
                sendMsg = sendMsg + (string)checkeditem + "&";
            }
            Console.WriteLine(sendMsg);

            // send group info to server
            sendMsg = sendMsg + user_ID + "&createGroup";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            this.Close();
        }

        private void CreateGroupForm_Load(object sender, EventArgs e)
        {
            // change design
            // chk_GroupUser = new CheckedListBox();
            chk_GroupUser.Location = new Point(100, 100);
            chk_GroupUser.Name = "chk_GroupUser";
            chk_GroupUser.CheckOnClick = true;
            // btn_OpenGroup.Size = new Size(50, 50);
            for (int i = 0; i < userList.Count; i++)
            {
                chk_GroupUser.Items.Add(userList[i]);
            }
            Controls.Add(chk_GroupUser);
        }
    }
}
