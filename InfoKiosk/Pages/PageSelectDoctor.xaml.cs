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
    public partial class PageSelectDoctor : Page {
		private bool isLoaded = false;
		private bool isSourceDocInfo;
		private string title;
		private Dictionary<int, KeyValuePair<PageScrollableContent, Border>> pages = new Dictionary<int, KeyValuePair<PageScrollableContent, Border>>();
		private int currentPageIndex = 0;

		public PageSelectDoctor(string department, bool isSourceDocInfo = false) {
            InitializeComponent();
			MainWindow.Instance.SetupPage(this, ButtonBack, ButtonHome);
			this.isSourceDocInfo = isSourceDocInfo;
			title = isSourceDocInfo ? "Врачи" : "Оценка приёма у врача";

			UserContent.JournalOwnership = JournalOwnership.OwnsJournal;
			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle(title);

				if (!isLoaded) {
					List<string> pageObjects = new List<string>();
					List<string> doctors = DataProvider.surveyDepartmentsAndDoctors[department];
					for (int i = 0; i < doctors.Count; i++) {
						pageObjects.Add(doctors[i]);

						if (pageObjects.Count == 10)
							AddPageToList(pageObjects);
					}

					if (pageObjects.Count > 0)
						AddPageToList(pageObjects);

					UserContent.Content = pages[0].Key;

					if (pages.Count > 1) {
						ButtonScrollDown.Click += ButtonScrollDown_Click;
						ButtonScrollUp.Click += ButtonScrollUp_Click;
						pages[0].Value.Background = new SolidColorBrush(Colors.Gray);

						ButtonScrollDown.Visibility = Visibility.Visible;
						GridPagesIndicator.Visibility = Visibility.Visible;
					}

					currentPageIndex = 0;

					isLoaded = true;
				}
			};

			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonScrollDown, ButtonScrollUp });

			if (isSourceDocInfo) {
				TextBlockSubtitle.Text = "Выберите врача, для просмотра подробной информации" + Environment.NewLine + "Для выбора коснитесь нужной кнопки:";
				TextBlockSubtitle.FontSize = 40;
			}

			TextBlockDepartment.Text = department;
			TextBlockDepartment.FontSize = 40;
			TextBlockSubtitle.FontSize = 40;
			ImageDepartment.Source = ControlsFactory.ImageSourceForBitmap((System.Drawing.Bitmap)ControlsFactory.GetImageForDepartment(department));
		}

		private void AddPageToList(List<string> objects) {
			PageScrollableContent page = new PageScrollableContent(
				objects.ToArray(),
				ControlsFactory.ElementType.Doctor,
				ButtonDoctor_Click,
				5,
				objects.Count < 10 && currentPageIndex == 0);
			objects.Clear();
			RowDefinition rowDefinition = new RowDefinition();
			GridPagesIndicator.RowDefinitions.Add(rowDefinition);
			Border border = new Border();
			border.Background = new SolidColorBrush(Colors.LightGray);
			border.Margin = new Thickness(0, 2, 0, 2);
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

		private void ButtonDoctor_Click(object sender, RoutedEventArgs e) {
			Button button = sender as Button;
			string docName = button.Tag as string;
			Page page;

			if (isSourceDocInfo)
				page = new PageDocInfo();
			else
				page = new PageRateDoctor(docName);

			NavigationService.Navigate(page);
		}

		private void ButtonSearch_Click(object sender, RoutedEventArgs e) {

		}
	}
}
