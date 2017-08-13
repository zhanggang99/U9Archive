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
            int taskCount=10000;
            DataWrite dataWrite= new DataWrite();
            dataWrite.UpdateUIDelegate += UpdataUIStatus;//绑定更新任务状态的委托
            dataWrite.TaskCallBack += Accomplish;//绑定完成任务要调用的委托

            Thread 

            this.rtbTxt.Text+="/r/n完成";
        }
        //停止迁移
        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        delegate void AsynUpdateUI(int step);


        //更新UI
        private void UpdataUIStatus(int step)
        {
            if (InvokeRequired)
            {
                this.Invoke(new AsynUpdateUI(delegate(int s)
                {
                    this.rtbTxt.Text = s.ToString();
                }), step);
            }
            else
            {
                this.rtbTxt.Text ="100";
            }
        }
        //完成任务时需要调用
        private void Accomplish()
        {
            //还可以进行其他的一些完任务完成之后的逻辑处理
            this.rtbTxt.Text+="任务完成";
        }
    }

     public class DataWrite
    {
        public delegate void UpdateUI(int step);//声明一个更新主线程的委托
        public UpdateUI UpdateUIDelegate;

        public delegate void AccomplishTask();//声明一个在完成任务时通知主线程的委托
        public AccomplishTask TaskCallBack;
       
        public void Write(int lineCount)
        {
            for (int i = 0; i < lineCount; i++)
            {
                Thread.Sleep(1000);
                //写入一条数据，调用更新主线程ui状态的委托
                UpdateUIDelegate(i);
            }
            //任务完成时通知主线程作出相应的处理
            TaskCallBack();
        }
    }
}
