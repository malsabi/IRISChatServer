using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using IRISChatServer.Views;
using IRISChatServer.Services;
using IRISChatServer.ViewModels;

namespace IRISChatServer
{
    public partial class App : Application
    {
        #region "Properties"
        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; private set; }
        #endregion

        #region "Constructors / Destructors"
        public App()
        {
            Initialize();
        }

        ~App()
        {
        }
        #endregion

        #region "Initialization"
        /// <summary>
        /// Initializes the services and sets the current to this instance.
        /// </summary>
        private void Initialize()
        {
            Current = this;
            Services = ConfigureServices();
            InitializeServices();
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IServerService, ServerService>();
            services.AddSingleton<IDatabaseManagerService, DatabaseManagerService>();
            services.AddSingleton<IServerService, ServerService>();
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// This method is used to initialize all of the services after they are configured directly.
        /// </summary>
        private void InitializeServices()
        {
            INavigationService Navigator = Services.GetService<INavigationService>();
            Navigator.RegisterView(typeof(DashboardViewModel), new DashboardPage());
            Navigator.RegisterView(typeof(ConfigurationViewModel), new ConfigurationPage());
        }
        #endregion

        #region "Application Lifecycle"
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
            base.OnStartup(e);
        }
        #endregion

        #region "Service"
        /// <summary>
        /// Returns a service otherwise null.
        /// </summary>
        /// <typeparam name="T">Represents the service interface.</typeparam>
        /// <returns>Returns a service <see cref="T"/> from the service provider.</returns>
        public static T GetService<T>()
        {
            if (Current == null)
            {
                return default;
            }
            else
            {
                return Current.Services.GetService<T>();
            }
        }
        #endregion
    }
}