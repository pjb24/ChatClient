
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
            this.lb_Result = new System.Windows.Forms.ListBox();
            this.txt_Send = new System.Windows.Forms.TextBox();
            this.btn_Send = new System.Windows.Forms.Button();
            this.lb_UserList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Leave = new System.Windows.Forms.Button();
            this.btn_Invitation = new System.Windows.Forms.Button();
            this.btn_SendFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_Result
            // 
            this.lb_Result.FormattingEnabled = true;
            this.lb_Result.ItemHeight = 12;
            this.lb_Result.Location = new System.Drawing.Point(0, 144);
            this.lb_Result.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_Result.Name = "lb_Result";
            this.lb_Result.Size = new System.Drawing.Size(335, 340);
            this.lb_Result.TabIndex = 1;
            // 
            // txt_Send
            // 
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
            this.lb_UserList.FormattingEnabled = true;
            this.lb_UserList.ItemHeight = 12;
            this.lb_UserList.Location = new System.Drawing.Point(0, 35);
            this.lb_UserList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_UserList.Name = "lb_UserList";
            this.lb_UserList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lb_UserList.Size = new System.Drawing.Size(216, 100);
            this.lb_UserList.TabIndex = 5;
            this.lb_UserList.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "채팅방 회원목록";
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
            this.btn_SendFile.Visible = false;
            this.btn_SendFile.Click += new System.EventHandler(this.btn_SendFile_Click);
            // 
            // ChatGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(334, 561);
            this.Controls.Add(this.btn_SendFile);
            this.Controls.Add(this.btn_Invitation);
            this.Controls.Add(this.btn_Leave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_UserList);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.txt_Send);
            this.Controls.Add(this.lb_Result);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "ChatGroupForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "채팅방";
            this.Load += new System.EventHandler(this.ChatGroupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lb_Result;
        private System.Windows.Forms.TextBox txt_Send;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.ListBox lb_UserList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Leave;
        private System.Windows.Forms.Button btn_Invitation;
        private System.Windows.Forms.Button btn_SendFile;
    }
}