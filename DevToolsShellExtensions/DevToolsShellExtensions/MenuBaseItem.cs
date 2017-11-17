using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using SharpShell.SharpContextMenu;

namespace DevToolsShellExtensions
{
    public abstract class MenuBaseItem : ToolStripMenuItem
    {
        protected SharpContextMenu Menu { get; }

        protected MenuBaseItem(SharpContextMenu menu)
        {
            this.Menu = menu;

            this.Text = this.GetMenuText();
            this.Image = this.GetIconImage();
            this.Visible = this.GetIsVisible();
            this.Available = this.Visible;
            this.Click += this.HandleClick;
        }

        protected abstract void HandleClick(object sender, EventArgs e);
        protected abstract string GetMenuText();
        protected abstract bool GetIsVisible();
        protected abstract Bitmap GetIconImage();

        protected virtual IEnumerable<string> GetFileTypesHandled()
        {
            return Enumerable.Empty<string>();
        }

        protected virtual IEnumerable<FileInfo> GetFilesToHandle()
        {
            var extTypes = this.GetFileTypesHandled();

            if (this.Menu == null)
                return Enumerable.Empty<FileInfo>();

            return this.Menu.SelectedItemPaths
                .Where(IsFile)
                .Select(t => new FileInfo(t))
                .Where(t => extTypes.Contains(t.Extension));
        }

        protected virtual IEnumerable<DirectoryInfo> GetDirectoriesToHandle()
        {
            if (this.Menu == null)
                return Enumerable.Empty<DirectoryInfo>();

            return this.Menu.SelectedItemPaths
                .Where(IsDirectory)
                .Select(t => new DirectoryInfo(t));
        }

        protected static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        protected static bool IsFile(string path)
        {
            return File.Exists(path);
        }

        protected static bool IsFileOrDirectory(string path)
        {
            return IsFile(path) | IsDirectory(path);
        }

        protected static FileInfo DownloadFile(string url, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                using (var client = new WebClient())
                {
                    client.DownloadFile(url, filePath);
                }

                return new FileInfo(filePath);
            }
            catch
            {
                return null;
            }
        }
    }

    public abstract class MenuBaseSeparator : ToolStripSeparator
    {
        protected SharpContextMenu Menu { get; }

        protected MenuBaseSeparator(SharpContextMenu menu)
        {
            this.Menu = menu;

            this.Visible = this.GetIsVisible();
        }

        protected abstract bool GetIsVisible();

        protected virtual IEnumerable<string> GetFileTypesHandled()
        {
            return Enumerable.Empty<string>();
        }

        protected virtual IEnumerable<FileInfo> GetFilesToHandle()
        {
            var extTypes = this.GetFileTypesHandled();

            if (this.Menu == null)
                return Enumerable.Empty<FileInfo>();

            return this.Menu.SelectedItemPaths
                .Where(IsFile)
                .Select(t => new FileInfo(t))
                .Where(t => extTypes.Contains(t.Extension));
        }

        protected virtual IEnumerable<DirectoryInfo> GetDirectoriesToHandle()
        {
            if (this.Menu == null)
                return Enumerable.Empty<DirectoryInfo>();

            return this.Menu.SelectedItemPaths
                .Where(IsDirectory)
                .Select(t => new DirectoryInfo(t));
        }

        protected static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        protected static bool IsFile(string path)
        {
            return File.Exists(path);
        }

        protected static bool IsFileOrDirectory(string path)
        {
            return IsFile(path) | IsDirectory(path);
        }
    }
}
