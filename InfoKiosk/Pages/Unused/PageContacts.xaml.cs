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
	/// Логика взаимодействия для PageContacts.xaml
	/// </summary>
	public partial class PageContacts : Page {
		public PageContacts() {
			InitializeComponent();
			MainWindow.Instance.SetupPage(this);

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("Контакты");
			};

			TextBlockLeftPart.FontSize = 30;
			TextBlockRightPart.FontSize = 30;
			ButtonBack.FontSize = 30;
			ButtonBack.Style = Application.Current.MainWindow.FindResource("RoundCorner") as Style;
			ButtonFeedback.FontSize = 30;
			ButtonFeedback.Style = Application.Current.MainWindow.FindResource("RoundCorner") as Style;
		}

		private void ButtonBack_Click(object sender, RoutedEventArgs e) {
			if (NavigationService.CanGoBack)
				NavigationService.GoBack();
		}

		private void ButtonFeedback_Click(object sender, RoutedEventArgs e) {
			PageFeedback pageFeedback = new PageFeedback();
			NavigationService.Navigate(pageFeedback);
		}
	}
}
