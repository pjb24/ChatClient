﻿using System;
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
        public NetworkStream stream = default(NetworkStream);

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

            string user_Info = user_ID + "register" + user_PW;

            byte[] buffer = Encoding.Unicode.GetBytes(user_Info + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            this.Close();
        }
    }
}
