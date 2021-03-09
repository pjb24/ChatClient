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
    public partial class CreateRoomForm : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);

        public string user_ID = string.Empty;
        public uint msgid = 0;

        public CreateRoomForm()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Hide();
            LobbyForm lobbyForm = new LobbyForm();
            lobbyForm.ShowDialog();
            this.Close();
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {

        }

        private void CreateRoomForm_Load(object sender, EventArgs e)
        {

        }
    }
}
