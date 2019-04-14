using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string[] testuris = { "https://i.pximg.net/c/540x540_70/img-master/img/2019/03/19/00/00/07/73755434_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2019/02/07/00/00/05/73042381_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2019/01/03/00/00/01/72461417_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/12/11/00/00/07/72059628_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/10/28/00/00/15/71377287_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/09/12/00/00/05/70653098_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/09/11/00/00/09/70639418_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/09/10/00/00/04/70624207_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/08/02/00/00/06/69972773_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/07/26/00/00/08/69859628_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/07/04/00/00/05/69526240_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/07/03/00/00/08/69512461_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/06/11/00/00/01/69174779_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/05/12/00/00/16/68698297_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/05/11/00/00/07/68684734_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/05/10/00/00/11/68670770_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/05/09/00/00/07/68656810_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/05/08/00/00/10/68641893_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/04/09/00/00/25/68145953_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/03/07/00/00/13/67609464_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/02/10/00/20/51/67190100_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2018/01/08/00/45/23/66698609_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2017/12/15/00/03/19/66292640_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2017/12/08/00/22/01/66201209_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2017/11/17/00/43/47/65922934_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2017/11/09/00/31/49/65809956_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2017/11/05/00/25/26/65752703_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2017/11/02/00/32/03/65707174_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2017/10/27/00/15/47/65608790_p0_master1200.jpg", "https://i.pximg.net/c/540x540_70/img-master/img/2017/10/24/00/10/08/65569960_p0_master1200.jpg" };
        ObservableCollection<ViewModels.WaterfallItemViewModel> testItems = new ObservableCollection<ViewModels.WaterfallItemViewModel>();
        public MainPage()
        {
            this.InitializeComponent();
            WaterfallListView.ItemsSource = testItems;
            LoadImages();
        }

        public async void LoadImages()
        {
            foreach(var uri in testuris)
            {
                var item = ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { Title = "test", ImageUri = uri });
                await item.LoadImageAsync();
                testItems.Add(item);
            }
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {

        }
    }
}
