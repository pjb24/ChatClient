
namespace TestClient
{
    partial class TestClientUI
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_Result = new System.Windows.Forms.ListBox();
            this.txt_UserID = new System.Windows.Forms.TextBox();
            this.txt_UserPW = new System.Windows.Forms.TextBox();
            this.btn_SignInSubmit = new System.Windows.Forms.Button();
            this.btn_OpenRegister = new System.Windows.Forms.Button();
            this.btn_RegisterSubmit = new System.Windows.Forms.Button();
            this.btn_RegisterClose = new System.Windows.Forms.Button();
            this.lb_GroupList = new System.Windows.Forms.ListBox();
            this.btn_OpenCreateGroup = new System.Windows.Forms.Button();
            this.btn_SignOut = new System.Windows.Forms.Button();
            this.btn_PullUser = new System.Windows.Forms.Button();
            this.btn_PullGroup = new System.Windows.Forms.Button();
            this.clb_GroupingUser = new System.Windows.Forms.CheckedListBox();
            this.btn_Create = new System.Windows.Forms.Button();
            this.btn_CreateClose = new System.Windows.Forms.Button();
            this.lb_UserList = new System.Windows.Forms.ListBox();
            this.lbl_Register = new System.Windows.Forms.Label();
            this.lbl_SignIn = new System.Windows.Forms.Label();
            this.rdo_PublicRoom = new System.Windows.Forms.RadioButton();
            this.rdo_PrivateRoom = new System.Windows.Forms.RadioButton();
            this.txt_RoomName = new System.Windows.Forms.TextBox();
            this.lbl_RoomName = new System.Windows.Forms.Label();
            this.lbl_RoomAccess = new System.Windows.Forms.Label();
            this.lbl_UserID = new System.Windows.Forms.Label();
            this.lbl_UserPW = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_Result
            // 
            this.lb_Result.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_Result.FormattingEnabled = true;
            this.lb_Result.HorizontalScrollbar = true;
            this.lb_Result.ItemHeight = 12;
            this.lb_Result.Location = new System.Drawing.Point(430, 631);
            this.lb_Result.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_Result.Name = "lb_Result";
            this.lb_Result.Size = new System.Drawing.Size(1384, 194);
            this.lb_Result.TabIndex = 0;
            // 
            // txt_UserID
            // 
            this.txt_UserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_UserID.Font = new System.Drawing.Font("굴림", 14F);
            this.txt_UserID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txt_UserID.Location = new System.Drawing.Point(430, 139);
            this.txt_UserID.MaxLength = 20;
            this.txt_UserID.Name = "txt_UserID";
            this.txt_UserID.Size = new System.Drawing.Size(160, 29);
            this.txt_UserID.TabIndex = 1;
            // 
            // txt_UserPW
            // 
            this.txt_UserPW.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_UserPW.Font = new System.Drawing.Font("굴림", 14F);
            this.txt_UserPW.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txt_UserPW.Location = new System.Drawing.Point(430, 206);
            this.txt_UserPW.MaxLength = 20;
            this.txt_UserPW.Name = "txt_UserPW";
            this.txt_UserPW.PasswordChar = '*';
            this.txt_UserPW.Size = new System.Drawing.Size(160, 29);
            this.txt_UserPW.TabIndex = 2;
            // 
            // btn_SignInSubmit
            // 
            this.btn_SignInSubmit.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_SignInSubmit.Location = new System.Drawing.Point(430, 266);
            this.btn_SignInSubmit.Name = "btn_SignInSubmit";
            this.btn_SignInSubmit.Size = new System.Drawing.Size(160, 30);
            this.btn_SignInSubmit.TabIndex = 3;
            this.btn_SignInSubmit.Text = "로그인";
            this.btn_SignInSubmit.UseVisualStyleBackColor = true;
            this.btn_SignInSubmit.Click += new System.EventHandler(this.btn_SignInSubmit_Click);
            // 
            // btn_OpenRegister
            // 
            this.btn_OpenRegister.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_OpenRegister.Location = new System.Drawing.Point(430, 339);
            this.btn_OpenRegister.Name = "btn_OpenRegister";
            this.btn_OpenRegister.Size = new System.Drawing.Size(160, 30);
            this.btn_OpenRegister.TabIndex = 3;
            this.btn_OpenRegister.Text = "회원가입";
            this.btn_OpenRegister.UseVisualStyleBackColor = true;
            this.btn_OpenRegister.Click += new System.EventHandler(this.btn_OpenRegister_Click);
            // 
            // btn_RegisterSubmit
            // 
            this.btn_RegisterSubmit.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_RegisterSubmit.Location = new System.Drawing.Point(753, 266);
            this.btn_RegisterSubmit.Name = "btn_RegisterSubmit";
            this.btn_RegisterSubmit.Size = new System.Drawing.Size(160, 30);
            this.btn_RegisterSubmit.TabIndex = 4;
            this.btn_RegisterSubmit.TabStop = false;
            this.btn_RegisterSubmit.Text = "등록";
            this.btn_RegisterSubmit.UseVisualStyleBackColor = true;
            this.btn_RegisterSubmit.Click += new System.EventHandler(this.btn_RegisterSubmit_Click);
            // 
            // btn_RegisterClose
            // 
            this.btn_RegisterClose.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_RegisterClose.Location = new System.Drawing.Point(753, 339);
            this.btn_RegisterClose.Name = "btn_RegisterClose";
            this.btn_RegisterClose.Size = new System.Drawing.Size(160, 30);
            this.btn_RegisterClose.TabIndex = 5;
            this.btn_RegisterClose.TabStop = false;
            this.btn_RegisterClose.Text = "취소";
            this.btn_RegisterClose.UseVisualStyleBackColor = true;
            this.btn_RegisterClose.Click += new System.EventHandler(this.btn_RegisterClose_Click);
            // 
            // lb_GroupList
            // 
            this.lb_GroupList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_GroupList.Font = new System.Drawing.Font("굴림", 14F);
            this.lb_GroupList.FormattingEnabled = true;
            this.lb_GroupList.ItemHeight = 19;
            this.lb_GroupList.Location = new System.Drawing.Point(987, 204);
            this.lb_GroupList.Name = "lb_GroupList";
            this.lb_GroupList.Size = new System.Drawing.Size(335, 306);
            this.lb_GroupList.TabIndex = 6;
            this.lb_GroupList.TabStop = false;
            this.lb_GroupList.DoubleClick += new System.EventHandler(this.lb_GroupList_DoubleClick);
            // 
            // btn_OpenCreateGroup
            // 
            this.btn_OpenCreateGroup.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_OpenCreateGroup.Location = new System.Drawing.Point(1202, 478);
            this.btn_OpenCreateGroup.Name = "btn_OpenCreateGroup";
            this.btn_OpenCreateGroup.Size = new System.Drawing.Size(120, 35);
            this.btn_OpenCreateGroup.TabIndex = 7;
            this.btn_OpenCreateGroup.TabStop = false;
            this.btn_OpenCreateGroup.Text = "채팅방 생성";
            this.btn_OpenCreateGroup.UseVisualStyleBackColor = true;
            this.btn_OpenCreateGroup.Click += new System.EventHandler(this.btn_OpenCreateGroup_Click);
            // 
            // btn_SignOut
            // 
            this.btn_SignOut.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_SignOut.Location = new System.Drawing.Point(1202, 519);
            this.btn_SignOut.Name = "btn_SignOut";
            this.btn_SignOut.Size = new System.Drawing.Size(120, 35);
            this.btn_SignOut.TabIndex = 8;
            this.btn_SignOut.TabStop = false;
            this.btn_SignOut.Text = "로그아웃";
            this.btn_SignOut.UseVisualStyleBackColor = true;
            this.btn_SignOut.Click += new System.EventHandler(this.btn_SignOut_Click);
            // 
            // btn_PullUser
            // 
            this.btn_PullUser.Location = new System.Drawing.Point(987, 478);
            this.btn_PullUser.Name = "btn_PullUser";
            this.btn_PullUser.Size = new System.Drawing.Size(120, 35);
            this.btn_PullUser.TabIndex = 9;
            this.btn_PullUser.TabStop = false;
            this.btn_PullUser.Text = "회원목록 가져오기";
            this.btn_PullUser.UseVisualStyleBackColor = true;
            this.btn_PullUser.Visible = false;
            this.btn_PullUser.Click += new System.EventHandler(this.btn_PullUser_Click);
            // 
            // btn_PullGroup
            // 
            this.btn_PullGroup.Location = new System.Drawing.Point(987, 519);
            this.btn_PullGroup.Name = "btn_PullGroup";
            this.btn_PullGroup.Size = new System.Drawing.Size(120, 35);
            this.btn_PullGroup.TabIndex = 10;
            this.btn_PullGroup.TabStop = false;
            this.btn_PullGroup.Text = "채팅방 가져오기";
            this.btn_PullGroup.UseVisualStyleBackColor = true;
            this.btn_PullGroup.Visible = false;
            this.btn_PullGroup.Click += new System.EventHandler(this.btn_PullGroup_Click);
            // 
            // clb_GroupingUser
            // 
            this.clb_GroupingUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clb_GroupingUser.CheckOnClick = true;
            this.clb_GroupingUser.Font = new System.Drawing.Font("굴림", 14F);
            this.clb_GroupingUser.FormattingEnabled = true;
            this.clb_GroupingUser.Location = new System.Drawing.Point(1479, 112);
            this.clb_GroupingUser.Name = "clb_GroupingUser";
            this.clb_GroupingUser.Size = new System.Drawing.Size(335, 386);
            this.clb_GroupingUser.TabIndex = 11;
            this.clb_GroupingUser.TabStop = false;
            // 
            // btn_Create
            // 
            this.btn_Create.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_Create.Location = new System.Drawing.Point(1479, 518);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(110, 45);
            this.btn_Create.TabIndex = 12;
            this.btn_Create.TabStop = false;
            this.btn_Create.Text = "생성";
            this.btn_Create.UseVisualStyleBackColor = true;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // btn_CreateClose
            // 
            this.btn_CreateClose.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_CreateClose.Location = new System.Drawing.Point(1704, 519);
            this.btn_CreateClose.Name = "btn_CreateClose";
            this.btn_CreateClose.Size = new System.Drawing.Size(110, 45);
            this.btn_CreateClose.TabIndex = 13;
            this.btn_CreateClose.TabStop = false;
            this.btn_CreateClose.Text = "취소";
            this.btn_CreateClose.UseVisualStyleBackColor = true;
            this.btn_CreateClose.Click += new System.EventHandler(this.btn_CreateClose_Click);
            // 
            // lb_UserList
            // 
            this.lb_UserList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_UserList.Font = new System.Drawing.Font("굴림", 14F);
            this.lb_UserList.FormattingEnabled = true;
            this.lb_UserList.ItemHeight = 19;
            this.lb_UserList.Location = new System.Drawing.Point(987, 12);
            this.lb_UserList.Name = "lb_UserList";
            this.lb_UserList.Size = new System.Drawing.Size(335, 173);
            this.lb_UserList.TabIndex = 14;
            // 
            // lbl_Register
            // 
            this.lbl_Register.AutoSize = true;
            this.lbl_Register.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Register.ForeColor = System.Drawing.Color.White;
            this.lbl_Register.Location = new System.Drawing.Point(761, 63);
            this.lbl_Register.Name = "lbl_Register";
            this.lbl_Register.Size = new System.Drawing.Size(143, 32);
            this.lbl_Register.TabIndex = 15;
            this.lbl_Register.Text = "회원가입";
            // 
            // lbl_SignIn
            // 
            this.lbl_SignIn.AutoSize = true;
            this.lbl_SignIn.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_SignIn.ForeColor = System.Drawing.Color.White;
            this.lbl_SignIn.Location = new System.Drawing.Point(455, 63);
            this.lbl_SignIn.Name = "lbl_SignIn";
            this.lbl_SignIn.Size = new System.Drawing.Size(111, 32);
            this.lbl_SignIn.TabIndex = 15;
            this.lbl_SignIn.Text = "로그인";
            // 
            // rdo_PublicRoom
            // 
            this.rdo_PublicRoom.AutoSize = true;
            this.rdo_PublicRoom.Checked = true;
            this.rdo_PublicRoom.Font = new System.Drawing.Font("굴림", 14F);
            this.rdo_PublicRoom.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.rdo_PublicRoom.Location = new System.Drawing.Point(1645, 75);
            this.rdo_PublicRoom.Name = "rdo_PublicRoom";
            this.rdo_PublicRoom.Size = new System.Drawing.Size(65, 23);
            this.rdo_PublicRoom.TabIndex = 16;
            this.rdo_PublicRoom.TabStop = true;
            this.rdo_PublicRoom.Text = "공개";
            this.rdo_PublicRoom.UseVisualStyleBackColor = true;
            // 
            // rdo_PrivateRoom
            // 
            this.rdo_PrivateRoom.AutoSize = true;
            this.rdo_PrivateRoom.Font = new System.Drawing.Font("굴림", 14F);
            this.rdo_PrivateRoom.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.rdo_PrivateRoom.Location = new System.Drawing.Point(1711, 75);
            this.rdo_PrivateRoom.Name = "rdo_PrivateRoom";
            this.rdo_PrivateRoom.Size = new System.Drawing.Size(84, 23);
            this.rdo_PrivateRoom.TabIndex = 17;
            this.rdo_PrivateRoom.Text = "비공개";
            this.rdo_PrivateRoom.UseVisualStyleBackColor = true;
            // 
            // txt_RoomName
            // 
            this.txt_RoomName.Font = new System.Drawing.Font("굴림", 14F);
            this.txt_RoomName.Location = new System.Drawing.Point(1489, 37);
            this.txt_RoomName.MaxLength = 20;
            this.txt_RoomName.Name = "txt_RoomName";
            this.txt_RoomName.Size = new System.Drawing.Size(306, 29);
            this.txt_RoomName.TabIndex = 18;
            // 
            // lbl_RoomName
            // 
            this.lbl_RoomName.AutoSize = true;
            this.lbl_RoomName.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_RoomName.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_RoomName.Location = new System.Drawing.Point(1485, 9);
            this.lbl_RoomName.Name = "lbl_RoomName";
            this.lbl_RoomName.Size = new System.Drawing.Size(110, 19);
            this.lbl_RoomName.TabIndex = 19;
            this.lbl_RoomName.Text = "채팅방 이름";
            // 
            // lbl_RoomAccess
            // 
            this.lbl_RoomAccess.AutoSize = true;
            this.lbl_RoomAccess.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_RoomAccess.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_RoomAccess.Location = new System.Drawing.Point(1485, 77);
            this.lbl_RoomAccess.Name = "lbl_RoomAccess";
            this.lbl_RoomAccess.Size = new System.Drawing.Size(154, 19);
            this.lbl_RoomAccess.TabIndex = 20;
            this.lbl_RoomAccess.Text = "채팅방 공개 설정";
            // 
            // lbl_UserID
            // 
            this.lbl_UserID.AutoSize = true;
            this.lbl_UserID.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_UserID.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_UserID.Location = new System.Drawing.Point(401, 141);
            this.lbl_UserID.Name = "lbl_UserID";
            this.lbl_UserID.Size = new System.Drawing.Size(23, 19);
            this.lbl_UserID.TabIndex = 21;
            this.lbl_UserID.Text = "ID";
            // 
            // lbl_UserPW
            // 
            this.lbl_UserPW.AutoSize = true;
            this.lbl_UserPW.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_UserPW.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_UserPW.Location = new System.Drawing.Point(389, 208);
            this.lbl_UserPW.Name = "lbl_UserPW";
            this.lbl_UserPW.Size = new System.Drawing.Size(35, 19);
            this.lbl_UserPW.TabIndex = 22;
            this.lbl_UserPW.Text = "PW";
            // 
            // TestClientUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(1904, 861);
            this.Controls.Add(this.lbl_UserPW);
            this.Controls.Add(this.lbl_UserID);
            this.Controls.Add(this.lbl_RoomAccess);
            this.Controls.Add(this.lbl_RoomName);
            this.Controls.Add(this.txt_RoomName);
            this.Controls.Add(this.rdo_PrivateRoom);
            this.Controls.Add(this.rdo_PublicRoom);
            this.Controls.Add(this.lbl_SignIn);
            this.Controls.Add(this.lbl_Register);
            this.Controls.Add(this.lb_UserList);
            this.Controls.Add(this.btn_CreateClose);
            this.Controls.Add(this.btn_Create);
            this.Controls.Add(this.clb_GroupingUser);
            this.Controls.Add(this.btn_PullGroup);
            this.Controls.Add(this.btn_PullUser);
            this.Controls.Add(this.btn_SignOut);
            this.Controls.Add(this.btn_OpenCreateGroup);
            this.Controls.Add(this.lb_GroupList);
            this.Controls.Add(this.btn_RegisterClose);
            this.Controls.Add(this.btn_RegisterSubmit);
            this.Controls.Add(this.btn_OpenRegister);
            this.Controls.Add(this.btn_SignInSubmit);
            this.Controls.Add(this.txt_UserPW);
            this.Controls.Add(this.txt_UserID);
            this.Controls.Add(this.lb_Result);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "TestClientUI";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Client";
            this.Load += new System.EventHandler(this.TestClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ListBox lb_Result;
        private System.Windows.Forms.TextBox txt_UserID;
        private System.Windows.Forms.TextBox txt_UserPW;
        private System.Windows.Forms.Button btn_SignInSubmit;
        private System.Windows.Forms.Button btn_OpenRegister;
        private System.Windows.Forms.Button btn_RegisterSubmit;
        private System.Windows.Forms.Button btn_RegisterClose;
        private System.Windows.Forms.ListBox lb_GroupList;
        private System.Windows.Forms.Button btn_OpenCreateGroup;
        private System.Windows.Forms.Button btn_SignOut;
        private System.Windows.Forms.Button btn_PullUser;
        private System.Windows.Forms.Button btn_PullGroup;
        private System.Windows.Forms.CheckedListBox clb_GroupingUser;
        private System.Windows.Forms.Button btn_Create;
        private System.Windows.Forms.Button btn_CreateClose;
        private System.Windows.Forms.ListBox lb_UserList;
        private System.Windows.Forms.Label lbl_Register;
        private System.Windows.Forms.Label lbl_SignIn;
        private System.Windows.Forms.RadioButton rdo_PublicRoom;
        private System.Windows.Forms.RadioButton rdo_PrivateRoom;
        private System.Windows.Forms.TextBox txt_RoomName;
        private System.Windows.Forms.Label lbl_RoomName;
        private System.Windows.Forms.Label lbl_RoomAccess;
        private System.Windows.Forms.Label lbl_UserID;
        private System.Windows.Forms.Label lbl_UserPW;
    }
}

