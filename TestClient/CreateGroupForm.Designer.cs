
namespace TestClient
{
    partial class CreateGroupForm
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
            this.btn_Create = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.clb_GroupUser = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // btn_Create
            // 
            this.btn_Create.Location = new System.Drawing.Point(12, 515);
            this.btn_Create.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(100, 35);
            this.btn_Create.TabIndex = 0;
            this.btn_Create.Text = "생성";
            this.btn_Create.UseVisualStyleBackColor = true;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(222, 515);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(100, 35);
            this.btn_Close.TabIndex = 1;
            this.btn_Close.Text = "닫기";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // clb_GroupUser
            // 
            this.clb_GroupUser.CheckOnClick = true;
            this.clb_GroupUser.FormattingEnabled = true;
            this.clb_GroupUser.Location = new System.Drawing.Point(0, 0);
            this.clb_GroupUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clb_GroupUser.Name = "clb_GroupUser";
            this.clb_GroupUser.Size = new System.Drawing.Size(335, 484);
            this.clb_GroupUser.TabIndex = 2;
            // 
            // CreateGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 561);
            this.Controls.Add(this.clb_GroupUser);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Create);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CreateGroupForm";
            this.Text = "채팅방 생성";
            this.Load += new System.EventHandler(this.CreateGroupForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Create;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.CheckedListBox clb_GroupUser;
    }
}