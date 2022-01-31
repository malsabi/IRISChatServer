using System.Collections.ObjectModel;
using IRISChatServer.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IRISChatServer.ViewModels
{
    public class DashboardViewModel : ObservableObject
    {
        #region "Properties"
        public ObservableCollection<LogEntry> Log { get; private set; }
        #endregion

        #region "Constructors / Destructors"
        public DashboardViewModel()
        {
            Log = Logger.Entries;
        }

        ~DashboardViewModel()
        {
        }
        #endregion
    }
}