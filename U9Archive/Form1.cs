using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace U9Archive
{
    public partial class Form1 : Form
    {
        private Thread m_runThread = null;
        private int m_ifThreadStoped = 1;
        private string datetimestr = DateTime.Now.ToString("D");
        public Form1()
        {
            InitializeComponent();
            var s = $"{12}+{23}={12 + 23}";
            string ss = s;
        }
        private void Log(string s)
        {
            s = $"[{DateTime.Now}]{s}\r\n";
        }
        private void btn_pt_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "选择归档脚本所在根目录";
            if (fbd.ShowDialog()==DialogResult.OK)
            {
                this.textBox1.Text = fbd.SelectedPath;
            }
        }
        private void ShowLogSetText(string text)
        {

        }
    }
}
