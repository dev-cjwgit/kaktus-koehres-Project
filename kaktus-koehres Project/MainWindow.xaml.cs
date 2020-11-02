using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using kaktus_koehres_Project.VIewModels;
using WinHttp;
using System.Text;
using Microsoft.VisualBasic;

namespace kaktus_koehres_Project
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private WinHttpRequest winhttp = new WinHttpRequest();
        private NotifyIcon notify;
        private WindowState PrevWindowState = WindowState.Normal;
        public MainWindow()
        {
            InitializeComponent();
            CactusListBox.ItemsSource = ListBoxViewViewModel.GetInstance();

            

            //CactusListBox.Items.Refresh();
        }

        #region Windows Form Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var anim = new DoubleAnimation(0, 1, (Duration)TimeSpan.FromSeconds(1));
            this.BeginAnimation(UIElement.OpacityProperty, anim);

            winhttp.Open("GET", "https://www.kaktus-koehres.de/shop/Cactus-seeds---1.html");
            winhttp.SetRequestHeader("accept-language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
            winhttp.SetRequestHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            winhttp.Send();
            winhttp.WaitForResponse();

            string result = Strings.Split(Encoding.Default.GetString(winhttp.ResponseBody), "<u>Cactus seeds</u>")[1];
            string[] list = Strings.Split(result, "</a></div><div class=");
            for(int i = 0; i < list.Length - 1; i++) { 

                string cacuts_name = Strings.Split(Strings.Split(list[i], "</a></div><div class=\"")[0], "\">")[3];
                ListBoxViewViewModel.GetInstance().Add(cacuts_name);
            }
            CactusListBox.Items.Refresh();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.WindowState = PrevWindowState;
                var anim = new DoubleAnimation(0, 1, (Duration)TimeSpan.FromSeconds(0.3));
                anim.Completed += (s, _) =>
                {
                    notify.Dispose();

                };
                this.BeginAnimation(UIElement.OpacityProperty, anim);
            }
            else
            {
                Opacity = 1.0;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        #endregion

        #region Windows TitleBar Button Events

        private void ToggleButton1_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = (bool)ToggleButton1.IsChecked;
        }

        private void WindowsTitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void WindowsMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            PrevWindowState = WindowState;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.3));
            anim.Completed += (s, _) =>
            {
                this.WindowState = WindowState.Minimized;
                TrayInit();

            };

            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void WindowsMaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState temp = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
            DoubleAnimation anim1 = new DoubleAnimation(1, 0, (Duration)TimeSpan.FromSeconds(0.33));
            DoubleAnimation anim2 = new DoubleAnimation(0, 1, (Duration)TimeSpan.FromSeconds(0.33));

            anim1.Completed += (s, _) =>
            {
                this.WindowState = temp;
                this.BeginAnimation(OpacityProperty, anim2);
            };

            this.BeginAnimation(OpacityProperty, anim1);
        }

        private void WindowsCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Tray Mode

        private void TrayInit()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem item1 = new MenuItem();
            menu.MenuItems.Add(item1);

            item1.Index = 0;
            item1.Text = "Close App";
            item1.Click += (s, _) =>
            {
                notify.Dispose();
                Environment.Exit(0);
            };

            notify = new NotifyIcon();
            notify.DoubleClick += new EventHandler(Window_Activated);
            notify.Icon = Properties.Resources.ProgramMainIcon;
            notify.Visible = true;
            notify.ContextMenu = menu;
        }








        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
