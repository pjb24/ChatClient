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
        public static TestClientUI testClientUI;
        public TestClientUI()
        {
            InitializeComponent();
            testClientUI = this;
        }

        private void TestClient_Load(object sender, EventArgs e)
        {
            var client_socket = new AsynchronousClient();
            client_socket.BeginStartClient(client_socket.StartClientCallback, client_socket);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
