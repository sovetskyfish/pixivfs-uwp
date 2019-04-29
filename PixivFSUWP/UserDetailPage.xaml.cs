using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PixivFSCS;
using PixivFS;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserDetailPage : Page
    {
        int userid = 0;
        Data.UserDetail detail;
        public UserDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((Frame.Parent as Grid)?.Parent as MainPage)?.SelectNavPlaceholder("用户");
            userid = (int)e.Parameter;
            base.OnNavigatedTo(e);
            _ = loadContents();
        }

        async Task loadContents()
        {
            var res = await Task.Run(
                    () =>
                    new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                    .csfriendly_user_detail(userid.ToString())
            );
            detail = Data.UserDetail.FromJsomValue(res);
            string _getText(string input) => (input == "") ? "保密" : input;
            txtUsername.Text = detail.Name;
            txtAccount.Text = "@" + detail.Account;
            txtWebPage.Text = (detail.WebPage == "") ? "无/保密" : detail.WebPage;
            if (detail.Gender == "") txtGender.Text = "保密";
            else txtGender.Text = (detail.Gender == "male") ? "男" : "女";
            txtBirthday.Text = _getText(detail.BirthDay);
            txtRegion.Text = _getText(detail.Region);
            txtJob.Text = _getText(detail.Job);
            string _getHW(string input) => (input == "") ? "未知" : input;
            txtPC.Text = _getHW(detail.PC);
            txtMonitor.Text = _getHW(detail.Monitor);
            txtTool.Text = _getHW(detail.Tool);
            txtScanner.Text = _getHW(detail.Scanner);
            txtTablet.Text = _getHW(detail.Tablet);
            txtMouse.Text = _getHW(detail.Mouse);
            txtPrinter.Text = _getHW(detail.Printer);
            txtDesktop.Text = _getHW(detail.Desktop);
            txtMusic.Text = _getHW(detail.Music);
            txtDesk.Text = _getHW(detail.Desk);
            txtChair.Text = _getHW(detail.Chair);
            imgAvatar.ImageSource = await Data.OverAll.LoadImageAsync(detail.AvatarUrl);
        }
    }
}
