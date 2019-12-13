using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace PostInstallationConfiguration
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
        }
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            //change link
            AddShortcutToDesktop("Smurf Wallet OW");
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            //delete shortcut
            //RemoveShortcut();
        }

        //private

        public void RemoveShortcut()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string shortcutFullPath = desktopPath + "/" + "SmurfWalletOW.lnk";
            if (File.Exists(shortcutFullPath))
            {
                File.Delete(shortcutFullPath);
            }
        }


        private void AddShortcutToDesktop(string linkName)
        {
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".lnk"))
            {
                string app = this.Context.Parameters["targetDir"] + "\\SmurfWalletOW.exe";
                string icon = this.Context.Parameters["targetDir"] + "\\Icons\\wallet.ico";
                writer.WriteLine("URL=file:///" + app);
                writer.WriteLine("IconIndex=0");
                icon = icon.Replace('\\', '/');
                writer.WriteLine("IconFile=" + icon);
                writer.Flush();
            }
        }       
    }
}
