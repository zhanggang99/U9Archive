using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace U9Archive
{
    public partial class Form3 : Form
    {
        //todo: 1.树控制加载处理。2.树控制与右边列表同步。 3。列表控件加载时，右边【评估】、【】
        public Form3()
        {
            InitializeComponent();
            LoadTree();
        }

        private void btnComputer_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void btnStop_Click(object sender, EventArgs e)
        {

        }

        //加载tree控件树
        private void LoadTree()
        {
            foreach (var item in MigrateModuleInfo.Instance.MigrateModules)
            {
                if (!treeU9Menu.Nodes.ContainsKey(item.Module))
                {
                    treeU9Menu.Nodes.Add(item.Module,item.Module);
                    treeU9Menu.Nodes.Find(item.Module, false)[0].Nodes.Add(item.ModuleValue,item.ModuleValue);
                        
                }
                else
                {
                    treeU9Menu.Nodes.Find(item.Module, false)[0].Nodes.Add(item.ModuleValue, item.ModuleValue);
                }
            }
        }

        private void treeU9Menu_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckNodes(e.Node);
        }

        private void CheckNodes(TreeNode checkRoot)
        {
            foreach (TreeNode tn in checkRoot.Nodes)
            {
                tn.Checked = checkRoot.Checked;
            }
        }
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
                if (node.Attributes["MigrateSql"] != null)
                    sp.ModuleMigrateSql = node.Attributes["MigrateSql"].Value;
                if (node.Attributes["EvaluateSql"] != null)
                    sp.ModuleEvaluateSql = node.Attributes["EvaluateSql"].Value;

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
        public string ModuleMigrateSql { get; set; }
        public string ModuleEvaluateSql { get; set; }
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





}
