﻿#pragma checksum "C:\Users\Kong_mono\Documents\Visual Studio 2012\Projects\News\News\Copy of ArchivePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "629D8553912FA936CB49A464536E3435"
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
    
    
    public partial class ArchivePage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Media.ImageBrush Background;
        
        internal System.Windows.Controls.TextBlock textHead;
        
        internal System.Windows.Controls.Grid GridTop;
        
        internal Microsoft.Phone.Controls.WebBrowser descriptionLabel;
        
        internal System.Windows.Controls.StackPanel stackPanel;
        
        internal System.Windows.Controls.TextBlock textView;
        
        internal System.Windows.Controls.TextBlock viewnum;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/News;component/Copy%20of%20ArchivePage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Background = ((System.Windows.Media.ImageBrush)(this.FindName("Background")));
            this.textHead = ((System.Windows.Controls.TextBlock)(this.FindName("textHead")));
            this.GridTop = ((System.Windows.Controls.Grid)(this.FindName("GridTop")));
            this.descriptionLabel = ((Microsoft.Phone.Controls.WebBrowser)(this.FindName("descriptionLabel")));
            this.stackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("stackPanel")));
            this.textView = ((System.Windows.Controls.TextBlock)(this.FindName("textView")));
            this.viewnum = ((System.Windows.Controls.TextBlock)(this.FindName("viewnum")));
            this.ToolBox = ((System.Windows.Controls.ListBox)(this.FindName("ToolBox")));
            this.LoadingPanel = ((System.Windows.Controls.Grid)(this.FindName("LoadingPanel")));
            this.loadingLabel = ((System.Windows.Controls.TextBlock)(this.FindName("loadingLabel")));
            this.performanceProgressBarCustomized = ((System.Windows.Controls.ProgressBar)(this.FindName("performanceProgressBarCustomized")));
        }
    }
}

