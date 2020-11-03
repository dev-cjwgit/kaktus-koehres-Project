using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using kaktus_koehres_Project.VIewModels;
using WinHttp;
using System.Text;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Windows.Threading;
using System.Security.Policy;
using System.Collections.Generic;

namespace kaktus_koehres_Project
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private dynamic cactus_info;
        private string cookie_value = "";
        private BackgroundWorker thread = new BackgroundWorker();
        private WinHttpRequest winhttp = new WinHttpRequest();
        private NotifyIcon notify;
        private WindowState PrevWindowState = WindowState.Normal;
        public MainWindow()
        {
            InitializeComponent();
            init();
            thread.RunWorkerAsync();
        }

        private void InitBinding()
        {
            CactusListBox.ItemsSource = CactusViewModel.GetInstance();
            DetailListBox.ItemsSource = DetailListBoxViewModel.GetInstance();
            PageLabel.DataContext = PageLabelViewModel.GetInstacne();
            MaxPageLabel.DataContext = PageLabelViewModel.GetInstacne();

            priceText1.DataContext = PriceViewModel.GetInstance();
            priceText2.DataContext = PriceViewModel.GetInstance();
            priceText3.DataContext = PriceViewModel.GetInstance();
            priceText4.DataContext = PriceViewModel.GetInstance();
        }

        private void init()
        {
            InitBinding();
            thread.DoWork += (s, _) =>
            {
                winhttp.Open("GET", "https://www.kaktus-koehres.de/shop/Cactus-seeds---1.html", true);
                winhttp.SetRequestHeader("accept-language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
                winhttp.SetRequestHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                winhttp.Send();
                winhttp.WaitForResponse();
                cookie_value = Strings.Split(winhttp.GetResponseHeader("set-cookie"), "; path=/")[0];
                string result = Strings.Split(Encoding.Default.GetString(winhttp.ResponseBody), "<u>Cactus seeds</u>")[1];
                string[] list = Strings.Split(result, "</a></div><div class=");

                for (int i = 0; i < list.Length - 1; i++)
                {
                    string cactus_name = Strings.Split(list[i], "\">")[3];
                    string page_code = Strings.Split(Strings.Split(list[i], ".html?")[0], "---")[1];
                    CactusModel.GetInstance().Add(new CactusForm() { Cactus_Name = cactus_name, Page_Code = page_code });
                }

                CactusViewModel.SetSource(CactusModel.GetDataSource());


                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    CactusListBox.Items.Refresh();
                }));
            };

        }

        #region Windows Form Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var anim = new DoubleAnimation(0, 1, (Duration)TimeSpan.FromSeconds(1));
            this.BeginAnimation(UIElement.OpacityProperty, anim);
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

        private int GetCactusDetailView(string url)
        {
            try
            {
                DetailModel.GetInstance().Clear();
                winhttp.Open("GET", url, false);
                //winhttp.SetRequestHeader("accept-language", "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");
                //winhttp.SetRequestHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                //winhttp.SetRequestHeader("referer", url);
                winhttp.Send();
                winhttp.WaitForResponse();
                string result = Strings.Split(Encoding.Default.GetString(winhttp.ResponseBody), "(of in total <b>")[1];
                int list_count = Convert.ToInt32(Strings.Split(result, "</b>")[0]);
                int page_count = (list_count - 1) / 10 + 1;


                string[] list = Strings.Split(result, "#993333;\">");
                for (int i = 1; i < list.Length; i++)
                {
                    string[] price = Strings.Split(list[i], "<tr><td> ");
                    List<double> temp = new List<double>();
                    for(int j = 1; j < price.Length; j++)
                    {
                        temp.Add(Convert.ToDouble(Strings.Split(price[j]," EUR")[0].ToString().Replace(',','.')));
                    }
                    DetailModel.GetInstance().Add(new DetailForm() { Cactus_Name = Strings.Split(list[i], " </strong><em>")[0], Price=temp });
                }

                DetailListBoxViewModel.SetSource(DetailModel.GetDataSource());
                DetailListBox.Items.Refresh();
                return page_count;
            }
            catch
            {
                GetCactusDetailView(url);
                return 0;
            }
        }

        private void CactusListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dynamic meta_data = sender as dynamic;
            cactus_info = CactusModel.GetInstance()[meta_data.SelectedIndex];
            string url = "https://www.kaktus-koehres.de/shop/Cactus-seeds/" + cactus_info.Cactus_Name + "---" + cactus_info.Page_Code + "-1.html?" + cookie_value;
            
            PageLabelViewModel.GetInstacne().MaxPage = GetCactusDetailView(url);
            PageLabelViewModel.GetInstacne().Page = 1;

        }

        private void PagePrevButton_Click(object sender, RoutedEventArgs e)
        {
            int temp = PageLabelViewModel.GetInstacne().Page;
            PageLabelViewModel.GetInstacne().Page--;
            if (cactus_info != null && PageLabelViewModel.GetInstacne().Page != temp)
            {
                string url = "https://www.kaktus-koehres.de/shop/Cactus-seeds/" + cactus_info.Cactus_Name + "---" + cactus_info.Page_Code + "-" + PageLabelViewModel.GetInstacne().Page + ".html?" + cookie_value;
                GetCactusDetailView(url);
            }
        }

        private void PageNextButton_Click(object sender, RoutedEventArgs e)
        {
            int temp = PageLabelViewModel.GetInstacne().Page;
            PageLabelViewModel.GetInstacne().Page++;
            if (cactus_info != null && PageLabelViewModel.GetInstacne().Page != temp)
            {
                string url = "https://www.kaktus-koehres.de/shop/Cactus-seeds/" + cactus_info.Cactus_Name + "---" + cactus_info.Page_Code + "-" + PageLabelViewModel.GetInstacne().Page + ".html?" + cookie_value;
                GetCactusDetailView(url);
            }
        }

        private void DetailListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WinHttpRequest http = new WinHttpRequest();
            /*
             * https://www.google.com/search?q=LOBIVIA+acchaensis&hl=ko&tbm=isch
             * 
             * 
             */
            dynamic meta_data = sender as dynamic;
            var temp = DetailModel.GetInstance()[meta_data.SelectedIndex];
            double[] d = new double[] { 0, 0, 0, 0 };
            int idx = 0;
            foreach(var item in temp.Price)
            {
                d[idx] = item;
                idx++;
            }
            PriceViewModel.GetInstance().Price = d;
            http.Open("GET", "https://www.google.com/search?q=" + temp.Cactus_Name + "&hl=ko&tbm=isch");
            http.Send();
            http.WaitForResponse();

            System.Windows.Clipboard.Clear();
            System.Windows.Clipboard.SetText(Encoding.Default.GetString(winhttp.ResponseBody));
        }

    }
}
