﻿#pragma checksum "C:\Users\Kong_mono\Documents\Visual Studio 2012\Projects\News\News\ProgramDetail.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "861D37C793D6A454B1E62E1E430DCD8E"
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
    
    
    public partial class ProgramDetail : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Panorama HClusivePanorama;
        
        internal System.Windows.Media.ImageBrush bg_img;
        
        internal Microsoft.Phone.Controls.PanoramaItem PanoramaItem;
        
        internal System.Windows.Controls.ListBox ProgramListBox;
        
        internal System.Windows.Controls.Grid LoadingPanel;
        
        internal System.Windows.Controls.TextBlock loadingLabel;
        
        internal System.Windows.Controls.ProgressBar loadingProgressBar;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/News;component/ProgramDetail.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.HClusivePanorama = ((Microsoft.Phone.Controls.Panorama)(this.FindName("HClusivePanorama")));
            this.bg_img = ((System.Windows.Media.ImageBrush)(this.FindName("bg_img")));
            this.PanoramaItem = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("PanoramaItem")));
            this.ProgramListBox = ((System.Windows.Controls.ListBox)(this.FindName("ProgramListBox")));
            this.LoadingPanel = ((System.Windows.Controls.Grid)(this.FindName("LoadingPanel")));
            this.loadingLabel = ((System.Windows.Controls.TextBlock)(this.FindName("loadingLabel")));
            this.loadingProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("loadingProgressBar")));
        }
    }
}

