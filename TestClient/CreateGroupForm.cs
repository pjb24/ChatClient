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

        public CreateGroupForm()
        {
            InitializeComponent();
            // pull users
            /*
            string msg = "request freinds" + user_ID;
            byte[] buffer = Encoding.Unicode.GetBytes(msg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();*/

            // change design

        }

        private void CreateGroupForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            // send group info to server
            /*
            string user_Info = user_ID + "createGroup";

            byte[] buffer = Encoding.Unicode.GetBytes(user_Info + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            */

            this.Close();
        }
    }
}
