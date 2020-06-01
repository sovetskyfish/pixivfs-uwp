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
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Security.Credentials;
using PixivCS;
using static PixivFSUWP.Data.OverAll;
using Windows.Data.Json;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private string username = null;
        private string password = null;
        private string refreshToken = null;
        private bool useToken = false;
        private bool directConnetion = false;
        private bool isCancelled = false;
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public LoginPage()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
            if (localSettings.Values["directConnection"] == null)
                localSettings.Values["directConnection"] = false;
            directConnetion = (bool)localSettings.Values["directConnection"];
            chkExperimental.IsChecked = directConnetion;
            var refreshTokenCredential = GetCredentialFromLocker(refreshTokenResource);
            var passwordCredential = GetCredentialFromLocker(passwordResource);
            if (passwordCredential != null)
            {
                passwordCredential.RetrievePassword();
                username = passwordCredential.UserName;
                password = passwordCredential.Password;
                if (refreshTokenCredential != null)
                {
                    refreshTokenCredential.RetrievePassword();
                    refreshToken = refreshTokenCredential.Password;
                    useToken = true;
                }
                else useToken = false;
                Login();
            }
        }

        private async void BtnReg_Click(object sender, RoutedEventArgs e) =>
            await Launcher.LaunchUriAsync(new
                Uri(@"https://accounts.pixiv.net/signup?lang=zh&source=pc&view_type=page&ref=wwwtop_accounts_index"));

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            username = txtUserName.Text;
            password = txtPassword.Password;
            if (username == "") txtUserName.Focus(FocusState.Programmatic);
            else if (password == "") txtPassword.Focus(FocusState.Programmatic);
            else
            {
                useToken = false;
                Login();
            }
        }

        private async void Login()
        {
            //启用实验性功能
            directConnetion = chkExperimental.IsChecked.Value;
            GlobalBaseAPI.ExperimentalConnection = directConnetion;
            //保存设置
            localSettings.Values["directConnection"] = directConnetion;
            stkTxts.Visibility = Visibility.Collapsed;
            stkBtns.Visibility = Visibility.Collapsed;
            btnTrouble.Visibility = Visibility.Collapsed;
            ringProgress.IsActive = true;
            grdLoading.Visibility = Visibility.Visible;
            bool success;
            PixivCS.Objects.AuthResult res = null;
            //异步执行登录
            try
            {
                if (useToken)
                    res = await GlobalBaseAPI.AuthAsync(refreshToken);
                else
                    res = await GlobalBaseAPI.AuthAsync(username, password);
                success = true;
            }
            catch
            {
                success = false;
                if (useToken)
                {
                    useToken = false;
                    try
                    {
                        res = await GlobalBaseAPI.AuthAsync(username, password);
                        success = true;
                    }
                    catch
                    {
                        success = false;
                    }
                }
            }
            if (success)
            {
                //登录成功
                //储存凭证
                var vault = new PasswordVault();
                try
                {
                    vault.Remove(GetCredentialFromLocker(passwordResource));
                    vault.Remove(GetCredentialFromLocker(refreshTokenResource));
                }
                catch { }
                finally
                {
                    vault.Add(new PasswordCredential(passwordResource, username, password));
                    vault.Add(new PasswordCredential(refreshTokenResource, username, Data.OverAll.GlobalBaseAPI.RefreshToken));
                }
                //保存当前的身份信息
                currentUser = Data.CurrentUser.FromObject(res.Response.User);
                //如果取消了登录，则避免跳转到主页面
                if (isCancelled) resetView();
                else Frame.Navigate(typeof(MainPage),null, App.FromRightTransitionInfo);
            }
            else btnTrouble.Visibility = Visibility.Visible;
        }

        private async void BtnTrouble_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new
                Uri(@"https://github.com/tobiichiamane/pixivfs-uwp/blob/master/TroubleShoot.md"));
            resetView();
        }

        private void resetView()
        {
            isCancelled = false;
            stkTxts.Visibility = Visibility.Visible;
            stkBtns.Visibility = Visibility.Visible;
            ringProgress.IsActive = false;
            grdLoading.Visibility = Visibility.Collapsed;
            txtUserName.Text = username;
            txtPassword.Password = password;
            txtPassword.Focus(FocusState.Programmatic);
            txtPassword.SelectAll();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new
                Uri(@"https://www.pixiv.net/member_illust.php?mode=medium&illust_id=31251762"));
        }

        private async void btnReport_Click(object sender, RoutedEventArgs e)
        {
            //阻止跳转到主页面
            isCancelled = true;
            //在新窗口中打开发送反馈的窗口
            await ShowNewWindow(typeof(ReportIssuePage), null);
        }
    }
}
