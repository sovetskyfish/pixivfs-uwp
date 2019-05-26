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

        public LoginPage()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
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
            stkTxts.Visibility = Visibility.Collapsed;
            stkBtns.Visibility = Visibility.Collapsed;
            btnTrouble.Visibility = Visibility.Collapsed;
            ringProgress.IsActive = true;
            grdLoading.Visibility = Visibility.Visible;
            bool success;
            JsonObject res = null;
            //异步执行登录
            try
            {
                if (useToken)
                    res = await GlobalBaseAPI.Auth(refreshToken);
                else
                    res = await GlobalBaseAPI.Auth(username, password);
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
                        res = await GlobalBaseAPI.Auth(username, password);
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
                currentUser = Data.CurrentUser.FromJsonValue(res["response"].GetObject()["user"].GetObject());
                Frame.Navigate(typeof(MainPage));
            }
            else btnTrouble.Visibility = Visibility.Visible;
        }

        private async void BtnTrouble_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new
                Uri(@"https://github.com/tobiichiamane/pixivfs-uwp/blob/master/TroubleShoot.md"));
            stkTxts.Visibility = Visibility.Visible;
            stkBtns.Visibility = Visibility.Visible;
            ringProgress.IsActive = false;
            grdLoading.Visibility = Visibility.Collapsed;
            txtPassword.Focus(FocusState.Programmatic);
            txtPassword.SelectAll();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new
                Uri(@"https://www.pixiv.net/member_illust.php?mode=medium&illust_id=31251762"));
        }
    }
}
