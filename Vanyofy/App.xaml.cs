using System.Windows;
using Vanyofy.Logging;

namespace Vanyofy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            Logger.Log.Info("start vanyofy");
            
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);

            Logger.Log.Error(errorMessage);

            e.Handled = true;
        }
    }
}
