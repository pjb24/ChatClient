
namespace TestClient
{
    partial class RoomManagerConfig
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
            this.clb_Manager = new System.Windows.Forms.CheckedListBox();
            this.clb_CommonUser = new System.Windows.Forms.CheckedListBox();
            this.btn_TurnOffManagementRight = new System.Windows.Forms.Button();
            this.btn_GrantManagementRight = new System.Windows.Forms.Button();
            this.btn_Submit = new System.Windows.Forms.Button();
            this.lbl_Manager = new System.Windows.Forms.Label();
            this.lbl_CommonUser = new System.Windows.Forms.Label();
            this.btn_Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clb_Manager
            // 
            this.clb_Manager.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clb_Manager.CheckOnClick = true;
            this.clb_Manager.Font = new System.Drawing.Font("굴림", 14F);
            this.clb_Manager.FormattingEnabled = true;
            this.clb_Manager.Location = new System.Drawing.Point(16, 42);
            this.clb_Manager.Name = "clb_Manager";
            this.clb_Manager.Size = new System.Drawing.Size(200, 178);
            this.clb_Manager.TabIndex = 0;
            // 
            // clb_CommonUser
            // 
            this.clb_CommonUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clb_CommonUser.CheckOnClick = true;
            this.clb_CommonUser.Font = new System.Drawing.Font("굴림", 14F);
            this.clb_CommonUser.FormattingEnabled = true;
            this.clb_CommonUser.Location = new System.Drawing.Point(16, 283);
            this.clb_CommonUser.Name = "clb_CommonUser";
            this.clb_CommonUser.Size = new System.Drawing.Size(200, 178);
            this.clb_CommonUser.TabIndex = 1;
            // 
            // btn_TurnOffManagementRight
            // 
            this.btn_TurnOffManagementRight.Location = new System.Drawing.Point(222, 42);
            this.btn_TurnOffManagementRight.Name = "btn_TurnOffManagementRight";
            this.btn_TurnOffManagementRight.Size = new System.Drawing.Size(100, 35);
            this.btn_TurnOffManagementRight.TabIndex = 2;
            this.btn_TurnOffManagementRight.Text = "관리자 권한\r\n해제";
            this.btn_TurnOffManagementRight.UseVisualStyleBackColor = true;
            this.btn_TurnOffManagementRight.Click += new System.EventHandler(this.btn_TurnOffManagementRight_Click);
            // 
            // btn_GrantManagementRight
            // 
            this.btn_GrantManagementRight.Font = new System.Drawing.Font("굴림", 9F);
            this.btn_GrantManagementRight.Location = new System.Drawing.Point(222, 283);
            this.btn_GrantManagementRight.Name = "btn_GrantManagementRight";
            this.btn_GrantManagementRight.Size = new System.Drawing.Size(100, 35);
            this.btn_GrantManagementRight.TabIndex = 3;
            this.btn_GrantManagementRight.Text = "관리자 권한\r\n부여";
            this.btn_GrantManagementRight.UseVisualStyleBackColor = true;
            this.btn_GrantManagementRight.Click += new System.EventHandler(this.btn_GrantManagementRight_Click);
            // 
            // btn_Submit
            // 
            this.btn_Submit.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_Submit.Location = new System.Drawing.Point(41, 491);
            this.btn_Submit.Name = "btn_Submit";
            this.btn_Submit.Size = new System.Drawing.Size(100, 35);
            this.btn_Submit.TabIndex = 4;
            this.btn_Submit.Text = "확인";
            this.btn_Submit.UseVisualStyleBackColor = true;
            this.btn_Submit.Click += new System.EventHandler(this.btn_Submit_Click);
            // 
            // lbl_Manager
            // 
            this.lbl_Manager.AutoSize = true;
            this.lbl_Manager.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_Manager.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_Manager.Location = new System.Drawing.Point(12, 9);
            this.lbl_Manager.Name = "lbl_Manager";
            this.lbl_Manager.Size = new System.Drawing.Size(129, 19);
            this.lbl_Manager.TabIndex = 5;
            this.lbl_Manager.Text = "채팅방 관리자";
            // 
            // lbl_CommonUser
            // 
            this.lbl_CommonUser.AutoSize = true;
            this.lbl_CommonUser.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_CommonUser.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_CommonUser.Location = new System.Drawing.Point(12, 252);
            this.lbl_CommonUser.Name = "lbl_CommonUser";
            this.lbl_CommonUser.Size = new System.Drawing.Size(91, 19);
            this.lbl_CommonUser.TabIndex = 6;
            this.lbl_CommonUser.Text = "일반 회원";
            // 
            // btn_Close
            // 
            this.btn_Close.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_Close.Location = new System.Drawing.Point(188, 491);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(100, 35);
            this.btn_Close.TabIndex = 7;
            this.btn_Close.Text = "닫기";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // RoomManagerConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(334, 561);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.lbl_CommonUser);
            this.Controls.Add(this.lbl_Manager);
            this.Controls.Add(this.btn_Submit);
            this.Controls.Add(this.btn_GrantManagementRight);
            this.Controls.Add(this.btn_TurnOffManagementRight);
            this.Controls.Add(this.clb_CommonUser);
            this.Controls.Add(this.clb_Manager);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RoomManagerConfig";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "관리자 설정";
            this.Load += new System.EventHandler(this.RoomManagerConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clb_Manager;
        private System.Windows.Forms.CheckedListBox clb_CommonUser;
        private System.Windows.Forms.Button btn_TurnOffManagementRight;
        private System.Windows.Forms.Button btn_GrantManagementRight;
        private System.Windows.Forms.Button btn_Submit;
        private System.Windows.Forms.Label lbl_Manager;
        private System.Windows.Forms.Label lbl_CommonUser;
        private System.Windows.Forms.Button btn_Close;
    }
}