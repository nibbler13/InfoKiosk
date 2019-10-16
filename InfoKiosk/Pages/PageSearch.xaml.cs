using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace InfoKiosk.Pages {
	/// <summary>
	/// Interaction logic for PageSearch.xaml
	/// </summary>
	public partial class PageSearch : Page {
		private OnscreenKeyboard onscreenKeyboard;
		private readonly PageSelectDepartment.Source source;
		private readonly Dictionary<int, KeyValuePair<PageScrollableContent, Border>> pages =
			new Dictionary<int, KeyValuePair<PageScrollableContent, Border>>();
		private int currentPageIndex = 0;

		public PageSearch(PageSelectDepartment.Source source) {
			InitializeComponent();

			this.source = source;

			MainWindow.Instance.SetupPage(this, ButtonBack, ButtonHome);
			SetHintText();

			Loaded += (s, e) => {
				onscreenKeyboard = new OnscreenKeyboard(ActualWidth, BorderKeyboard.ActualHeight, 0, 0, 9, 30, OnscreenKeyboard.KeyboardType.Letters);
				Canvas canvasKeyboard = onscreenKeyboard.CreateOnscreenKeyboard();
				canvasKeyboard.HorizontalAlignment = HorizontalAlignment.Stretch;
				canvasKeyboard.VerticalAlignment = VerticalAlignment.Center;
				canvasKeyboard.Margin = new Thickness(0, 5, 0, 0);
				Grid.SetRow(canvasKeyboard, 2);
				GridSearch.Children.Add(canvasKeyboard);
				onscreenKeyboard.SetTextBoxInput(TextBoxEntered);
				onscreenKeyboard.SetEnterButtonClick(ButtonEnter_Click);

				TextBoxEntered.Width = canvasKeyboard.Width;
				TextBoxEntered.Focus();

				MainWindow.Instance.SetupTitle("", "Поиск");
			};

			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonClear, ButtonScrollDown, ButtonScrollUp });
		}

		private void ButtonClear_Click(object sender, RoutedEventArgs e) {
			TextBoxEntered.Clear();
			SetHintText();
		}

		private void ButtonEnter_Click(object sender, RoutedEventArgs e) {
			StartSearch();
		}

		private void TextBoxEntered_TextChanged(object sender, TextChangedEventArgs e) {
			if (TextBoxEntered.Text.Length < 2) {
				SetHintText();
				return;
			}

			StartSearch();
		}

		private void SetLabelInfoToNothingFound() {
			string nothingFound = "К сожалению, ничего не найдено" + Environment.NewLine +
				"Попробуйте изменить строку поиска";
			SetHintText(nothingFound);
		}

		private void SetHintText(string text = "") {
			string hintText = string.Empty;

			if (string.IsNullOrEmpty(text))
				switch (source) {
					case PageSelectDepartment.Source.Price:
						hintText = "Начните вводить название услуги";
						break;
					case PageSelectDepartment.Source.DocInfo:
						break;
					case PageSelectDepartment.Source.Timetable:
					case PageSelectDepartment.Source.DocRate:
						hintText = "Начните вводить фамилию врача";
						break;
					default:
						break;
				} else
				hintText = text;

			TextBlockHint.Text = hintText;

			ButtonScrollDown.Visibility = Visibility.Hidden;
			ButtonScrollUp.Visibility = Visibility.Hidden;
			GridPagesIndicator.Visibility = Visibility.Hidden;
			UserContent.Visibility = Visibility.Hidden;

			TextBlockHint.Visibility = Visibility.Visible;
		}

		private void StartSearch() {
			string text = NormalizeString(TextBoxEntered.Text);
			Logging.ToLog("Поиск по тексту: " + text);

			if (string.IsNullOrWhiteSpace(text) ||
				string.IsNullOrEmpty(text)) {
				SetHintText();
				return;
			}

			pages.Clear();
			GridPagesIndicator.RowDefinitions.Clear();
			GridPagesIndicator.Children.Clear();
			currentPageIndex = 0;

			List<string> objectsFounded = new List<string>();
			int maxElementsPerPage = 1;

			switch (source) {
				case PageSelectDepartment.Source.Price:
					maxElementsPerPage = 4;
					foreach (List<ItemService> itemServiceList in Services.DataProvider.Services.Values) {
						foreach (ItemService itemService in itemServiceList) {
							string[] values = text.Split(' ');
							bool contains = true;
							string normalizedName = NormalizeString(itemService.Name);

							foreach (string value in values) 
								if (!normalizedName.Contains(value)) {
									contains = false;
									break;
								}

							if (contains)
								objectsFounded.Add(itemService.Name + "@" + itemService.Cost);
						}
					}
					break;

				case PageSelectDepartment.Source.DocInfo:
					break;

				case PageSelectDepartment.Source.Timetable:
					maxElementsPerPage = 5;
					foreach (KeyValuePair<string, SortedDictionary<string, SortedDictionary<string, string>>> pair in Services.DataProvider.Schedule) 
						foreach (string doctor in pair.Value.Keys) 
							if (NormalizeString(doctor).StartsWith(text))
								objectsFounded.Add(doctor + "@" + pair.Key);
					
					break;

				case PageSelectDepartment.Source.DocRate:
					maxElementsPerPage = 5;
					foreach (KeyValuePair<string, List<ItemDoctor>> pair in Services.DataProvider.Survey) 
						foreach (ItemDoctor doctor in pair.Value) 
							if (NormalizeString(doctor.Name).StartsWith(text))
								objectsFounded.Add(doctor + "@" + pair.Key);
						
					break;

				default:
					break;
			}

			if (objectsFounded.Count == 0) {
				Logging.ToLog("По заданному тексту докторов не найдено");
				SetLabelInfoToNothingFound();
				return;
			}

			objectsFounded.Sort();

			List<string> pageObjects = new List<string>();
			for (int i = 0; i < objectsFounded.Count; i++) {
				pageObjects.Add(objectsFounded[i]);

				if (pageObjects.Count == maxElementsPerPage)
					AddPageToList(pageObjects, maxElementsPerPage);
			}

			if (pageObjects.Count > 0)
				AddPageToList(pageObjects, maxElementsPerPage);

			if (pages.Count == 0) {
				SetLabelInfoToNothingFound();
				return;
			}

			try {
				UserContent.Navigate(pages[0].Key);
			} catch (Exception e) {
				Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
				return;
			}

			UserContent.Visibility = Visibility.Visible;
			TextBlockHint.Visibility = Visibility.Hidden;

			if (pages.Count > 1) {
				pages[0].Value.Background = new SolidColorBrush(Colors.Gray);

				ButtonScrollDown.Visibility = Visibility.Visible;
				GridPagesIndicator.Visibility = Visibility.Visible;
			} else {
				ButtonScrollDown.Visibility = Visibility.Hidden;
				ButtonScrollUp.Visibility = Visibility.Hidden;
				GridPagesIndicator.Visibility = Visibility.Hidden;
			}

			currentPageIndex = 0;
		}

		private static string NormalizeString(string str) {
			return str.ToLower().Replace("ё", "е");
		}

		private void ButtonScrollUp_Click(object sender, RoutedEventArgs e) {
			ChangePage(-1);
		}

		private void ButtonScrollDown_Click(object sender, RoutedEventArgs e) {
			ChangePage(1);
		}

		private void ChangePage(int offset) {
			//Debug.WriteLine("currentPageIndex: " + currentPageIndex + " | offset: " + offset);

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

		private void AddPageToList(List<string> objects, int maxElementsPerPage) {
			PageScrollableContent page;

			switch (source) {
				case PageSelectDepartment.Source.Price:
					List<ItemService> services = new List<ItemService>();
					foreach (string item in objects) {
						string[] values = item.Split('@');
						ItemService itemService = new ItemService(values[0], Convert.ToInt32(values[1]), string.Empty, string.Empty, string.Empty);
						services.Add(itemService);
					}

					page = new PageScrollableContent(services.ToArray(), maxElementsPerPage);
					break;
				case PageSelectDepartment.Source.DocInfo:
					return;
				case PageSelectDepartment.Source.Timetable:
					List<KeyValuePair<string, SortedDictionary<string, string>>> schedule = new List<KeyValuePair<string, SortedDictionary<string, string>>>();
					foreach (string item in objects) {
						string[] values = item.Split('@');
						schedule.Add(new KeyValuePair<string, SortedDictionary<string, string>>(values[0], Services.DataProvider.Schedule[values[1]][values[0]]));
					}

					page = new PageScrollableContent(schedule.ToArray(), maxElementsPerPage);
					break;
				case PageSelectDepartment.Source.DocRate:
					List<string> doctors = new List<string>();
					foreach (string item in objects)
						doctors.Add(item.Replace("@", Environment.NewLine + Environment.NewLine));

					doctors.Sort();

					page = new PageScrollableContent(
						doctors.ToArray(),
						ControlsFactory.ElementType.Search,
						ButtonResult_Click,
						maxElementsPerPage,
						objects.Count < maxElementsPerPage && currentPageIndex == 0);
					break;
				default:
					return;
			}

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

		private void ButtonResult_Click(object sender, RoutedEventArgs e) {
			Button button = sender as Button;
			Page page = new PageRateDoctor(button.Tag as ItemDoctor);
			NavigationService.Navigate(page);
		}
	}
}
