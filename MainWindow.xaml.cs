using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Threading;
using System.Globalization;
//using System.Diagnostics;

namespace NMS_PauseBeGone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static System.Timers.Timer aTimer;
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public MainWindow()
        {
            InitializeComponent();
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;

            chkbox_active.Content = lang.Resource1.toolActivate;
            window_main.Title = lang.Resource1.toolTitle;

            aTimer = new System.Timers.Timer(500);
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(dt_Tick);
            aTimer.Enabled = true;
        }

        void dt_Tick(object sender, EventArgs e)
        {
            IntPtr hWnd = FindWindow(null, "No Man's Sky");
            //Trace.WriteLine(hWnd);
            Dispatcher.Invoke(() =>
            {
                if (hWnd != IntPtr.Zero)
                { //找到窗口
                    label_gameStatus.Content = lang.Resource1.gameStatus + lang.Resource1.gameRunning;
                    label_gameStatus.Foreground = new SolidColorBrush(Colors.Green);
                    if ((bool)chkbox_active.IsChecked)
                    {
                        PostMessage(hWnd, 0x006, new IntPtr(1), IntPtr.Zero);
                    }
                }
                else
                { //未找到窗口
                    label_gameStatus.Content = lang.Resource1.gameStatus + lang.Resource1.gameNotFound;
                    label_gameStatus.Foreground = new SolidColorBrush(Colors.Maroon);

                };
            });
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
