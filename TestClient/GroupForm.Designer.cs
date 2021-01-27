
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
            this.SuspendLayout();
            // 
            // btn_CreateGroup
            // 
            this.btn_CreateGroup.Location = new System.Drawing.Point(411, 359);
            this.btn_CreateGroup.Name = "btn_CreateGroup";
            this.btn_CreateGroup.Size = new System.Drawing.Size(124, 45);
            this.btn_CreateGroup.TabIndex = 0;
            this.btn_CreateGroup.Text = "Create Group";
            this.btn_CreateGroup.UseVisualStyleBackColor = true;
            this.btn_CreateGroup.Click += new System.EventHandler(this.btn_CreateGroup_Click);
            // 
            // btn_SignOut
            // 
            this.btn_SignOut.Location = new System.Drawing.Point(582, 381);
            this.btn_SignOut.Name = "btn_SignOut";
            this.btn_SignOut.Size = new System.Drawing.Size(75, 23);
            this.btn_SignOut.TabIndex = 1;
            this.btn_SignOut.Text = "Sign out";
            this.btn_SignOut.UseVisualStyleBackColor = true;
            this.btn_SignOut.Click += new System.EventHandler(this.btn_SignOut_Click);
            // 
            // btn_PullGroup
            // 
            this.btn_PullGroup.Location = new System.Drawing.Point(107, 381);
            this.btn_PullGroup.Name = "btn_PullGroup";
            this.btn_PullGroup.Size = new System.Drawing.Size(102, 23);
            this.btn_PullGroup.TabIndex = 2;
            this.btn_PullGroup.Text = "Pull Group";
            this.btn_PullGroup.UseVisualStyleBackColor = true;
            this.btn_PullGroup.Click += new System.EventHandler(this.btn_PullGroup_Click);
            // 
            // btn_PullUser
            // 
            this.btn_PullUser.Location = new System.Drawing.Point(260, 381);
            this.btn_PullUser.Name = "btn_PullUser";
            this.btn_PullUser.Size = new System.Drawing.Size(93, 23);
            this.btn_PullUser.TabIndex = 3;
            this.btn_PullUser.Text = "Pull User";
            this.btn_PullUser.UseVisualStyleBackColor = true;
            this.btn_PullUser.Click += new System.EventHandler(this.btn_PullUser_Click);
            // 
            // GroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_PullUser);
            this.Controls.Add(this.btn_PullGroup);
            this.Controls.Add(this.btn_SignOut);
            this.Controls.Add(this.btn_CreateGroup);
            this.Name = "GroupForm";
            this.Text = "GroupForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_CreateGroup;
        private System.Windows.Forms.Button btn_SignOut;
        private System.Windows.Forms.Button btn_PullGroup;
        private System.Windows.Forms.Button btn_PullUser;
    }
}