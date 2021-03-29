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

using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using MyMessageProtocol;

namespace TestClient
{
    public partial class SignInForm : Form
    {
        const int TEXT_MAX_LENGTH = 20;

        public static uint msgid = 0;

        public delegate void SignInSuccessHandler();
        public event SignInSuccessHandler OnSignInSuccess;

        private int dataFormat = 0;

        public SignInForm()
        {
            InitializeComponent();
            GlobalClass.signInForm = this;
        }

        private void btn_SignIn_Click(object sender, EventArgs e)
        {
            string user_ID = string.Empty;
            user_ID = txt_UserID.Text;

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

            TcpClient clientSocket = new TcpClient();
            NetworkStream stream = default(NetworkStream);

            string IP = string.Empty;
            int port = 0;
            IP = ConfigurationManager.AppSettings["IP"];
            port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            dataFormat = int.Parse(ConfigurationManager.AppSettings["dataFormat"]);

            try
            {
                // (serverIP, port) 연결 시도, 현재는 Loopback 사용중
                clientSocket.Connect(IP, port);
                // NetworkStream 정보 저장
                stream = clientSocket.GetStream();
            }
            catch (SocketException se)
            {
                Console.WriteLine(string.Format("clientSocket.Connect - SocketException : {0}", se.StackTrace));
                MessageBox.Show(this, "서버에 접속할 수 없습니다.\n 잠시 후 다시 시도해주세요.", "알림");
            }

            SHA256Managed sHA256Managed = new SHA256Managed();
            byte[] temp = { };
            temp = sHA256Managed.ComputeHash(Encoding.Unicode.GetBytes(txt_UserPW.Text));
            string user_PW = string.Empty;
            user_PW = string.Join(string.Empty, Array.ConvertAll(temp, b => b.ToString("X2")));

            User user = new User()
            {
                UserID = user_ID,
                UserPW = user_PW
            };

            string serialized = string.Empty;
            if (dataFormat == 1)
            {
                serialized = JsonConvert.SerializeObject(user);
            }
            else if (dataFormat == 2)
            {
                ISerializer serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                serialized = serializer.Serialize(user);
            }

            byte[] Key = Cryption.KeyGenerator(msgid.ToString());
            byte[] IV = Cryption.IVGenerator(CONSTANTS.REQ_SIGNIN.ToString());

            string encrypted = string.Empty;
            encrypted = Cryption.EncryptString_Aes(serialized, Key, IV);

            PacketMessage reqMsg = new PacketMessage();
            reqMsg.Body = new RequestRegister()
            {
                msg = encrypted
            };
            reqMsg.Header = new Header()
            {
                MSGID = msgid++,
                MSGTYPE = CONSTANTS.REQ_SIGNIN,
                BODYLEN = (uint)reqMsg.Body.GetSize(),
                FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                LASTMSG = CONSTANTS.LASTMSG,
                SEQ = 0
            };
            MessageUtil.Send(stream, reqMsg);

            GetMessage(stream, clientSocket);
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            txt_UserID.Clear();
            txt_UserPW.Clear();
            this.Hide();
            GlobalClass.registerForm.Location = this.Location;
            GlobalClass.registerForm.Show();
        }

        private void txt_UserID_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = new TextBox();
            textBox = sender as TextBox;

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
            TextBox textBox = new TextBox();
            textBox = sender as TextBox;

            if (txt_UserPW.Text.Length > TEXT_MAX_LENGTH)
            {
                textBox.TextChanged -= txt_UserPW_TextChanged;
                textBox.Text = textBox.Text.Substring(0, TEXT_MAX_LENGTH);
                textBox.TextChanged += txt_UserPW_TextChanged;
                MessageBox.Show(this, "비밀번호는 20자까지만 허용됩니다.", "알림");
            }
        }

        private int GetMessage(NetworkStream stream, TcpClient clientSocket)
        {
            while (true)
            {
                try
                {
                    PacketMessage message = MessageUtil.Receive(stream);
                    if (message != null)
                    {
                        switch (message.Header.MSGTYPE)
                        {
                            case CONSTANTS.RES_SIGNIN_SUCCESS:
                                {
                                    ResponseSignInSuccess resBody = (ResponseSignInSuccess)message.Body;
                                    this.Hide();
                                    GlobalClass.lobbyForm.Location = this.Location;
                                    GlobalClass.lobbyForm.user_ID = txt_UserID.Text;
                                    GlobalClass.lobbyForm.clientSocket = clientSocket;
                                    GlobalClass.lobbyForm.stream = stream;
                                    GlobalClass.lobbyForm.dataFormat = dataFormat;
                                    txt_UserID.Clear();
                                    txt_UserPW.Clear();
                                    GlobalClass.lobbyForm.Show();
                                    if (this.OnSignInSuccess != null)
                                    {
                                        OnSignInSuccess();
                                    }
                                    return 0;
                                }
                            case CONSTANTS.RES_SIGNIN_FAIL_NOT_EXIST:
                                {
                                    stream.Close();
                                    clientSocket.Close();
                                    MessageBox.Show(this, "등록된 회원이 아닙니다.", "로그인 실패");
                                    txt_UserID.Clear();
                                    txt_UserPW.Clear();
                                    return 0;
                                }
                            case CONSTANTS.RES_SIGNIN_FAIL_WRONG_PASSWORD:
                                {
                                    stream.Close();
                                    clientSocket.Close();
                                    MessageBox.Show(this, "잘못된 비밀번호 입니다.", "로그인 실패");
                                    txt_UserID.Clear();
                                    txt_UserPW.Clear();
                                    return 0;
                                }
                            case CONSTANTS.RES_SIGNIN_FAIL_ONLINE_USER:
                                {
                                    stream.Close();
                                    clientSocket.Close();
                                    MessageBox.Show(this, "이미 접속 중인 사용자 입니다.", "로그인 실패");
                                    txt_UserID.Clear();
                                    txt_UserPW.Clear();
                                    return 0;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace);
                }
            }
        }

        private void SignInForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
