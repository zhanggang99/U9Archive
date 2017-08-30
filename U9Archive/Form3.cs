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
    public class ArItem
    {
        public string arProc { get; set; }
        public string lb { get; set; }
    }
    public partial class Form3 : Form
    {
        public static List<ArItem> list = new List<ArItem>();
        public static CancellationTokenSource tokenSource = new CancellationTokenSource();
        public static CancellationToken token = tokenSource.Token;
        public static string arDate = "2016-01-01";
        public Form3()
        {
            InitializeComponent();
            list.Add(new ArItem { arProc = "ubf2017101", lb = "lb1" });
            list.Add(new ArItem { arProc = "pm20170101", lb = "lb2" });
            list.Add(new ArItem { arProc = "so20170101", lb = "lb3" });
            //RunAr();
            //RunData();
        }

        static void RunAr()
        {
            var task = new Task(() =>
            {
                Console.WriteLine("Thread Start");
                for (int i = 0; i < 15; i++)
                {
                    //token.ThrowIfCancellationRequested();

                    token.WaitHandle.WaitOne(1000);//模拟其他工作，这里以1秒输出一个数字
                    Console.WriteLine(i);
                }
            }, token, TaskCreationOptions.AttachedToParent);

            try
            {
                task.Start();
                if (!task.Wait(5 * 1000, token))//5秒超时，则取消任务
                {
                    tokenSource.Cancel();//如果任务取消，则抛出一个CancellationRequestd异常。
                }
                if (task.IsCompleted)
                {
                    Console.WriteLine("Task Canceled");
                }
                if (task.IsCompleted)
                {
                    Console.WriteLine("Task Finished");
                }
                Console.WriteLine("\nTask Finished");

            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();

            }
        }

        static void RunData()
        {
            Task<int> task = new Task<int>(() =>
            {
                Console.WriteLine("Thread Start");//这是一次卸载子任务
                for (int i = 0; i < 15; i++)
                {
                    token.WaitHandle.WaitOne(1000);//模拟其他工作，这里以1秒输出一个数字
                    Console.WriteLine(i);
                }
                if (token.IsCancellationRequested)
                    return 00000;//rollback
                else
                    return 10000;//commit
            }, token, TaskCreationOptions.AttachedToParent);
            try
            {
                task.Start();
                if (!task.Wait(5 * 1000, token))//5秒超时，则取消任务
                {
                    tokenSource.Cancel();//如果任务取消，则抛出一个CancellationRequestd异常。
                    Console.WriteLine(task.Result);

                }
                if (task.IsCanceled)
                {
                    Console.WriteLine("Task Canceled");
                }
                if (task.IsCompleted)
                {
                    Console.WriteLine("Task Finished");
                    Console.WriteLine(task.Result);

                }
                Console.WriteLine("卸载完成");

            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();

            }
        }

        private void pinggu()
        {
            foreach (var item in list)
            {
                this.arData(item.arProc,item.lb);
               
            }
        }

        private void arData(string proc, string updateLb)
        {
            Task<int> task = new Task<int>((item) =>
            {
                Console.WriteLine("Thread Start");//这是一次卸载子任务

                for (int i = 0; i < 15; i++)
                {
                    token.WaitHandle.WaitOne(1000);//模拟其他工作，这里以1秒输出一个数字
                    Console.WriteLine(proc+":"+i.ToString());
                }
               
                if (token.IsCancellationRequested)
                    return 00000;//rollback
                else
                    return 10000;//commit
            }, token, TaskCreationOptions.AttachedToParent);
            try
            {
                task.Start();
                //if (!task.Wait(5 * 1000, token))//5秒超时，则取消任务
                //{
                //    tokenSource.Cancel();//如果任务取消，则抛出一个CancellationRequestd异常。
                //}
                if (task.IsCanceled)
                    this.UpdateCount(0, updateLb);
                if (task.IsCompleted)
                    this.UpdateCount(task.Result, updateLb);//这里返回真正的归档数据行数

            }
            catch (AggregateException)
            {
                this.UpdateCount(0, updateLb);

            }
        }
        delegate void AsynAccomplishCallBack(int count);
        private void UpdateCount(int recodeCount, string updateLb)
        {
            if (InvokeRequired)
            {
                this.Invoke(new AsynAccomplishCallBack(s =>
                {
                    this.Controls.Find(updateLb, true)[0].Text += s.ToString();
                }), recodeCount);
            }
            else
                //还可以进行其他的一些完任务完成之后的逻辑处理
                this.Controls.Find(updateLb, true)[0].Text += recodeCount.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pinggu();
        }
    }

}
