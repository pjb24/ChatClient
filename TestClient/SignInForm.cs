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
        public NetworkStream stream = default(NetworkStream);

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

            this.Close();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.stream = stream;
            registerForm.Show();
        }
    }
}
