namespace ServerSuperIO.Tool
{
    partial class AddForm
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.btAddServer = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Top;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(684, 520);
            this.propertyGrid1.TabIndex = 0;
            // 
            // btAddServer
            // 
            this.btAddServer.Location = new System.Drawing.Point(251, 526);
            this.btAddServer.Name = "btAddServer";
            this.btAddServer.Size = new System.Drawing.Size(80, 26);
            this.btAddServer.TabIndex = 1;
            this.btAddServer.Text = "应用";
            this.btAddServer.UseVisualStyleBackColor = true;
            this.btAddServer.Click += new System.EventHandler(this.btAddServer_Click);
            // 
            // btExit
            // 
            this.btExit.Location = new System.Drawing.Point(337, 526);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(80, 26);
            this.btExit.TabIndex = 2;
            this.btExit.Text = "取消";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 561);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.btAddServer);
            this.Controls.Add(this.propertyGrid1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "增加服务";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button btAddServer;
        private System.Windows.Forms.Button btExit;
    }
}