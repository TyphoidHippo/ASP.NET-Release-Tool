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
        private readonly StreamWriter _Log;
        private readonly string _PathSettingsFile;
        private readonly IReadOnlyCollection<StringControls.StringBase> _StringControls;

        private readonly StringControls.Folder _PC_RootSource;
        private readonly StringControls.Folder _PC_RootStable;
        private readonly StringControls.Folder _PC_RootBackups;
        private readonly StringControls.Folder _PC_RootDemoBeta;
        private readonly StringControls.Folder _PC_RootProductionLive;
        private readonly StringControls.Folder _PC_RootProductionClean;

        private readonly StringControls.FilePath _PC_ConfigStable;
        private readonly StringControls.FilePath _PC_ConfigDemoBeta;
        private readonly StringControls.FilePath _PC_ConfigProductionLive;

        private readonly StringControls.Integer _PC_IORetries;

        private int IORetries { get { return this._PC_IORetries.Value; } }

        private static T CreatePathControl<T>(List<StringControls.StringBase> Storage,
            Button pButton, TextBox pTextBox, Action pSaveCallback, string pPathKey) where T : StringControls.PathBase
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
                var stringControls = new List<StringControls.StringBase>();
                this._PC_RootSource = CreatePathControl<StringControls.Folder>(stringControls, this.btnPathRootSource, this.txtPathRootSource, this.SaveSettingsFile, "PathRootSource");
                this._PC_RootStable = CreatePathControl<StringControls.Folder>(stringControls, this.btnPathRootStable, this.txtPathRootStable, this.SaveSettingsFile, "PathRootStable");
                this._PC_RootBackups = CreatePathControl<StringControls.Folder>(stringControls, this.btnPathRootBackups, this.txtPathRootBackups, this.SaveSettingsFile, "PathRootBackups");
                this._PC_RootDemoBeta = CreatePathControl<StringControls.Folder>(stringControls, this.btnPathRootDemoBeta, this.txtPathRootDemoBeta, this.SaveSettingsFile, "PathRootDemoBeta");
                this._PC_RootProductionLive = CreatePathControl<StringControls.Folder>(stringControls, this.btnPathRootProductionLive, this.txtPathRootProductionLive, this.SaveSettingsFile, "PathRootProductionLive");
                this._PC_RootProductionClean = CreatePathControl<StringControls.Folder>(stringControls, this.btnPathRootProductionClean, this.txtPathRootProductionClean, this.SaveSettingsFile, "PathRootProductionClean");

                this._PC_ConfigStable = CreatePathControl<StringControls.FilePath>(stringControls, this.btnPathConfigStable, this.txtPathConfigStable, this.SaveSettingsFile, "PathConfigStable");
                this._PC_ConfigDemoBeta = CreatePathControl<StringControls.FilePath>(stringControls, this.btnPathConfigDemoBeta, this.txtPathConfigDemoBeta, this.SaveSettingsFile, "PathConfigDemoBeta");
                this._PC_ConfigProductionLive = CreatePathControl<StringControls.FilePath>(stringControls, this.btnPathConfigProductionLive, this.txtPathConfigProductionLive, this.SaveSettingsFile, "PathConfigProductionLive");

                this._PC_IORetries = new StringControls.Integer(this.txtIORetries, this.SaveSettingsFile, "IORetries");
                stringControls.Add(this._PC_IORetries);

                this._StringControls = stringControls;
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

            foreach (var pc in this._StringControls)
            {
                pc.Load(paths);
            }
        }

        private void SaveSettingsFile()
        {
            using (var sw = File.CreateText(this._PathSettingsFile))
            {
                foreach (var pc in this._StringControls)
                {
                    pc.Save(sw);
                }
            }
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
            this.chkReleaseLive_CleanAfterRelease.InvokeSetEnabled(pEnabled);
            this.chkReleaseLive_CleanBeforeRelease.InvokeSetEnabled(pEnabled);
            this.chkReleaseLive_CopyCurrentReleaseToStable.InvokeSetEnabled(pEnabled);
            this.btnReleaseLive.InvokeSetEnabled(pEnabled);
            this.btnRestoreBackup.InvokeSetEnabled(pEnabled);
            this.btnRestoreStable.InvokeSetEnabled(pEnabled);
            this.btnReleaseDemoBeta.InvokeSetEnabled(pEnabled);

            foreach (var pc in this._StringControls)
            {
                if (pc is StringControls.PathBase)
                {
                    ((StringControls.PathBase)pc).Enabled = pEnabled;
                }
            }
        }

        private void CheckForBadWebConfig()
        {
            string badWebConfig = this._PC_RootSource.Value + "web.config";
            if (File.Exists(badWebConfig))
            {
                var dr = MessageBox.Show("Web.config found in source files.  Delete?", "STOP!", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                if (dr != DialogResult.OK && dr != DialogResult.Yes)
                {
                    MessageBox.Show("Remove the web.config from the build source before attempting to release");
                    return;
                }
                List<string> errors = new List<string>();
                FileOperations.AttemptAction(IORetries, () => File.Delete(badWebConfig), "Deleting web.config from build source", errors, this._Log);
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

            if (this.chkReleaseLive_CopyCurrentReleaseToStable.Checked)
            {
                commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.ClearFolder(this._PC_RootStable.Value, this.UpdateTaskStatus, this._Log, IORetries), "Clearing Stable"));
                commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyFolder(this._PC_RootProductionClean.Value, this._PC_RootStable.Value, this.UpdateTaskStatus, this._Log, IORetries), "Copying Production-Clean to Stable"));
                commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyWebConfig(this._PC_ConfigStable.Value, this._PC_RootStable.Value, this.UpdateTaskStatus, this._Log, IORetries), "Copying web.config to Stable"));
            }
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.BackupFolder(this._PC_RootProductionClean.Value, this._PC_RootBackups.Value, this.UpdateTaskStatus, this._Log, IORetries), "Backing up Production-Clean"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.ClearFolder(this._PC_RootProductionClean.Value, this.UpdateTaskStatus, this._Log, IORetries), "Clearing Production-Clean"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyFolder(this._PC_RootSource.Value, this._PC_RootProductionClean.Value, this.UpdateTaskStatus, this._Log, IORetries), "Copying Build Source to Production-Clean"));

            if (this.chkReleaseLive_CleanBeforeRelease.Checked)
            {
                commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.ClearFolder(this._PC_RootProductionLive.Value, this.UpdateTaskStatus, this._Log, IORetries), "Clearing Production-Live"));
            }

            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyFolder(this._PC_RootSource.Value, this._PC_RootProductionLive.Value, this.UpdateTaskStatus, this._Log, IORetries), "Copying Build Source to Production-Live"));
            if(this.chkReleaseLive_CleanAfterRelease.Checked)
            {
                commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.ClearExtraFiles(this._PC_RootSource.Value, this._PC_RootProductionLive.Value, this.UpdateTaskStatus, this._Log, IORetries), "Clearing extra files from Production-Live that don't exist in build source"));
            }
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyWebConfig(this._PC_ConfigProductionLive.Value, this._PC_RootProductionLive.Value, this.UpdateTaskStatus, this._Log, IORetries), "Copying web.config to Production-Live"));

            this.RunCommands("Releasing Production-Live", commands);
        }

        private void btnReleaseDemoBeta_Click(object sender, EventArgs e)
        {
            this.CheckForBadWebConfig();
            List<Tuple<Func<IReadOnlyCollection<string>>, string>> commands = new List<Tuple<Func<IReadOnlyCollection<string>>, string>>();

            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.ClearFolder(this._PC_RootDemoBeta.Value, this.UpdateTaskStatus, this._Log, IORetries), "Clearing Demo/Beta"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyFolder(this._PC_RootSource.Value, this._PC_RootDemoBeta.Value, this.UpdateTaskStatus, this._Log, IORetries), "Copying Build Source to Demo/Beta"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyWebConfig(this._PC_ConfigDemoBeta.Value, this._PC_RootDemoBeta.Value, this.UpdateTaskStatus, this._Log, IORetries), "Copying web.config to Demo/Beta"));

            this.RunCommands("Releasing Demo/Beta", commands);
        }

        private void btnRestoreStable_Click(object sender, EventArgs e)
        {
            List<Tuple<Func<IReadOnlyCollection<string>>, string>> commands = new List<Tuple<Func<IReadOnlyCollection<string>>, string>>();

            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyFolder(this._PC_RootStable.Value, this._PC_RootProductionLive.Value, this.UpdateTaskStatus, this._Log, IORetries), "Restoring from Stable to Production-Live"));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyWebConfig(this._PC_ConfigProductionLive.Value, this._PC_RootProductionLive.Value, this.UpdateTaskStatus, this._Log, IORetries), "Copying web.config to Production-Live"));

            this.RunCommands("Restoring from Stable to Production-Live", commands);
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            string pathBackup;
            {
                var fbd = new FolderBrowserDialog();
                fbd.SelectedPath = this._PC_RootBackups.Value;

                var dr = fbd.ShowDialog();

                if (dr != DialogResult.OK && dr != DialogResult.Yes) { return; }

                pathBackup = StringControls.NormalizeFolderPath(fbd.SelectedPath);
            }

            string msg = string.Format("Restoring Backup ({0}) to Production-Live", Path.GetDirectoryName(pathBackup));

            List<Tuple<Func<IReadOnlyCollection<string>>, string>> commands = new List<Tuple<Func<IReadOnlyCollection<string>>, string>>();
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyFolder(pathBackup, this._PC_RootProductionLive.Value, this.UpdateTaskStatus, this._Log, IORetries), msg));
            commands.Add(new Tuple<Func<IReadOnlyCollection<string>>, string>(() => FileOperations.CopyWebConfig(this._PC_ConfigProductionLive.Value, this._PC_RootProductionLive.Value, this.UpdateTaskStatus, this._Log, IORetries), "Copying web.config to Production-Live"));

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
