
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
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
            AddShortcutToDesktop();
            Process.Start( this.Context.Parameters["targetDir"] + "VC_redist.x64.exe");
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
            RemoveShortcut();
            Process.Start(this.Context.Parameters["targetDir"] + "VC_redist.x64.exe");
        }

        //private

        public void RemoveShortcut()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string shortcutFullPath = desktopPath + "\\SmurfWalletOW.url";
            if (File.Exists(shortcutFullPath))
            {
                File.Delete(shortcutFullPath);
            }
        }
        
        private void AddShortcutToDesktop()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var targetPath = this.Context.Parameters["targetDir"];
            string app = targetPath + "SmurfWalletOW.exe";
            string icon = targetPath + "Icons\\wallet.ico";
            app = app.Replace("/", "\\");
            icon = icon.Replace("/", "\\");
            using (StreamWriter writer = new StreamWriter(desktopPath + "\\SmurfWalletOW.url"))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + app);
                writer.WriteLine("IconIndex=0");
                writer.WriteLine("IconFile=" + icon);
                writer.Flush();
            }
        }       
    }
}
