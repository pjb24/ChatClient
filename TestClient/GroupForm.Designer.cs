﻿
namespace TestClient
{
    partial class GroupForm
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
            this.btn_CreateGroup = new System.Windows.Forms.Button();
            this.btn_SignOut = new System.Windows.Forms.Button();
            this.btn_PullGroup = new System.Windows.Forms.Button();
            this.btn_PullUser = new System.Windows.Forms.Button();
            this.lbl_UserID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_CreateGroup
            // 
            this.btn_CreateGroup.Location = new System.Drawing.Point(195, 287);
            this.btn_CreateGroup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_CreateGroup.Name = "btn_CreateGroup";
            this.btn_CreateGroup.Size = new System.Drawing.Size(103, 36);
            this.btn_CreateGroup.TabIndex = 0;
            this.btn_CreateGroup.Text = "채팅방 생성";
            this.btn_CreateGroup.UseVisualStyleBackColor = true;
            this.btn_CreateGroup.Click += new System.EventHandler(this.btn_CreateGroup_Click);
            // 
            // btn_SignOut
            // 
            this.btn_SignOut.Location = new System.Drawing.Point(486, 287);
            this.btn_SignOut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_SignOut.Name = "btn_SignOut";
            this.btn_SignOut.Size = new System.Drawing.Size(66, 36);
            this.btn_SignOut.TabIndex = 1;
            this.btn_SignOut.Text = "Sign out";
            this.btn_SignOut.UseVisualStyleBackColor = true;
            this.btn_SignOut.Visible = false;
            this.btn_SignOut.Click += new System.EventHandler(this.btn_SignOut_Click);
            // 
            // btn_PullGroup
            // 
            this.btn_PullGroup.Location = new System.Drawing.Point(337, 287);
            this.btn_PullGroup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_PullGroup.Name = "btn_PullGroup";
            this.btn_PullGroup.Size = new System.Drawing.Size(113, 36);
            this.btn_PullGroup.TabIndex = 2;
            this.btn_PullGroup.Text = "채팅방 가져오기";
            this.btn_PullGroup.UseVisualStyleBackColor = true;
            this.btn_PullGroup.Click += new System.EventHandler(this.btn_PullGroup_Click);
            // 
            // btn_PullUser
            // 
            this.btn_PullUser.Location = new System.Drawing.Point(44, 287);
            this.btn_PullUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_PullUser.Name = "btn_PullUser";
            this.btn_PullUser.Size = new System.Drawing.Size(116, 36);
            this.btn_PullUser.TabIndex = 3;
            this.btn_PullUser.Text = "회원목록 가져오기";
            this.btn_PullUser.UseVisualStyleBackColor = true;
            this.btn_PullUser.Click += new System.EventHandler(this.btn_PullUser_Click);
            // 
            // lbl_UserID
            // 
            this.lbl_UserID.AutoSize = true;
            this.lbl_UserID.Location = new System.Drawing.Point(286, 41);
            this.lbl_UserID.Name = "lbl_UserID";
            this.lbl_UserID.Size = new System.Drawing.Size(38, 12);
            this.lbl_UserID.TabIndex = 4;
            this.lbl_UserID.Text = "label1";
            // 
            // GroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 360);
            this.Controls.Add(this.lbl_UserID);
            this.Controls.Add(this.btn_PullUser);
            this.Controls.Add(this.btn_PullGroup);
            this.Controls.Add(this.btn_SignOut);
            this.Controls.Add(this.btn_CreateGroup);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "GroupForm";
            this.Text = "채팅방";
            this.Load += new System.EventHandler(this.GroupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_CreateGroup;
        private System.Windows.Forms.Button btn_SignOut;
        private System.Windows.Forms.Button btn_PullGroup;
        private System.Windows.Forms.Button btn_PullUser;
        private System.Windows.Forms.Label lbl_UserID;
    }
}