﻿#pragma checksum "C:\Users\Kong_mono\Documents\Visual Studio 2012\Projects\News\News\DaraNewsDetailPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "04C0910428752EE5423A12C02CA03112"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace News {
    
    
    public partial class DaraNewsDetailPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Pivot Title;
        
        internal Microsoft.Phone.Controls.PivotItem PivotItem;
        
        internal System.Windows.Controls.Image imageDetail;
        
        internal System.Windows.Controls.TextBlock textDetail;
        
        internal System.Windows.Controls.Grid LoadingPanel;
        
        internal System.Windows.Controls.TextBlock loadingLabel;
        
        internal System.Windows.Controls.ProgressBar loadingProgressBar;
        
        internal Microsoft.Phone.Shell.ApplicationBar AppBar;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/News;component/DaraNewsDetailPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Title = ((Microsoft.Phone.Controls.Pivot)(this.FindName("Title")));
            this.PivotItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("PivotItem")));
            this.imageDetail = ((System.Windows.Controls.Image)(this.FindName("imageDetail")));
            this.textDetail = ((System.Windows.Controls.TextBlock)(this.FindName("textDetail")));
            this.LoadingPanel = ((System.Windows.Controls.Grid)(this.FindName("LoadingPanel")));
            this.loadingLabel = ((System.Windows.Controls.TextBlock)(this.FindName("loadingLabel")));
            this.loadingProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("loadingProgressBar")));
            this.AppBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("AppBar")));
        }
    }
}

