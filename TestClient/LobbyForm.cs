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
using System.Threading;
using System.Configuration;

using MyMessageProtocol;

namespace TestClient
{
    public partial class LobbyForm : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);

        public string user_ID = string.Empty;
        public uint msgid = 0;

        public LobbyForm()
        {
            InitializeComponent();

            string IP = ConfigurationManager.AppSettings["IP"];
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);

            try
            {
                // (serverIP, port) 연결 시도, 현재는 Loopback 사용중, Exception 처리 필요
                clientSocket.Connect(IP, port);
                // NetworkStream 정보 저장, NetworkStream?
                stream = clientSocket.GetStream();
            }
            catch (SocketException se)
            {
                Console.WriteLine(string.Format("clientSocket.Connect - SocketException : {0}", se.StackTrace));
                MessageBox.Show(this, "서버에 접속할 수 없습니다.\n 잠시 후 다시 시도해주세요.", "알림");
            }
        }

        private void Pull_UserList()
        {
            
        }

        private void Pull_RoomList()
        {
            
        }

        private void btn_CreateRoom_Click(object sender, EventArgs e)
        {
            this.Hide();
            CreateRoomForm createRoomForm = new CreateRoomForm();
            createRoomForm.ShowDialog();
            this.Close();
        }

        private void btn_SignOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignInForm signInForm = new SignInForm();
            signInForm.ShowDialog();
            this.Close();
        }
    }
}
