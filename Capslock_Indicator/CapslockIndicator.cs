using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Capslock_Indicator
{
    class CapslockIndicator
    {
        NotifyIcon notifyIcon;
        ContextMenu menu;
        Hooker hooker;
        Icon icoOn, icoOff;

        //Initialize
        public CapslockIndicator()
        {
            notifyIcon = new NotifyIcon() {
                Text = "Capslock Indicator",
            };

            icoOn = new Icon(SystemIcons.Exclamation, 32, 32);
            icoOff = new Icon(SystemIcons.Shield, 32, 32);

            menu = new ContextMenu();
            menu.MenuItems.Add("김주진(http://www.ciiwolstudio.com)", mnu_homepage);
            menu.MenuItems.Add("Exit", mnu_Exit);

            notifyIcon.ContextMenu = menu;

        }

        //Run
        public void run()
        {
            notifyIcon.Visible = true;
            changeIcon();
            hooker = new Hooker(this);
            Application.Run();
        }

        //open homepage
        private void mnu_homepage(object sender, EventArgs e)
        {
           Process.Start("http://www.ciiwolstudio.com");
        }

        //quit application
        private void mnu_Exit(object sender, EventArgs e)
        {
            notifyIcon.Dispose();
            hooker.Dispose();
            Application.Exit();
        }

        //chagne icon
        public void changeIcon()
        {
            notifyIcon.Icon = Console.CapsLock?icoOn:icoOff;
        }

    }
}
