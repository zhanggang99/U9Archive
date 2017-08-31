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

    public partial class Form4 : Form
    {
        public static List<ArItem> list2 = new List<ArItem>();
        public static CancellationTokenSource tokenSource = new CancellationTokenSource();
        public static CancellationToken token = tokenSource.Token;
        public static string arDate = "2016-01-01";
        public Form4()
        {
            InitializeComponent();
            list2.Add(new ArItem { arProc = "ubf2017101", lb = "label1" });
            list2.Add(new ArItem { arProc = "pm20170101", lb = "label2" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in list2)
            {
                Thread.Sleep(100);

                arshuju2(item);
            }
        }

        //private async void arshuju(ArItem aritem)
        //{
        //    #region
        //    //try
        //    //{
        //    //    Task<int> task = new Task<int>(() =>
        //    //    {
        //    //        //Console.WriteLine("Thread Start");//这是一次卸载子任务

        //    //        for (int i = 0; i < 5; i++)
        //    //        {
        //    //            token.WaitHandle.WaitOne(1000);//模拟其他工作，这里以1秒输出一个数字
        //    //            //Console.WriteLine(aritem.arProc + ":" + i.ToString());
        //    //        }

        //    //        if (token.IsCancellationRequested)
        //    //            return 00000;//rollback
        //    //        else
        //    //        {
        //    //            this.UpdateCount(10000);
        //    //        }
        //    //        return 10000;//commit

        //    //    }, token, TaskCreationOptions.AttachedToParent);

        //    //    task.Start();
        //    //}
        //    //catch
        //    //{
        //    //    this.UpdateCount(-1);
        //    //}
        //    #endregion
        //    await Task.Run(() =>
        //    {
        //        try
        //        {
        //            //Parallel.For(0, 1, new ParallelOptions { MaxDegreeOfParallelism = 3 }, x =>
        //            //  {
        //            //     int result = ExecSQL(aritem);
        //            //      UpdateCount(result,aritem.lb);
        //            //  });
        //            int result = ExecSQL(aritem);
        //            UpdateCount(result, aritem.lb);
        //        }
        //        catch (Exception)
        //        {
        //            UpdateCount(-1, aritem.lb);
        //        }
        //    });
        //}

        private void arshuju2(ArItem aritem)
        {
            int account = 1;
            Jeguo result;

            Task.Run(() =>
            {
                do
                {
                    try
                    {
                        result = ExecSQL(aritem);
                        UpdateCount(result, aritem.lb);
                        account = result.count;//返回本次归档的单据数量。
                    }
                    catch (Exception)
                    {
                        UpdateCount(new Jeguo() { begindate = "1991", count = -1 }, aritem.lb);
                    }
                } while (account > 400);
            }).ContinueWith((t) =>
            {
                Thread.Sleep(2000);
                UpdateCount(new Jeguo { begindate = "2016-01-03", count = -5 }, aritem.lb);
            });

        }

        private void UpdateCount(Jeguo recodeCount, string lb)
        {
            BeginInvoke(new Action(() =>
            {

                //this.label1.Text = recodeCount.ToString();
                //Label l1 = (Label)this.Controls.Find(lb, true)[0];
                // this.Controls.Find(lb, true)[0].Text = ((Convert.ToInt32(this.Controls.Find(lb, true)[0].Text)) + recodeCount.count).ToString();
                this.Controls.Find(lb, true)[0].Text = recodeCount.count.ToString();
            }));
        }
        private Jeguo ExecSQL(ArItem ai)
        {
            var rand = new Random();
            Thread.Sleep(rand.Next(500, 5000));

            return new Jeguo { begindate = "2016-01-01", count = rand.Next(300, 3000) };
        }
    }
    public class Jeguo
    {
        public string begindate { get; set; }
        public int count { get; set; }
    }
    public class ArItem
    {
        public string arProc { get; set; }
        public string lb { get; set; }
    }

}
