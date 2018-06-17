using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASP.NET_Release_Tool.PathControls
{
    abstract class PathBase
    {
        public static string NormalizeFolderPath(string p)
        {
            if (string.IsNullOrWhiteSpace(p)) { return ""; }

            p = p.Trim();
            p = p.Replace('/', '\\');
            if (!p.EndsWith("\\")) { p = p + "\\"; }
            return p;
        }

        private static string _LastFolderPath = "";

        private readonly Button _Button;
        private readonly TextBox _TextBox;
        private readonly Action _SaveCallback;

        protected abstract bool PathResultIsFolder { get; }
        public string Path
        {
            get { return this.PathResultIsFolder ? NormalizeFolderPath(this._TextBox.Text) : this._TextBox.Text ; }
        }
        public bool Enabled
        {
            get { return this._Button.Enabled; }
            set { this._Button.Enabled = value; }
        }

        public readonly string PathKey;

        protected PathBase(Button pButton, TextBox pTextBox, Action pSaveCallback, string pPathKey)
        {
            this._Button = pButton;
            this._TextBox = pTextBox;
            this._Button.Click += new System.EventHandler(this.Click);
            this._SaveCallback = pSaveCallback;
            this.PathKey = pPathKey;
        }


        public void Load(Dictionary<string, string> p)
        {
            var key = this.PathKey.ToUpper();
            var val = p.ContainsKey(key) ? p[key] : "";
            this._TextBox.Text = val;
        }

        public void Save(StreamWriter sw)
        {
            sw.WriteLine(this.PathKey + "=" + this._TextBox.Text);
        }

        protected abstract string Click(string pStartPath);

        private void Click(object sender, EventArgs e)
        {
            string startPath = string.IsNullOrWhiteSpace(_LastFolderPath)
                ? this._TextBox.Text : _LastFolderPath;

            string newPath = this.Click(startPath);
            if (string.IsNullOrWhiteSpace(newPath)) { return; }

            _LastFolderPath = System.IO.Path.GetDirectoryName(newPath);

            this._TextBox.Text = newPath;
            this._SaveCallback();
        }
    }


    class Folder : PathBase
    {
        public Folder(Button pButton, TextBox pTextBox, Action pSaveCallback, string pPathKey)
            : base(pButton, pTextBox, pSaveCallback, pPathKey)
        {
        }

        protected override bool PathResultIsFolder { get { return true; } }

        protected override string Click(string pStartPath)
        {
            var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = pStartPath;

            var dr = fbd.ShowDialog();

            if (dr != DialogResult.OK && dr != DialogResult.Yes) { return null; }

            return fbd.SelectedPath;
        }
    }


    class FilePath : PathBase
    {
        public FilePath(Button pButton, TextBox pTextBox, Action pSaveCallback, string pPathKey)
            : base(pButton, pTextBox, pSaveCallback, pPathKey)
        {
        }

        protected override bool PathResultIsFolder { get { return false; } }

        protected override string Click(string pStartPath)
        {
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DefaultExt = "config";
            ofd.AddExtension = false;
            ofd.Filter = "Config File(*.config)|*.config|All files (*.*)|*.*";
            ofd.InitialDirectory = pStartPath;
            ofd.Multiselect = false;
            ofd.ReadOnlyChecked = false;
            ofd.ValidateNames = true;
            ofd.Title = "Open: " + this.PathKey;

            var dr = ofd.ShowDialog();

            if (dr != DialogResult.OK && dr != DialogResult.Yes) { return null; }

            return ofd.FileName;
        }
    }
}