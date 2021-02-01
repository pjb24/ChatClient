
namespace TestClient
{
    partial class TestClientUI
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_Result = new System.Windows.Forms.ListBox();
            this.txt_UserID = new System.Windows.Forms.TextBox();
            this.txt_UserPW = new System.Windows.Forms.TextBox();
            this.btn_SignInSubmit = new System.Windows.Forms.Button();
            this.btn_SignInRegister = new System.Windows.Forms.Button();
            this.btn_RegisterSubmit = new System.Windows.Forms.Button();
            this.btn_RegisterClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_Result
            // 
            this.lb_Result.FormattingEnabled = true;
            this.lb_Result.ItemHeight = 15;
            this.lb_Result.Location = new System.Drawing.Point(1582, 778);
            this.lb_Result.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_Result.Name = "lb_Result";
            this.lb_Result.Size = new System.Drawing.Size(308, 244);
            this.lb_Result.TabIndex = 0;
            // 
            // txt_UserID
            // 
            this.txt_UserID.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txt_UserID.Location = new System.Drawing.Point(78, 263);
            this.txt_UserID.Name = "txt_UserID";
            this.txt_UserID.Size = new System.Drawing.Size(180, 25);
            this.txt_UserID.TabIndex = 1;
            this.txt_UserID.Text = "ID";
            this.txt_UserID.Enter += new System.EventHandler(this.txt_UserID_Enter);
            this.txt_UserID.Leave += new System.EventHandler(this.txt_UserID_Leave);
            // 
            // txt_UserPW
            // 
            this.txt_UserPW.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txt_UserPW.Location = new System.Drawing.Point(78, 310);
            this.txt_UserPW.Name = "txt_UserPW";
            this.txt_UserPW.Size = new System.Drawing.Size(180, 25);
            this.txt_UserPW.TabIndex = 2;
            this.txt_UserPW.Text = "PW";
            this.txt_UserPW.Enter += new System.EventHandler(this.txt_UserPW_Enter);
            this.txt_UserPW.Leave += new System.EventHandler(this.txt_UserPW_Leave);
            // 
            // btn_SignInSubmit
            // 
            this.btn_SignInSubmit.Location = new System.Drawing.Point(78, 361);
            this.btn_SignInSubmit.Name = "btn_SignInSubmit";
            this.btn_SignInSubmit.Size = new System.Drawing.Size(180, 40);
            this.btn_SignInSubmit.TabIndex = 3;
            this.btn_SignInSubmit.Text = "로그인";
            this.btn_SignInSubmit.UseVisualStyleBackColor = true;
            this.btn_SignInSubmit.Click += new System.EventHandler(this.btn_SignIn_Click);
            // 
            // btn_SignInRegister
            // 
            this.btn_SignInRegister.Location = new System.Drawing.Point(78, 441);
            this.btn_SignInRegister.Name = "btn_SignInRegister";
            this.btn_SignInRegister.Size = new System.Drawing.Size(180, 40);
            this.btn_SignInRegister.TabIndex = 3;
            this.btn_SignInRegister.Text = "회원가입";
            this.btn_SignInRegister.UseVisualStyleBackColor = true;
            this.btn_SignInRegister.Click += new System.EventHandler(this.btn_Register_Click);
            // 
            // btn_RegisterSubmit
            // 
            this.btn_RegisterSubmit.Location = new System.Drawing.Point(552, 399);
            this.btn_RegisterSubmit.Name = "btn_RegisterSubmit";
            this.btn_RegisterSubmit.Size = new System.Drawing.Size(75, 40);
            this.btn_RegisterSubmit.TabIndex = 4;
            this.btn_RegisterSubmit.Text = "등록";
            this.btn_RegisterSubmit.UseVisualStyleBackColor = true;
            // 
            // btn_RegisterClose
            // 
            this.btn_RegisterClose.Location = new System.Drawing.Point(682, 399);
            this.btn_RegisterClose.Name = "btn_RegisterClose";
            this.btn_RegisterClose.Size = new System.Drawing.Size(75, 40);
            this.btn_RegisterClose.TabIndex = 5;
            this.btn_RegisterClose.Text = "닫기";
            this.btn_RegisterClose.UseVisualStyleBackColor = true;
            // 
            // TestClientUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.btn_RegisterClose);
            this.Controls.Add(this.btn_RegisterSubmit);
            this.Controls.Add(this.btn_SignInRegister);
            this.Controls.Add(this.btn_SignInSubmit);
            this.Controls.Add(this.txt_UserPW);
            this.Controls.Add(this.txt_UserID);
            this.Controls.Add(this.lb_Result);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TestClientUI";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.TestClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ListBox lb_Result;
        private System.Windows.Forms.TextBox txt_UserID;
        private System.Windows.Forms.TextBox txt_UserPW;
        private System.Windows.Forms.Button btn_SignInSubmit;
        private System.Windows.Forms.Button btn_SignInRegister;
        private System.Windows.Forms.Button btn_RegisterSubmit;
        private System.Windows.Forms.Button btn_RegisterClose;
    }
}

