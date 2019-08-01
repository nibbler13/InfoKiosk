using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace InfoKiosk {
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application {
		private void Application_Startup(object sender, StartupEventArgs e) {
			Logging.ToLog("App - ===== Запуск приложения");


			DispatcherUnhandledException += App_DispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			//string configFilePath = string.Empty;
			//string advertisementFilePath = string.Empty;

			//if (e.Args.Length == 1) {
			//	try {
			//		string arg = e.Args[0].ToString().TrimStart('"').TrimEnd('"');
			//		Logging.ToLog("App - Переданный параметр: " + arg);
			//		configFilePath = Path.Combine(arg, "InfoscreenConfig.xml");
			//		advertisementFilePath = Path.Combine(arg, "Advertisement.xml");
			//	} catch (Exception exc) {
			//		Logging.ToLog("App - " + exc.Message + Environment.NewLine + exc.StackTrace);
			//	}
			//} else
			//	Logging.ToLog("App - приложение принимает в качестве первого параметра " +
			//		"путь к файлу конфигурации, в качестве второго параметра путь к файлу с " +
			//		"информационными сообщениями");

			//Logging.ToLog("App - путь к файлу настроек: " + configFilePath);
			//Logging.ToLog("App - путь к файлу информационных сообщений: " + advertisementFilePath);
			MainWindow window = new MainWindow();
			window.Show();
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
			Logging.ToLog("!!! CurrentDomain_UnhandledException: " + e.ExceptionObject.ToString());
			HandleException(e.ExceptionObject as Exception);
		}

		private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
			//Logging.ToLog(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
			//SystemMail.SendMail(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
			//Logging.ToLog("!!!App - Аварийное завершение работы");
			//Process.GetCurrentProcess().Kill();
			//Console.WriteLine(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);

			Logging.ToLog("!!! App_DispatcherUnhandledException: " + e.ToString());
			HandleException(e.Exception);
		}

		private void HandleException(Exception exception) {
			if (exception != null) {
				string msg = exception.Message + Environment.NewLine + exception.StackTrace;
				Logging.ToLog(msg);
				//Mail.SendMail("Необработанное исключение", msg, mailTo);
			}

			if (exception.InnerException != null) {
				Logging.ToLog(exception.InnerException.Message + Environment.NewLine + exception.InnerException.StackTrace);
			}

			Logging.ToLog("!!!App - Аварийное завершение работы");
			//ExcelInterop.Instance.CloseExcel();
			Process.GetCurrentProcess().Kill();
		}
	}
}
