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
        public Form3()
        {
            InitializeComponent();
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

        }
    }

    public class Result
    {
        public string begindate { get; set; }
        public int count { get; set; }
    }
    public class ArItem
    {
        public string arProc { get; set; }
        public string Potisoon { get; set; }
    }



















    /// <summary>
    /// 数据迁移配置文件
    /// </summary>
    public class ARModuleInfo
    {
        private static readonly object _syncLock = new object();

        private static bool bFwatchRun = false;
        private static ARModuleInfo m_ARMigrate;

        //所有未迁移的模块
        public List<MigrateModuleInfo> RestModules { get; set; }
        //所有选择要迁移的模块
        public List<MigrateModuleInfo> TodoModules { get; set; }
        //所有已迁移的模块
        public List<MigrateModuleInfo> DoneModules { get; set; }
        private System.Collections.Specialized.HybridDictionary m_CustomConfig = new System.Collections.Specialized.HybridDictionary();
        public System.Collections.Specialized.HybridDictionary CustomConfig
        {
            get
            {
                return m_CustomConfig;
            }
        }

        public static ARModuleInfo Instance
        {
            get
            {
                if (m_ARMigrate != null)
                {
                    return m_ARMigrate;
                }
                lock (_syncLock)
                {
                    if (m_ARMigrate == null)
                    {
                        m_ARMigrate = new ARModuleInfo();
                    }
                    return m_ARMigrate;
                }
            }
        }

        ARModuleInfo()
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

            XmlNode rootnode = dom.SelectSingleNode("ARRoot/ARMigrateRestItems");
            LoadModuleItem(rootnode);

            rootnode = dom.SelectSingleNode("ARRoot/ARMigrateToDoItems");
            LoadModuleItem(rootnode);

            rootnode = dom.SelectSingleNode("ARRoot/ARMigrateDoneItems");
            LoadModuleItem(rootnode);

            //自定义配置，配置为测试环境等。
            rootnode = dom.SelectSingleNode("ARRoot/ARCustomConfigurations");
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
                if (node.Name != "ARItem") continue;

                MigrateModuleInfo sp = new MigrateModuleInfo();

                if (node.Attributes["key"] != null)
                    sp.ModuleKey = node.Attributes["key"].Value;
                if (node.Attributes["value"] != null)
                    sp.ModuleValue = node.Attributes["value"].Value;
                if (node.Attributes["MigrateSql"] != null)
                    sp.ModuleMigrateSql = node.Attributes["MigrateSql"].Value;
                if (node.Attributes["EvaluateSql"] != null)
                    sp.ModuleEvaluateSql = node.Attributes["EvaluateSql"].Value;

                if (rootnode.Name == "ARMigrateRestItems")
                    RestModules.Add(sp);
                else if (rootnode.Name == "ARMigrateToDoItems")
                    TodoModules.Add(sp);
                else if (rootnode.Name == "ARMigrateDoneItems")
                    DoneModules.Add(sp);
            }
        }
        private void ReLoad()
        {
            Thread.Sleep(2000);
            lock (_syncLock)
            {
                m_ARMigrate = new ARModuleInfo();
            }
        }

    }

    public class MigrateModuleInfo
    {
        public string ModuleKey { get; set; }
        public string ModuleValue { get; set; }
        public string ModuleMigrateSql { get; set; }
        public string ModuleEvaluateSql { get; set; }
    }
    //1。增加匹配文件，项目：迁移模块、迁移存储过程、评估存储过程、恢复单据存储过程
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
