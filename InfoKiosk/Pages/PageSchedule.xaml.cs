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
	public partial class PageSchedule : Page {
		private Dictionary<int, KeyValuePair<PageScrollableContent, Border>> pages = new Dictionary<int, KeyValuePair<PageScrollableContent, Border>>();
		private int currentPageIndex = 0;

		public PageSchedule(string department) {
            InitializeComponent();
			MainWindow.Instance.SetupPage(this, ButtonBack, ButtonHome);

			UserContent.JournalOwnership = JournalOwnership.OwnsJournal;
			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("Расписание приёма врачей");
			};

			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonScrollDown, ButtonScrollUp });

			TextBlockDepartment.Text = department;
			TextBlockDepartment.FontSize = 40;
			ImageDepartment.Source = ControlsFactory.ImageSourceForBitmap((System.Drawing.Bitmap)ControlsFactory.GetImageForDepartment(department));

			List<KeyValuePair<string, SortedDictionary<string, string>>> pageObjects = new List<KeyValuePair<string, SortedDictionary<string, string>>>();
			foreach (KeyValuePair<string, SortedDictionary<string, string>> keyValuePair in DataProvider.scheduleDepartmentsAndDoctors[department]) {
				pageObjects.Add(keyValuePair);

				if (pageObjects.Count == 11)
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
		}

		private void AddPageToList(List<KeyValuePair<string, SortedDictionary<string, string>>> objects) {
			PageScrollableContent page = new PageScrollableContent(objects.ToArray());
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
	}
}
