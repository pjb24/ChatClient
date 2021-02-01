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
            Initializer.stream.Write(buffer, 0, buffer.Length);
            Initializer.stream.Flush();

            txt_ID.Clear();
            txt_PW.Clear();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {            
            if (Initializer.testClientUI.WindowState == FormWindowState.Minimized)
            {
                Initializer.testClientUI.WindowState = FormWindowState.Normal;
                Initializer.testClientUI.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            }
            Initializer.testClientUI.Activate();
            
            this.Close();
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            try
            {
                RegisterForm registerForm = new RegisterForm();
                registerForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SignInForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
