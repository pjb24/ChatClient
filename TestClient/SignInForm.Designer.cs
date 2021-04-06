
namespace TestClient
{
    partial class SignInForm
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
            this.txt_UserID = new System.Windows.Forms.TextBox();
            this.txt_UserPW = new System.Windows.Forms.TextBox();
            this.btn_SignIn = new System.Windows.Forms.Button();
            this.btn_Register = new System.Windows.Forms.Button();
            this.lbl_SignIn = new System.Windows.Forms.Label();
            this.lbl_UserID = new System.Windows.Forms.Label();
            this.lbl_UserPW = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_UserID
            // 
            this.txt_UserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_UserID.Font = new System.Drawing.Font("굴림", 14F);
            this.txt_UserID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txt_UserID.Location = new System.Drawing.Point(87, 256);
            this.txt_UserID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_UserID.Name = "txt_UserID";
            this.txt_UserID.Size = new System.Drawing.Size(160, 29);
            this.txt_UserID.TabIndex = 1;
            this.txt_UserID.TextChanged += new System.EventHandler(this.txt_UserID_TextChanged);
            this.txt_UserID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_UserID_KeyDown);
            // 
            // txt_UserPW
            // 
            this.txt_UserPW.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_UserPW.Font = new System.Drawing.Font("굴림", 14F);
            this.txt_UserPW.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txt_UserPW.Location = new System.Drawing.Point(87, 295);
            this.txt_UserPW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_UserPW.Name = "txt_UserPW";
            this.txt_UserPW.PasswordChar = '*';
            this.txt_UserPW.Size = new System.Drawing.Size(160, 29);
            this.txt_UserPW.TabIndex = 2;
            this.txt_UserPW.TextChanged += new System.EventHandler(this.txt_UserPW_TextChanged);
            this.txt_UserPW.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_UserPW_KeyDown);
            // 
            // btn_SignIn
            // 
            this.btn_SignIn.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_SignIn.Location = new System.Drawing.Point(87, 354);
            this.btn_SignIn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_SignIn.Name = "btn_SignIn";
            this.btn_SignIn.Size = new System.Drawing.Size(160, 30);
            this.btn_SignIn.TabIndex = 3;
            this.btn_SignIn.Text = "로그인";
            this.btn_SignIn.UseVisualStyleBackColor = true;
            this.btn_SignIn.Click += new System.EventHandler(this.btn_SignIn_Click);
            // 
            // btn_Register
            // 
            this.btn_Register.Font = new System.Drawing.Font("굴림", 14F);
            this.btn_Register.Location = new System.Drawing.Point(87, 404);
            this.btn_Register.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Register.Name = "btn_Register";
            this.btn_Register.Size = new System.Drawing.Size(160, 30);
            this.btn_Register.TabIndex = 4;
            this.btn_Register.Text = "회원가입";
            this.btn_Register.UseVisualStyleBackColor = true;
            this.btn_Register.Click += new System.EventHandler(this.btn_Register_Click);
            // 
            // lbl_SignIn
            // 
            this.lbl_SignIn.AutoSize = true;
            this.lbl_SignIn.Font = new System.Drawing.Font("굴림", 24F);
            this.lbl_SignIn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_SignIn.Location = new System.Drawing.Point(112, 112);
            this.lbl_SignIn.Name = "lbl_SignIn";
            this.lbl_SignIn.Size = new System.Drawing.Size(111, 32);
            this.lbl_SignIn.TabIndex = 4;
            this.lbl_SignIn.Text = "로그인";
            // 
            // lbl_UserID
            // 
            this.lbl_UserID.AutoSize = true;
            this.lbl_UserID.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_UserID.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_UserID.Location = new System.Drawing.Point(58, 258);
            this.lbl_UserID.Name = "lbl_UserID";
            this.lbl_UserID.Size = new System.Drawing.Size(23, 19);
            this.lbl_UserID.TabIndex = 22;
            this.lbl_UserID.Text = "ID";
            // 
            // lbl_UserPW
            // 
            this.lbl_UserPW.AutoSize = true;
            this.lbl_UserPW.Font = new System.Drawing.Font("굴림", 14F);
            this.lbl_UserPW.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_UserPW.Location = new System.Drawing.Point(46, 297);
            this.lbl_UserPW.Name = "lbl_UserPW";
            this.lbl_UserPW.Size = new System.Drawing.Size(35, 19);
            this.lbl_UserPW.TabIndex = 23;
            this.lbl_UserPW.Text = "PW";
            // 
            // SignInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(334, 561);
            this.Controls.Add(this.lbl_UserPW);
            this.Controls.Add(this.lbl_UserID);
            this.Controls.Add(this.lbl_SignIn);
            this.Controls.Add(this.btn_Register);
            this.Controls.Add(this.btn_SignIn);
            this.Controls.Add(this.txt_UserPW);
            this.Controls.Add(this.txt_UserID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "SignInForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "로그인";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SignInForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_UserID;
        private System.Windows.Forms.TextBox txt_UserPW;
        private System.Windows.Forms.Button btn_SignIn;
        private System.Windows.Forms.Button btn_Register;
        private System.Windows.Forms.Label lbl_SignIn;
        private System.Windows.Forms.Label lbl_UserID;
        private System.Windows.Forms.Label lbl_UserPW;
    }
}