using PapeOverlay;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomOverlay
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyCustomApplicationContext());
        }

        public class MyCustomApplicationContext : ApplicationContext
        {
            private readonly NotifyIcon trayIcon;
            private OverlayApplication overlayApplication;

            public MyCustomApplicationContext()
            {
                // Initialize Tray Icon
                trayIcon = new NotifyIcon()
                {
                    Icon = Icon.ExtractAssociatedIcon("./icon.png"),
                    Visible = true
                };

                trayIcon.ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Start Overlay", Start),
                    new MenuItem("Quit", Close)
                });
            }

            private void Start(object sender, EventArgs e)
            {
                if (overlayApplication == null || !overlayApplication.IsStarted)
                {
                    overlayApplication = new CustomOverlay(System.AppDomain.CurrentDomain.BaseDirectory + "/overlay");
                    overlayApplication.OnEvent += OverlayApplication_OnEvent;
                    overlayApplication.OnError += OverlayApplication_OnError;
                    overlayApplication.InitializeOverlay();
                    overlayApplication.Start();
                }
            }

            private void OverlayApplication_OnError(OverlayApplication arg1, ApplicationError arg2)
            {
                trayIcon.ShowBalloonTip(2000, arg2.title, arg2.message, GetErrorToolTipIcon(arg2.severity));
                if(arg2.severity == ApplicationErrorSeverity.Fatal)
                {
                    Thread.Sleep(4000);
                    Close(null, null);
                }
            }

            private ToolTipIcon GetErrorToolTipIcon(ApplicationErrorSeverity severity)
            {
                if (severity == ApplicationErrorSeverity.Fatal)
                    return ToolTipIcon.Error;
                else
                    return ToolTipIcon.Warning;
            }

            private void OverlayApplication_OnEvent(OverlayApplication arg1, ApplicationEvent arg2)
            {
                trayIcon.ShowBalloonTip(2000, arg2.title, arg2.message, ToolTipIcon.Info);
            }

            private void Close(object sender, EventArgs e)
            {
                // Hide tray icon, otherwise it will remain shown until user mouses over it
                trayIcon.Visible = false;
                if (overlayApplication != null && overlayApplication.IsStarted)
                {
                    overlayApplication.OnEvent -= OverlayApplication_OnEvent;
                    overlayApplication.OnError -= OverlayApplication_OnError;
                    overlayApplication.Close();
                }
                Application.Exit();
            }
        }
    }
}
