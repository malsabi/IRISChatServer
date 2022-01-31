using System.Windows.Controls;
using IRISChatServer.Services;
using IRISChatServer.ViewModels;

namespace IRISChatServer.Views
{
    public partial class MasterPage : Page
    {
        public MasterPage()
        {
            InitializeComponent();
            App.GetService<INavigationService>().SetCurrentFrame(ScenarioFrame);
        }

        private void SelectionChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            ListBox NavigationListBox = sender as ListBox;
            if (NavigationListBox.SelectedItem is ListBoxItem SelectedListBoxItem)
            {
                if (SelectedListBoxItem.Tag.Equals(nameof(DashboardViewModel)))
                {
                    App.GetService<INavigationService>().Navigate<DashboardViewModel>();
                }
                else
                {
                    App.GetService<INavigationService>().Navigate<ConfigurationViewModel>();
                }
            }
        }
    }
}