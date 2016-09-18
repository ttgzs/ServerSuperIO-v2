using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerSuperIO.Config;

namespace ServerSuperIO.Tool
{
    public partial class AddForm : Form
    {
        public object ConfigObject { get; private set; }

        public AddForm(object obj)
        {
            InitializeComponent();
            InitServerConfig(obj);
        }

        private void InitServerConfig(object obj)
        {
            this.propertyGrid1.SelectedObject = obj;
        }

        private void btAddServer_Click(object sender, EventArgs e)
        {
            ConfigObject = this.propertyGrid1.SelectedObject;
            this.Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            ConfigObject = null;
            this.Close();
        }
    }
}
