﻿#pragma checksum "C:\Users\Kong_mono\documents\visual studio 2012\Projects\News\News\AddChannelPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FD7BFC980DF1387D9F9E24D31C848CEF"
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
    
    
    public partial class AddChannelPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Panorama HClusivePanorama;
        
        internal System.Windows.Media.ImageBrush bg_img;
        
        internal Microsoft.Phone.Controls.PanoramaItem PanoramaItem;
        
        internal System.Windows.Controls.ListBox ListBox;
        
        internal System.Windows.Controls.Grid LoadingPanel_List;
        
        internal System.Windows.Controls.TextBlock loadingLabel_List;
        
        internal System.Windows.Controls.ProgressBar loadingProgressBar_List;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/News;component/AddChannelPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.HClusivePanorama = ((Microsoft.Phone.Controls.Panorama)(this.FindName("HClusivePanorama")));
            this.bg_img = ((System.Windows.Media.ImageBrush)(this.FindName("bg_img")));
            this.PanoramaItem = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("PanoramaItem")));
            this.ListBox = ((System.Windows.Controls.ListBox)(this.FindName("ListBox")));
            this.LoadingPanel_List = ((System.Windows.Controls.Grid)(this.FindName("LoadingPanel_List")));
            this.loadingLabel_List = ((System.Windows.Controls.TextBlock)(this.FindName("loadingLabel_List")));
            this.loadingProgressBar_List = ((System.Windows.Controls.ProgressBar)(this.FindName("loadingProgressBar_List")));
            this.AppBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("AppBar")));
        }
    }
}

