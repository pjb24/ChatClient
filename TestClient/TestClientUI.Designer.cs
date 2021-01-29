
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
            this.txt_Send = new System.Windows.Forms.TextBox();
            this.btn_Send = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.btn_Register = new System.Windows.Forms.Button();
            this.btn_SignIn = new System.Windows.Forms.Button();
            this.btn_Group = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_Result
            // 
            this.lb_Result.FormattingEnabled = true;
            this.lb_Result.ItemHeight = 12;
            this.lb_Result.Location = new System.Drawing.Point(10, 10);
            this.lb_Result.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_Result.Name = "lb_Result";
            this.lb_Result.Size = new System.Drawing.Size(668, 196);
            this.lb_Result.TabIndex = 0;
            // 
            // txt_Send
            // 
            this.txt_Send.Location = new System.Drawing.Point(10, 184);
            this.txt_Send.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Send.Name = "txt_Send";
            this.txt_Send.Size = new System.Drawing.Size(547, 21);
            this.txt_Send.TabIndex = 1;
            this.txt_Send.Visible = false;
            this.txt_Send.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Send_KeyDown);
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(589, 182);
            this.btn_Send.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(89, 20);
            this.btn_Send.TabIndex = 3;
            this.btn_Send.Text = "전송";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Visible = false;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(589, 224);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(89, 25);
            this.btn_Close.TabIndex = 3;
            this.btn_Close.Text = "닫기";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // btn_Register
            // 
            this.btn_Register.Location = new System.Drawing.Point(28, 224);
            this.btn_Register.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Register.Name = "btn_Register";
            this.btn_Register.Size = new System.Drawing.Size(66, 18);
            this.btn_Register.TabIndex = 4;
            this.btn_Register.Text = "Register";
            this.btn_Register.UseVisualStyleBackColor = true;
            this.btn_Register.Visible = false;
            // 
            // btn_SignIn
            // 
            this.btn_SignIn.Location = new System.Drawing.Point(10, 222);
            this.btn_SignIn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_SignIn.Name = "btn_SignIn";
            this.btn_SignIn.Size = new System.Drawing.Size(71, 27);
            this.btn_SignIn.TabIndex = 5;
            this.btn_SignIn.Text = "로그인";
            this.btn_SignIn.UseVisualStyleBackColor = true;
            this.btn_SignIn.Click += new System.EventHandler(this.btn_SignIn_Click);
            // 
            // btn_Group
            // 
            this.btn_Group.Location = new System.Drawing.Point(120, 224);
            this.btn_Group.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Group.Name = "btn_Group";
            this.btn_Group.Size = new System.Drawing.Size(66, 18);
            this.btn_Group.TabIndex = 6;
            this.btn_Group.Text = "Group";
            this.btn_Group.UseVisualStyleBackColor = true;
            this.btn_Group.Visible = false;
            // 
            // TestClientUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 274);
            this.Controls.Add(this.btn_Group);
            this.Controls.Add(this.btn_SignIn);
            this.Controls.Add(this.btn_Register);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.txt_Send);
            this.Controls.Add(this.lb_Result);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TestClientUI";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.TestClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_Send;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.Button btn_Close;
        public System.Windows.Forms.ListBox lb_Result;
        private System.Windows.Forms.Button btn_Register;
        private System.Windows.Forms.Button btn_SignIn;
        private System.Windows.Forms.Button btn_Group;
    }
}

