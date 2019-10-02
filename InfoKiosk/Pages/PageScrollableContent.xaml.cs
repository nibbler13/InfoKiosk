using System;
using System.Collections.Generic;
using System.Globalization;
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
	/// <summary>
	/// Interaction logic for PageContent.xaml
	/// </summary>
	public partial class PageScrollableContent : Page {
		private bool isLoaded = false;
		private enum TextBlockStyle { Department, DoctorName, WorkTime, Days }
		private readonly int maxRows;

		private PageScrollableContent() {
			InitializeComponent();
			MainWindow.Instance.SetupPage(this);
		}

		public PageScrollableContent(object[] objects,
							   ControlsFactory.ElementType type,
							   RoutedEventHandler routedEventHandler,
							   int elementsInRow,
							   bool isSinglePage = false) : this() {
			WrapPanelContent.Visibility = Visibility.Visible;

			Loaded += (s, e) => {
				if (isLoaded)
					return;

				PageTemplate.FillPanelWithElements(
					WrapPanelContent, objects, type, routedEventHandler, elementsInRow);

				if (isSinglePage) {
					WrapPanelContent.HorizontalAlignment = HorizontalAlignment.Center;
					WrapPanelContent.VerticalAlignment = VerticalAlignment.Center;
				}

				isLoaded = true;
			};
		}

		public PageScrollableContent(ItemService[] objects, int maxRows = 10) : this() {
			this.maxRows = maxRows;
			for (int i = 0; i < maxRows; i++)
				GridServices.RowDefinitions.Add(new RowDefinition());

			GridServices.Visibility = Visibility.Visible;
			SetupPricePage(objects);
		}

		public PageScrollableContent(KeyValuePair<string, SortedDictionary<string, string>>[] objects, int maxRows = 12) : this() {
			this.maxRows = maxRows;
			for (int i = 0; i < maxRows; i++) 
				GridSchedule.RowDefinitions.Add(new RowDefinition());
			
			GridSchedule.Visibility = Visibility.Visible;
			SetupSchedulePage(objects);
		}

		private void SetupPricePage(ItemService[] objects) {
			Border borderFirst = new Border() {
				BorderThickness = new Thickness(0, 0, 0, 2),
				BorderBrush = new SolidColorBrush(Color.FromRgb(218, 218, 218)),// "#FFDADADA",
				HorizontalAlignment = HorizontalAlignment.Stretch,// "Stretch",
				VerticalAlignment = VerticalAlignment.Top// "Stretch"
			};

			Grid.SetRow(borderFirst, 0);// = "8",
			Grid.SetColumnSpan(borderFirst, 2);// = "2"
			GridServices.Children.Add(borderFirst);

			int row = 0;
			foreach (ItemService item in objects) {
				TextBlock textBlockName = new TextBlock() {
					Text = item.Name,
					FontSize = 30,
					TextWrapping = TextWrapping.Wrap,
					TextTrimming = TextTrimming.CharacterEllipsis,
					VerticalAlignment = VerticalAlignment.Center
				};
				//textBlockName.FontFamily = new FontFamily("FuturaLightC");
				Grid.SetRow(textBlockName, row);
				GridServices.Children.Add(textBlockName);

				TextBlock textBlockCost = new TextBlock() {
					Text = item.Cost.ToString("C0", CultureInfo.CreateSpecificCulture("ru-RU")),
					FontSize = 30,
					TextWrapping = TextWrapping.Wrap,
					TextTrimming = TextTrimming.CharacterEllipsis,
					HorizontalAlignment = HorizontalAlignment.Right,
					VerticalAlignment = VerticalAlignment.Center
				};
				//textBlockCost.FontFamily = new FontFamily("FuturaLightC");
				Grid.SetRow(textBlockCost, row);
				Grid.SetColumn(textBlockCost, 1);
				GridServices.Children.Add(textBlockCost);

				Border border = new Border() {
					BorderThickness = new Thickness(0, 0, 0, 2),
					BorderBrush = new SolidColorBrush(Color.FromRgb(218, 218, 218)),// "#FFDADADA",
					HorizontalAlignment = HorizontalAlignment.Stretch,// "Stretch",
					VerticalAlignment = VerticalAlignment.Bottom// "Stretch"
				};

				Grid.SetRow(border, row);// = "8",
				Grid.SetColumnSpan(border, 2);// = "2"
				GridServices.Children.Add(border);

				row++;
				if (row >= maxRows)
					break;
			}
		}

		private void SetupSchedulePage(KeyValuePair<string, SortedDictionary<string, string>>[] objects) {
			CreateHeader();

			int row = 1;
			foreach (KeyValuePair<string, SortedDictionary<string, string>> doctorsSchedule in objects) {
				CreateTextBlock(doctorsSchedule.Key, row, 0, TextBlockStyle.DoctorName);

				int column = 1;
				int i = 0;
				foreach (KeyValuePair<string, string> schedule in doctorsSchedule.Value) {
					CreateTextBlock(schedule.Value, row, column + i, TextBlockStyle.WorkTime);
					i++;
				}

				row++;
				if (row >= maxRows)
					break;
			}
		}





		private void CreateHeader() {
			//int offsetSat = -1;
			//int offsetSun = -1;

			for (int i = 0; i < 7; i++) {
				string dayOfWeek = string.Empty;
				DateTime dateToShow = DateTime.Now.AddDays(i);
				switch (dateToShow.DayOfWeek) {
					case DayOfWeek.Sunday:
						dayOfWeek = "Вс";
						//offsetSun = i;
						break;
					case DayOfWeek.Monday:
						dayOfWeek = "Пн";
						break;
					case DayOfWeek.Tuesday:
						dayOfWeek = "Вт";
						break;
					case DayOfWeek.Wednesday:
						dayOfWeek = "Ср";
						break;
					case DayOfWeek.Thursday:
						dayOfWeek = "Чт";
						break;
					case DayOfWeek.Friday:
						dayOfWeek = "Пт";
						break;
					case DayOfWeek.Saturday:
						dayOfWeek = "Сб";
						//offsetSat = i;
						break;
					default:
						break;
				}

				string month = string.Empty;
				switch (dateToShow.Month) {
					case 1:
						month = "янв.";
						break;
					case 2:
						month = "фев.";
						break;
					case 3:
						month = "мар.";
						break;
					case 4:
						month = "апр.";
						break;
					case 5:
						month = "мая";
						break;
					case 6:
						month = "июн.";
						break;
					case 7:
						month = "июл.";
						break;
					case 8:
						month = "авг.";
						break;
					case 9:
						month = "сен.";
						break;
					case 10:
						month = "окт.";
						break;
					case 11:
						month = "ноя.";
						break;
					case 12:
						month = "дек.";
						break;
					default:
						break;
				}

				dayOfWeek += " " + dateToShow.Day + " " + month;
				if (i == 0)
					dayOfWeek = "Сегодня";

				CreateTextBlock(dayOfWeek, 0, i + 1, TextBlockStyle.Days);
			}
		}

		//private void CreateWeekendsBackground(int columnOffset) {
		//	for (int i = 0; i < GridSchedule.RowDefinitions.Count; i++) {
		//		Border border = new Border {
		//			Background = new SolidColorBrush(Color.FromArgb(20, 249, 141, 60))
		//		};
		//		Grid.SetRow(border, i);
		//		Grid.SetColumn(border, 1 + columnOffset);
		//		Grid.SetZIndex(border, -1);
		//		GridSchedule.Children.Add(border);
		//	}
		//}

		private TextBlock CreateTextBlock(string text, int row, int column, TextBlockStyle textBlockStyle) {
			int columnSpan = 1;
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
			double fontSize = Application.Current.MainWindow.ActualHeight / 35;
			Brush foreground = new SolidColorBrush(Color.FromRgb(45, 61, 63));
			FontFamily fontFamily = new FontFamily("FuturaLightC");
			//FontWeight fontWeight = FontWeights.Light;
			Thickness margin = new Thickness(0);

			switch (textBlockStyle) {
				case TextBlockStyle.Department:
					fontSize *= 1.3;
					columnSpan = 8;
					foreground = Brushes.White;
					//fontWeight = FontWeights.DemiBold;
					break;
				case TextBlockStyle.DoctorName:
					fontSize *= 1.15;
					horizontalAlignment = HorizontalAlignment.Left;
					//fontWeight = FontWeights.Normal;
					margin = new Thickness(10, 0, 10, 0);
					break;
				case TextBlockStyle.WorkTime:
					fontSize *= 0.9;
					//fontWeight = FontWeights.ExtraLight;
					margin = new Thickness(3, 0, 3, 0);
					if (text.StartsWith("0"))
						text = text.Substring(1, text.Length - 1);
					break;
				case TextBlockStyle.Days:
					fontFamily = new FontFamily("FuturaFuturisC");
					//fontWeight = FontWeights.DemiBold;
					break;
				default:
					break;
			}

			Border border = new Border();
			if (textBlockStyle == TextBlockStyle.Department) {
				border.Background = new SolidColorBrush(Color.FromRgb(0, 169, 220));
				border.HorizontalAlignment = HorizontalAlignment.Stretch;
			} else {
				border.BorderThickness = new Thickness(1, 1, 1, 1);
				border.BorderBrush = Brushes.LightGray;
			}

			Grid.SetRow(border, row);
			Grid.SetColumn(border, column);
			Grid.SetColumnSpan(border, columnSpan);
			GridSchedule.Children.Add(border);

			TextBlock textBlock = new TextBlock {
				Text = text,
				HorizontalAlignment = horizontalAlignment,
				FontSize = fontSize,
				Foreground = foreground,
				VerticalAlignment = VerticalAlignment.Center,
				FontFamily = fontFamily,
				//FontWeight = fontWeight,
				TextWrapping = TextWrapping.Wrap,
				Margin = margin
			};

			Grid.SetRow(textBlock, row);
			Grid.SetColumn(textBlock, column);
			Grid.SetColumnSpan(textBlock, columnSpan);

			if (textBlockStyle == TextBlockStyle.DoctorName) {
				textBlock.Text = string.Empty;
				textBlock.Inlines.Add(new Run(text.Substring(0, 1)) { FontFamily = new FontFamily("FuturaFuturisC") });
				textBlock.Inlines.Add(text.Substring(1, text.Length - 1));
			}

			if (textBlockStyle == TextBlockStyle.Department) {
				Image image = new Image {
					Source = ControlsFactory.ImageSourceForBitmap((System.Drawing.Bitmap)ControlsFactory.GetImageForDepartment(text)),
					Margin = new Thickness(0, 4, 20, 4),
					VerticalAlignment = VerticalAlignment.Center
				};
				RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);

				StackPanel stackPanel = new StackPanel {
					Orientation = Orientation.Horizontal,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
				stackPanel.Children.Add(image);
				stackPanel.Children.Add(textBlock);

				Grid.SetRow(stackPanel, row);
				Grid.SetColumn(stackPanel, column);
				Grid.SetColumnSpan(stackPanel, columnSpan);

				GridSchedule.Children.Add(stackPanel);
			} else
				GridSchedule.Children.Add(textBlock);

			return textBlock;
		}
	}
}
