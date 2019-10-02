using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace InfoKiosk {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private PageSelectSection rootPage;
		private static MainWindow instance = null;
		private static readonly object padlock = new object();
		private readonly DispatcherTimer autoCloseTimer;
		private readonly DateTime dateTimeStart;
		private int infoPageCloseCounter = 0;
		public Style StyleButtonRoundedCorner { get; private set; }


		public static MainWindow Instance {
			get {
				lock (padlock) {
					if (instance == null)
						instance = new MainWindow();

					return instance;
				}
			}
		}




		public MainWindow() {
			InitializeComponent();

			PreviewKeyDown += (s, e) => {
				if (e.Key.Equals(Key.Escape)) {
					Logging.ToLog("MainWindow - ===== Завершение работы по нажатию клавиши ESC");
					Application.Current.Shutdown();
				}
			};

			StartTimeDelimiterTick();

			FrameMain.JournalOwnership = JournalOwnership.OwnsJournal;

			autoCloseTimer = new DispatcherTimer {
				Interval = TimeSpan.FromSeconds(Services.Configuration.Instance.AutoCloseTimerIntervalInSeconds)
			};

			autoCloseTimer.Tick += AutoCloseTimer_Tick;
			FrameMain.Navigated += FrameMain_Navigated;
			PreviewMouseDown += MainWindow_PreviewMouseDown;

			Services.DataProvider.LoadData();

			Loaded += (s, e) => {
				rootPage = new PageSelectSection();
				FrameMain.NavigationService.Navigate(rootPage);
			};

			instance = this;
			StyleButtonRoundedCorner = Application.Current.MainWindow.FindResource("RoundCorner") as Style;

			dateTimeStart = DateTime.Now;
			DispatcherTimer autoQuitTimer = new DispatcherTimer {
				Interval = TimeSpan.FromMinutes(1)
			};

			autoQuitTimer.Tick += (s, e) => {
				if (DateTime.Now.Date != dateTimeStart.Date) {
					Logging.ToLog("MainWindow - ==== Автоматическое завершение работы");
					Application.Current.Shutdown();
				}
			};
			autoQuitTimer.Start();

			if (Services.Configuration.Instance.IsDebug) {
				Topmost = false;
				Cursor = Cursors.Arrow;
			}
		}


		private void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
			if (FrameMain.Content is PageSelectSection)
				return;

			ResetAutoCloseTimer();
		}

		public void ResetAutoCloseTimer() {
			autoCloseTimer.Stop();
			autoCloseTimer.Start();
			infoPageCloseCounter = 0;
		}

		private void FrameMain_Navigated(object sender, NavigationEventArgs e) {
			if (e.Content is PageSelectSection) {
				autoCloseTimer.Stop();
				return;
			}

			ResetAutoCloseTimer();
		}


		private void AutoCloseTimer_Tick(object sender, EventArgs e) {
			infoPageCloseCounter++;

			if (FrameMain.Content is PageServices ||
				FrameMain.Content is PageSchedule)
				if (infoPageCloseCounter < Services.Configuration.Instance.AutoCloseTImerInfoPageMultiplier)
					return;

			Logging.ToLog("MainWindow - Автозакрытие страницы по таймеру");

			if (ItemSurveyResult.Instance.NeedToWriteToDb) {
				if (FrameMain.Content is PageCallback)
					ItemSurveyResult.Instance.SetPhoneNumber("Timeout");
				else if (FrameMain.Content is PageComment)
					ItemSurveyResult.Instance.SetComment("Timeout");

				ItemSurveyResult.WriteSurveyResultToDb();
			}

			CloseAllPages(null, null);
		}

		private void NavigateBack(object sender, RoutedEventArgs e) {
			Logging.ToLog("MainWindow - Возврат на предыдущую страницу");

			if (FrameMain.NavigationService.CanGoBack) {
				FrameMain.NavigationService.GoBack();
				FrameMain.NavigationService.RemoveBackEntry();
			}
		}

		public void CloseAllPages(object sender, RoutedEventArgs e) {
			Logging.ToLog("MainWindow - закрытие всех страниц");

			try {
				while (FrameMain.NavigationService.CanGoBack) {
					FrameMain.NavigationService.GoBack();
					FrameMain.NavigationService.RemoveBackEntry();
				}
			} catch (Exception exc) {
				Logging.ToLog(exc.Message + Environment.NewLine + exc.StackTrace);
			}

			if (!(FrameMain.Content is PageRangeSelection))
				FrameMain.NavigationService.Navigate(rootPage);

			if (ItemSurveyResult.Instance.NeedToWriteToDb)
				ItemSurveyResult.WriteSurveyResultToDb();

			autoCloseTimer.Stop();
			GC.Collect();
		}


		public void SetupPage(Page page, Button buttonBack = null, Button buttonHome = null) {
			page.FontFamily = new FontFamily("FuturaLightC");
			page.FontSize = 50;
			page.Foreground = new SolidColorBrush(Color.FromRgb(30, 30, 30));
			page.PreviewMouseDown += MainWindow_PreviewMouseDown;

			if (buttonBack != null) {
				buttonBack.Click += NavigateBack;
				ApplyStyleForButtons(new List<Button> { buttonBack });
			}

			if (buttonHome != null) {
				buttonHome.Click += CloseAllPages;
				ApplyStyleForButtons(new List<Button> { buttonHome });
			}
		}

		public static void ApplyStyleForButtons(List<Button> buttons, double fontSize = 30) {
			foreach (Button button in buttons) {
				button.Style = MainWindow.Instance.StyleButtonRoundedCorner;
				button.FontSize = fontSize;
			}
		}

		public void SetupTitle(string title, string details = "") {
			Logging.ToLog("MainWindow - Переход на страницу: " + title + 
				(string.IsNullOrEmpty(details) ? string.Empty : " @ " + details));
			TextBlockTitle.Visibility = Visibility.Visible;
			TextBlockTitle.Text = title;
		}

		private void StartTimeDelimiterTick() {
			DispatcherTimer timerTimeDilimeterTick = new DispatcherTimer {
				Interval = TimeSpan.FromSeconds(1)
			};

			timerTimeDilimeterTick.Tick += (s, ev) => {
				Application.Current.Dispatcher.Invoke((Action)delegate {
					TextBlockTimeSplitter.Visibility =
						TextBlockTimeSplitter.Visibility == Visibility.Visible ?
						Visibility.Hidden : Visibility.Visible;
					TextBlockTimeHours.Text = DateTime.Now.Hour.ToString();
					TextBlockTimeMinutes.Text = DateTime.Now.ToString("mm");
				});
			};
			timerTimeDilimeterTick.Start();
		}
	}
}
