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
    public partial class RoomConfig : Form
    {
        public string roomName = string.Empty;
        public int accessRight = 0;

        public delegate void ChangeRoomConfigHandler(int accessRight, string roomName);
        public event ChangeRoomConfigHandler OnChangeRoomConfig;

        public RoomConfig()
        {
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            roomName = txt_RoomName.Text;
            if (rdo_PublicRoom.Checked)
            {
                accessRight = 1;
            }
            else
            {
                accessRight = 0;
            }

            if (this.OnChangeRoomConfig != null)
            {
                OnChangeRoomConfig(accessRight, roomName);
            }
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
