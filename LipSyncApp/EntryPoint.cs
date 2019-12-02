using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptPortal.Vegas;
using System.Drawing;

/* Basic flow of Vegas extension */
namespace LipSyncApp
{
    public class LipSyncModule : ICustomCommandModule
    {
        public Vegas myVegas = null;
        internal LipSyncUserControl myControl = null;


        public void InitializeModule(Vegas vegas)
        {
            myVegas = vegas;
        }

        CustomCommand myCommand = new CustomCommand(CommandCategory.Tools, "MyCommand");

        public ICollection GetCustomCommands()
        {
            myCommand.DisplayName = "LipSyncApp";
            myCommand.Invoked += this.HandleInvoked;
            myCommand.MenuPopup += this.HandleMenuPopup;
            

            return new CustomCommand[] { myCommand };
        }

        void HandleInvoked(Object sender, EventArgs args)
        {
            if (!myVegas.ActivateDockView("LipSyncApp"))
            {
                myControl = new LipSyncUserControl(myVegas);
                myControl.Dock = System.Windows.Forms.DockStyle.Fill;

                DockableControl dockView = new DockableControl("LipSyncApp");
                dockView.AutoLoadCommand = myCommand;
                dockView.PersistDockWindowState = true;
                dockView.DefaultFloatingSize = new Size(540, 480);
                dockView.DefaultDockWindowStyle = DockWindowStyle.Docked;
                dockView.Dock = System.Windows.Forms.DockStyle.Fill;
                dockView.Controls.Add(myControl);
                myVegas.LoadDockView(dockView);
            }
        }

        void HandleMenuPopup(Object sender, EventArgs args)
        {
            myCommand.Checked = myVegas.FindDockView("LipSyncApp");
        }

    }
}
