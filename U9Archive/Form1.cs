using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        }
       
        
        private void btn_pt_Click(object sender, EventArgs e)
        {
            if (Interlocked.Equals(m_ifThreadStoped,0))
            {
                MessageBox.Show("请先停止当前正在进行的操作！");
                return;
            }
            Interlocked.Exchange(ref m_ifThreadStoped, 0);
            m_runThread = new Thread(new ThreadStart(RunArchiveUBFModule));
            m_runThread.Start();
        }
        private void RunArchiveUBFModule()
        {
            string src = this.textBox1.Text + "";
            if (!Directory.Exists(src))
            {
                Log($"脚本路径不存在：+{src}");
                goto ExitHere;
            }

            //开始执行ubf的脚本。
            DirectoryInfo fd = new DirectoryInfo(src);
            try
            {
                FileInfo[] files = fd.GetFiles("*.sql");
                foreach (FileInfo fi in files)
                {
                    string ubfSql = utils.GetStringFromFile(fi.FullName);
                    int o = SqlHelper.ExecuteNonQuery(ubfSql, null);
                }
                Log("UBF模块归档成功");
            }
            catch
            {
                Log("UBF模块归档失败");
            }
            finally
            {
                Interlocked.Exchange(ref m_ifThreadStoped, 1);
            }
            return;

        ExitHere:
            Interlocked.Exchange(ref m_ifThreadStoped, 1);
            return;

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
        private void Log(string s)
        {
            s = $"[{DateTime.Now}]{s}\r\n";
            ShowLogSetText(s);
            utils.Log(s);
        }
        private delegate void ShowLogSetTextCallback(string text);
        private void ShowLogSetText(string text)
        {
            if (this.ShowLog.InvokeRequired)
            {
                ShowLogSetTextCallback d = new ShowLogSetTextCallback(ShowLogSetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.ShowLog.AppendText(text);
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            Interlocked.Exchange(ref m_ifThreadStoped, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Interlocked.Equals(m_ifThreadStoped, 0))
            {
                MessageBox.Show("请先停止当前正在进行的操作！");
                return;
            }
            Interlocked.Exchange(ref m_ifThreadStoped, 0);
            m_runThread = new Thread(new ThreadStart(RunArchivePreSql));
            m_runThread.Start();
        }

        private void RunArchivePreSql()
        {
            string src = this.textBox1.Text + "";
            if (!Directory.Exists(src))
            {
                Log($"脚本路径不存在：+{src}");
                goto ExitHere;
            }

            //开始执行ubf的脚本。
            DirectoryInfo fd = new DirectoryInfo(src+ "\\PreSQL");
            try
            {
                FileInfo[] files = fd.GetFiles("*.sql");
                foreach (FileInfo fi in files)
                {
                    string ubfSql = utils.GetStringFromFile(fi.FullName);
                    int o = SqlHelper.ExecuteNonQuery(ubfSql, null);
                }
                Log("创建归档记录表ARModuleHisLogd成功");
            }
            catch (Exception ex)
            {
                Log("创建归档记录表ARModuleHisLog失败");
                Log(ex.Message);
            }
            finally
            {
                Interlocked.Exchange(ref m_ifThreadStoped, 1);
            }
            return;

            ExitHere:
            Interlocked.Exchange(ref m_ifThreadStoped, 1);
            return;
        }
    }
}
