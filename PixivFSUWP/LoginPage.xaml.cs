using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PixivFS;
using PixivFSCS;
using System.Threading.Tasks;
using System.Diagnostics;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
        }

        private async void BtnReg_Click(object sender, RoutedEventArgs e) =>
            await Launcher.LaunchUriAsync(new
                Uri(@"https://accounts.pixiv.net/signup?lang=zh&source=pc&view_type=page&ref=wwwtop_accounts_index"));

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUserName.Text;
            var password = txtPassword.Password;
            if (username == "") txtUserName.Focus(FocusState.Programmatic);
            else if (password == "") txtPassword.Focus(FocusState.Programmatic);
            else
            {
                //异步执行登录
                var logintask = Task.Run(() =>
                 {
                     try
                     {
                         Data.OverAll.GlobalBaseAPI.csfriendly_login(username, password);
                         return true;
                     }
                     catch
                     {
                         return false;
                     }
                 });
                stkTxts.Visibility = Visibility.Collapsed;
                stkBtns.Visibility = Visibility.Collapsed;
                btnTrouble.Visibility = Visibility.Collapsed;
                ringProgress.IsActive = true;
                grdLoading.Visibility = Visibility.Visible;
                var loginres = await logintask;
                if (loginres)
                {
                    //登陆成功
                }
                else btnTrouble.Visibility = Visibility.Visible;
            }
        }

        private async void BtnTrouble_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new
                Uri(@""));
            stkTxts.Visibility = Visibility.Visible;
            stkBtns.Visibility = Visibility.Visible;
            ringProgress.IsActive = false;
            grdLoading.Visibility = Visibility.Collapsed;
            txtPassword.Focus(FocusState.Programmatic);
            txtPassword.SelectAll();
        }
    }
}
