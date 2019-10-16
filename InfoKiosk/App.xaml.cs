using InfoKiosk.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace InfoKiosk {
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application {
		private void Application_Startup(object sender, StartupEventArgs e) {
			string msg = "App - ===== Запуск приложения";
			Logging.ToLog(msg);
			string adminAddress = Services.Config.Instance.MailAdminAddress;
			ClientMail.SendMail(msg, msg, adminAddress);

			DispatcherUnhandledException += App_DispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			if (!Services.Config.Instance.IsConfigReadedSuccessfull) {
				ClientMail.SendMail("Ошибка конфигурации", "Не удалось считать файл конфигурации: " + 
					Services.Config.Instance.ConfigFilePath, adminAddress);
				Logging.ToLog("App - !!! Конфигурация не загружена");
			}

			Logging.CheckAndCleanOldFiles(Services.Config.Instance.MaxLogfilesQuantity);

			MainWindow window = new MainWindow();
			window.Show();
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
			Logging.ToLog("App - !!! CurrentDomain_UnhandledException: " + e.ExceptionObject.ToString());
			HandleException(e.ExceptionObject as Exception);
		}

		private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
			Logging.ToLog("App - !!! App_DispatcherUnhandledException: " + e.ToString());
			HandleException(e.Exception);
		}

		private void HandleException(Exception exception) {
			if (exception != null) {
				string msg = exception.Message + Environment.NewLine + exception.StackTrace;
				Logging.ToLog("App - " + msg);
				ClientMail.SendMail("Необработанное исключение", msg, Services.Config.Instance.MailAdminAddress);
			}

			if (exception.InnerException != null) {
				Logging.ToLog("App - " + exception.InnerException.Message + Environment.NewLine + exception.InnerException.StackTrace);
			}

			Logging.ToLog("App - !!! Аварийное завершение работы");
			Process.GetCurrentProcess().Kill();
		}
	}
}
