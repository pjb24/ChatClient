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
    public partial class ChatGroupForm : Form
    {
        public NetworkStream stream = default(NetworkStream);
        public string group;
        public string user_ID;

        public ChatGroupForm()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            string sendMsg = this.txt_Send.Text + "&" + group + "&" + user_ID + "&groupChat";

            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void txt_Send_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btn_Send_Click(sender, e);
                    txt_Send.SelectAll();
                    break;
                default:
                    break;
            }
        }

        // 크로스스레드 문제로 사용함
        public void DisplayText(string text)
        {
            if (lb_Result.InvokeRequired)
            {
                lb_Result.BeginInvoke(new MethodInvoker(delegate
                {
                    lb_Result.Items.Add(text + Environment.NewLine);
                }));
            }
            else
                lb_Result.Items.Add(text + Environment.NewLine);
        }
    }
}
