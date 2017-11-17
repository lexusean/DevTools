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
    public abstract class PowerShellMenuItemBase : MenuBaseItem
    {
        protected static string PowerShellExePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"System32\WindowsPowerShell\v1.0\powershell.exe");
        protected static string PowerShellIseExePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"System32\WindowsPowerShell\v1.0\powershell_ise.exe");

        protected PowerShellMenuItemBase(SharpContextMenu menu)
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

        protected override Bitmap GetIconImage()
        {
            return null;
        }
    }

    public class PowerShellMenuSeparator : MenuBaseSeparator
    {
        public PowerShellMenuSeparator(SharpContextMenu menu)
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

        protected override bool GetIsVisible()
        {
            return this.GetFilesToHandle().Any() | this.GetDirectoriesToHandle().Any();
        }
    }

    public class RunInPowerShellMenuItem : PowerShellMenuItemBase
    {
        public RunInPowerShellMenuItem(SharpContextMenu menu)
            : base(menu)
        {

        }

        protected override void HandleClick(object sender, EventArgs e)
        {
            var filesToOpen = this.GetFilesToHandle();
            foreach (var file in filesToOpen)
            {
                var args = new string[]
                {
                    "-ExecutionPolicy",
                    "bypass",
                    "-NoExit",
                    "-File",
                    file.FullName
                };

                var psInfo = new ProcessStartInfo()
                {
                    FileName = PowerShellExePath,
                    Arguments = string.Join(" ", args),
                    Verb = "runas"
                };

                Process.Start(psInfo);
            }
        }

        protected override string GetMenuText()
        {
            return "Run in PSH Admin";
        }

        protected override bool GetIsVisible()
        {
            return this.GetFilesToHandle().Any();
        }
    }

    public class OpenPowerShellMenuItem : PowerShellMenuItemBase
    {
        public OpenPowerShellMenuItem(SharpContextMenu menu)
            : base(menu)
        {

        }

        protected override void HandleClick(object sender, EventArgs e)
        {
            var dirsToOpen = this.GetDirectoriesToHandle();
            foreach (var d in dirsToOpen)
            {
                var args = new string[]
                {
                    "-ExecutionPolicy",
                    "bypass",
                    "-NoExit",
                    "-Command",
                    $"Set-Location {d.FullName}"
                };

                var psInfo = new ProcessStartInfo()
                {
                    FileName = PowerShellExePath,
                    Arguments = string.Join(" ", args),
                    Verb = "runas"
                };

                Process.Start(psInfo);
            }
        }

        protected override string GetMenuText()
        {
            return "Open in PSH Admin";
        }

        protected override bool GetIsVisible()
        {
            return this.GetDirectoriesToHandle().Any();
        }
    }

    public class RunInPowerShellIseMenuItem : PowerShellMenuItemBase
    {
        public RunInPowerShellIseMenuItem(SharpContextMenu menu)
            : base(menu)
        {

        }

        protected override void HandleClick(object sender, EventArgs e)
        {
            var filesToOpen = this.GetFilesToHandle();
            foreach (var file in filesToOpen)
            {
                var args = new string[]
                {
                    file.FullName
                };

                var psInfo = new ProcessStartInfo()
                {
                    FileName = PowerShellIseExePath,
                    Arguments = string.Join(" ", args),
                    Verb = "runas"
                };

                Process.Start(psInfo);
            }
        }

        protected override string GetMenuText()
        {
            return "Edit in PSH_ISE";
        }

        protected override bool GetIsVisible()
        {
            return this.GetFilesToHandle().Any();
        }
    }
}
