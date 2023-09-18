using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media; 
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
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

        private Dictionary<string, string> lang_map = new Dictionary<string, string>();


        public MainWindow()
        {
            InitializeComponent();
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;

            //启动时钟
            aTimer = new System.Timers.Timer(500);
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(dt_Tick);
            aTimer.Enabled = true;

            //初始化语言
            string language = CultureInfo.CurrentCulture.Name;
            if (language.StartsWith("zh"))
            {
                lang_map["chkbox_active"] = "使工具生效";
                lang_map["window_main"] = "别tm再暂停了！";
                lang_map["label_gameStatus"] = "游戏状态：";
                lang_map["gameStatus_on"] = "运行中";
                lang_map["gameStatus_off"] = "未找到";
            } 
            else
            {
                lang_map["chkbox_active"] = "Tool Activated";
                lang_map["window_main"] = "PAUSE BE GONE";
                lang_map["label_gameStatus"] = "Game Status: ";
                lang_map["gameStatus_on"] = "Running";
                lang_map["gameStatus_off"] = "Not Found";
            }
            chkbox_active.Content = lang_map["chkbox_active"];
            window_main.Title = lang_map["window_main"];

        }

        void dt_Tick(object sender, EventArgs e)
        {
            IntPtr hWnd = FindWindow(null, "No Man's Sky");
            //Trace.WriteLine(hWnd);
            Dispatcher.Invoke(() =>
            {
                if (hWnd != IntPtr.Zero)
                { //找到窗口
                    label_gameStatus.Content = lang_map["label_gameStatus"] + lang_map["gameStatus_on"];
                    label_gameStatus.Foreground = new SolidColorBrush(Colors.Green);
                    if ((bool)chkbox_active.IsChecked)
                    {
                        PostMessage(hWnd, 0x006, new IntPtr(1), IntPtr.Zero);
                    }
                }
                else
                { //未找到窗口
                    label_gameStatus.Content = lang_map["label_gameStatus"] + lang_map["gameStatus_off"];
                    label_gameStatus.Foreground = new SolidColorBrush(Colors.Maroon);
                };
            });
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
