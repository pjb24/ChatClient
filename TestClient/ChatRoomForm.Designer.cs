
namespace TestClient
{
    partial class ChatRoomForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_Result = new System.Windows.Forms.ListBox();
            this.txt_Send = new System.Windows.Forms.TextBox();
            this.btn_Send = new System.Windows.Forms.Button();
            this.lb_UserList = new System.Windows.Forms.ListBox();
            this.lbl_UserList = new System.Windows.Forms.Label();
            this.btn_Leave = new System.Windows.Forms.Button();
            this.btn_Invitation = new System.Windows.Forms.Button();
            this.btn_SendFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_BanishUser = new System.Windows.Forms.Button();
            this.btn_ChangeRoomConfig = new System.Windows.Forms.Button();
            this.btn_ManagerConfig = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_Result
            // 
            this.lb_Result.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_Result.Font = new System.Drawing.Font("굴림", 14F);
            this.lb_Result.FormattingEnabled = true;
            this.lb_Result.ItemHeight = 19;
            this.lb_Result.Location = new System.Drawing.Point(0, 204);
            this.lb_Result.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_Result.Name = "lb_Result";
            this.lb_Result.Size = new System.Drawing.Size(435, 268);
            this.lb_Result.TabIndex = 1;
            // 
            // txt_Send
            // 
            this.txt_Send.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Send.Font = new System.Drawing.Font("굴림", 14F);
            this.txt_Send.Location = new System.Drawing.Point(12, 501);
            this.txt_Send.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Send.Multiline = true;
            this.txt_Send.Name = "txt_Send";
            this.txt_Send.Size = new System.Drawing.Size(204, 49);
            this.txt_Send.TabIndex = 2;
            this.txt_Send.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Send_KeyDown);
            // 
            // btn_Send
            // 
            this.btn_Send.Font = new System.Drawing.Font("굴림", 9F);
            this.btn_Send.Location = new System.Drawing.Point(222, 501);
            this.btn_Send.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(100, 35);
            this.btn_Send.TabIndex = 3;
            this.btn_Send.Text = "전송";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // lb_UserList
            // 
            this.lb_UserList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_UserList.Font = new System.Drawing.Font("굴림", 14F);
            this.lb_UserList.FormattingEnabled = true;
            this.lb_UserList.ItemHeight = 19;
            this.lb_UserList.Location = new System.Drawing.Point(0, 35);
            this.lb_UserList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_UserList.Name = "lb_UserList";
            this.lb_UserList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lb_UserList.Size = new System.Drawing.Size(216, 154);
            this.lb_UserList.TabIndex = 5;
            this.lb_UserList.TabStop = false;
            // 
            // lbl_UserList
            // 
            this.lbl_UserList.AutoSize = true;
            this.lbl_UserList.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_UserList.ForeColor = System.Drawing.Color.White;
            this.lbl_UserList.Location = new System.Drawing.Point(12, 9);
            this.lbl_UserList.Name = "lbl_UserList";
            this.lbl_UserList.Size = new System.Drawing.Size(148, 19);
            this.lbl_UserList.TabIndex = 6;
            this.lbl_UserList.Text = "채팅방 회원목록";
            // 
            // btn_Leave
            // 
            this.btn_Leave.Location = new System.Drawing.Point(222, 35);
            this.btn_Leave.Name = "btn_Leave";
            this.btn_Leave.Size = new System.Drawing.Size(100, 35);
            this.btn_Leave.TabIndex = 7;
            this.btn_Leave.Text = "채팅방 나가기";
            this.btn_Leave.UseVisualStyleBackColor = true;
            this.btn_Leave.Click += new System.EventHandler(this.btn_Leave_Click);
            // 
            // btn_Invitation
            // 
            this.btn_Invitation.Location = new System.Drawing.Point(222, 97);
            this.btn_Invitation.Name = "btn_Invitation";
            this.btn_Invitation.Size = new System.Drawing.Size(100, 35);
            this.btn_Invitation.TabIndex = 8;
            this.btn_Invitation.Text = "채팅방 초대";
            this.btn_Invitation.UseVisualStyleBackColor = true;
            this.btn_Invitation.Click += new System.EventHandler(this.btn_Invitation_Click);
            // 
            // btn_SendFile
            // 
            this.btn_SendFile.Location = new System.Drawing.Point(222, 159);
            this.btn_SendFile.Name = "btn_SendFile";
            this.btn_SendFile.Size = new System.Drawing.Size(100, 35);
            this.btn_SendFile.TabIndex = 9;
            this.btn_SendFile.Text = "파일 전송";
            this.btn_SendFile.UseVisualStyleBackColor = true;
            this.btn_SendFile.Click += new System.EventHandler(this.btn_SendFile_Click);
            // 
            // btn_BanishUser
            // 
            this.btn_BanishUser.Font = new System.Drawing.Font("굴림", 9F);
            this.btn_BanishUser.Location = new System.Drawing.Point(328, 35);
            this.btn_BanishUser.Name = "btn_BanishUser";
            this.btn_BanishUser.Size = new System.Drawing.Size(100, 35);
            this.btn_BanishUser.TabIndex = 10;
            this.btn_BanishUser.Text = "회원 추방";
            this.btn_BanishUser.UseVisualStyleBackColor = true;
            this.btn_BanishUser.Click += new System.EventHandler(this.btn_banishUser_Click);
            // 
            // btn_ChangeRoomConfig
            // 
            this.btn_ChangeRoomConfig.Font = new System.Drawing.Font("굴림", 9F);
            this.btn_ChangeRoomConfig.Location = new System.Drawing.Point(328, 97);
            this.btn_ChangeRoomConfig.Name = "btn_ChangeRoomConfig";
            this.btn_ChangeRoomConfig.Size = new System.Drawing.Size(100, 35);
            this.btn_ChangeRoomConfig.TabIndex = 11;
            this.btn_ChangeRoomConfig.Text = "채팅방 설정";
            this.btn_ChangeRoomConfig.UseVisualStyleBackColor = true;
            this.btn_ChangeRoomConfig.Click += new System.EventHandler(this.btn_changeRoomConfig_Click);
            // 
            // btn_ManagerConfig
            // 
            this.btn_ManagerConfig.Font = new System.Drawing.Font("굴림", 9F);
            this.btn_ManagerConfig.Location = new System.Drawing.Point(328, 159);
            this.btn_ManagerConfig.Name = "btn_ManagerConfig";
            this.btn_ManagerConfig.Size = new System.Drawing.Size(100, 35);
            this.btn_ManagerConfig.TabIndex = 12;
            this.btn_ManagerConfig.Text = "관리자 설정";
            this.btn_ManagerConfig.UseVisualStyleBackColor = true;
            this.btn_ManagerConfig.Click += new System.EventHandler(this.btn_managerConfig_Click);
            // 
            // ChatRoomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(434, 561);
            this.Controls.Add(this.btn_ManagerConfig);
            this.Controls.Add(this.btn_ChangeRoomConfig);
            this.Controls.Add(this.btn_BanishUser);
            this.Controls.Add(this.btn_SendFile);
            this.Controls.Add(this.btn_Invitation);
            this.Controls.Add(this.btn_Leave);
            this.Controls.Add(this.lbl_UserList);
            this.Controls.Add(this.lb_UserList);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.txt_Send);
            this.Controls.Add(this.lb_Result);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "ChatRoomForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "채팅방";
            this.Load += new System.EventHandler(this.ChatGroupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lb_Result;
        private System.Windows.Forms.TextBox txt_Send;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.Label lbl_UserList;
        private System.Windows.Forms.Button btn_Leave;
        private System.Windows.Forms.Button btn_Invitation;
        private System.Windows.Forms.Button btn_SendFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.Button btn_BanishUser;
        public System.Windows.Forms.Button btn_ChangeRoomConfig;
        public System.Windows.Forms.Button btn_ManagerConfig;
        public System.Windows.Forms.ListBox lb_UserList;
    }
}