using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace docrex
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DockerClient client;

        private ObservableCollection<Container> _containers = new ObservableCollection<Container>()
        {
            //new Container() { Label = "People", Symbol = Windows.UI.Xaml.Controls.Symbol.People  },
            //new Container() { Label = "Globe", Symbol = Windows.UI.Xaml.Controls.Symbol.Globe },
            //new Container() { Label = "Message", Symbol = Windows.UI.Xaml.Controls.Symbol.Message },
            //new Container() { Label = "Mail", Symbol = Windows.UI.Xaml.Controls.Symbol.Mail },
        };

        public ObservableCollection<Container> Containers
        {
            get { return _containers; }
        }

        public MainPage()
        {
            client = new DockerClientConfiguration(
               new Uri("http://192.168.1.33:2376/"))
               .CreateClient();

            this.loadContainers();
            this.InitializeComponent();
        }

        async void loadContainers()
        {
            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
            new ContainersListParameters()
            {
                Limit = 10,
            });
            foreach(ContainerListResponse container in containers)
            {
                Debug.WriteLine(container.Names[0]);
                Containers.Add(new Container() { Label = container.Names[0], Symbol = Windows.UI.Xaml.Controls.Symbol.Globe } );
            }
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

    }
    public class Container
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
    }
}
