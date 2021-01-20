using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestClient
{
    public partial class TestClientUI : Form
    {
        AsynchronousClient socket = new AsynchronousClient();
        public static TestClientUI testClientUI;
        public TestClientUI()
        {
            InitializeComponent();
            testClientUI = this;
            
        }

        private void TestClient_Load(object sender, EventArgs e)
        {
            
            socket.BeginStartClient(socket.StartClientCallback, socket);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            socket.BeginConnect(AsynchronousClient.ConnectCallback, socket);
            string text = txt_Send.Text;
            socket.BeginSend(socket, text, AsynchronousClient.SendCallback, socket);
            
        }
    }
}
