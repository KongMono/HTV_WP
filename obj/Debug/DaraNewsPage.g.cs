﻿#pragma checksum "C:\Users\Kong_mono\Documents\Visual Studio 2012\Projects\News\News\DaraNewsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A540C086D96B27160F25B337E7B26357"
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
    
    
    public partial class DaraNewsPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Panorama HClusivePanorama;
        
        internal System.Windows.Media.ImageBrush bg_img;
        
        internal Microsoft.Phone.Controls.PanoramaItem PanoramaItem;
        
        internal System.Windows.Controls.ListBox ListBox;
        
        internal System.Windows.Controls.Grid LoadingPanel_HClusiveList;
        
        internal System.Windows.Controls.TextBlock loadingLabel_HClusiveList;
        
        internal System.Windows.Controls.ProgressBar loadingProgressBar_HClusiveList;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/News;component/DaraNewsPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.HClusivePanorama = ((Microsoft.Phone.Controls.Panorama)(this.FindName("HClusivePanorama")));
            this.bg_img = ((System.Windows.Media.ImageBrush)(this.FindName("bg_img")));
            this.PanoramaItem = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("PanoramaItem")));
            this.ListBox = ((System.Windows.Controls.ListBox)(this.FindName("ListBox")));
            this.LoadingPanel_HClusiveList = ((System.Windows.Controls.Grid)(this.FindName("LoadingPanel_HClusiveList")));
            this.loadingLabel_HClusiveList = ((System.Windows.Controls.TextBlock)(this.FindName("loadingLabel_HClusiveList")));
            this.loadingProgressBar_HClusiveList = ((System.Windows.Controls.ProgressBar)(this.FindName("loadingProgressBar_HClusiveList")));
        }
    }
}

