
namespace TestClient
{
    partial class InvitationForm
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
            this.btn_InvitationClose = new System.Windows.Forms.Button();
            this.btn_Invitation = new System.Windows.Forms.Button();
            this.clb_InviteUser = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // btn_InvitationClose
            // 
            this.btn_InvitationClose.Location = new System.Drawing.Point(225, 511);
            this.btn_InvitationClose.Name = "btn_InvitationClose";
            this.btn_InvitationClose.Size = new System.Drawing.Size(110, 45);
            this.btn_InvitationClose.TabIndex = 16;
            this.btn_InvitationClose.TabStop = false;
            this.btn_InvitationClose.Text = "취소";
            this.btn_InvitationClose.UseVisualStyleBackColor = true;
            this.btn_InvitationClose.Click += new System.EventHandler(this.btn_InvitationClose_Click);
            // 
            // btn_Invitation
            // 
            this.btn_Invitation.Location = new System.Drawing.Point(0, 510);
            this.btn_Invitation.Name = "btn_Invitation";
            this.btn_Invitation.Size = new System.Drawing.Size(110, 45);
            this.btn_Invitation.TabIndex = 15;
            this.btn_Invitation.TabStop = false;
            this.btn_Invitation.Text = "초대";
            this.btn_Invitation.UseVisualStyleBackColor = true;
            this.btn_Invitation.Click += new System.EventHandler(this.btn_Invitation_Click);
            // 
            // clb_InviteUser
            // 
            this.clb_InviteUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clb_InviteUser.CheckOnClick = true;
            this.clb_InviteUser.FormattingEnabled = true;
            this.clb_InviteUser.Location = new System.Drawing.Point(0, 4);
            this.clb_InviteUser.Name = "clb_InviteUser";
            this.clb_InviteUser.Size = new System.Drawing.Size(335, 482);
            this.clb_InviteUser.TabIndex = 14;
            this.clb_InviteUser.TabStop = false;
            // 
            // InvitationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(334, 561);
            this.Controls.Add(this.btn_InvitationClose);
            this.Controls.Add(this.btn_Invitation);
            this.Controls.Add(this.clb_InviteUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "InvitationForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "채팅방 초대";
            this.Load += new System.EventHandler(this.InvitationForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_InvitationClose;
        private System.Windows.Forms.Button btn_Invitation;
        private System.Windows.Forms.CheckedListBox clb_InviteUser;
    }
}