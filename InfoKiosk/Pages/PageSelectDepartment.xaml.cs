using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InfoKiosk {
    public partial class PageSelectDepartment : Page {
		private bool isLoaded = false;

		public enum Source { Price, DocInfo, Timetable, DocRate }
		private readonly Source source;
		private readonly string title;
		private readonly string subtitle;
		private readonly Dictionary<int, KeyValuePair<PageScrollableContent, Border>> pages =
			new Dictionary<int, KeyValuePair<PageScrollableContent, Border>>();
		private int currentPageIndex = 0;

		public PageSelectDepartment(Source source) {
			InitializeComponent();
			MainWindow.Instance.SetupPage(this, ButtonBack, ButtonHome);
			this.source = source;

			switch (source) {
				case Source.Price:
					title = "Услуги и цены";
					subtitle = "Выберите отделение, для которого Вы хотите увидеть список услуг";
					TextBlockInsideSearchButton.Text = "Поиск по названию услуги";
					ButtonBack.Visibility = Visibility.Hidden;
					break;
				case Source.DocInfo:
					title = "Врачи";
					subtitle = "Выберите отделение, для которого Вы хотите увидеть список врачей";
					break;
				case Source.Timetable:
					title = "Расписание приёма врачей";
					subtitle = "Выберите отделение, для которого Вы хотите увидеть расписание";
					ButtonBack.Visibility = Visibility.Hidden;
					break;
				case Source.DocRate:
					title = "Оценка приёма у врача";
					subtitle = "Выберите отделение, в котором Вы были на приёме";
					break;
				default:
					title = string.Empty;
					subtitle = string.Empty;
					break;
			}

			UserContent.JournalOwnership = JournalOwnership.OwnsJournal;
			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle(title);

				if (!isLoaded) {
					List<string> departments = new List<string>();

					if (source == Source.Price)
						try {
							departments = Services.DataProvider.Services.Keys.ToList();
						} catch (Exception exc) {
							Logging.ToLog(exc.Message + Environment.NewLine + exc.StackTrace);
						}

					else if (source == Source.Timetable)
						try {
							departments = Services.DataProvider.Schedule.Keys.ToList();
						} catch (Exception exc) {
							Logging.ToLog(exc.Message + Environment.NewLine + exc.StackTrace);
						}

					else if (source == Source.DocRate)
						try {
							foreach (KeyValuePair<string, List<ItemDoctor>> item in Services.DataProvider.Survey)
								if (item.Value.Count > 0)
									departments.Add(item.Key);
						} catch (Exception exc) {
							Logging.ToLog(exc.Message + Environment.NewLine + exc.StackTrace);
						}

					List<string> pageObjects = new List<string>();
					try {
						for (int i = 0; i < departments.Count; i++) {
							pageObjects.Add(departments[i]);

							if (pageObjects.Count == 24)
								AddPageToList(pageObjects);
						}

						if (pageObjects.Count > 0)
							AddPageToList(pageObjects);
					} catch (Exception exc) {
						Logging.ToLog(exc.Message + Environment.NewLine + exc.StackTrace);
					}

					if (pages.Count > 0)
						UserContent.Content = pages[0].Key;
					else {
						UserContent.Visibility = Visibility.Hidden;
						TextBlockNoData.Visibility = Visibility.Visible;
						return;
					}

					isLoaded = true;

					if (pages.Count > 1) {
						ButtonScrollDown.Click += ButtonScrollDown_Click;
						ButtonScrollUp.Click += ButtonScrollUp_Click;
						pages[0].Value.Background = new SolidColorBrush(Colors.Gray);

						ButtonScrollDown.Visibility = Visibility.Visible;
						GridPagesIndicator.Visibility = Visibility.Visible;
					}

					currentPageIndex = 0;
				}
			};

			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonSearch, ButtonScrollUp, ButtonScrollDown });

			TextBlockSubtitle.Text = subtitle;
			TextBlockSubtitle.FontSize = 40;
		}

		private void AddPageToList(List<string> objects) {
			PageScrollableContent page = new PageScrollableContent(
				objects.ToArray(),
				ControlsFactory.ElementType.Department,
				ButtonDepartment_Click,
				4,
				objects.Count < 24 && currentPageIndex == 0);
			objects.Clear();
			RowDefinition rowDefinition = new RowDefinition();
			GridPagesIndicator.RowDefinitions.Add(rowDefinition);
			Border border = new Border {
				Background = new SolidColorBrush(Colors.LightGray),
				Margin = new Thickness(0, 2, 0, 2)
			};

			Grid.SetRow(border, currentPageIndex);
			GridPagesIndicator.Children.Add(border);

			pages.Add(currentPageIndex, new KeyValuePair<PageScrollableContent, Border>(page, border));
			currentPageIndex++;
		}

		private void ButtonScrollUp_Click(object sender, RoutedEventArgs e) {
			ChangePage(-1);
		}

		private void ButtonScrollDown_Click(object sender, RoutedEventArgs e) {
			ChangePage(1);
		}

		private void ChangePage(int offset) {
			pages[currentPageIndex].Value.Background = new SolidColorBrush(Colors.LightGray);
			currentPageIndex += offset;
			pages[currentPageIndex].Value.Background = new SolidColorBrush(Colors.Gray);
			UserContent.Content = pages[currentPageIndex].Key;

			if (currentPageIndex == pages.Count - 1) {
				ButtonScrollUp.Visibility = Visibility.Visible;
				ButtonScrollDown.Visibility = Visibility.Hidden;
			} else if (currentPageIndex == 0) {
				ButtonScrollUp.Visibility = Visibility.Hidden;
				ButtonScrollDown.Visibility = Visibility.Visible;
			} else {
				ButtonScrollUp.Visibility = Visibility.Visible;
				ButtonScrollDown.Visibility = Visibility.Visible;
			}
		}

		private void ButtonDepartment_Click(object sender, RoutedEventArgs e) {
			Button button = sender as Button;
			string tag = button.Tag as string;

			if (source == Source.Timetable) {
				NavigationService.Navigate(new PageSchedule(tag));
				return;
			} else if (source == Source.Price) {
				NavigationService.Navigate(new PageServices(tag));
				return;
			}

			bool isSourceDocInfo = source == Source.DocInfo;
			PageSelectDoctor pageSelectDoctor = new PageSelectDoctor(tag, isSourceDocInfo);
			NavigationService.Navigate(pageSelectDoctor);
		}

		private void ButtonSearch_Click(object sender, RoutedEventArgs e) {
			Pages.PageSearch pageSearch = new Pages.PageSearch(source);
			NavigationService.Navigate(pageSearch);
		}
	}
}
