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

        public GroupForm()
        {
            InitializeComponent();
            // pull Group

            // change design
        }

        private void btn_CreateGroup_Click(object sender, EventArgs e)
        {
            CreateGroupForm createGroupForm = new CreateGroupForm();
            createGroupForm.stream = stream;
            createGroupForm.Show();
        }

        private void btn_SignOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // group click -> open group chatting room
    }
}
