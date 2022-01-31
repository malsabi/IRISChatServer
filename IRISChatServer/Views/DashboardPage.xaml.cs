using System.Windows.Controls;
using System.Collections.Generic;
using IRISChatServer.Models;
using IRISChatServer.ViewModels;

namespace IRISChatServer.Views
{
    public partial class DashboardPage : Page
    {
        public DashboardViewModel ViewModel => (DashboardViewModel)DataContext;

        public DashboardPage()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
            List<User> items = new List<User>();
            for (int i = 0; i < 0; i++)
            {
                items.Add(new User("127.0.0." + i.ToString(), "Mohammed Ahmed", "xxdarklord65xx", "10-10-2022", "10-10-2022", true, true));
                Logging.Logger.Information("User added: " + "127.0.0." + i.ToString());
                Logging.Logger.Success("User added: " + "127.0.0." + i.ToString());
                Logging.Logger.Warning("User added: " + "127.0.0." + i.ToString());
                Logging.Logger.Error("User added: " + "127.0.0." + i.ToString());
            }
            UsersListview.ItemsSource = items;
        }
    }
}