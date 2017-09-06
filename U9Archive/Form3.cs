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

        public static List<MigrateItem> list = new List<MigrateItem>();

        public static string currentDbConnectionStr = "data source=.;initial catalog=mx;user id=sa;pwd=yonyou@123";
        public static string migrateDbConnectionStr = "data source=.;initial catalog=mx_his;user id=sa;pwd=yonyou@123";

        public Form3()
        {
            InitializeComponent();
            LoadTree();
            InitDataGridView();

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
                //todo：加载真实的数据
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
                dgvU9.Rows.Add(dr);

                MigrateItem mm = new MigrateItem { ModuleKey = migrateModle.ModuleKey, EarlyDate = "", ModuleName = migrateModle.ModuleValue, toMigirateCount = 0, doneMigirateCount = 0, State = "未开始", Evaluate = "评估", Migrate = "迁移", SQLEvaluate = migrateModle.MigrateSql, BeforeMigrateSql = migrateModle.BeforeMigrateSql, MigrateSql = migrateModle.MigrateSql, AfterMigrateSql = migrateModle.AfterMigrateSql };
                if (list.Find(t => t.ModuleName == migrateModle.ModuleValue) ==null )
                    list.Add(mm);

                return;
            }

        }
        private void InitDataGridView()
        {
            DataGridViewTextBoxColumn ModuleName = new DataGridViewTextBoxColumn();
            ModuleName.HeaderText = "模块名称";

            DataGridViewTextBoxColumn EarlyDate = new DataGridViewTextBoxColumn();
            EarlyDate.HeaderText = "单据最早日期";

            DataGridViewTextBoxColumn toMigirateCount = new DataGridViewTextBoxColumn();
            toMigirateCount.HeaderText = "评估数量";

            DataGridViewTextBoxColumn doneMigirateCount = new DataGridViewTextBoxColumn();
            doneMigirateCount.HeaderText = "已迁移数量";

            DataGridViewTextBoxColumn State = new DataGridViewTextBoxColumn();
            State.HeaderText = "归档状态";

            DataGridViewDisableButtonColumn Evaluate = new DataGridViewDisableButtonColumn();
            Evaluate.HeaderText = "评估";

            DataGridViewDisableButtonColumn Migrate = new DataGridViewDisableButtonColumn();
            Migrate.HeaderText = "评估";


            DataGridViewTextBoxColumn EvaluateSql = new DataGridViewTextBoxColumn();
            EvaluateSql.HeaderText = "EvaluateSql";

            DataGridViewTextBoxColumn BeforeMigrateSql = new DataGridViewTextBoxColumn();
            BeforeMigrateSql.HeaderText = "BeforeMigrateSql";

            DataGridViewTextBoxColumn MigrateSql = new DataGridViewTextBoxColumn();
            MigrateSql.HeaderText = "MigrateSql";

            DataGridViewTextBoxColumn AfterMigrateSql = new DataGridViewTextBoxColumn();
            AfterMigrateSql.HeaderText = "AfterMigrateSql";


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

        }
        //动态删除行
        private void doWithGridUnChecked(TreeNode tn)
        {
            if (!tn.Checked)
            {
                for (int i = 0; i < dgvU9.Rows.Count; i++)
                {
                    if (dgvU9.Rows[i].Cells[0].Value != null && dgvU9.Rows[i].Cells[0].Value.ToString() == tn.Name)
                    {
                        dgvU9.Rows.RemoveAt(i);
                        list.Remove(list.Find(t => t.ModuleName == tn.Name));
                        return;
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
                if (((DataGridViewDisableButtonCell)this.dgvU9.Rows[e.RowIndex].Cells[5]).Enabled)
                {
                    string SQLEvaluate = dgvU9[8, e.RowIndex].Value.ToString();
                    // string SQLMigrate = dgvU9[9, e.RowIndex].Value.ToString();
                    this.dgvU9[4, e.RowIndex].Value = "评估中...";
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[e.RowIndex].Cells[6]).Enabled = false;
                    Evaluate(SQLEvaluate, e.RowIndex);
                }
            }
            else if (e.ColumnIndex == 6)//迁移
            {
                if (!((DataGridViewDisableButtonCell)this.dgvU9.Rows[e.RowIndex].Cells[6]).Enabled)//此时正在评估，则不能做迁移
                    return;
                //开始迁移后，评估不应该再响应事件
                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[e.RowIndex].Cells[5]).Enabled = false;

                this.dgvU9[4, e.RowIndex].Value = "评估中...";
                string evaluateSql = dgvU9[8, e.RowIndex].Value.ToString();
                string beforeMigrateSql = dgvU9[9, e.RowIndex].Value.ToString();
                string migrateSql = "";
                string afterMigrateSql = "";

                //如果在迁移前，没有估过，则要做一次评估。EvaluateResult result;
                if (this.dgvU9[4, e.RowIndex].Value.ToString() != "评估完成")
                    MigrateWithEvluate(evaluateSql, beforeMigrateSql, migrateSql, afterMigrateSql,e.RowIndex);
                else
                    Migrate(beforeMigrateSql, migrateSql, afterMigrateSql,e.RowIndex);
            }
        }
        private void Evaluate(string Evaluatesql, int rowindex)
        {
            EvaluateResult result;
            Task.Run(() =>
            {
                try
                {
                    result = ExecEvaluateSQL(Evaluatesql);
                    UpdateCount(result, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateCountError(ex.Message, rowindex);
                    return;
                }
                finally
                {
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[6]).Enabled = true;
                }
            });
        }
        private void Migrate(string BeforeMigrateSql, string migrateSql, string afterMigrateSql, int rowindex)
        {
            MigrateResult result;
            Task.Run(() =>
            {
                try
                {
                    result = ExecBeforeMigrateSQL(BeforeMigrateSql);
                    UpdateBeforeMigrateCount(result, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateBeforeMigrateCountError(ex.Message, rowindex);
                    return;
                }
                finally
                {
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                }
            }).ContinueWith(t =>
            {
                try
                {
                    result = ExecMigrateSQL(migrateSql);
                    UpdateMigrateCount(result, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateMigrateCountCountError(ex.Message, rowindex);
                    return;
                }
                finally
                {
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                }
            }).ContinueWith(tt =>
            {
                try
                {
                    result = ExecAfterMigrateSQL(afterMigrateSql); 
                    UpdateAfterMigrateCount(result, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateAfterMigrateCountError(ex.Message, rowindex);
                    return;
                }
                finally
                {
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                }
            });

        }
        private void MigrateWithEvluate(string Evaluatesql, string BeforeMigrateSql, string migrateSql, string afterMigrateSql, int rowindex)
        {
            EvaluateResult result;
            MigrateResult result2;
            Task.Run(() =>
            {
                try
                {
                    result = ExecEvaluateSQL(Evaluatesql);
                    UpdateCount(result, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateCountError(ex.Message, rowindex);
                    return;
                }
                finally
                {
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                }
            }).ContinueWith(t =>
            {
                try
                {
                    result2 = ExecBeforeMigrateSQL(BeforeMigrateSql);
                    UpdateBeforeMigrateCount(result2, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateBeforeMigrateCountError(ex.Message, rowindex);
                    return;
                }
                finally
                {
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                }
            }).ContinueWith(t =>
            {
                try
                {
                    result2 = ExecMigrateSQL(migrateSql);
                    UpdateMigrateCount(result2, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateMigrateCountCountError(ex.Message, rowindex);
                    return;
                }
                finally
                {
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                }
            }).ContinueWith(tt =>
            {
                try
                {
                    result2 = ExecAfterMigrateSQL(afterMigrateSql);
                    UpdateAfterMigrateCount(result2, rowindex);
                }
                catch (Exception ex)
                {
                    UpdateAfterMigrateCountError(ex.Message, rowindex);
                    return;
                }
                finally
                {
                    ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[5]).Enabled = true;
                }
            });

        }
        private void UpdateCount(EvaluateResult result, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[1, rowindex].Value = result.date;
                this.dgvU9[2, rowindex].Value = result.Evaluatecount.ToString();
                this.dgvU9[4, rowindex].Value = "评估完成";

                MigrateItem item= list.Find(t => t.ModuleName == this.dgvU9[0, rowindex].Value.ToString());
                if (item!=null)
                {
                    item.State = "评估完成"; 
                    item.EarlyDate= result.date;
                    item.toMigirateCount = result.Evaluatecount;
                }

                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[6]).Enabled = true;
            }));
        }

        private void UpdateCountError(string err, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = err;
                ((DataGridViewDisableButtonCell)this.dgvU9.Rows[rowindex].Cells[6]).Enabled = true;
            }));
        }


        private void UpdateBeforeMigrateCount(MigrateResult result, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = "准备工作完成";

            }));
        }

        private void UpdateBeforeMigrateCountError(string err, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = err;

            }));
        }

        private void UpdateMigrateCount(MigrateResult result, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = "迁移执行中...";

            }));
        }

        private void UpdateMigrateCountCountError(string err, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = err;

            }));
        }


        private void UpdateAfterMigrateCount(MigrateResult result, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = "迁移完成";

            }));
        }

        private void UpdateAfterMigrateCountError(string err, int rowindex)
        {
            BeginInvoke(new Action(() =>
            {
                this.dgvU9[4, rowindex].Value = err;

            }));
        }





        private EvaluateResult ExecEvaluateSQL(string sql)
        {
            var rand = new Random();
            Thread.Sleep(rand.Next(500, 5000));

            return new EvaluateResult { date = "2016-61-61", Evaluatecount = rand.Next(300, 3000) };
        }

        private MigrateResult ExecBeforeMigrateSQL(string sql)
        {
            var rand = new Random();
            Thread.Sleep(rand.Next(500, 5000));

            return new MigrateResult { MigrateCount = rand.Next(300, 3000) };
        }

        private MigrateResult ExecMigrateSQL(string sql)
        {
            var rand = new Random();
            Thread.Sleep(rand.Next(500, 5000));

            return new MigrateResult { MigrateCount = rand.Next(300, 3000) };
        }

        private MigrateResult ExecAfterMigrateSQL(string sql)
        {
            var rand = new Random();
            Thread.Sleep(rand.Next(500, 5000));

            return new MigrateResult { MigrateCount = rand.Next(300, 3000) };
        }

    }

    public class EvaluateResult
    {
        public string date { get; set; }
        public int Evaluatecount { get; set; }
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
