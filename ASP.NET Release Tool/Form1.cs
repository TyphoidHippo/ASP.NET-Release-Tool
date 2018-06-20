using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ASP.NET_Release_Tool
{
    public partial class Form1 : Form
    {
        private const int IORetries = 50;
        private readonly StreamWriter _Log;
        private readonly string _PathSettingsFile;
        private readonly IReadOnlyCollection<PathControls.PathBase> _PathControls;

        private readonly PathControls.Folder _PC_RootSource;
        private readonly PathControls.Folder _PC_RootStable;
        private readonly PathControls.Folder _PC_RootBackups;
        private readonly PathControls.Folder _PC_RootDemoBeta;
        private readonly PathControls.Folder _PC_RootProductionLive;
        private readonly PathControls.Folder _PC_RootProductionClean;

        private readonly PathControls.FilePath _PC_ConfigStable;
        private readonly PathControls.FilePath _PC_ConfigDemoBeta;
        private readonly PathControls.FilePath _PC_ConfigProductionLive;

        private static T CreatePC<T>(List<PathControls.PathBase> Storage,
            Button pButton, TextBox pTextBox, Action pSaveCallback, string pPathKey) where T : PathControls.PathBase
        {
            T result = (T)Activator.CreateInstance(typeof(T), pButton, pTextBox, pSaveCallback, pPathKey);
            Storage.Add(result);
            return result;
        }

        public Form1()
        {
            this.InitializeComponent();
            this.FormClosing += Form1_FormClosing;
            this._SystemTrayIcon.ContextMenu = new ContextMenu();
            this._SystemTrayIcon.ContextMenu.MenuItems.Add(new MenuItem("Close", _SystemTrayIcon_MenuCloseClick));

            {
                List<PathControls.PathBase> pathControls = new List<PathControls.PathBase>();
                this._PC_RootSource = CreatePC<PathControls.Folder>(pathControls, this.btnPathRootSource, this.txtPathRootSource, this.SaveSettingsFile, "PathRootSource");
                this._PC_RootStable = CreatePC<PathControls.Folder>(pathControls, this.btnPathRootStable, this.txtPathRootStable, this.SaveSettingsFile, "PathRootStable");
                this._PC_RootBackups = CreatePC<PathControls.Folder>(pathControls, this.btnPathRootBackups, this.txtPathRootBackups, this.SaveSettingsFile, "PathRootBackups");
                this._PC_RootDemoBeta = CreatePC<PathControls.Folder>(pathControls, this.btnPathRootDemoBeta, this.txtPathRootDemoBeta, this.SaveSettingsFile, "PathRootDemoBeta");
                this._PC_RootProductionLive = CreatePC<PathControls.Folder>(pathControls, this.btnPathRootProductionLive, this.txtPathRootProductionLive, this.SaveSettingsFile, "PathRootProductionLive");
                this._PC_RootProductionClean = CreatePC<PathControls.Folder>(pathControls, this.btnPathRootProductionClean, this.txtPathRootProductionClean, this.SaveSettingsFile, "PathRootProductionClean");

                this._PC_ConfigStable = CreatePC<PathControls.FilePath>(pathControls, this.btnPathConfigStable, this.txtPathConfigStable, this.SaveSettingsFile, "PathConfigStable");
                this._PC_ConfigDemoBeta = CreatePC<PathControls.FilePath>(pathControls, this.btnPathConfigDemoBeta, this.txtPathConfigDemoBeta, this.SaveSettingsFile, "PathConfigDemoBeta");
                this._PC_ConfigProductionLive = CreatePC<PathControls.FilePath>(pathControls, this.btnPathConfigProductionLive, this.txtPathConfigProductionLive, this.SaveSettingsFile, "PathConfigProductionLive");
                this._PathControls = pathControls;
            }
            {
                var pathAppRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                this._PathSettingsFile = pathAppRoot + "\\settings.ini";
                this._Log = File.CreateText(pathAppRoot + "\\log.log");
            }
            {
                int failCount = 0;
                while (true)
                {
                    try
                    {
                        this.LoadSettingsFile();
                        this.SaveSettingsFile();
                        break;
                    }
                    catch (FileNotFoundException)
                    {
                        if (failCount++ > 2) { throw; }
                        this.SaveSettingsFile();
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
        }

        #region Form Events
        protected override void OnClosed(EventArgs e)
        {
            this._Log.Dispose();
            base.OnClosed(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this._SystemTrayIcon.Visible = true;
            }
        }
        #endregion

        private void LoadSettingsFile()
        {
            Dictionary<string, string> paths = new Dictionary<string, string>();
            using (var fs = File.OpenText(this._PathSettingsFile))
            {
                while (!fs.EndOfStream)
                {
                    var line = fs.ReadLine();
                    var parts = line.Split('=');
                    if (parts.Length != 2) { continue; }

                    var key = parts[0].Trim();
                    var val = parts[1].Trim();
                    paths.Add(key.ToUpper(), val);
                }
            }

            foreach (var pc in this._PathControls)
            {
                pc.Load(paths);
            }
        }

        private void SaveSettingsFile()
        {
            using (var sw = File.CreateText(this._PathSettingsFile))
            {
                foreach (var pc in this._PathControls)
                {
                    pc.Save(sw);
                }
            }
        }


        private static void AttemptAction(Action pAction, string pActionDescriptionForErrorMessage, List<string> pErrorOut, StreamWriter pLogger)
        {
            bool success = false;
            for (int i = 0; i < IORetries; i++)
            {
                try
                {
                    pAction();
                    success = true;
                    break;
                }
                catch (Exception ex)
                {
                    pLogger.WriteLine("==ERROR==" + ex.InnermostExceptionMessage());
                }
            }
            if (!success) { pErrorOut.Add(string.Format("Failed ({0} attempts) at {1}", IORetries, pActionDescriptionForErrorMessage)); }
        }

        private static IReadOnlyCollection<string> ClearFolder(string pFolder, Action<double, string> pCallbackStatus, StreamWriter pLogger)
        {
            pFolder = PathControls.PathBase.NormalizeFolderPath(pFolder);
            pCallbackStatus(0, "Calculating...");

            var files = Directory.GetFiles(pFolder, "*", SearchOption.AllDirectories);
            var folders = Directory.GetDirectories(pFolder, "*", SearchOption.AllDirectories);

            int itemCount = files.Length + folders.Length;

            List<string> errors = new List<string>();
            int iItem = 0;

            foreach (var file in files)
            {
                pCallbackStatus(
                    ++iItem / (double)itemCount,
                    string.Format("Deleting {0} : {1}",
                    Path.GetDirectoryName(file),
                    Path.GetFileName(file)));

                AttemptAction(() => File.Delete(file), "deleting file: " + file, errors, pLogger);
            }

            foreach (var folder in folders)
            {
                pCallbackStatus(++iItem / (double)itemCount, "Deleting " + folder);

                if (Directory.Exists(folder))
                {
                    AttemptAction(() => Directory.Delete(folder, true), "deleting folder: " + folder, errors, pLogger);
                }
            }

            return errors;
        }

        private static IReadOnlyCollection<string> CopyFolder(string pSourceRoot, string pDestinationRoot, Action<double, string> pCallbackStatus, StreamWriter pLogger)
        {
            pSourceRoot = PathControls.PathBase.NormalizeFolderPath(pSourceRoot);
            pDestinationRoot = PathControls.PathBase.NormalizeFolderPath(pDestinationRoot);
            pCallbackStatus(0, "Calculating...");

            var files = Directory.GetFiles(pSourceRoot, "*", SearchOption.AllDirectories);
            var folders = Directory.GetDirectories(pSourceRoot, "*", SearchOption.AllDirectories);
            int itemCount = files.Length + folders.Length;
            List<string> errors = new List<string>();
            int iItem = 0;

            foreach (var folder in folders)
            {
                pCallbackStatus(++iItem / (double)itemCount, "Creating " + folder);

                var relFolder = folder.Substring(pSourceRoot.Length);
                var dstFolder = pDestinationRoot + relFolder;
                AttemptAction(() => Directory.CreateDirectory(dstFolder), "creating folder: " + relFolder, errors, pLogger);
            }

            foreach (var file in files)
            {
                pCallbackStatus(
                    ++iItem / (double)itemCount,
                    string.Format("Copying {0} : {1}",
                    Path.GetDirectoryName(file),
                    Path.GetFileName(file)));

                var relFile = file.Substring(pSourceRoot.Length);
                var dstFile = pDestinationRoot + relFile;
                AttemptAction(() => File.Copy(file, dstFile, true), "copying file: " + relFile, errors, pLogger);
            }

            return errors;
        }

        private static IReadOnlyCollection<string> BackupFolder(string pSourceRoot, string pDestinationRoot, Action<double, string> pCallbackStatus, StreamWriter pLogger)
        {
            pDestinationRoot = PathControls.PathBase.NormalizeFolderPath(pDestinationRoot);
            string rel = DateTime.Now.ToString("yyyyMMdd HH.mm.ss tt");
            return CopyFolder(pSourceRoot, pDestinationRoot + rel, pCallbackStatus, pLogger);
        }

        private static IReadOnlyCollection<string> CopyWebConfig(string pSourceFile, string pDestinationFolder, Action<double, string> pCallbackStatus, StreamWriter pLogger)
        {
            string msg = string.Format("Copying config file from {0} to {1}", Path.GetDirectoryName(pSourceFile), pDestinationFolder);

            pCallbackStatus(0.25, msg);
            List<string> errors = new List<string>();
            AttemptAction(() => File.Copy(pSourceFile, pDestinationFolder + "web.config", true), msg, errors, pLogger);
            pCallbackStatus(1.0, "Complete");

            return errors;
        }

        private void UpdateTotalStatus(double pPercent, string pMessage)
        {
            pPercent = pPercent < 0 ? 0 : pPercent > 1 ? 1 : pPercent;
            int range = this.progressBarTotal.Maximum - this.progressBarTotal.Minimum;
            this.progressBarTotal.InvokeSetValue(
                this.progressBarTotal.Minimum + (int)(pPercent * range));

            this.lblProgressTotal.InvokeSetText(pMessage);
            this._Log.WriteLine("Main Event: " + pMessage);
        }

        private void UpdateTaskStatus(double pPercent, string pMessage)
        {
            pPercent = pPercent < 0 ? 0 : pPercent > 1 ? 1 : pPercent;
            int range = this.progressBarCurrent.Maximum - this.progressBarCurrent.Minimum;
            this.progressBarCurrent.InvokeSetValue(
                this.progressBarCurrent.Minimum + (int)(pPercent * range));

            this.lblProgressCurrent.InvokeSetText(pMessage);
            this._Log.WriteLine("  Task Event: " + pMessage);
        }

        private void RunCommands(string pMainCommandDescription, List<Tuple<Func<IReadOnlyCollection<string>>, string>> pCommands)
        {
            this.EnableInputs(false);
            List<string> errors = new List<string>();

            Task.Run(() =>
            {
                for (int i = 0; i < pCommands.Count; i++)
                {
                    var cmd = pCommands[i];
                    this.UpdateTotalStatus(i / (double)pCommands.Count, cmd.Item2);
                    errors.AddRange(cmd.Item1());
                }

                this.UpdateTotalStatus(1.0, pMainCommandDescription + " completed!");

                if (errors.Count > 0)
                {
                    MessageBox.Show(string.Join("\r\n", errors), "Errors encountered while " + pMainCommandDescription);
                }
                this.EnableInputs(true);
            });
        }

        private void EnableInputs(bool pEnabled)
        {
            this.chkClearLiveFiles.InvokeSetEnabled(pEnabled);
            this.btnReleaseDemoBeta.InvokeSetEnabled(pEnabled);
            this.btnReleaseLive.InvokeSetEnabled(pEnabled);
            this.btnRestoreBackup.InvokeSetEnabled(pEnabled);
            this.btnRestoreStable.InvokeSetEnabled(pEnabled);

            foreach (var pc in this._PathControls) { pc.Enabled = pEnabled; }
        }

        private void CheckForBadWebConfig()
        {
            string badWebConfig = this._PC_RootSource.Path + "web.config";
            if (File.Exists(badWebConfig))
            {
                var dr = MessageBox.Show("Web.config found in source files.  Delete?", "STOP!", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                if (dr != DialogResult.OK && dr != DialogResult.Yes)
                {
                    MessageBox.Show("Remove the web.config from the build source before attempting to release");
                    return;
                }
                List<string> errors = new List<string>();
                AttemptAction(() => File.Delete(badWebConfig), "Deleting web.config from build source", errors, this._Log);
                if (errors.Count > 0)
                {
                    errors.Add("\r\n\r\nThis file must be deleted before release can continue (this really indicates you've got a path wrong - double check everything!)");
                    MessageBox.Show(string.Join("\r\n", errors), "Error deleting bad web.config file from build source");
                    return;
                }
            }
        }

        #region Control Events
        private void btnReleaseLive_Click(object sender, EventArgs e)
        {
            /* Clear    Stable
             * Copy     Production-Clean        to Stable
             * Copy     Production-Clean        to Backups
             * Copy     Stable.config           to Stable
             * Clear    Production-Clean
             * Copy     Build Source            to Production-Clean
             * if(Clear-Production-Live)        then        Clear Production-Live
             * Copy     Build Source            to Production-Live
             * Copy     Production-Live.config  to Production-Live
            /**/

            this.CheckForBadWebConfig();
            List<Tuple<Func<IReadOnlyCollection<string>>, string>> commands = new List<Tuple<Func<IReadOnlyCollection<string>>, string>>();

            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => ClearFolder(this._PC_RootStable.Path, this.UpdateTaskStatus, this._Log), "Clearing Stable"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyFolder(this._PC_RootProductionClean.Path, this._PC_RootStable.Path, this.UpdateTaskStatus, this._Log), "Copying Production-Clean to Stable"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => BackupFolder(this._PC_RootProductionClean.Path, this._PC_RootBackups.Path, this.UpdateTaskStatus, this._Log), "Backing up Production-Clean"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyWebConfig(this._PC_ConfigStable.Path, this._PC_RootStable.Path, this.UpdateTaskStatus, this._Log), "Copying web.config to Stable"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => ClearFolder(this._PC_RootProductionClean.Path, this.UpdateTaskStatus, this._Log), "Clearing Production-Clean"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyFolder(this._PC_RootSource.Path, this._PC_RootProductionClean.Path, this.UpdateTaskStatus, this._Log), "Copying Build Source to Production-Clean"));

            if (this.chkClearLiveFiles.Checked)
            {
                commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => ClearFolder(this._PC_RootProductionLive.Path, this.UpdateTaskStatus, this._Log), "Clearing Production-Live"));
            }

            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyFolder(this._PC_RootSource.Path, this._PC_RootProductionLive.Path, this.UpdateTaskStatus, this._Log), "Copying Build Source to Production-Live"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyWebConfig(this._PC_ConfigProductionLive.Path, this._PC_RootProductionLive.Path, this.UpdateTaskStatus, this._Log), "Copying web.config to Production-Live"));

            this.RunCommands("Releasing Production-Live", commands);
        }

        private void btnReleaseDemoBeta_Click(object sender, EventArgs e)
        {
            this.CheckForBadWebConfig();
            List<Tuple<Func<IReadOnlyCollection<string>>, string>> commands = new List<Tuple<Func<IReadOnlyCollection<string>>, string>>();

            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => ClearFolder(this._PC_RootDemoBeta.Path, this.UpdateTaskStatus, this._Log), "Clearing Demo/Beta"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyFolder(this._PC_RootSource.Path, this._PC_RootDemoBeta.Path, this.UpdateTaskStatus, this._Log), "Copying Build Source to Demo/Beta"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyWebConfig(this._PC_ConfigDemoBeta.Path, this._PC_RootDemoBeta.Path, this.UpdateTaskStatus, this._Log), "Copying web.config to Demo/Beta"));

            this.RunCommands("Releasing Demo/Beta", commands);
        }

        private void btnRestoreStable_Click(object sender, EventArgs e)
        {
            List<Tuple<Func<IReadOnlyCollection<string>>, string>> commands = new List<Tuple<Func<IReadOnlyCollection<string>>, string>>();

            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyFolder(this._PC_RootStable.Path, this._PC_RootProductionLive.Path, this.UpdateTaskStatus, this._Log), "Restoring from Stable to Production-Live"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyWebConfig(this._PC_ConfigProductionLive.Path, this._PC_RootProductionLive.Path, this.UpdateTaskStatus, this._Log), "Copying web.config to Production-Live"));

            this.RunCommands("Restoring from Stable to Production-Live", commands);
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            string pathBackup;
            {
                var fbd = new FolderBrowserDialog();
                fbd.SelectedPath = this._PC_RootBackups.Path;

                var dr = fbd.ShowDialog();

                if (dr != DialogResult.OK && dr != DialogResult.Yes) { return; }

                pathBackup = PathControls.PathBase.NormalizeFolderPath(fbd.SelectedPath);
            }

            string msg = string.Format("Restoring Backup ({0}) to Production-Live", Path.GetDirectoryName(pathBackup));

            List<Tuple<Func<IReadOnlyCollection<string>>, string>> commands = new List<Tuple<Func<IReadOnlyCollection<string>>, string>>();
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyFolder(pathBackup, this._PC_RootProductionLive.Path, this.UpdateTaskStatus, this._Log), msg));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => CopyWebConfig(this._PC_ConfigProductionLive.Path, this._PC_RootProductionLive.Path, this.UpdateTaskStatus, this._Log), "Copying web.config to Production-Live"));

            this.RunCommands(msg, commands);
        }

        private void _SystemTrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Focus();
            this.BringToFront();
            this._SystemTrayIcon.Visible = false;
        }

        private void _SystemTrayIcon_MenuCloseClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }

    #region Entry Point
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debugger.Break();
        }
    }
    #endregion

    static class Extensions
    {
        public static string InnermostExceptionMessage(this Exception p)
        {
            while (p.InnerException != null) { p = p.InnerException; }
            return p.Message;
        }

        public static void SafeInvoke<T>(this T p, Action<T> pAction) where T : Control
        { 
            if(p.InvokeRequired)
            {
                p.Invoke(pAction, p);
            }
            else
            {
                pAction(p);
            }
        }
        public static void InvokeSetText(this Label p, string pText)
        {
            p.SafeInvoke((x) => x.Text = pText);
        }
        public static void InvokeSetValue(this ProgressBar p, int pValue)
        {
            p.SafeInvoke((x) => x.Value = pValue);
        }
        public static void InvokeSetEnabled(this Control p, bool pEnabled)
        {
            p.SafeInvoke((x) => x.Enabled = pEnabled);
        }
    }
}
