using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASP.NET_Release_Tool
{
    static class StringControls
    {
        public static string NormalizeFolderPath(string p)
        {
            if (string.IsNullOrWhiteSpace(p)) { return ""; }

            p = p.Trim();
            p = p.Replace('/', '\\');
            if (!p.EndsWith("\\")) { p = p + "\\"; }
            return p;
        }

        public abstract class StringBase
        {
            public readonly string SettingsKey;
            private readonly Action _SaveCallback;
            private readonly TextBox _TextBox;
            private string _LastValue;
            protected virtual string DefaultValue { get { return ""; } }

            public virtual string Value { get { return this._TextBox.Text; } }
            protected virtual void SetValue(string value)
            {
                this._LastValue = this._TextBox.Text;
                this._TextBox.Text = value;
                this._SaveCallback();
            }

            protected StringBase(TextBox pTextBox, Action pSaveCallback, string pSettingsKey)
            {
                this._TextBox = pTextBox;
                this._SaveCallback = pSaveCallback;
                this.SettingsKey = pSettingsKey;
                this._TextBox.TextChanged += new System.EventHandler(this._TextBox_TextChanged);
            }

            public void Load(Dictionary<string, string> p)
            {
                var key = this.SettingsKey.ToUpper();
                var val = p.ContainsKey(key) ? p[key] : this.DefaultValue;
                this.SetValue(val);
            }

            public void Save(StreamWriter sw)
            {
                sw.WriteLine(this.SettingsKey + "=" + this._TextBox.Text);
            }



            private void _TextBox_TextChanged(object sender, EventArgs e)
            {
                var newValue = this._TextBox.Text;
                if(!this.Validate(newValue))
                {
                    newValue = this._LastValue;
                }
                this.SetValue(newValue);
            }
            protected abstract bool Validate(string value);
        }

        public abstract class PathBase : StringBase
        {
            private static string _LastFolderPath = "";

            private readonly Button _Button;

            public bool Enabled
            {
                get { return this._Button.Enabled; }
                set { this._Button.InvokeSetEnabled(value); }
            }

            protected PathBase(Button pButton, TextBox pTextBox, Action pSaveCallback, string pPathKey)
                :base(pTextBox, pSaveCallback, pPathKey)
            {
                this._Button = pButton;
                this._Button.Click += new System.EventHandler(this.Click);
            }


            protected abstract string Click(string pStartPath);

            private void Click(object sender, EventArgs e)
            {
                string startPath = string.IsNullOrWhiteSpace(_LastFolderPath)
                    ? this.Value : _LastFolderPath;

                string newPath = this.Click(startPath);
                if (string.IsNullOrWhiteSpace(newPath)) { return; }

                _LastFolderPath = System.IO.Path.GetDirectoryName(newPath);

                this.SetValue(newPath);
            }
        }

        public class Integer : StringBase
        {
            public Integer(TextBox pTextBox, Action pSaveCallback, string pSettingsKey)
                :base(pTextBox, pSaveCallback, pSettingsKey)
            {
            }
            public new int Value { get { return int.Parse(base.Value); } }
            protected override string DefaultValue { get { return "100"; } }
            protected override void SetValue(string value)
            {
                int val;
                if(!int.TryParse(value, out val))
                {
                    val = int.Parse(this.DefaultValue);
                }
                base.SetValue(val.ToString());
            }
            protected override bool Validate(string value)
            {
                int i;
                return int.TryParse(value, out i);
            }
        }

        public class Folder : PathBase
        {
            public Folder(Button pButton, TextBox pTextBox, Action pSaveCallback, string pPathKey)
                : base(pButton, pTextBox, pSaveCallback, pPathKey)
            {
            }

            public override string Value
            {
                get { return NormalizeFolderPath(base.Value); }
            }

            protected override string Click(string pStartPath)
            {
                var fbd = new FolderBrowserDialog();
                fbd.SelectedPath = pStartPath;

                var dr = fbd.ShowDialog();

                if (dr != DialogResult.OK && dr != DialogResult.Yes) { return null; }

                return fbd.SelectedPath;
            }

            protected override bool Validate(string value)
            {
                return Directory.Exists(value);
            }
        }


        public class FilePath : PathBase
        {
            public FilePath(Button pButton, TextBox pTextBox, Action pSaveCallback, string pPathKey)
                : base(pButton, pTextBox, pSaveCallback, pPathKey)
            {
            }

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
                ofd.Title = "Open: " + this.SettingsKey;

                var dr = ofd.ShowDialog();

                if (dr != DialogResult.OK && dr != DialogResult.Yes) { return null; }

                return ofd.FileName;
            }

            protected override bool Validate(string value)
            {
                return File.Exists(value);
            }
        }
    }
}