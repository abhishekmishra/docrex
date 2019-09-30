using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace docrex
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContainersPage : Page
    {
        private ObservableCollection<ContainerListResponse> _containers = new ObservableCollection<ContainerListResponse>() { };

        public ObservableCollection<ContainerListResponse> Containers
        {
            get { return _containers; }
        }

        public ContainersPage()
        {
            this.loadContainers();
            this.InitializeComponent();
            dataGrid.DataContext = Containers;
        }

        async void loadContainers()
        {
            IList<ContainerListResponse> containers = await ((App)Application.Current).client.Containers.ListContainersAsync(
            new ContainersListParameters()
            {
                Limit = 10,
            });
            Containers.Clear();
            foreach (ContainerListResponse container in containers)
            {
                Debug.WriteLine(container.Names[0]);
                Containers.Add(container);
            }
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }

    public class ContainerFirstNameConverter: IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            IList<string> names = (IList<string>)value;
            if(names.Count > 0)
            {
                return names[0];
            }
            else
            {
                return "NoName";
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
