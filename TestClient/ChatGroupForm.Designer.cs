
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
            this.btn_Close.Location = new System.Drawing.Point(569, 250);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(66, 26);
            this.btn_Close.TabIndex = 0;
            this.btn_Close.Text = "닫기";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // lb_Result
            // 
            this.lb_Result.FormattingEnabled = true;
            this.lb_Result.ItemHeight = 12;
            this.lb_Result.Location = new System.Drawing.Point(10, 62);
            this.lb_Result.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_Result.Name = "lb_Result";
            this.lb_Result.Size = new System.Drawing.Size(624, 172);
            this.lb_Result.TabIndex = 1;
            // 
            // txt_Send
            // 
            this.txt_Send.Location = new System.Drawing.Point(10, 256);
            this.txt_Send.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Send.Name = "txt_Send";
            this.txt_Send.Size = new System.Drawing.Size(448, 21);
            this.txt_Send.TabIndex = 2;
            this.txt_Send.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Send_KeyDown);
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(484, 251);
            this.btn_Send.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(66, 26);
            this.btn_Send.TabIndex = 3;
            this.btn_Send.Text = "전송";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // lbl_User
            // 
            this.lbl_User.AutoSize = true;
            this.lbl_User.Location = new System.Drawing.Point(28, 21);
            this.lbl_User.Name = "lbl_User";
            this.lbl_User.Size = new System.Drawing.Size(38, 12);
            this.lbl_User.TabIndex = 4;
            this.lbl_User.Text = "label1";
            // 
            // lb_UserList
            // 
            this.lb_UserList.FormattingEnabled = true;
            this.lb_UserList.ItemHeight = 12;
            this.lb_UserList.Location = new System.Drawing.Point(475, 62);
            this.lb_UserList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_UserList.Name = "lb_UserList";
            this.lb_UserList.Size = new System.Drawing.Size(160, 172);
            this.lb_UserList.TabIndex = 5;
            this.lb_UserList.Visible = false;
            // 
            // ChatGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 298);
            this.Controls.Add(this.lb_UserList);
            this.Controls.Add(this.lbl_User);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.txt_Send);
            this.Controls.Add(this.lb_Result);
            this.Controls.Add(this.btn_Close);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ChatGroupForm";
            this.Text = "채팅방";
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