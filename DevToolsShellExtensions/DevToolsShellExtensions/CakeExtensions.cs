using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace DevToolsShellExtensions
{
    public abstract class CakeMenuItemBase : MenuBaseItem
    {
        protected CakeMenuItemBase(SharpContextMenu menu)
            : base(menu)
        {

        }

        protected override IEnumerable<string> GetFileTypesHandled()
        {
            return new[]
            {
                ".ps1",
                ".psm1",
            };
        }

        private FileInfo GetCakeImageFile()
        {
            const string iconUrl = @"https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-medium.png";
            const string tempCakeFileName = "cakebuild_icon.png";

            var cakeIconFile = Path.Combine(Path.GetTempPath(), tempCakeFileName);
            if (!File.Exists(cakeIconFile))
            {
                return DownloadFile(iconUrl, cakeIconFile);
            }

            return new FileInfo(cakeIconFile);
        }

        private Bitmap _CakeImage = null;
        private Bitmap CakeImage
        {
            get
            {
                if (this._CakeImage == null)
                {
                    var cakeImageFile = this.GetCakeImageFile();
                    if (cakeImageFile.Exists)
                    {
                        this._CakeImage = new Bitmap(cakeImageFile.FullName);
                    }
                }

                return this._CakeImage;
            }
        }

        protected override Bitmap GetIconImage()
        {
            return this.CakeImage;
        }
    }

    public class CakeMenuSeparator : MenuBaseSeparator
    {
        public CakeMenuSeparator(SharpContextMenu menu)
            : base(menu)
        { 
        }

        protected override IEnumerable<string> GetFileTypesHandled()
        {
            return new[]
            {
                ".cake",
            };
        }

        protected override bool GetIsVisible()
        {
            return this.GetFilesToHandle().Any() | this.GetDirectoriesToHandle().Any();
        }
    }

    public class AddCakeScriptsShellMenuItem : PowerShellMenuItemBase
    {
        private List<FileInfo> _Scripts = null;
        public List<FileInfo> Scripts
        {
            get
            {
                if (this._Scripts == null || !this._Scripts.Any())
                {
                    this._Scripts = this.GetCakeScripts().ToList();
                }

                return this._Scripts;
            }
        }

        public AddCakeScriptsShellMenuItem(SharpContextMenu menu)
            : base(menu)
        {

        }

        private IEnumerable<FileInfo> GetCakeScripts()
        {
            var cakeScripts = new Tuple<string, string>[]
            {
                new Tuple<string, string>("build.ps1", @"https://cakebuild.net/download/bootstrapper/windows"),
                new Tuple<string, string>("build.sh", @"https://cakebuild.net/download/bootstrapper/linux"),
                new Tuple<string, string>("build.cake",
                    @"https://github.com/cake-build/example/blob/master/build.cake"),
            };

            var tempDir = Path.GetTempPath();
            var files = new List<FileInfo>();

            foreach (var cScript in cakeScripts)
            {
                var scriptPath = Path.Combine(tempDir, cScript.Item1);
                if (File.Exists(scriptPath))
                {
                    files.Add(new FileInfo(scriptPath));
                }
                else
                {
                    var file = DownloadFile(cScript.Item2, scriptPath);
                    files.Add(file);
                }
            }

            return files;
        }

        protected override void HandleClick(object sender, EventArgs e)
        {
            var dirsToOpen = this.GetDirectoriesToHandle();
            foreach (var d in dirsToOpen)
            {
                foreach (var s in this.Scripts)
                {
                    s.CopyTo(Path.Combine(d.FullName, s.Name), true);
                }
            }
        }

        protected override string GetMenuText()
        {
            return "Add Cake Scripts";
        }

        protected override bool GetIsVisible()
        {
            return this.GetDirectoriesToHandle().Any() && this.Scripts.Any();
        }
    }
}
