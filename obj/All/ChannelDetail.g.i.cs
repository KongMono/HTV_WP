﻿#pragma checksum "C:\Users\Kong_mono\documents\visual studio 2012\Projects\News\News\ChannelDetail.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3F59AAF65C83197A3480C4A1F0C35524"
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
using Microsoft.PlayerFramework;
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
    
    
    public partial class ChannelDetail : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Media.ImageBrush Background;
        
        internal System.Windows.Controls.TextBlock textHead;
        
        internal System.Windows.Controls.Grid GridTop;
        
        internal System.Windows.Controls.Image PicTop;
        
        internal System.Windows.Controls.Image iconPlay;
        
        internal Microsoft.PlayerFramework.MediaPlayer player;
        
        internal Microsoft.Phone.Controls.WebBrowser descriptionLabel;
        
        internal System.Windows.Controls.StackPanel stackPanel;
        
        internal System.Windows.Controls.TextBlock textView;
        
        internal System.Windows.Controls.TextBlock viewnum;
        
        internal System.Windows.Controls.Grid GridButtom;
        
        internal System.Windows.Controls.ListBox ToolBox;
        
        internal System.Windows.Controls.Grid LoadingPanel;
        
        internal System.Windows.Controls.TextBlock loadingLabel;
        
        internal System.Windows.Controls.ProgressBar performanceProgressBarCustomized;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/News;component/ChannelDetail.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Background = ((System.Windows.Media.ImageBrush)(this.FindName("Background")));
            this.textHead = ((System.Windows.Controls.TextBlock)(this.FindName("textHead")));
            this.GridTop = ((System.Windows.Controls.Grid)(this.FindName("GridTop")));
            this.PicTop = ((System.Windows.Controls.Image)(this.FindName("PicTop")));
            this.iconPlay = ((System.Windows.Controls.Image)(this.FindName("iconPlay")));
            this.player = ((Microsoft.PlayerFramework.MediaPlayer)(this.FindName("player")));
            this.descriptionLabel = ((Microsoft.Phone.Controls.WebBrowser)(this.FindName("descriptionLabel")));
            this.stackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("stackPanel")));
            this.textView = ((System.Windows.Controls.TextBlock)(this.FindName("textView")));
            this.viewnum = ((System.Windows.Controls.TextBlock)(this.FindName("viewnum")));
            this.GridButtom = ((System.Windows.Controls.Grid)(this.FindName("GridButtom")));
            this.ToolBox = ((System.Windows.Controls.ListBox)(this.FindName("ToolBox")));
            this.LoadingPanel = ((System.Windows.Controls.Grid)(this.FindName("LoadingPanel")));
            this.loadingLabel = ((System.Windows.Controls.TextBlock)(this.FindName("loadingLabel")));
            this.performanceProgressBarCustomized = ((System.Windows.Controls.ProgressBar)(this.FindName("performanceProgressBarCustomized")));
        }
    }
}

