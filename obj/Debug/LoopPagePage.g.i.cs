﻿#pragma checksum "C:\Users\Kong_mono\Documents\Visual Studio 2012\Projects\News\News\LoopPagePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E5ED11823129E397DF5F43D35D5D3AE0"
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
    
    
    public partial class ChapterPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Pivot PivotMain;
        
        internal Microsoft.Phone.Controls.WebBrowser descriptionLabel;
        
        internal System.Windows.Controls.Grid LoadingPanel;
        
        internal System.Windows.Controls.TextBlock loadingLabel;
        
        internal Microsoft.Phone.Controls.PerformanceProgressBar loadingProgressBar;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/News;component/LoopPagePage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PivotMain = ((Microsoft.Phone.Controls.Pivot)(this.FindName("PivotMain")));
            this.descriptionLabel = ((Microsoft.Phone.Controls.WebBrowser)(this.FindName("descriptionLabel")));
            this.LoadingPanel = ((System.Windows.Controls.Grid)(this.FindName("LoadingPanel")));
            this.loadingLabel = ((System.Windows.Controls.TextBlock)(this.FindName("loadingLabel")));
            this.loadingProgressBar = ((Microsoft.Phone.Controls.PerformanceProgressBar)(this.FindName("loadingProgressBar")));
        }
    }
}

