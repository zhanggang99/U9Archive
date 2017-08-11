using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace U9Archive
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            button3.Enabled = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        //迁移评估
        private void button1_Click(object sender, EventArgs e)
        {

        }

        //开始迁移
        private void button2_Click(object sender, EventArgs e)
        {

        }
        //停止迁移
        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
    }
}
