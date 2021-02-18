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
        /*
        public NetworkStream stream = default(NetworkStream);
        public List<string> userList = new List<string>();
        public string user_ID;
        */

        // private CheckedListBox chk_GroupUser = new CheckedListBox();

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
            foreach(object checkeditem in clb_GroupUser.CheckedItems)
            {
                sendMsg = sendMsg + (string)checkeditem + "&";
            }
            Console.WriteLine(sendMsg);

            // send group info to server
            sendMsg = sendMsg + Initializer.user_ID + "&createGroup";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            Initializer.stream.Write(buffer, 0, buffer.Length);
            Initializer.stream.Flush();

            this.Close();
        }

        private void CreateGroupForm_Load(object sender, EventArgs e)
        {
            // change design
            // chk_GroupUser = new CheckedListBox();
            // clb_GroupUser.Location = new Point(10, 10);
            // clb_GroupUser.Name = "clb_GroupUser";
            // clb_GroupUser.CheckOnClick = true;
            // btn_OpenGroup.Size = new Size(50, 50);
            for (int i = 0; i < Initializer.userList.Count; i++)
            {
                clb_GroupUser.Items.Add(Initializer.userList[i]);
            }
            // Controls.Add(clb_GroupUser);
        }
    }
}
