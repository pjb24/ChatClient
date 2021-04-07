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

using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using MyMessageProtocol;

namespace TestClient
{
    public partial class CreateRoomForm : Form
    {
        public Dictionary<int, string> userList = new Dictionary<int, string>();

        public string user_ID = string.Empty;

        public delegate void SubmitCreateRoomHandler(string msg);
        public event SubmitCreateRoomHandler OnSubmitCreateRoom;

        public int dataFormat = 0;

        public CreateRoomForm()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            if (clb_RoomUser.CheckedItems.Count == 0)
            {
                MessageBox.Show("채팅방에 초대할 인원을 선택해주십시오.", "알림");
                return;
            }

            int accessRight = 0;
            string roomName = string.Empty;
            string creator = user_ID;
            int creatorNo = SearchUserNoByUserID(user_ID);
            // 채팅방에 들어갈 회원들 수집
            List<string> tempUsers = new List<string>();
            List<int> tempUsersNo = new List<int>();

            foreach (object checkeditem in clb_RoomUser.CheckedItems)
            {
                tempUsersNo.Add(SearchUserNoByUserID(checkeditem.ToString()));
                tempUsers.Add(checkeditem.ToString());
            }

            // 정렬
            tempUsers.Sort();
            string group = string.Join(", ", tempUsers);

            // 채팅방 이름 설정
            if (txt_RoomName.Text.Length == 0)
            {
                string groupName = string.Empty;
                groupName = creator + ", ";
                // 채팅방 이름이 20자가 넘어가면 20자로 자르기
                if ((groupName.Length + group.Length) > 15)
                {
                    groupName = groupName + group;
                    groupName = groupName.Substring(0, 15) + " Room";
                }
                else
                {
                    groupName = groupName + group + " Room";
                }
                roomName = groupName;
            }
            else
            {
                roomName = txt_RoomName.Text;
            }

            // 채팅방 접근 권한 설정
            if (rdo_PublicRoom.Checked)
            {
                accessRight = 1;
            }
            else
            {
                accessRight = 0;
            }

            Relation relation1 = new Relation()
            {
                UserNo = creatorNo
            };

            Room room = new Room()
            {
                AccessRight = accessRight,
                Name = roomName
            };
            List<Relation> relations = new List<Relation>();
            relations.Add(relation1);
            foreach (int temp in tempUsersNo)
            {
                Relation relation = new Relation()
                {
                    UserNo = temp
                };
                relations.Add(relation);
            }
            room.Relation = relations;

            string msg = string.Empty;
            if (dataFormat == 1)
            {
                msg = JsonConvert.SerializeObject(room);
            }
            else if (dataFormat == 2)
            {
                ISerializer serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                msg = serializer.Serialize(room);
            }

            if (OnSubmitCreateRoom != null)
            {
                OnSubmitCreateRoom(msg);
            }
            /*
            // 설정 값 초기화
            txt_RoomName.Clear();
            rdo_PublicRoom.Checked = true;
            rdo_PrivateRoom.Checked = false;
            // 초대 회원 선택 초기화
            for (int i = 0; i < clb_RoomUser.Items.Count; i++)
            {
                clb_RoomUser.SetItemChecked(i, false);
            }
            */
            this.Close();
        }

        private void CreateRoomForm_Load(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, string> user in userList)
            {
                if (!clb_RoomUser.Items.Contains(user.Value) && !user_ID.Equals(user.Value))
                {
                    clb_RoomUser.Items.Add(user.Value);
                }
            }
        }

        private void CreateRoomForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalClass.lobbyForm.Location = this.Location;
            GlobalClass.lobbyForm.Show();
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
