using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace docrex
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SystemInfoPage : Page
    {
        public SystemInfoResponse SystemInfo { get; set; }
        private ObservableCollection<JSONMessage> _events = new ObservableCollection<JSONMessage>() { };

        public ObservableCollection<JSONMessage> Events
        {
            get { return _events; }
        }
        public SystemInfoPage()
        {
            this.InitializeComponent();
            this.getSystemInfo();
        }

        async void getSystemInfo()
        {
            DockerClient client = ((App)Application.Current).client;
            SystemInfo = await client.System.GetSystemInfoAsync();
            Debug.WriteLine("Docker version is -> " + SystemInfo.ServerVersion);
            Bindings.Update();
            ProgressRing.IsActive = false;
            SystemInfoDetails.Visibility = Visibility.Visible;
            ExtendedInfo.Visibility = Visibility.Visible;
            EventsInfo.Visibility = Visibility.Visible;

            CancellationTokenSource cancellation = new CancellationTokenSource();
            var eventsProgress = new Progress<JSONMessage>(ShowEvent);
            await client.System.MonitorEventsAsync(new ContainerEventsParameters(), eventsProgress, cancellation.Token);
        }

        void ShowEvent(JSONMessage eventMessage)
        {
            Debug.WriteLine("Received message id -> " + eventMessage.ID);
            _events.Insert(0, eventMessage);
            Bindings.Update();
        }
    }
}
