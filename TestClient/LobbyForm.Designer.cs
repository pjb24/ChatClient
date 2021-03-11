
namespace TestClient
{
    partial class LobbyForm
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
            this.btn_CreateRoom = new System.Windows.Forms.Button();
            this.btn_SignOut = new System.Windows.Forms.Button();
            this.lb_RoomList = new System.Windows.Forms.ListBox();
            this.lb_UserList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btn_CreateRoom
            // 
            this.btn_CreateRoom.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_CreateRoom.Location = new System.Drawing.Point(12, 518);
            this.btn_CreateRoom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_CreateRoom.Name = "btn_CreateRoom";
            this.btn_CreateRoom.Size = new System.Drawing.Size(120, 35);
            this.btn_CreateRoom.TabIndex = 0;
            this.btn_CreateRoom.Text = "채팅방 생성";
            this.btn_CreateRoom.UseVisualStyleBackColor = true;
            this.btn_CreateRoom.Click += new System.EventHandler(this.btn_CreateRoom_Click);
            // 
            // btn_SignOut
            // 
            this.btn_SignOut.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_SignOut.Location = new System.Drawing.Point(202, 518);
            this.btn_SignOut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_SignOut.Name = "btn_SignOut";
            this.btn_SignOut.Size = new System.Drawing.Size(120, 35);
            this.btn_SignOut.TabIndex = 1;
            this.btn_SignOut.Text = "로그아웃";
            this.btn_SignOut.UseVisualStyleBackColor = true;
            this.btn_SignOut.Click += new System.EventHandler(this.btn_SignOut_Click);
            // 
            // lb_RoomList
            // 
            this.lb_RoomList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_RoomList.Font = new System.Drawing.Font("굴림", 14F);
            this.lb_RoomList.FormattingEnabled = true;
            this.lb_RoomList.ItemHeight = 19;
            this.lb_RoomList.Location = new System.Drawing.Point(0, 192);
            this.lb_RoomList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_RoomList.Name = "lb_RoomList";
            this.lb_RoomList.Size = new System.Drawing.Size(335, 306);
            this.lb_RoomList.TabIndex = 5;
            this.lb_RoomList.DoubleClick += new System.EventHandler(this.lb_RoomList_DoubleClick);
            // 
            // lb_UserList
            // 
            this.lb_UserList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_UserList.Font = new System.Drawing.Font("굴림", 14F);
            this.lb_UserList.FormattingEnabled = true;
            this.lb_UserList.ItemHeight = 19;
            this.lb_UserList.Location = new System.Drawing.Point(0, 1);
            this.lb_UserList.Name = "lb_UserList";
            this.lb_UserList.Size = new System.Drawing.Size(335, 173);
            this.lb_UserList.TabIndex = 7;
            // 
            // LobbyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(334, 561);
            this.Controls.Add(this.lb_UserList);
            this.Controls.Add(this.btn_SignOut);
            this.Controls.Add(this.btn_CreateRoom);
            this.Controls.Add(this.lb_RoomList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "LobbyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "로비";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LobbyForm_FormClosing);
            this.Load += new System.EventHandler(this.LobbyForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_CreateRoom;
        private System.Windows.Forms.Button btn_SignOut;
        private System.Windows.Forms.ListBox lb_RoomList;
        private System.Windows.Forms.ListBox lb_UserList;
    }
}