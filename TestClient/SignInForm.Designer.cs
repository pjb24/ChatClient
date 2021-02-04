
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
            this.txt_ID = new System.Windows.Forms.TextBox();
            this.txt_PW = new System.Windows.Forms.TextBox();
            this.btn_SignIn = new System.Windows.Forms.Button();
            this.btn_Register = new System.Windows.Forms.Button();
            this.lbl_SignIn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_ID
            // 
            this.txt_ID.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txt_ID.Location = new System.Drawing.Point(87, 256);
            this.txt_ID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_ID.Name = "txt_ID";
            this.txt_ID.Size = new System.Drawing.Size(160, 21);
            this.txt_ID.TabIndex = 1;
            this.txt_ID.Text = "ID";
            this.txt_ID.Enter += new System.EventHandler(this.txt_ID_Enter);
            this.txt_ID.Leave += new System.EventHandler(this.txt_ID_Leave);
            // 
            // txt_PW
            // 
            this.txt_PW.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txt_PW.Location = new System.Drawing.Point(87, 295);
            this.txt_PW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_PW.Name = "txt_PW";
            this.txt_PW.Size = new System.Drawing.Size(160, 21);
            this.txt_PW.TabIndex = 2;
            this.txt_PW.Text = "PW";
            this.txt_PW.Enter += new System.EventHandler(this.txt_PW_Enter);
            this.txt_PW.Leave += new System.EventHandler(this.txt_PW_Leave);
            // 
            // btn_SignIn
            // 
            this.btn_SignIn.Location = new System.Drawing.Point(87, 354);
            this.btn_SignIn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_SignIn.Name = "btn_SignIn";
            this.btn_SignIn.Size = new System.Drawing.Size(160, 30);
            this.btn_SignIn.TabIndex = 0;
            this.btn_SignIn.Text = "로그인";
            this.btn_SignIn.UseVisualStyleBackColor = true;
            this.btn_SignIn.Click += new System.EventHandler(this.btn_SignIn_Click);
            // 
            // btn_Register
            // 
            this.btn_Register.Location = new System.Drawing.Point(87, 404);
            this.btn_Register.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Register.Name = "btn_Register";
            this.btn_Register.Size = new System.Drawing.Size(160, 30);
            this.btn_Register.TabIndex = 3;
            this.btn_Register.Text = "회원가입";
            this.btn_Register.UseVisualStyleBackColor = true;
            this.btn_Register.Click += new System.EventHandler(this.btn_Register_Click);
            // 
            // lbl_SignIn
            // 
            this.lbl_SignIn.AutoSize = true;
            this.lbl_SignIn.Font = new System.Drawing.Font("굴림", 24F);
            this.lbl_SignIn.Location = new System.Drawing.Point(112, 112);
            this.lbl_SignIn.Name = "lbl_SignIn";
            this.lbl_SignIn.Size = new System.Drawing.Size(111, 32);
            this.lbl_SignIn.TabIndex = 4;
            this.lbl_SignIn.Text = "로그인";
            // 
            // SignInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 561);
            this.Controls.Add(this.lbl_SignIn);
            this.Controls.Add(this.btn_Register);
            this.Controls.Add(this.btn_SignIn);
            this.Controls.Add(this.txt_PW);
            this.Controls.Add(this.txt_ID);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SignInForm";
            this.Text = "로그인";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SignInForm_FormClosed);
            this.Load += new System.EventHandler(this.SignInForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_ID;
        private System.Windows.Forms.TextBox txt_PW;
        private System.Windows.Forms.Button btn_SignIn;
        private System.Windows.Forms.Button btn_Register;
        private System.Windows.Forms.Label lbl_SignIn;
    }
}