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
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            string user_ID = txt_ID.Text;
            string user_PW = txt_PW.Text;

            string sendMsg = user_ID + "&" + user_PW + "register";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            
            Initializer.stream.Write(buffer, 0, buffer.Length);
            Initializer.stream.Flush();

            txt_ID.Clear();
            txt_PW.Clear();
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
