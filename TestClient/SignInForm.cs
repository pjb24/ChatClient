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

        private void SignInForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Initializer.testClientUI.WindowState == FormWindowState.Minimized)
            {
                Initializer.testClientUI.WindowState = FormWindowState.Normal;
                Initializer.testClientUI.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            }
            Initializer.testClientUI.Activate();
        }

        private void txt_ID_Enter(object sender, EventArgs e)
        {
            if (txt_ID.Text == "ID")
            {
                txt_ID.Text = "";

                txt_ID.ForeColor = Color.Black;
            }
        }

        private void txt_ID_Leave(object sender, EventArgs e)
        {
            if (txt_ID.Text == "")
            {
                txt_ID.Text = "ID";

                txt_ID.ForeColor = Color.Silver;
            }
        }

        private void txt_PW_Enter(object sender, EventArgs e)
        {
            if (txt_PW.Text == "PW")
            {
                txt_PW.Text = "";

                txt_PW.ForeColor = Color.Black;

                txt_PW.PasswordChar = '*';
            }
        }

        private void txt_PW_Leave(object sender, EventArgs e)
        {
            if (txt_PW.Text == "")
            {
                txt_PW.Text = "PW";

                txt_PW.ForeColor = Color.Silver;

                txt_PW.PasswordChar = '\0';
            }
        }
    }
}
