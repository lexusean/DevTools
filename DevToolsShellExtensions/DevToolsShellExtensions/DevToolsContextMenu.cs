using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace DevToolsShellExtensions
{
    public class DevToolsContextMenuStrip : ContextMenuStrip
    {
        public static DevToolsContextMenuStrip GetInstance()
        {
            return new DevToolsContextMenuStrip();
        }

        private DevToolsContextMenuStrip()
        {
            this.BackColor = Color.LawnGreen;
            this.Text = "DevTools";
        }
    }

    public class DevToolsMenuItemFactory
    {
        public static IEnumerable<ToolStripItem> GetRegisteredMenuItems(SharpContextMenu menu)
        {
            if(menu == null)
                yield break;

            yield return new OpenVs2017MenuItem(menu);
            yield return new PowerShellMenuSeparator(menu);
            yield return new RunInPowerShellMenuItem(menu);
            yield return new RunInPowerShellIseMenuItem(menu);
            yield return new OpenPowerShellMenuItem(menu);
            yield return new CakeMenuSeparator(menu);
            yield return new AddCakeScriptsShellMenuItem(menu);
        }
    }

    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    public class DevToolsContextMenu : SharpContextMenu
    {
        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = DevToolsContextMenuStrip.GetInstance();
            var menuItems = DevToolsMenuItemFactory.GetRegisteredMenuItems(this);

            foreach (var mItem in menuItems)
            {
                if(mItem.Visible)
                    menu.Items.Add(mItem);
            }

            return menu;
        }
    }
}
