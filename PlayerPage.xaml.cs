using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace News
{
    public partial class PlayerPage : PhoneApplicationPage
    {
        public PlayerPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            player.Source = new Uri((Application.Current as App).SmoothStreamURL, UriKind.Absolute);
            base.OnNavigatedTo(e);
        }

        //protected override void OnNavigatedFrom(NavigationEventArgs e)
        //{

        //    player.Stop();

        //    base.OnNavigatedFrom(e);
        //}
    }
}