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
	/// <summary>
	/// Логика взаимодействия для PageFeedback.xaml
	/// </summary>
	public partial class PageFeedback : Page {
		private OnscreenKeyboard onscreenKeyboard;
		private TextBox previosFocusedTextBox;

		public PageFeedback() {
			InitializeComponent();
			MainWindow.Instance.SetupPage(this, ButtonBack, ButtonHome);
			
			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("Обратная связь");

				onscreenKeyboard = new OnscreenKeyboard(ActualWidth, BorderKeyboard.ActualHeight, 0, 0, 9, 30, OnscreenKeyboard.KeyboardType.Full);
				Canvas canvasKeyboard = onscreenKeyboard.CreateOnscreenKeyboard();
				canvasKeyboard.HorizontalAlignment = HorizontalAlignment.Stretch;
				canvasKeyboard.VerticalAlignment = VerticalAlignment.Center;
				Grid.SetRow(canvasKeyboard, 8);
				Grid.SetColumnSpan(canvasKeyboard, 5);
				GridMain.Children.Add(canvasKeyboard);
				onscreenKeyboard.SetTextBoxInput(TextBoxName);

				TextBoxName.Focus();
			};

			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonSend });
		}

		private void ButtonSend_Click(object sender, RoutedEventArgs e) {
			PageFeedbackThanks pageFeedbackThanks = new PageFeedbackThanks();
			NavigationService.Navigate(pageFeedbackThanks);
		}

		private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
			if (previosFocusedTextBox != null)
				previosFocusedTextBox.Background = Brushes.White;

			TextBox textBox = sender as TextBox;
			textBox.Background = Brushes.Beige;
			previosFocusedTextBox = textBox;

			onscreenKeyboard.SetTextBoxInput(textBox);
		}
	}
}
