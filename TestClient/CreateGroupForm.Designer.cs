
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
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
            this.clb_GroupUser.Location = new System.Drawing.Point(0, 112);
            this.clb_GroupUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clb_GroupUser.Name = "clb_GroupUser";
            this.clb_GroupUser.Size = new System.Drawing.Size(335, 388);
            this.clb_GroupUser.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 14F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "채팅방 이름";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 14F);
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "채팅방 공개 설정";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("굴림", 14F);
            this.textBox1.Location = new System.Drawing.Point(16, 37);
            this.textBox1.MaxLength = 20;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(306, 29);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("굴림", 14F);
            this.radioButton1.Location = new System.Drawing.Point(167, 75);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(65, 23);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "공개";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("굴림", 14F);
            this.radioButton2.Location = new System.Drawing.Point(238, 75);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(84, 23);
            this.radioButton2.TabIndex = 7;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "비공개";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // CreateGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 561);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clb_GroupUser);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Create);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CreateGroupForm";
            this.Text = "채팅방 생성";
            this.Load += new System.EventHandler(this.CreateGroupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Create;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.CheckedListBox clb_GroupUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}