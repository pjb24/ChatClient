﻿
namespace TestClient
{
    partial class ChatGroupForm
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
            this.btn_Close = new System.Windows.Forms.Button();
            this.lb_Result = new System.Windows.Forms.ListBox();
            this.txt_Send = new System.Windows.Forms.TextBox();
            this.btn_Send = new System.Windows.Forms.Button();
            this.lbl_User = new System.Windows.Forms.Label();
            this.lb_UserList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(650, 313);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 32);
            this.btn_Close.TabIndex = 0;
            this.btn_Close.Text = "닫기";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // lb_Result
            // 
            this.lb_Result.FormattingEnabled = true;
            this.lb_Result.ItemHeight = 15;
            this.lb_Result.Location = new System.Drawing.Point(12, 77);
            this.lb_Result.Name = "lb_Result";
            this.lb_Result.Size = new System.Drawing.Size(713, 214);
            this.lb_Result.TabIndex = 1;
            // 
            // txt_Send
            // 
            this.txt_Send.Location = new System.Drawing.Point(12, 320);
            this.txt_Send.Name = "txt_Send";
            this.txt_Send.Size = new System.Drawing.Size(512, 25);
            this.txt_Send.TabIndex = 2;
            this.txt_Send.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Send_KeyDown);
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(553, 314);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(75, 32);
            this.btn_Send.TabIndex = 3;
            this.btn_Send.Text = "Send";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // lbl_User
            // 
            this.lbl_User.AutoSize = true;
            this.lbl_User.Location = new System.Drawing.Point(32, 26);
            this.lbl_User.Name = "lbl_User";
            this.lbl_User.Size = new System.Drawing.Size(45, 15);
            this.lbl_User.TabIndex = 4;
            this.lbl_User.Text = "label1";
            // 
            // lb_UserList
            // 
            this.lb_UserList.FormattingEnabled = true;
            this.lb_UserList.ItemHeight = 15;
            this.lb_UserList.Location = new System.Drawing.Point(543, 77);
            this.lb_UserList.Name = "lb_UserList";
            this.lb_UserList.Size = new System.Drawing.Size(182, 214);
            this.lb_UserList.TabIndex = 5;
            this.lb_UserList.Visible = false;
            // 
            // ChatGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 373);
            this.Controls.Add(this.lb_UserList);
            this.Controls.Add(this.lbl_User);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.txt_Send);
            this.Controls.Add(this.lb_Result);
            this.Controls.Add(this.btn_Close);
            this.Name = "ChatGroupForm";
            this.Text = "ChatGroupForm";
            this.Load += new System.EventHandler(this.ChatGroupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.ListBox lb_Result;
        private System.Windows.Forms.TextBox txt_Send;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.Label lbl_User;
        private System.Windows.Forms.ListBox lb_UserList;
    }
}