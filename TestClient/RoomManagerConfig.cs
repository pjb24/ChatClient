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
    public partial class RoomManagerConfig : Form
    {
        public int roomNo = 0;
        // room 회원 정보
        public Dictionary<int, Tuple<int, int, int>> usersInRoom = new Dictionary<int, Tuple<int, int, int>>();
        // 전체 회원목록
        public Dictionary<int, string> userList = new Dictionary<int, string>();

        public delegate void ChangeManagerConfigHandler(List<int> changedUser);
        public event ChangeManagerConfigHandler OnChangeManagerConfig;

        public RoomManagerConfig()
        {
            InitializeComponent();
        }

        // sort할지 말지
        // 아래 clb로 내림
        private void btn_TurnOffManagementRight_Click(object sender, EventArgs e)
        {
            foreach (object checkeditem in clb_Manager.CheckedItems)
            {
                // 체크된 회원 아래로 내림
                clb_CommonUser.Items.Add(checkeditem);
                // 체크된 회원 위에서 지움
                clb_Manager.Items.Remove(checkeditem);
            }
            
        }

        // 위의 clb로 올림
        private void btn_GrantManagementRight_Click(object sender, EventArgs e)
        {
            foreach (object checkeditem in clb_CommonUser.CheckedItems)
            {
                // 체크된 회원 위로 올림
                clb_Manager.Items.Add(checkeditem);
                // 체크된 회원 아래에서 지움
                clb_CommonUser.Items.Remove(checkeditem);
            }
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            int userNo = 0;
            List<int> changedUser = new List<int>();
            // 상태가 변한 회원 정보 server 전송
            foreach(object temp in clb_Manager.Items)
            {
                userNo = SearchUserNoByUserID(temp.ToString());
                foreach (KeyValuePair<int, Tuple<int, int, int>> tmp in usersInRoom)
                {
                    if (tmp.Value.Item1.Equals(roomNo) && tmp.Value.Item2.Equals(userNo) && tmp.Value.Item3.Equals(0))
                    {
                        changedUser.Add(userNo);
                    }
                }
            }
            foreach (object temp in clb_CommonUser.Items)
            {
                userNo = SearchUserNoByUserID(temp.ToString());
                foreach (KeyValuePair<int, Tuple<int, int, int>> tmp in usersInRoom)
                {
                    if (tmp.Value.Item1.Equals(roomNo) && tmp.Value.Item2.Equals(userNo) && tmp.Value.Item3.Equals(1))
                    {
                        changedUser.Add(userNo);
                    }
                }
            }

            if (this.OnChangeManagerConfig != null)
            {
                OnChangeManagerConfig(changedUser);
            }
            this.Close();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RoomManagerConfig_Load(object sender, EventArgs e)
        {
            // 채팅방 관리자, 일반 회원 clb 채우기
            foreach(KeyValuePair<int, Tuple<int, int, int>> temp in usersInRoom)
            {
                if (temp.Value.Item1.Equals(roomNo))
                {
                    // 일반 회원
                    if (temp.Value.Item3.Equals(0))
                    {
                        clb_CommonUser.Items.Add(userList[temp.Value.Item2]);
                    }
                    // 채팅방 관리자
                    else if (temp.Value.Item3.Equals(1))
                    {
                        clb_Manager.Items.Add(userList[temp.Value.Item2]);
                    }
                }
            }
        }

        private int SearchUserNoByUserID(string userID)
        {
            int userNo = 0;
            foreach (KeyValuePair<int, string> temp in userList)
            {
                if (temp.Value.Equals(userID))
                {
                    userNo = temp.Key;
                    break;
                }
            }
            return userNo;
        }
    }
}
