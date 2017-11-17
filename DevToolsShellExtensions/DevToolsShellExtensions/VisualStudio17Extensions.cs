using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace DevToolsShellExtensions
{
    public abstract class Vs2017MenuItemBaseItem : MenuBaseItem
    {
        protected const string DevEnvPath =
            @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\devenv.exe";

        protected Vs2017MenuItemBaseItem(SharpContextMenu menu)
            : base(menu)
        {

        }

        protected override IEnumerable<string> GetFileTypesHandled()
        {
            return new[]
            {
                ".sln",
                ".csproj",
                ".cs",
                ".ps1",
                ".psm1"
            };
        }

        protected override bool GetIsVisible()
        {
            var filesToHandle = this.GetFilesToHandle();
            return filesToHandle.Any();
        }

        protected override Bitmap GetIconImage()
        {
            try
            {
                var ico = System.Drawing.Icon.ExtractAssociatedIcon(DevEnvPath);
                if (ico == null)
                    return null;

                return ico.ToBitmap();
            }
            catch
            {
                return null;
            }
        }
    }

    public class OpenVs2017MenuItem : Vs2017MenuItemBaseItem
    {
        public OpenVs2017MenuItem(SharpContextMenu menu)
            : base(menu)
        {

        }

        protected override void HandleClick(object sender, EventArgs e)
        {
            var filesToOpen = this.GetFilesToHandle();
            foreach (var file in filesToOpen)
            {
                var psInfo = new ProcessStartInfo()
                {
                    FileName = DevEnvPath,
                    Arguments = file.FullName
                };

                Process.Start(psInfo);
            }
        }

        protected override string GetMenuText()
        {
            return "Open in VS17";
        }
    }
}
