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
    public partial class SignInForm : Form
    {
        public TestClientUI testClientUI = new TestClientUI();
        public NetworkStream stream = default(NetworkStream);
        public RegisterForm registerForm = new RegisterForm();

        public SignInForm()
        {
            InitializeComponent();
        }

        private void btn_SignIn_Click(object sender, EventArgs e)
        {
            string user_ID = txt_ID.Text;
            string user_PW = txt_PW.Text;

            string sendMsg = user_ID + "&" + user_PW + "signin";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {            
            if (testClientUI.WindowState == FormWindowState.Minimized)
            {
                testClientUI.WindowState = FormWindowState.Normal;
                testClientUI.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            }
            testClientUI.Activate();
            
            this.Close();
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            registerForm.stream = stream;
            registerForm.Show();
        }

        private void SignInForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
