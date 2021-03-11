
namespace TestClient
{
    partial class CreateRoomForm
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
            this.btn_Submit = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.clb_RoomUser = new System.Windows.Forms.CheckedListBox();
            this.lbl_RoomName = new System.Windows.Forms.Label();
            this.lbl_RoomAccess = new System.Windows.Forms.Label();
            this.txt_RoomName = new System.Windows.Forms.TextBox();
            this.rdo_PublicRoom = new System.Windows.Forms.RadioButton();
            this.rdo_PrivateRoom = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btn_Submit
            // 
            this.btn_Submit.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_Submit.Location = new System.Drawing.Point(12, 515);
            this.btn_Submit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Submit.Name = "btn_Submit";
            this.btn_Submit.Size = new System.Drawing.Size(120, 35);
            this.btn_Submit.TabIndex = 0;
            this.btn_Submit.Text = "생성";
            this.btn_Submit.UseVisualStyleBackColor = true;
            this.btn_Submit.Click += new System.EventHandler(this.btn_Submit_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_Close.Location = new System.Drawing.Point(202, 515);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(120, 35);
            this.btn_Close.TabIndex = 1;
            this.btn_Close.Text = "닫기";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // clb_RoomUser
            // 
            this.clb_RoomUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clb_RoomUser.CheckOnClick = true;
            this.clb_RoomUser.Font = new System.Drawing.Font("굴림", 14F);
            this.clb_RoomUser.FormattingEnabled = true;
            this.clb_RoomUser.Location = new System.Drawing.Point(0, 112);
            this.clb_RoomUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clb_RoomUser.Name = "clb_RoomUser";
            this.clb_RoomUser.Size = new System.Drawing.Size(335, 386);
            this.clb_RoomUser.TabIndex = 2;
            // 
            // lbl_RoomName
            // 
            this.lbl_RoomName.AutoSize = true;
            this.lbl_RoomName.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_RoomName.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_RoomName.Location = new System.Drawing.Point(12, 9);
            this.lbl_RoomName.Name = "lbl_RoomName";
            this.lbl_RoomName.Size = new System.Drawing.Size(110, 19);
            this.lbl_RoomName.TabIndex = 3;
            this.lbl_RoomName.Text = "채팅방 이름";
            // 
            // lbl_RoomAccess
            // 
            this.lbl_RoomAccess.AutoSize = true;
            this.lbl_RoomAccess.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_RoomAccess.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_RoomAccess.Location = new System.Drawing.Point(12, 77);
            this.lbl_RoomAccess.Name = "lbl_RoomAccess";
            this.lbl_RoomAccess.Size = new System.Drawing.Size(154, 19);
            this.lbl_RoomAccess.TabIndex = 4;
            this.lbl_RoomAccess.Text = "채팅방 공개 설정";
            // 
            // txt_RoomName
            // 
            this.txt_RoomName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_RoomName.Font = new System.Drawing.Font("굴림", 14F);
            this.txt_RoomName.Location = new System.Drawing.Point(12, 36);
            this.txt_RoomName.Name = "txt_RoomName";
            this.txt_RoomName.Size = new System.Drawing.Size(306, 29);
            this.txt_RoomName.TabIndex = 5;
            // 
            // rdo_PublicRoom
            // 
            this.rdo_PublicRoom.AutoSize = true;
            this.rdo_PublicRoom.Checked = true;
            this.rdo_PublicRoom.Font = new System.Drawing.Font("굴림", 14F);
            this.rdo_PublicRoom.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.rdo_PublicRoom.Location = new System.Drawing.Point(167, 75);
            this.rdo_PublicRoom.Name = "rdo_PublicRoom";
            this.rdo_PublicRoom.Size = new System.Drawing.Size(65, 23);
            this.rdo_PublicRoom.TabIndex = 6;
            this.rdo_PublicRoom.TabStop = true;
            this.rdo_PublicRoom.Text = "공개";
            this.rdo_PublicRoom.UseVisualStyleBackColor = true;
            // 
            // rdo_PrivateRoom
            // 
            this.rdo_PrivateRoom.AutoSize = true;
            this.rdo_PrivateRoom.Font = new System.Drawing.Font("굴림", 14F);
            this.rdo_PrivateRoom.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.rdo_PrivateRoom.Location = new System.Drawing.Point(238, 75);
            this.rdo_PrivateRoom.Name = "rdo_PrivateRoom";
            this.rdo_PrivateRoom.Size = new System.Drawing.Size(84, 23);
            this.rdo_PrivateRoom.TabIndex = 7;
            this.rdo_PrivateRoom.TabStop = true;
            this.rdo_PrivateRoom.Text = "비공개";
            this.rdo_PrivateRoom.UseVisualStyleBackColor = true;
            // 
            // CreateRoomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(334, 561);
            this.Controls.Add(this.rdo_PrivateRoom);
            this.Controls.Add(this.rdo_PublicRoom);
            this.Controls.Add(this.txt_RoomName);
            this.Controls.Add(this.lbl_RoomAccess);
            this.Controls.Add(this.lbl_RoomName);
            this.Controls.Add(this.clb_RoomUser);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Submit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "CreateRoomForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "채팅방 생성";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateRoomForm_FormClosing);
            this.Load += new System.EventHandler(this.CreateRoomForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Submit;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.CheckedListBox clb_RoomUser;
        private System.Windows.Forms.Label lbl_RoomName;
        private System.Windows.Forms.Label lbl_RoomAccess;
        private System.Windows.Forms.TextBox txt_RoomName;
        private System.Windows.Forms.RadioButton rdo_PublicRoom;
        private System.Windows.Forms.RadioButton rdo_PrivateRoom;
    }
}