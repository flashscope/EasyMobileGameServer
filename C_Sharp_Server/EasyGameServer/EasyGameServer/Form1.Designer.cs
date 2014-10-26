namespace EasyGameServer
{
    partial class Form1
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.bu_server_start = new System.Windows.Forms.Button();
            this.bu_test = new System.Windows.Forms.Button();
            this.bu_console_clear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bu_server_start
            // 
            this.bu_server_start.Location = new System.Drawing.Point(84, 12);
            this.bu_server_start.Name = "bu_server_start";
            this.bu_server_start.Size = new System.Drawing.Size(106, 23);
            this.bu_server_start.TabIndex = 0;
            this.bu_server_start.Text = "server_start";
            this.bu_server_start.UseVisualStyleBackColor = true;
            this.bu_server_start.Click += new System.EventHandler(this.bu_server_start_Click);
            // 
            // bu_test
            // 
            this.bu_test.Location = new System.Drawing.Point(197, 173);
            this.bu_test.Name = "bu_test";
            this.bu_test.Size = new System.Drawing.Size(75, 23);
            this.bu_test.TabIndex = 1;
            this.bu_test.Text = "test";
            this.bu_test.UseVisualStyleBackColor = true;
            this.bu_test.Click += new System.EventHandler(this.bu_test_Click);
            // 
            // bu_console_clear
            // 
            this.bu_console_clear.Location = new System.Drawing.Point(84, 227);
            this.bu_console_clear.Name = "bu_console_clear";
            this.bu_console_clear.Size = new System.Drawing.Size(106, 23);
            this.bu_console_clear.TabIndex = 2;
            this.bu_console_clear.Text = "console_clear";
            this.bu_console_clear.UseVisualStyleBackColor = true;
            this.bu_console_clear.Click += new System.EventHandler(this.bu_console_clear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.bu_console_clear);
            this.Controls.Add(this.bu_test);
            this.Controls.Add(this.bu_server_start);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bu_server_start;
        private System.Windows.Forms.Button bu_test;
        private System.Windows.Forms.Button bu_console_clear;
    }
}

