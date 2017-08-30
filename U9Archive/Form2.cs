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
//using System.Threading;
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
            //task
            Task t = new Task((c) =>
            {
                int count = (int)c;
                for (int i = 0; i < count; i++)
                {
                    Thread.Sleep(10);
                }

            },100);
            t.Start();
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
           
        }
        public static void WorkPro(object count)
        {
            int sum = 0;
            //做一些耗时的操作
            for (int i = 0; i < (int)count; i++)
            {
                sum += i;
                Thread.Sleep(100);
            }
        }
        //停止迁移
        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }



        ////开始迁移
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    //thread
        //    Thread t = new Thread(WorkPro);
        //    t.IsBackground = true;
        //    t.Start(1000);

        //}
        //public static void WorkPro(object count)
        //{
        //    int sum = 0;
        //    //做一些耗时的操作
        //    for (int i = 0; i < (int)count; i++)
        //    {
        //        sum += i;
        //        Thread.Sleep(100);
        //    }
        //}


        ////开始迁移
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    //BeginInvoke
        //    DoWork d = new DoWork(WorkPro);
        //    IAsyncResult r = d.BeginInvoke(100, Callback, d);
        //    //int result = d.EndInvoke(r);
        //    //this.rtbTxt.Text = result.ToString();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Thread.Sleep(10);//主线程需要做的工作
        //    }
        //    this.rtbTxt.Text += "\r\n主线程 down";
        //}

        public void Callback(IAsyncResult r)
        {
            DoWork d = (DoWork)r.AsyncState;
            int v = d.EndInvoke(r);

            if (InvokeRequired)
            {
                this.Invoke(new AsynUpdateUI(s => {
                    this.rtbTxt.Text += "结果是：" + s.ToString()+"\r\n";
                }),v);
            }
            else
                this.rtbTxt.Text += "执行完成"+d.EndInvoke(r).ToString();
            
        }
        public delegate int DoWork(int count);
        public static int WorkPro(int count)
        {
            int sum = 0;
            //做一些耗时的操作
            for (int i = 0; i < count; i++)
            {
                sum += i;
                Thread.Sleep(100);
            }
            return sum;
        }


        //private void button2_Click(object sender, EventArgs e)
        //{
        //    int taskCount = 10;
        //    DataWrite dataWrite = new DataWrite();
        //    dataWrite.UpdateUIDelegate += UpdataUIStatus;//绑定更新任务状态的委托
        //    dataWrite.TaskCallBack += Accomplish;//绑定完成任务要调用的委托

        //    Thread thread = new Thread(new ParameterizedThreadStart(dataWrite.Write));
        //    thread.IsBackground = true;
        //    thread.Start(taskCount);
        //}
        delegate void AsynUpdateUI(int step);
        //更新UI
        private void UpdataUIStatus(int step)
        {
            if (InvokeRequired)
            {
                this.Invoke(new AsynUpdateUI(delegate(int s)
                {
                    this.rtbTxt.Text += s.ToString()+ "\r\n";
                }), step);
            }
            else
            {
                this.rtbTxt.Text += "100";
            }
        }

        delegate void AsynAccomplishCallBack(string text);

        //完成任务时需要调用
        private void Accomplish(string txt)
        {
            if (InvokeRequired)
            {
                this.Invoke(new AsynAccomplishCallBack(s =>
                {
                    this.rtbTxt.Text += s;
                }),txt);
            }
            else

            //还可以进行其他的一些完任务完成之后的逻辑处理
                this.rtbTxt.Text+=txt;
        }
    }

     public class DataWrite
    {
        public delegate void UpdateUI(int step);//声明一个更新主线程的委托
        public UpdateUI UpdateUIDelegate;

        public delegate void AccomplishTask(string txt);//声明一个在完成任务时通知主线程的委托
        public AccomplishTask TaskCallBack;
       
        public void Write(object lineCount)
        {
            for (int i = 0; i < (int)lineCount; i++)
            {
                Thread.Sleep(1000);
                //写入一条数据，调用更新主线程ui状态的委托
                UpdateUIDelegate(i);
            }
            //任务完成时通知主线程作出相应的处理
            TaskCallBack("完成");
        }
    }
}
