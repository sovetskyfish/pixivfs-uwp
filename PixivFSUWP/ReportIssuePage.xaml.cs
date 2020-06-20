using Microsoft.Toolkit.Uwp.Helpers;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static PixivFSUWP.Data.OverAll;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ReportIssuePage : Page
    {
        public ReportIssuePage()
        {
            this.InitializeComponent();

            txtDetails.Text += "General:\n";
            txtDetails.Text += string.Format("OS version: build {0}\n", SystemInformation.OperatingSystemVersion.Build);
            txtDetails.Text += string.Format("App version: {0}.{1}.{2} {3}\n",
                Package.Current.Id.Version.Major,
                Package.Current.Id.Version.Minor,
                Package.Current.Id.Version.Build,
                Package.Current.Id.Architecture);
            txtDetails.Text += string.Format("Package ID: {0}\n\n", Package.Current.Id.Name);
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["isCrashed"] != null &&
                (bool)localSettings.Values["isCrashed"] == true &&
                localSettings.Values["exception"] != null)
            {
                localSettings.Values.Remove("isCrashed");
                var lastExeption = (string)localSettings.Values["exception"];
                txtDetails.Text += "Exception:\n";
                txtDetails.Text += lastExeption;
            }
        }

        private async void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(txtDetails.Text);
            Clipboard.SetContent(dataPackage);
            MessageDialog dialog = new MessageDialog(GetResourceString("CopiedInfoPlain"), GetResourceString("CopiedPlain"));
            await dialog.ShowAsync();
        }

        private async void BtnGitHub_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new
                Uri(@"https://github.com/tobiichiamane/pixivfs-uwp/issues/new/choose"));
        }

        private async void BtnEmail_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new
                Uri(@"mailto:tobiichiamane@outlook.jp"));
        }
    }
}
