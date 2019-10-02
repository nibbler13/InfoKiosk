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
	public partial class PageComment : Page {
		private OnscreenKeyboard onscreenKeyboard;

		public PageComment() {
            InitializeComponent();
			MainWindow.Instance.SetupPage(this, ButtonBack);

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle(string.Empty, "Желаете ли Вы оставить комментарий к своей оценке?");

				onscreenKeyboard = new OnscreenKeyboard(ActualWidth, BorderKeyboard.ActualHeight, 0, 0, 9, 30, OnscreenKeyboard.KeyboardType.Full);
				Canvas canvasKeyboard = onscreenKeyboard.CreateOnscreenKeyboard();
				canvasKeyboard.HorizontalAlignment = HorizontalAlignment.Stretch;
				canvasKeyboard.VerticalAlignment = VerticalAlignment.Center;
				Grid.SetRow(canvasKeyboard, 8);
				Grid.SetColumnSpan(canvasKeyboard, 5);
				GridComment.Children.Add(canvasKeyboard);
				onscreenKeyboard.SetTextBoxInput(TextBoxComment);

				TextBoxComment.Focus();
			};

			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonNext, ButtonClear });
			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonYes, ButtonNo }, 50);
		}

		private void ButtonYes_Click(object sender, RoutedEventArgs e) {
			GridQuestion.Visibility = Visibility.Hidden;
			GridComment.Visibility = Visibility.Visible;
			ButtonNext.Visibility = Visibility.Visible;
		}

		private void ButtonNo_Click(object sender, RoutedEventArgs e) {
			ItemSurveyResult.Instance.SetComment("skipped");

			PageCallback pageCallback = new PageCallback();
			NavigationService.Navigate(pageCallback);
		}

		private void ButtonNext_Click(object sender, RoutedEventArgs e) {
			ItemSurveyResult.Instance.SetComment(TextBoxComment.Text);

			PageCallback pageCallback = new PageCallback();
			NavigationService.Navigate(pageCallback);
		}


		private void ButtonClear_Click(object sender, RoutedEventArgs e) {
			TextBoxComment.Text = string.Empty;
		}
	}
}
