using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Threading;

namespace News
{
    public partial class Splashscreen : PhoneApplicationPage
    {
        public Splashscreen()
        {
            InitializeComponent();            

            callSplashscreen();
        }


        async void callSplashscreen()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)); 
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Check if ExtendedSplashscreen.xaml is on the backstack  and remove 
            if (NavigationService.BackStack.Count() == 1)
            {
                NavigationService.RemoveBackEntry();
            }

        }

    }
}