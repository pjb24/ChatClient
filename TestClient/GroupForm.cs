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
using System.Threading;

namespace TestClient
{
    public partial class GroupForm : Form
    {
        public NetworkStream stream = default(NetworkStream);
        public List<string> userList = new List<string>();
        public List<string> groupList = new List<string>();
        public string user_ID;

        // new TestClientUI(); 로 생성해버리면 stackoverflowException 발생 
        public TestClientUI testClientUI = null;
        // 동적 생성되는 버튼 저장
        private List<Button> btn_OpenGroup = new List<Button>();

        public GroupForm()
        {
            InitializeComponent();
        }

        // 동적 생성된 버튼 이벤트, 어떤 컨트롤에서 눌러진건지 확인
        private void btn_OpenGroup_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Control[] controls = this.Controls.Find("btn_OpenGroup" + btn.Tag, true);

            foreach(Control temp in controls)
            {
                // 확인된 컨트롤 정보로 open
                open_ChatGroup(temp);
            }
        }

        public void designGroup()
        {
            // groupList를 사용하여 버튼 컨트롤 동적 생성
            // 버튼의 Text를 사용자 편의적으로 바꿀 필요가 있음
            // change design
            for (int i = 0; i < groupList.Count; i++)
            {
                btn_OpenGroup.Add(new Button());
                btn_OpenGroup[i].Location = new Point(10 + 40 * i, 10 + 40 * i);
                btn_OpenGroup[i].Name = "btn_OpenGroup" + i.ToString();
                //btn_OpenGroup[i].Size = new Size(50, 50);
                btn_OpenGroup[i].Text = "Group" + i.ToString();
                btn_OpenGroup[i].UseVisualStyleBackColor = true;
                btn_OpenGroup[i].Tag = i;
                btn_OpenGroup[i].Click += new EventHandler(btn_OpenGroup_Click);
                Controls.Add(btn_OpenGroup[i]);
            }
            // window를 비활성화하여 WM_PAINT call
            // true 배경을 지우고 다시 그린다
            // false 현 배경 위에 다시 그린다
            Invalidate(false);
        }

        private void open_ChatGroup(Control sender)
        {
            Button btn = sender as Button;
            // 해당 윈도우가 이미 열려있을 때 처리
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.Name.Equals("chatGroupForm" + (int)btn.Tag))
                {
                    if (openForm.WindowState == FormWindowState.Minimized)
                    {
                        openForm.WindowState = FormWindowState.Normal;
                        openForm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
                    }
                    openForm.Activate();
                    return;
                }
            }
            ChatGroupForm chatGroupForm = new ChatGroupForm();
            chatGroupForm.stream = stream;
            chatGroupForm.group = groupList[(int)btn.Tag];
            chatGroupForm.user_ID = user_ID;
            chatGroupForm.Tag = (int)btn.Tag;
            chatGroupForm.Text = "chatGroupForm" + (int)btn.Tag;
            chatGroupForm.Name = "chatGroupForm" + (int)btn.Tag;

            // ChatGroupForm이 열렸을 때 TestClientUI에 Form 정보 저장
            testClientUI.open_ChatGroupForm(chatGroupForm);

            chatGroupForm.Show();
        }

        private void btn_CreateGroup_Click(object sender, EventArgs e)
        {
            // 1번 사용하고 더 사용하지 않기 때문에 함수 안에서 CreateGroupForm 객체 생성
            CreateGroupForm createGroupForm = new CreateGroupForm();
            createGroupForm.stream = stream;
            createGroupForm.userList = userList;
            createGroupForm.user_ID = user_ID;
            createGroupForm.Show();
        }

        // groupList 동기화 요청
        public void btn_PullGroup_Click(object sender, EventArgs e)
        {
            string sendMsg =  user_ID + "&requestGroupList";
            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        // userList 동기화 요청
        private void btn_PullUser_Click(object sender, EventArgs e)
        {
            string sendMsg = user_ID + "&requestUserList";
            byte[] buffer = Encoding.Unicode.GetBytes(sendMsg + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        // server에서 clientList 정리 필요
        private void btn_SignOut_Click(object sender, EventArgs e)
        {
            // Sign out 후 다른 사용자가 같은 클라이언트로 Sign in 하면
            // userList와 groupList는 어떻게 되는가
            // 현재는 이전 사용자의 데이터가 남아있다
            // 사용자에 따라 데이터가 달라져야하니 데이터 제어가 필요하다
            this.Close();
        }

        private void GroupForm_Load(object sender, EventArgs e)
        {
            this.lbl_UserID.Text = "클라이언트 ID : " + user_ID;
        }
    }
}
