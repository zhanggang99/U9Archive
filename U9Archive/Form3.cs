using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Serialization;

namespace U9Archive
{
    public partial class Form3 : Form
    {
        public static bool isMigrate = false;
        public static bool isStop = false;

        //public static List<MigrateItem> list = new List<MigrateItem>();

        //public static string currentDbConnectionStr = "data source=UF201080112;initial catalog=gj0407;user id=sa;pwd=ufsoft*123";
        //public static string migrateDbConnectionStr = "data source=UF201080140;initial catalog=gj0407his;user id=sa;pwd=ufsoft*123";

        public static string currentDbConnectionStr = "data source=zbh;initial catalog=MX;user id=sa;pwd=yonyou@123";
        public static string migrateDbConnectionStr = "data source=chenzenghui-pc;initial catalog=V6JinpanKongku20170625;user id=sa;pwd=xiangnian2010";


        public static string link;
        public static string db;

        public Form3()
        {
            InitializeComponent();
            LoadTree();
            InitDataGridView();
            InitDbList();
        }

        private void InitDbList()
        {
            List<string> dblist = new List<string>();
            SqlConnection conn = new SqlConnection(currentDbConnectionStr);
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.UBF_ARServerInfo", conn);
            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dblist.Add("[" + dr["LinkName"] + "]:[" + dr["ARDBName"].ToString() + "]");
            }
            this.dblist.DataSource = dblist;
            dr.Close();
        }
        //加载tree控件树
        private void LoadTree()
        {
            foreach (var item in MigrateModuleInfo.Instance.MigrateModules)
            {
                if (!treeU9Menu.Nodes.ContainsKey(item.Module))
                {
                    TreeNode tp = new TreeNode(item.Module);
                    tp.Tag = 0;
                    tp.Name = item.Module;
                    treeU9Menu.Nodes.Add(tp);

                    TreeNode tn = new TreeNode(item.ModuleValue);
                    tn.Tag = 1;
                    tn.Name = item.ModuleValue;
                    treeU9Menu.Nodes.Find(item.Module, false)[0].Nodes.Add(tn);

                }
                else
                {
                    TreeNode tn = new TreeNode(item.ModuleValue);
                    tn.Tag = 1;
                    tn.Name = item.ModuleValue;
                    treeU9Menu.Nodes.Find(item.Module, false)[0].Nodes.Add(tn);
                }
            }
        }

        private void treeU9Menu_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!isMigrate)
            {
                if (e.Node.Tag.ToString() == "0")
                {
                    CheckNodes(e.Node);
                    return;
                }
                else
                {
                    if (e.Node.Checked == true)
                    {
                        doWithGridChecked(e.Node);
                    }
                    else
                    {
                        doWithGridUnChecked(e.Node);
                    }
                }
            }

        }
        //动态添加行
        private void doWithGridChecked(TreeNode tn)
        {
            if (tn.Checked)
            {
                //todo：加载真实的数据  --已完成
                //MigrateItem item = new MigrateItem { ModuleKey = tn.Name, EarlyDate = "", ModuleName = tn.Name, toMigirateCount = 0, doneMigirateCount = 0, State = "未开始", Evaluate = "评估", Migrate = "迁移", SQLEvaluate = "", BeforeMigrateSql = "", MigrateSql = "", AfterMigrateSql = "" };
                //string[] row = new string[] { item.ModuleName, item.EarlyDate, item.toMigirateCount.ToString(), item.doneMigirateCount.ToString(), item.State, item.Evaluate, item.Migrate, item.SQLEvaluate, item.BeforeMigrateSql, item.MigrateSql, item.AfterMigrateSql };

                MigrateModuleItem migrateModle = MigrateModuleInfo.Instance.MigrateModules.Find(t => t.ModuleValue == tn.Name);

                for (int i = 0; i < dgvU9.Rows.Count; i++)
                {
                    if (dgvU9.Rows[i].Cells[0].Value != null && dgvU9.Rows[i].Cells[0].Value.ToString() == migrateModle.ModuleValue)
                        return;
                }

                DataGridViewRow dr = new DataGridViewRow();

                DataGridViewTextBoxCell moduleNameCell = new DataGridViewTextBoxCell();
                moduleNameCell.Value = migrateModle.ModuleValue;
                dr.Cells.Add(moduleNameCell);

                DataGridViewTextBoxCell earlyDateCell = new DataGridViewTextBoxCell();
                earlyDateCell.Value = "";
                dr.Cells.Add(earlyDateCell);

                DataGridViewTextBoxCell toMigirateCountCell = new DataGridViewTextBoxCell();
                toMigirateCountCell.Value = "0";
                dr.Cells.Add(toMigirateCountCell);

                DataGridViewTextBoxCell doneMigirateCountCell = new DataGridViewTextBoxCell();
                doneMigirateCountCell.Value = "0";
                dr.Cells.Add(doneMigirateCountCell);

                DataGridViewTextBoxCell StateCell = new DataGridViewTextBoxCell();
                StateCell.Value = "未开始";
                dr.Cells.Add(StateCell);

                DataGridViewDisableButtonCell EvaluateCell = new DataGridViewDisableButtonCell();
                EvaluateCell.Value = "评估";
                dr.Cells.Add(EvaluateCell);


                DataGridViewDisableButtonCell MigrateCell = new DataGridViewDisableButtonCell();
                MigrateCell.Value = "迁移";
                dr.Cells.Add(MigrateCell);


                DataGridViewTextBoxCell EvaluateSql = new DataGridViewTextBoxCell();
                EvaluateSql.Value = migrateModle.EvaluateSql;
                dr.Cells.Add(EvaluateSql);

                DataGridViewTextBoxCell BeforeMigrateSql = new DataGridViewTextBoxCell();
                BeforeMigrateSql.Value = migrateModle.BeforeMigrateSql;
                dr.Cells.Add(BeforeMigrateSql);

                DataGridViewTextBoxCell MigrateSql = new DataGridViewTextBoxCell();
                MigrateSql.Value = migrateModle.MigrateSql;
                dr.Cells.Add(MigrateSql);

                DataGridViewTextBoxCell AfterMigrateSql = new DataGridViewTextBoxCell();
                AfterMigrateSql.Value = migrateModle.AfterMigrateSql;
                dr.Cells.Add(AfterMigrateSql);

                DataGridViewTextBoxCell TableName = new DataGridViewTextBoxCell();
                TableName.Value = migrateModle.TableName;
                dr.Cells.Add(TableName);


                dgvU9.Rows.Add(dr);
                //MigrateItem mm = new MigrateItem { ModuleKey = migrateModle.ModuleKey, EarlyDate = "", ModuleName = migrateModle.ModuleValue, toMigirateCount = 0, doneMigirateCount = 0, State = "未开始", Evaluate = "评估", Migrate = "迁移", SQLEvaluate = migrateModle.MigrateSql, BeforeMigrateSql = migrateModle.BeforeMigrateSql, MigrateSql = migrateModle.MigrateSql, AfterMigrateSql = migrateModle.AfterMigrateSql };
                //if (list.Find(t => t.ModuleName == migrateModle.ModuleValue) ==null )
                //    list.Add(mm);

                return;
            }

        }
        private void InitDataGridView()
        {
            this.dgvU9.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewTextBoxColumn ModuleName = new DataGridViewTextBoxColumn();
            ModuleName.HeaderText = "模块名称";
            ModuleName.FillWeight = 10;

            DataGridViewTextBoxColumn EarlyDate = new DataGridViewTextBoxColumn();
            EarlyDate.HeaderText = "单据最早日期";
            EarlyDate.FillWeight = 10;

            DataGridViewTextBoxColumn toMigirateCount = new DataGridViewTextBoxColumn();
            toMigirateCount.HeaderText = "评估数量";
            toMigirateCount.FillWeight = 10;

            DataGridViewTextBoxColumn doneMigirateCount = new DataGridViewTextBoxColumn();
            doneMigirateCount.HeaderText = "已迁移数量";
            doneMigirateCount.FillWeight = 10;

            DataGridViewTextBoxColumn State = new DataGridViewTextBoxColumn();
            State.HeaderText = "归档状态";
            State.FillWeight = 30;

            DataGridViewDisableButtonColumn Evaluate = new DataGridViewDisableButtonColumn();
            Evaluate.HeaderText = "评估";
            Evaluate.FillWeight = 10;

            DataGridViewDisableButtonColumn Migrate = new DataGridViewDisableButtonColumn();
            Migrate.HeaderText = "迁移";
            Migrate.FillWeight = 10;

            DataGridViewTextBoxColumn EvaluateSql = new DataGridViewTextBoxColumn();
            EvaluateSql.HeaderText = "EvaluateSql";
            EvaluateSql.Visible = false;

            DataGridViewTextBoxColumn BeforeMigrateSql = new DataGridViewTextBoxColumn();
            BeforeMigrateSql.HeaderText = "BeforeMigrateSql";
            BeforeMigrateSql.Visible = false;

            DataGridViewTextBoxColumn MigrateSql = new DataGridViewTextBoxColumn();
            MigrateSql.HeaderText = "MigrateSql";
            MigrateSql.Visible = false;

            DataGridViewTextBoxColumn AfterMigrateSql = new DataGridViewTextBoxColumn();
            AfterMigrateSql.HeaderText = "AfterMigrateSql";
            AfterMigrateSql.Visible = false;

            DataGridViewTextBoxColumn TableName = new DataGridViewTextBoxColumn();
            TableName.HeaderText = "主表";
            TableName.Visible = false;

            dgvU9.Columns.Add(ModuleName);
            dgvU9.Columns.Add(EarlyDate);
            dgvU9.Columns.Add(toMigirateCount);
            dgvU9.Columns.Add(doneMigirateCount);
            dgvU9.Columns.Add(State);
            dgvU9.Columns.Add(Evaluate);
            dgvU9.Columns.Add(Migrate);
            dgvU9.Columns.Add(EvaluateSql);
            dgvU9.Columns.Add(BeforeMigrateSql);
            dgvU9.Columns.Add(MigrateSql);
            dgvU9.Columns.Add(AfterMigrateSql);
            dgvU9.Columns.Add(TableName);

        }
        //动态删除行
        private void doWithGridUnChecked(TreeNode tn)
        {
            if (!tn.Checked)
            {
                for (int i = 0; i < dgvU9.Rows.Count; i++)
                {
                    if (true)
                    {
                        //判断在删除时，是否已开始迁移，如果是，则退出，不允许删除行
                        if (dgvU9.Rows[i].Cells[4].Value.ToString() != "未开始")
                            return;
                    }
                }
                for (int i = 0; i < dgvU9.Rows.Count; i++)
                {
                    if (dgvU9.Rows[i].Cells[0].Value != null && dgvU9.Rows[i].Cells[0].Value.ToString() == tn.Name)
                    {
                        dgvU9.Rows.RemoveAt(i);
                        return;
                        //list.Remove(list.Find(t => t.ModuleName == tn.Name));
                    }
                }
            }

        }
        private void CheckNodes(TreeNode checkRoot)
        {
            foreach (TreeNode tn in checkRoot.Nodes)
            {
                tn.Checked = checkRoot.Checked;
                if (tn.Checked)
                {
                    doWithGridChecked(tn);
                }
                else
                {
                    doWithGridUnChecked(tn);
                }
            }
        }

        private void dgvU9_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)//评估
            {
                if (migrateDate.Value > System.DateTime.Now.AddMonths(-0))
                {
                    MessageBox.Show("只能迁移6月以前数据");
                    return;
                }

                if (((DataGridViewDisableButtonCell)this.dgvU9.Rows[e.RowIndex].Cells[5]).Enabled)
                {

                    string evaluateSql = dgvU9[7, e.RowIndex].Value.ToString();
                    this.dgvU9[4, e.RowIndex].Value = "评估中...";
                  
                    // list.Find(t => t.ModuleName == this.dgvU9.Rows[e.RowIndex].Cells[0].Value.ToString()).State = "评估中...";
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[e.RowIndex].Cells[6]).Enabled = false;

                    Evaluate(evaluateSql, e.RowIndex, migrateDate.Value);
                }
            }
            else if (e.ColumnIndex == 6)//迁移
            {
                if (migrateDate.Value > System.DateTime.Now.AddMonths(-0))
                {
                    MessageBox.Show("只能迁移6月以前数据");
                    return;
                }
                if (dblist.SelectedItem == null || dblist.SelectedValue.ToString().Split(':').Length != 2)
                {
                    MessageBox.Show("没有可迁移到历史库");
                    return;
                }
                link = dblist.SelectedValue.ToString().Split(':')[0];
                db = dblist.SelectedValue.ToString().Split(':')[1];

                isStop = false;
                if (!((DataGridViewDisableButtonCell)this.dgvU9.Rows[e.RowIndex].Cells[6]).Enabled || this.dgvU9[4, e.RowIndex].Value.ToString() == "评估中...")//此时正在评估，则不能做迁移
                    return;
                //开始迁移后，不在响应评估事件
                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[e.RowIndex].Cells[5]).Enabled = false;

                if (this.dgvU9.Rows[e.RowIndex].Cells[6].Value.ToString() == "中止")
                {
                    this.dgvU9.Rows[e.RowIndex].Cells[6].Value = "中止..";
                    this.dgvU9.Rows[e.RowIndex].Cells[4].Value = "迁移中止...";
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[e.RowIndex].Cells[6]).Enabled = false;
                }
                else if (this.dgvU9.Rows[e.RowIndex].Cells[6].Value.ToString() == "迁移")
                {
                    //开始迁移后，迁移按钮则变成 暂停 ，如果用户点击了暂停，则迁移过程停止。
                    this.dgvU9.Rows[e.RowIndex].Cells[6].Value = "中止";
                    string evaluateSql = dgvU9[7, e.RowIndex].Value.ToString();
                    string beforeMigrateSql = dgvU9[8, e.RowIndex].Value.ToString();
                    string migrateSql = dgvU9[9, e.RowIndex].Value.ToString();
                    string afterMigrateSql = dgvU9[10, e.RowIndex].Value.ToString();
                    string tableName = dgvU9[11, e.RowIndex].Value.ToString();
                    //如果在迁移前，没有估过，则要做一次评估。EvaluateResult result;
                    if (this.dgvU9[4, e.RowIndex].Value.ToString() != "评估完成")//   此后后续调试，发现并非如此,list先不用了：异步取Ui数据仍是旧值，换为从List取值。
                                                                             //if(list.Find(t=>t.ModuleName == this.dgvU9[0,e.RowIndex].Value.ToString()).State== "评估完成")
                        MigrateWithEvluate(evaluateSql, beforeMigrateSql, migrateSql, afterMigrateSql, e.RowIndex, migrateDate.Value, link, db, tableName);
                    else
                        Migrate(beforeMigrateSql, migrateSql, afterMigrateSql, e.RowIndex, migrateDate.Value, link, db, tableName);
                }
            }
        }
        private void Evaluate(string Evaluatesql, int rowindex, DateTime dt)
        {
            EvaluateResult result;
            this.dgvU9[4, rowindex].Style.ForeColor = Color.Black;
            Task.Run(() =>
            {
                try
                {
                    result = ExecEvaluateSQL(Evaluatesql, dt);
                    UpdateCount(result, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateCountError(ex.Message, rowindex);
                    return;
                }
            });
        }
        private void Migrate(string BeforeMigrateSql, string migrateSql, string afterMigrateSql, int rowindex, DateTime dt, string link, string db, string tb)
        {
            MigrateResult result;

            bool isError = false;
            string errorMeg = "";
            this.dgvU9[4, rowindex].Style.ForeColor = Color.Black;
            Task.Run(() =>
            {
                try
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                            this.dgvU9.Rows[rowindex].Cells[4].Value = "迁移中止...";
                        else
                            this.dgvU9[4, rowindex].Value = "迁移准备...";

                    }));
                    result = ExecBeforeMigrateSQL(BeforeMigrateSql, dt, link, db);
                    UpdateBeforeMigrateCount(result, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateBeforeMigrateCountError(ex.Message, rowindex);
                    isError = true;
                    errorMeg = ex.Message;
                    return;
                }
            }).ContinueWith(t =>
            {
                if (isError)
                    return;
                try
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                            this.dgvU9.Rows[rowindex].Cells[4].Value = "迁移中止...";
                        else
                            this.dgvU9[4, rowindex].Value = "迁移进行...";

                    }));

                    int i = 0;
                    string sqlGetCount = "select count(*) from ##" + tb;

                    SqlConnection conn = new SqlConnection(currentDbConnectionStr);
                    SqlCommand cmd = new SqlCommand(sqlGetCount, conn);
                    cmd.Connection.Open();
                    object obj = cmd.ExecuteScalar();
                    int rowCount = (int)obj;
                    while (rowCount> 0)
                    {
                        if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                            return;
                        result = ExecMigrateSQL(migrateSql, dt, link, db);
                        rowCount = rowCount - result.MigrateCount;
                        UpdateMigrateCount(result, rowindex);
                    }


                }
                catch (Exception ex)
                {
                    UpdateMigrateCountCountError(ex.Message, rowindex);
                    isError = true;
                    errorMeg = ex.Message;
                    return;
                }
            }).ContinueWith(tt =>
            {

                //if (isError)  可能有一些要启用索引的操作，这里暂时还要要执行
                //    return;
                try
            {
                    if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                    {
                        BeginInvoke(new Action(() =>
                        {
                            if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                            {
                                 this.dgvU9.Rows[rowindex].Cells[4].Value = "迁移中止...";
                            }
                            else if (isError)
                                this.dgvU9[4, rowindex].Value = errorMeg;
                            else
                                this.dgvU9[4, rowindex].Value = "迁移结束...";
                        }));
                    }

                    result = ExecAfterMigrateSQL(afterMigrateSql);
                    UpdateAfterMigrateCount(result, rowindex,isError,errorMeg);
                }
                catch (Exception ex)
                {
                    UpdateAfterMigrateCountError(ex.Message, rowindex);
                    return;
                }
            });

        }
        private void MigrateWithEvluate(string Evaluatesql, string beforeMigrateSql, string migrateSql, string afterMigrateSql, int rowindex, DateTime dt, string link, string db, string tb)
        {
            EvaluateResult result;
            //MigrateResult result2;
            bool isError = false;
            string errorMeg = "";

            Task.Run(() =>
            {
                try
                {
                    BeginInvoke(new Action(() =>
                    {
                        this.dgvU9[4, rowindex].Value = "评估开始...";

                    }));
                    result = ExecEvaluateSQL(Evaluatesql, dt);
                    UpdateCount(result, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateCountError(ex.Message, rowindex);
                    isError = true;
                    errorMeg = ex.Message;
                    return;
                }
            }).ContinueWith(t =>
            {
                if (isError)
                    return;
                Migrate(beforeMigrateSql, migrateSql, afterMigrateSql, rowindex, dt, link, db, tb);

                #region 重构支队
                //    try
                //    {
                //        if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                //            return;
                //        BeginInvoke(new Action(() =>
                //        {
                //            if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                //                this.dgvU9.Rows[rowindex].Cells[4].Value = "迁移中止...";
                //            else
                //                this.dgvU9[4, rowindex].Value = "迁移准备...";

                //        }));
                //        result2 = ExecBeforeMigrateSQL(BeforeMigrateSql, dt, link, db);
                //        UpdateBeforeMigrateCount(result2, rowindex);
                //    }
                //    catch (Exception ex)
                //    {
                //        UpdateBeforeMigrateCountError(ex.Message, rowindex);
                //        isError = true;
                //        errorMeg = ex.Message;
                //        return;
                //    }
                //}).ContinueWith(t =>
                //{
                //    if (isError)
                //        return;
                //    try
                //    {
                //        BeginInvoke(new Action(() =>
                //        {
                //            if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                //                this.dgvU9.Rows[rowindex].Cells[4].Value = "迁移中止...";
                //            else
                //                this.dgvU9[4, rowindex].Value = "迁移进行...";

                //        }));

                //        int i = 0;
                //        while (i < 3)
                //        {
                //            if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                //                return;
                //            result2 = ExecMigrateSQL(migrateSql, dt, link, db);
                //            UpdateMigrateCount(result2, rowindex);
                //            i++;
                //        }


                //    }
                //    catch (Exception ex)
                //    {
                //        UpdateMigrateCountCountError(ex.Message, rowindex);
                //        isError = true;
                //        errorMeg = ex.Message;
                //        return;
                //    }
                //}).ContinueWith(tt =>
                //{
                //    //if (isError)  可能有一些要启用索引的操作，这里暂时还要要执行
                //    //    return;
                //    try
                //    {

                //        if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                //        {

                //            BeginInvoke(new Action(() =>
                //            {
                //                if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                //                {
                //                    this.dgvU9.Rows[rowindex].Cells[6].Value = "迁移";
                //                    this.dgvU9.Rows[rowindex].Cells[4].Value = "迁移已中止";
                //                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[6]).Enabled = true;

                //                }
                //                else if (isError)
                //                    this.dgvU9[4, rowindex].Value = errorMeg;
                //                else
                //                    this.dgvU9[4, rowindex].Value = "迁移结束...";
                //            }));
                //            return;
                //        }

                //        result2 = ExecAfterMigrateSQL(afterMigrateSql);
                //        UpdateAfterMigrateCount(result2, rowindex,isError, errorMeg);
                //    }
                //    catch (Exception ex)
                //    {
                //        UpdateAfterMigrateCountError(ex.Message, rowindex);
                //        return;
                //    }
#endregion
            });

        }
        private void UpdateCount(EvaluateResult result, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[1, rowindex].Value = result.date;
                this.dgvU9[2, rowindex].Value = result.Evaluatecount.ToString();
                this.dgvU9[4, rowindex].Value = "评估完成";
                this.dgvU9[3, rowindex].Value = "0";
                this.dgvU9[4, rowindex].Style.ForeColor = Color.Green;

                //MigrateItem item = list.Find(t => t.ModuleName == this.dgvU9[0, rowindex].Value.ToString());
                //if (item!=null)
                //{
                //    item.State = "评估完成"; 
                //    item.EarlyDate= result.date;
                //    item.toMigirateCount = result.Evaluatecount;
                //}

                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[6]).Enabled = true;
                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
            }));
        }

        private void UpdateCountError(string err, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = err;
                this.dgvU9[4, rowindex].Style.ForeColor = Color.Red;

                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[6]).Enabled = true;
                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                //MigrateItem item = list.Find(t => t.ModuleName == this.dgvU9[0, rowindex].Value.ToString());
                //if (item != null)
                //{
                //    item.State = "评估出错";
                //}

            }));
        }


        private void UpdateBeforeMigrateCount(MigrateResult result, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                    this.dgvU9.Rows[rowindex].Cells[4].Value = "迁移中止...";
                else
                    this.dgvU9[4, rowindex].Value = "迁移准备成功";
            }));
        }

        private void UpdateBeforeMigrateCountError(string err, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = err;
                this.dgvU9[4, rowindex].Style.ForeColor = Color.Red;

                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[6]).Enabled = true;
            }));
        }

        private void UpdateMigrateCount(MigrateResult result, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                    this.dgvU9.Rows[rowindex].Cells[4].Value = "迁移中止...";
                else
                {
                    this.dgvU9[4, rowindex].Value = "迁移进行...";
                    this.dgvU9[3, rowindex].Value = ( Convert.ToInt32(this.dgvU9[3, rowindex].Value)+ result.MigrateCount).ToString();
                }
            }));
        }

        private void UpdateMigrateCountCountError(string err, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = err;
                this.dgvU9[4, rowindex].Style.ForeColor = Color.Red;

                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[6]).Enabled = true;


            }));
        }


        private void UpdateAfterMigrateCount(MigrateResult result, int rowindex,bool isError,string errMsg)
        {
            BeginInvoke(new Action(() =>
            {
                if (this.dgvU9.Rows[rowindex].Cells[6].Value.ToString() == "中止..")
                {
                    this.dgvU9.Rows[rowindex].Cells[6].Value = "迁移";
                    this.dgvU9.Rows[rowindex].Cells[4].Value = "迁移已中止";
                }
                else if(isError)
                {
                    this.dgvU9[4, rowindex].Value = errMsg;
                    this.dgvU9[4, rowindex].Style.ForeColor = Color.Red;
                    this.dgvU9.Rows[rowindex].Cells[6].Value = "迁移";
                }
                else
                {
                    this.dgvU9[4, rowindex].Value = "迁移结束";
                    this.dgvU9[4, rowindex].Style.ForeColor = Color.Green;
                    this.dgvU9.Rows[rowindex].Cells[6].Value = "迁移";
                }
                

                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[6]).Enabled = true;

            }));
        }

        private void UpdateAfterMigrateCountError(string err, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = err;
                this.dgvU9[4, rowindex].Style.ForeColor = Color.Red;
                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[6]).Enabled = true;
                this.dgvU9.Rows[rowindex].Cells[6].Value = "迁移";

            }));
        }

        private EvaluateResult ExecEvaluateSQL(string sql, DateTime dt)
        {
            SqlConnection conn = new SqlConnection(currentDbConnectionStr);
            SqlCommand cmd = new SqlCommand(sql + " '" + migrateDate.Value + "'", conn);
            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string dateTime = "1900-00-00";
            string count = "";
            if (dr.Read())
            {
                dateTime = ((DateTime)dr["CreatedOn"]).ToString("yyyy-MM-dd");
                count = dr["Count"].ToString();
            }
            dr.Close();

            return new EvaluateResult { date = dateTime, Evaluatecount = count };
        }

        private MigrateResult ExecBeforeMigrateSQL(string sql, DateTime dt, string link, string db)
        {
            SqlConnection conn = new SqlConnection(currentDbConnectionStr);
            SqlCommand cmd = new SqlCommand(sql, conn);// +" '" + migrateDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'," + " '" + link + "'," + " '" + db + "'", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            var paramters = new[]
            {
                new SqlParameter("@LinkServer",link),new SqlParameter("@ARDBName",db), new SqlParameter("@ardate", migrateDate.Value)
            };

            for (int i = 0; i < paramters.Length; i++)
            {
                cmd.Parameters.Add(paramters[i]);
            }

            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            int count = -1;
            if (dr.Read())
            {
                count = (int)dr["result"];
            }
            dr.Close();

            return new MigrateResult { MigrateCount = count };
        }

        private MigrateResult ExecMigrateSQL(string sql, DateTime dt, string link, string db)
        {
            int count = -1;
            SqlConnection conn = new SqlConnection(currentDbConnectionStr);
            SqlCommand cmd = new SqlCommand(sql + " '" + link + "', " + " '" + db + "'", conn);
            
            try
            {
                SqlDataReader dr;
                cmd.Connection.Open();
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    count = (int)dr["result"];
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            return new MigrateResult { MigrateCount = count };
        }

        private MigrateResult ExecAfterMigrateSQL(string sql)
        {
            SqlConnection conn = new SqlConnection(currentDbConnectionStr);
            SqlCommand cmd = new SqlCommand(sql,conn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            return new MigrateResult { MigrateCount = 0 };
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            isStop = true;
        }
    }

    public class EvaluateResult
    {
        public string date { get; set; }
        public string Evaluatecount { get; set; }
    }
    public class MigrateResult
    {
        public int MigrateCount { get; set; }
    }

    /// <summary>
    /// 数据迁移配置文件
    /// </summary>
    public class MigrateModuleInfo
    {
        private static readonly object _syncLock = new object();

        private static bool bFwatchRun = false;
        private static MigrateModuleInfo m_Migrate;

        //所有迁移模块
        public List<MigrateModuleItem> MigrateModules = new List<MigrateModuleItem>();
        private System.Collections.Specialized.HybridDictionary m_CustomConfig = new System.Collections.Specialized.HybridDictionary();
        public System.Collections.Specialized.HybridDictionary CustomConfig
        {
            get
            {
                return m_CustomConfig;
            }
        }

        public static MigrateModuleInfo Instance
        {
            get
            {
                if (m_Migrate != null)
                {
                    return m_Migrate;
                }
                lock (_syncLock)
                {
                    if (m_Migrate == null)
                    {
                        m_Migrate = new MigrateModuleInfo();
                    }
                    return m_Migrate;
                }
            }
        }

        MigrateModuleInfo()
        {
            this.Load();
        }
        private void Load()
        {
            string ConfigFileName = "U9MigrateConfig.xml";
            string ConfigDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string FileName = Path.Combine(ConfigDir, ConfigFileName);

            if (!bFwatchRun)
            {
                FileSystemWatcher watcher = new FileSystemWatcher(ConfigDir, ConfigFileName);
                watcher.Changed += Watcher_Changed;
            }

            if (!File.Exists(FileName))
            {
                throw new Exception("未发现迁移配置文件：" + FileName);
            }

            XmlDocument dom = new XmlDocument();
            dom.Load(FileName);

            XmlNode rootnode = dom.SelectSingleNode("MigrateRoot/MigrateItems");
            LoadModuleItem(rootnode);


            //自定义配置，配置为测试环境等。
            rootnode = dom.SelectSingleNode("MigrateRoot/MigrateCustomConfigurations");
            LoadCustomConfigurations(rootnode);
        }
        private void LoadCustomConfigurations(XmlNode rootnode)
        {
            if (rootnode == null) return;

            foreach (XmlNode node in rootnode.ChildNodes)
            {
                if (node.Name != "Item") continue;

                if (node.Attributes["key"] != null && !string.IsNullOrEmpty(node.Attributes["key"].Value) &&
                    node.Attributes["value"] != null && !string.IsNullOrEmpty(node.Attributes["value"].Value))
                    CustomConfig[node.Attributes["key"].Value] = node.Attributes["value"].Value;
            }
        }
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                this.ReLoad();
            }
            catch
            {
            }
        }

        private void LoadModuleItem(XmlNode rootnode)
        {
            if (rootnode == null) return;

            foreach (XmlNode node in rootnode.ChildNodes)
            {
                if (node.Name != "MigrateItem") continue;

                MigrateModuleItem sp = new MigrateModuleItem();

                if (node.Attributes["ModuleKey"] != null)
                    sp.ModuleKey = node.Attributes["ModuleKey"].Value;
                if (node.Attributes["ModuleValue"] != null)
                    sp.ModuleValue = node.Attributes["ModuleValue"].Value;
                if (node.Attributes["Module"] != null)
                    sp.Module = node.Attributes["Module"].Value;

                if (node.Attributes["EvaluateSql"] != null)
                    sp.EvaluateSql = node.Attributes["EvaluateSql"].Value;

                if (node.Attributes["BeforeMigrateSql"] != null)
                    sp.BeforeMigrateSql = node.Attributes["BeforeMigrateSql"].Value;

                if (node.Attributes["MigrateSql"] != null)
                    sp.MigrateSql = node.Attributes["MigrateSql"].Value;

                if (node.Attributes["AfterMigrateSql"] != null)
                    sp.AfterMigrateSql = node.Attributes["AfterMigrateSql"].Value;


                if (node.Attributes["TableName"] != null)
                    sp.TableName = node.Attributes["TableName"].Value;

                MigrateModules.Add(sp);
            }
        }
        private void ReLoad()
        {
            Thread.Sleep(2000);
            lock (_syncLock)
            {
                m_Migrate = new MigrateModuleInfo();
            }
        }

    }

    public class MigrateModuleItem
    {
        public string ModuleKey { get; set; }
        public string ModuleValue { get; set; }
        public string Module { get; set; }

        public string EvaluateSql { get; set; }
        public string BeforeMigrateSql { get; set; }
        public string MigrateSql { get; set; }
        public string AfterMigrateSql { get; set; }
        public string TableName { get; set; }

    }

    public class MigrateItem
    {
        public string ModuleName { get; set; }
        public string EarlyDate { get; set; }
        public int toMigirateCount { get; set; }
        public int doneMigirateCount { get; set; }
        public string State { get; set; }
        public string Evaluate { get; set; }
        public string Migrate { get; set; }

        public string ModuleKey { get; set; }
        public string SQLEvaluate { get; set; }
        public string BeforeMigrateSql { get; set; }
        public string MigrateSql { get; set; }
        public string AfterMigrateSql { get; set; }
        public string TableName { get; set; }


    }
    /// <summary>
    /// 归档入口参数
    /// </summary>
    [Serializable]
    public class ARParaInfo
    {
        private string m_ArchiveEntDBConn;
        private string m_ArchiveHisDBConn;
        private DateTime m_AccountingPeriod;
        private string m_ArchiveID;
        private ArchiveActionType m_ArchiveAction;
        private ArchiveClientInfo m_clientinfo;

        [XmlElement]
        public ArchiveActionType ArchiveAction
        {
            get { return m_ArchiveAction; }
            set { m_ArchiveAction = value; }
        }

        [XmlElement]
        public ArchiveClientInfo Clientinfo
        {
            get { return m_clientinfo; }
            set { m_clientinfo = value; }
        }

        /// <summary>
        /// 归档模块ID
        /// </summary>
        [XmlElement]
        public string ARID
        {
            get { return m_ArchiveID; }
            set { m_ArchiveID = value; }
        }

        /// <summary>
        /// 归档日期
        /// </summary>
        [XmlElement]
        public DateTime AccountingPeriod
        {
            get { return m_AccountingPeriod; }
            set { m_AccountingPeriod = value; }
        }

        /// <summary>
        /// 归档历史库连接
        /// </summary>
        [XmlElement]
        public string ArchiveHisDBConn
        {
            get { return m_ArchiveHisDBConn; }
            set { m_ArchiveHisDBConn = value; }
        }

        /// <summary>
        /// 当前企业库连接
        /// </summary>
        [XmlElement]
        public string ArchiveEntDBConn
        {
            get { return m_ArchiveEntDBConn; }
            set { m_ArchiveEntDBConn = value; }
        }
    }
    /// <summary>
    /// 库类别
    /// </summary>
    public enum ArchiveDBType
    {
        ArchiveHisDB,
        ArchiveEntDB
    }
    /// <summary>
    /// 归档执行动作
    /// </summary>
    public enum ArchiveActionType
    {
        ArchiveAllRun,
        ArchiveCreateView,
        ArchiveMigrate
    }
    public class ArchiveClientInfo
    {
        private string clientname;
        private string clientport;
        private string url;

        [XmlElement]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        [XmlElement]
        public string ClientPort
        {
            get { return clientport; }
            set { clientport = value; }
        }
        [XmlElement]
        public string ClientName
        {
            get { return clientname; }
            set { clientname = value; }
        }
    }


    /// <summary>
    /// Adapted from Double-buffering was added to remove flicker on re-paints.
    /// </summary>
    public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewDisableButtonColumn()
        {
            this.CellTemplate = new DataGridViewDisableButtonCell();
        }
    }
    public class DataGridViewDisableButtonCell : DataGridViewButtonCell
    {
        private bool enabledValue;

        public bool Enabled
        {
            get { return enabledValue; }
            set
            {
                if (enabledValue == value) return;
                enabledValue = value;
                // force the cell to be re-painted
                if (DataGridView != null) DataGridView.InvalidateCell(this);
            }
        }

        // Override the Clone method so that the Enabled property is copied. 
        public override object Clone()
        {
            var cell = (DataGridViewDisableButtonCell)base.Clone();
            cell.Enabled = Enabled;
            return cell;
        }

        // By default, enable the button cell. 
        public DataGridViewDisableButtonCell()
        {
            enabledValue = true;
        }

        protected override void Paint(
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates elementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The button cell is disabled, so paint the border, background, and disabled button for the cell. 
            if (!enabledValue)
            {
                var currentContext = BufferedGraphicsManager.Current;

                using (var myBuffer = currentContext.Allocate(graphics, cellBounds))
                {
                    // Draw the cell background, if specified. 
                    if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                    {
                        using (var cellBackground = new SolidBrush(cellStyle.BackColor))
                        {
                            myBuffer.Graphics.FillRectangle(cellBackground, cellBounds);
                        }
                    }

                    // Draw the cell borders, if specified. 
                    if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
                    {
                        PaintBorder(myBuffer.Graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
                    }

                    // Calculate the area in which to draw the button.
                    var buttonArea = cellBounds;
                    var buttonAdjustment = BorderWidths(advancedBorderStyle);
                    buttonArea.X += buttonAdjustment.X;
                    buttonArea.Y += buttonAdjustment.Y;
                    buttonArea.Height -= buttonAdjustment.Height;
                    buttonArea.Width -= buttonAdjustment.Width;

                    // Draw the disabled button.                
                    ButtonRenderer.DrawButton(myBuffer.Graphics, buttonArea, PushButtonState.Disabled);

                    // Draw the disabled button text.  
                    var formattedValueString = FormattedValue as string;
                    if (formattedValueString != null)
                    {
                        TextRenderer.DrawText(myBuffer.Graphics, formattedValueString, DataGridView.Font, buttonArea, SystemColors.GrayText, TextFormatFlags.PreserveGraphicsTranslateTransform | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    }

                    myBuffer.Render();
                }
            }
            else
            {
                // The button cell is enabled, so let the base class handle the painting. 
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }




}
