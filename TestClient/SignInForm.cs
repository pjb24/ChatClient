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
using System.Configuration;
using System.Security.Cryptography;

using MyMessageProtocol;

namespace TestClient
{
    public partial class SignInForm : Form
    {
        const int TEXT_MAX_LENGTH = 20;

        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);

        public static uint msgid = 0;

        public SignInForm()
        {
            InitializeComponent();
        }

        private void btn_SignIn_Click(object sender, EventArgs e)
        {
            string user_ID = txt_UserID.Text;

            if (user_ID.Length == 0)
            {
                MessageBox.Show(this, "ID를 입력해주세요", "알림");
                txt_UserID.Focus();
                return;
            }
            if (txt_UserPW.Text.Length == 0)
            {
                MessageBox.Show(this, "비밀번호를 입력해주세요", "알림");
                txt_UserPW.Focus();
                return;
            }

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

            SHA256Managed sHA256Managed = new SHA256Managed();
            byte[] temp = sHA256Managed.ComputeHash(Encoding.Unicode.GetBytes(txt_UserPW.Text));
            string user_PW = string.Join(string.Empty, Array.ConvertAll(temp, b => b.ToString("X2")));

            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Body = new RequestRegister()
            {
                msg = user_ID + "&" + user_PW
            };
            reqMsg.Header = new Header()
            {
                MSGID = msgid++,
                MSGTYPE = CONSTANTS.REQ_REGISTER,
                BODYLEN = (uint)reqMsg.Body.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };
            MessageUtil.Send(stream, reqMsg);
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                RegisterForm registerForm = new RegisterForm();
                registerForm.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void txt_UserID_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (txt_UserID.Text.Length > TEXT_MAX_LENGTH)
            {
                textBox.TextChanged -= txt_UserID_TextChanged;
                textBox.Text = textBox.Text.Substring(0, TEXT_MAX_LENGTH);
                textBox.TextChanged += txt_UserID_TextChanged;
                MessageBox.Show(this, "ID는 20자까지만 허용됩니다.", "알림");
            }
        }

        private void txt_UserPW_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (txt_UserPW.Text.Length > TEXT_MAX_LENGTH)
            {
                textBox.TextChanged -= txt_UserPW_TextChanged;
                textBox.Text = textBox.Text.Substring(0, TEXT_MAX_LENGTH);
                textBox.TextChanged += txt_UserPW_TextChanged;
                MessageBox.Show(this, "비밀번호는 20자까지만 허용됩니다.", "알림");
            }
        }
    }
}
