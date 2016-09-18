namespace ServerSuperIO.Tool
{
    partial class ToolMainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("工业站点1", 3, 3);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("TCP/IP", 2, 2, new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("温度传感器", 3, 3);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("COM", 1, 1, new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("工业数据采集", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode4});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolMainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ServerToolStripMenuItem,
            this.DeviceToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(944, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ServerToolStripMenuItem
            // 
            this.ServerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddServerToolStripMenuItem,
            this.DeleteServerToolStripMenuItem});
            this.ServerToolStripMenuItem.Name = "ServerToolStripMenuItem";
            this.ServerToolStripMenuItem.Size = new System.Drawing.Size(86, 25);
            this.ServerToolStripMenuItem.Text = "服务实例";
            // 
            // AddServerToolStripMenuItem
            // 
            this.AddServerToolStripMenuItem.Name = "AddServerToolStripMenuItem";
            this.AddServerToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.AddServerToolStripMenuItem.Text = "增加服务";
            this.AddServerToolStripMenuItem.Click += new System.EventHandler(this.AddServerToolStripMenuItem_Click);
            // 
            // DeleteServerToolStripMenuItem
            // 
            this.DeleteServerToolStripMenuItem.Name = "DeleteServerToolStripMenuItem";
            this.DeleteServerToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.DeleteServerToolStripMenuItem.Text = "删除服务";
            this.DeleteServerToolStripMenuItem.Click += new System.EventHandler(this.DeleteServerToolStripMenuItem_Click);
            // 
            // DeviceToolStripMenuItem
            // 
            this.DeviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddDeviceToolStripMenuItem,
            this.DeleteDeviceToolStripMenuItem});
            this.DeviceToolStripMenuItem.Name = "DeviceToolStripMenuItem";
            this.DeviceToolStripMenuItem.Size = new System.Drawing.Size(86, 25);
            this.DeviceToolStripMenuItem.Text = "设备驱动";
            // 
            // AddDeviceToolStripMenuItem
            // 
            this.AddDeviceToolStripMenuItem.Name = "AddDeviceToolStripMenuItem";
            this.AddDeviceToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.AddDeviceToolStripMenuItem.Text = "增加设备";
            this.AddDeviceToolStripMenuItem.Click += new System.EventHandler(this.AddDeviceToolStripMenuItem_Click);
            // 
            // DeleteDeviceToolStripMenuItem
            // 
            this.DeleteDeviceToolStripMenuItem.Name = "DeleteDeviceToolStripMenuItem";
            this.DeleteDeviceToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.DeleteDeviceToolStripMenuItem.Text = "删除设备";
            this.DeleteDeviceToolStripMenuItem.Click += new System.EventHandler(this.DeleteDeviceToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 29);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(944, 632);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.ImageIndex = 3;
            treeNode1.Name = "节点3";
            treeNode1.SelectedImageIndex = 3;
            treeNode1.Text = "工业站点1";
            treeNode2.ImageIndex = 2;
            treeNode2.Name = "节点1";
            treeNode2.SelectedImageIndex = 2;
            treeNode2.Text = "TCP/IP";
            treeNode3.ImageIndex = 3;
            treeNode3.Name = "节点4";
            treeNode3.SelectedImageIndex = 3;
            treeNode3.Text = "温度传感器";
            treeNode4.ImageIndex = 1;
            treeNode4.Name = "节点2";
            treeNode4.SelectedImageIndex = 1;
            treeNode4.Text = "COM";
            treeNode5.Name = "节点0";
            treeNode5.SelectedImageKey = "(默认值)";
            treeNode5.Text = "工业数据采集";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(257, 632);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "server.ico");
            this.imageList1.Images.SetKeyName(1, "串口1.png");
            this.imageList1.Images.SetKeyName(2, "网口.png");
            this.imageList1.Images.SetKeyName(3, "computer.ico");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer2.Size = new System.Drawing.Size(683, 632);
            this.splitContainer2.SplitterDistance = 402;
            this.splitContainer2.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(683, 402);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(683, 226);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // ToolMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 661);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ServerSuperIO配置工具";
            this.Load += new System.EventHandler(this.ToolMainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteDeviceToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ImageList imageList1;
    }
}

