using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace TestClient
{
    public partial class TestClientUI : Form
    {
        public static TestClientUI testClientUI;

        public TestClientUI()
        {
            InitializeComponent();
            testClientUI = this;
        }

        private void TestClient_Load(object sender, EventArgs e)
        {
            string data = "test";
            Thread workerThread = new Thread(new ParameterizedThreadStart(AsynchronousClient.StartClient));
            workerThread.Start(data);

            Thread listenThread = new Thread(SocketListener.StartListening);
            listenThread.Start();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            string data = txt_Send.Text;
            Thread workerThread = new Thread(new ParameterizedThreadStart(AsynchronousClient.StartClient));
            workerThread.Start(data);
        }

        private void txt_Send_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btn_Send_Click(sender, e);
                    txt_Send.SelectAll();
                    break;
            }
        }
    }
}
