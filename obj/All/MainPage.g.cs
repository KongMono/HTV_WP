﻿#pragma checksum "C:\Users\Kong_mono\documents\visual studio 2012\Projects\News\News\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E634CC312E7C75F7853A74F87089E663"
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
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ApplicationBar HTVSocietyBar;
        
        internal Microsoft.Phone.Shell.ApplicationBar StreamBar;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Panorama homePanorama;
        
        internal System.Windows.Media.ImageBrush bg_img;
        
        internal Microsoft.Phone.Controls.PanoramaItem PanoramaItem1;
        
        internal Microsoft.Phone.Controls.Pivot pivotSteam;
        
        internal Microsoft.Phone.Controls.PivotItem pivotFavorite;
        
        internal System.Windows.Controls.TextBlock pivotFavoriteText;
        
        internal System.Windows.Controls.ListBox liveTvFavoriteListBox;
        
        internal Microsoft.Phone.Controls.PivotItem pivotChannelAll;
        
        internal System.Windows.Controls.TextBlock pivotChannelAllText;
        
        internal System.Windows.Controls.TextBlock TextHeadChannel;
        
        internal System.Windows.Controls.ListBox liveTvListBox;
        
        internal System.Windows.Controls.Grid LoadingPanel_steaming;
        
        internal System.Windows.Controls.TextBlock loadingLabel_steaming;
        
        internal System.Windows.Controls.ProgressBar loadingProgressBar_steaming;
        
        internal Microsoft.Phone.Controls.PanoramaItem PanoramaItem2;
        
        internal System.Windows.Controls.TextBlock TextHead;
        
        internal System.Windows.Controls.ListBox TVSocietyNewsListBox;
        
        internal System.Windows.Controls.Grid TVSocietyGridAll;
        
        internal Microsoft.Phone.Controls.LongListSelector longListSelector;
        
        internal System.Windows.Controls.ListBox TVSocietyListTodayBox;
        
        internal System.Windows.Controls.ListBox TVSocietyListShotDaraBox;
        
        internal System.Windows.Controls.Grid LoadingPanel_TVSociety;
        
        internal System.Windows.Controls.TextBlock loadingLabel_TVSociety;
        
        internal System.Windows.Controls.ProgressBar loadingProgressBar_TVSociety;
        
        internal Microsoft.Phone.Controls.PanoramaItem PanoramaItem3;
        
        internal System.Windows.Controls.ListBox HClusiveListBox;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/News;component/MainPage.xaml", System.UriKind.Relative));
            this.HTVSocietyBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("HTVSocietyBar")));
            this.StreamBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("StreamBar")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.homePanorama = ((Microsoft.Phone.Controls.Panorama)(this.FindName("homePanorama")));
            this.bg_img = ((System.Windows.Media.ImageBrush)(this.FindName("bg_img")));
            this.PanoramaItem1 = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("PanoramaItem1")));
            this.pivotSteam = ((Microsoft.Phone.Controls.Pivot)(this.FindName("pivotSteam")));
            this.pivotFavorite = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("pivotFavorite")));
            this.pivotFavoriteText = ((System.Windows.Controls.TextBlock)(this.FindName("pivotFavoriteText")));
            this.liveTvFavoriteListBox = ((System.Windows.Controls.ListBox)(this.FindName("liveTvFavoriteListBox")));
            this.pivotChannelAll = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("pivotChannelAll")));
            this.pivotChannelAllText = ((System.Windows.Controls.TextBlock)(this.FindName("pivotChannelAllText")));
            this.TextHeadChannel = ((System.Windows.Controls.TextBlock)(this.FindName("TextHeadChannel")));
            this.liveTvListBox = ((System.Windows.Controls.ListBox)(this.FindName("liveTvListBox")));
            this.LoadingPanel_steaming = ((System.Windows.Controls.Grid)(this.FindName("LoadingPanel_steaming")));
            this.loadingLabel_steaming = ((System.Windows.Controls.TextBlock)(this.FindName("loadingLabel_steaming")));
            this.loadingProgressBar_steaming = ((System.Windows.Controls.ProgressBar)(this.FindName("loadingProgressBar_steaming")));
            this.PanoramaItem2 = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("PanoramaItem2")));
            this.TextHead = ((System.Windows.Controls.TextBlock)(this.FindName("TextHead")));
            this.TVSocietyNewsListBox = ((System.Windows.Controls.ListBox)(this.FindName("TVSocietyNewsListBox")));
            this.TVSocietyGridAll = ((System.Windows.Controls.Grid)(this.FindName("TVSocietyGridAll")));
            this.longListSelector = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("longListSelector")));
            this.TVSocietyListTodayBox = ((System.Windows.Controls.ListBox)(this.FindName("TVSocietyListTodayBox")));
            this.TVSocietyListShotDaraBox = ((System.Windows.Controls.ListBox)(this.FindName("TVSocietyListShotDaraBox")));
            this.LoadingPanel_TVSociety = ((System.Windows.Controls.Grid)(this.FindName("LoadingPanel_TVSociety")));
            this.loadingLabel_TVSociety = ((System.Windows.Controls.TextBlock)(this.FindName("loadingLabel_TVSociety")));
            this.loadingProgressBar_TVSociety = ((System.Windows.Controls.ProgressBar)(this.FindName("loadingProgressBar_TVSociety")));
            this.PanoramaItem3 = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("PanoramaItem3")));
            this.HClusiveListBox = ((System.Windows.Controls.ListBox)(this.FindName("HClusiveListBox")));
            this.LoadingPanel_HClusiveList = ((System.Windows.Controls.Grid)(this.FindName("LoadingPanel_HClusiveList")));
            this.loadingLabel_HClusiveList = ((System.Windows.Controls.TextBlock)(this.FindName("loadingLabel_HClusiveList")));
            this.loadingProgressBar_HClusiveList = ((System.Windows.Controls.ProgressBar)(this.FindName("loadingProgressBar_HClusiveList")));
        }
    }
}

