
namespace TestClient
{
    partial class RoomConfig
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
            this.lbl_RoomName = new System.Windows.Forms.Label();
            this.lbl_RoomAccess = new System.Windows.Forms.Label();
            this.txt_RoomName = new System.Windows.Forms.TextBox();
            this.rdo_PublicRoom = new System.Windows.Forms.RadioButton();
            this.rdo_PrivateRoom = new System.Windows.Forms.RadioButton();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_RoomName
            // 
            this.lbl_RoomName.AutoSize = true;
            this.lbl_RoomName.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_RoomName.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_RoomName.Location = new System.Drawing.Point(12, 26);
            this.lbl_RoomName.Name = "lbl_RoomName";
            this.lbl_RoomName.Size = new System.Drawing.Size(110, 19);
            this.lbl_RoomName.TabIndex = 0;
            this.lbl_RoomName.Text = "채팅방 이름";
            // 
            // lbl_RoomAccess
            // 
            this.lbl_RoomAccess.AutoSize = true;
            this.lbl_RoomAccess.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_RoomAccess.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_RoomAccess.Location = new System.Drawing.Point(12, 125);
            this.lbl_RoomAccess.Name = "lbl_RoomAccess";
            this.lbl_RoomAccess.Size = new System.Drawing.Size(154, 19);
            this.lbl_RoomAccess.TabIndex = 1;
            this.lbl_RoomAccess.Text = "채팅방 공개 설정";
            // 
            // txt_RoomName
            // 
            this.txt_RoomName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_RoomName.Font = new System.Drawing.Font("굴림", 14F);
            this.txt_RoomName.Location = new System.Drawing.Point(16, 65);
            this.txt_RoomName.MaxLength = 20;
            this.txt_RoomName.Name = "txt_RoomName";
            this.txt_RoomName.Size = new System.Drawing.Size(306, 29);
            this.txt_RoomName.TabIndex = 2;
            // 
            // rdo_PublicRoom
            // 
            this.rdo_PublicRoom.AutoSize = true;
            this.rdo_PublicRoom.Font = new System.Drawing.Font("굴림", 14F);
            this.rdo_PublicRoom.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.rdo_PublicRoom.Location = new System.Drawing.Point(167, 123);
            this.rdo_PublicRoom.Name = "rdo_PublicRoom";
            this.rdo_PublicRoom.Size = new System.Drawing.Size(65, 23);
            this.rdo_PublicRoom.TabIndex = 3;
            this.rdo_PublicRoom.TabStop = true;
            this.rdo_PublicRoom.Text = "공개";
            this.rdo_PublicRoom.UseVisualStyleBackColor = true;
            // 
            // rdo_PrivateRoom
            // 
            this.rdo_PrivateRoom.AutoSize = true;
            this.rdo_PrivateRoom.Font = new System.Drawing.Font("굴림", 14F);
            this.rdo_PrivateRoom.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.rdo_PrivateRoom.Location = new System.Drawing.Point(238, 123);
            this.rdo_PrivateRoom.Name = "rdo_PrivateRoom";
            this.rdo_PrivateRoom.Size = new System.Drawing.Size(84, 23);
            this.rdo_PrivateRoom.TabIndex = 4;
            this.rdo_PrivateRoom.TabStop = true;
            this.rdo_PrivateRoom.Text = "비공개";
            this.rdo_PrivateRoom.UseVisualStyleBackColor = true;
            // 
            // btn_OK
            // 
            this.btn_OK.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_OK.Location = new System.Drawing.Point(35, 194);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(100, 35);
            this.btn_OK.TabIndex = 5;
            this.btn_OK.Text = "확인";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_Cancel.Location = new System.Drawing.Point(199, 194);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(100, 35);
            this.btn_Cancel.TabIndex = 6;
            this.btn_Cancel.Text = "취소";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // RoomConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(334, 261);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.rdo_PrivateRoom);
            this.Controls.Add(this.rdo_PublicRoom);
            this.Controls.Add(this.txt_RoomName);
            this.Controls.Add(this.lbl_RoomAccess);
            this.Controls.Add(this.lbl_RoomName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RoomConfig";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "채팅방 설정";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_RoomName;
        private System.Windows.Forms.Label lbl_RoomAccess;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        public System.Windows.Forms.TextBox txt_RoomName;
        public System.Windows.Forms.RadioButton rdo_PublicRoom;
        public System.Windows.Forms.RadioButton rdo_PrivateRoom;
    }
}