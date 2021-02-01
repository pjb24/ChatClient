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
    public partial class ChatGroupForm : Form
    {
        // public NetworkStream stream = default(NetworkStream);
        // 열려있는 group의 name
        public string group = string.Empty;
        // 현재 클라이언트의 ID
        // public string user_ID = string.Empty;

        public ChatGroupForm()
        {
            InitializeComponent();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            string sendMsg = this.txt_Send.Text + "&" + group + "&" + Initializer.user_ID + "&groupChat";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            Initializer.stream.Write(buffer, 0, buffer.Length);
            Initializer.stream.Flush();

            txt_Send.Clear();
            txt_Send.Focus();
        }

        private void txt_Send_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btn_Send_Click(sender, e);
                    break;
                default:
                    break;
            }
        }

        // 크로스스레드 문제로 사용
        // TestClientUI에서 호출함, 호출 위치 변경 생각해볼것
        public void DisplayText(string text)
        {
            if (lb_Result.InvokeRequired)
            {
                lb_Result.BeginInvoke(new MethodInvoker(delegate
                {
                    lb_Result.Items.Add(text + Environment.NewLine);
                    lb_Result.SelectedIndex = lb_Result.Items.Count - 1;
                }));
            }
            else
            {
                lb_Result.Items.Add(text + Environment.NewLine);
                lb_Result.SelectedIndex = lb_Result.Items.Count - 1;
            }
        }

        private void ChatGroupForm_Load(object sender, EventArgs e)
        {

        }
    }
}
