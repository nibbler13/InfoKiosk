using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	public partial class PageCallback : Page {
		private OnscreenKeyboard onscreenKeyboard;
		private readonly string mask = "+7 (___) ___-__-__";
		private readonly TextBox textBoxData;

		public PageCallback() {
            InitializeComponent();
			MainWindow.Instance.SetupPage(this, ButtonBack);

			textBoxData = new TextBox();
			textBoxData.TextChanged += TextBoxData_TextChanged;

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle(string.Empty, "Можем ли мы перезвонить Вам?");

				onscreenKeyboard = new OnscreenKeyboard(ActualWidth, BorderKeyboard.ActualHeight, 0, 0, 9, 30, OnscreenKeyboard.KeyboardType.Number);
				Canvas canvasKeyboard = onscreenKeyboard.CreateOnscreenKeyboard();
				canvasKeyboard.HorizontalAlignment = HorizontalAlignment.Stretch;
				canvasKeyboard.VerticalAlignment = VerticalAlignment.Center;
				Grid.SetRow(canvasKeyboard, 2);
				Grid.SetColumn(canvasKeyboard, 1);
				GridPhoneNumber.Children.Add(canvasKeyboard);
				onscreenKeyboard.SetTextBoxInput(textBoxData);

				TextBoxPhoneNumber.Focus();
			};

			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonNext, ButtonClear });
			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonNo, ButtonYes }, 50);
		}

		private void TextBoxData_TextChanged(object sender, TextChangedEventArgs e) {
			string enteredText = textBoxData.Text;

			if (enteredText.Length > 10) {
				enteredText = enteredText.Substring(0, 10);
				textBoxData.Text = enteredText;
			}

			string updatedMask = mask;
			Regex regex = new Regex(Regex.Escape("_"));

			foreach (char c in enteredText)
				updatedMask = regex.Replace(updatedMask, c.ToString(), 1);

			TextBoxPhoneNumber.Text = updatedMask;
		}

		private void ButtonYes_Click(object sender, RoutedEventArgs e) {
			GridQuestion.Visibility = Visibility.Hidden;
			GridPhoneNumber.Visibility = Visibility.Visible;
			ButtonNext.Visibility = Visibility.Visible;
		}

		private void ButtonNo_Click(object sender, RoutedEventArgs e) {
			ItemSurveyResult.Instance.SetPhoneNumber("skipped");

			PageRateThanks pageRateThanks = new PageRateThanks();
			NavigationService.Navigate(pageRateThanks);
		}

		private void ButtonNext_Click(object sender, RoutedEventArgs e) {
			string phoneNumber = string.Empty;
			foreach (char item in TextBoxPhoneNumber.Text.Replace("+7", "")) 
				if (char.IsDigit(item))
					phoneNumber += item;
			
			ItemSurveyResult.Instance.SetPhoneNumber(phoneNumber);

			PageRateThanks pageRateThanks = new PageRateThanks();
			NavigationService.Navigate(pageRateThanks);
		}


		private void ButtonClear_Click(object sender, RoutedEventArgs e) {
			textBoxData.Clear();
		}
	}
}
