
namespace TestClient
{
    partial class RegisterForm
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
            this.btn_Submit = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_ID
            // 
            this.txt_ID.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txt_ID.Location = new System.Drawing.Point(54, 171);
            this.txt_ID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_ID.Name = "txt_ID";
            this.txt_ID.Size = new System.Drawing.Size(232, 25);
            this.txt_ID.TabIndex = 1;
            this.txt_ID.Text = "ID";
            this.txt_ID.Enter += new System.EventHandler(this.txt_ID_Enter);
            this.txt_ID.Leave += new System.EventHandler(this.txt_ID_Leave);
            // 
            // txt_PW
            // 
            this.txt_PW.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txt_PW.Location = new System.Drawing.Point(54, 239);
            this.txt_PW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_PW.Name = "txt_PW";
            this.txt_PW.Size = new System.Drawing.Size(232, 25);
            this.txt_PW.TabIndex = 2;
            this.txt_PW.Text = "PW";
            this.txt_PW.Enter += new System.EventHandler(this.txt_PW_Enter);
            this.txt_PW.Leave += new System.EventHandler(this.txt_PW_Leave);
            // 
            // btn_Submit
            // 
            this.btn_Submit.Location = new System.Drawing.Point(54, 316);
            this.btn_Submit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Submit.Name = "btn_Submit";
            this.btn_Submit.Size = new System.Drawing.Size(75, 31);
            this.btn_Submit.TabIndex = 3;
            this.btn_Submit.Text = "등록";
            this.btn_Submit.UseVisualStyleBackColor = true;
            this.btn_Submit.Click += new System.EventHandler(this.btn_Submit_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(211, 316);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 31);
            this.btn_Close.TabIndex = 0;
            this.btn_Close.Text = "닫기";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 553);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Submit);
            this.Controls.Add(this.txt_PW);
            this.Controls.Add(this.txt_ID);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "RegisterForm";
            this.Text = "회원가입";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_ID;
        private System.Windows.Forms.TextBox txt_PW;
        private System.Windows.Forms.Button btn_Submit;
        private System.Windows.Forms.Button btn_Close;
    }
}