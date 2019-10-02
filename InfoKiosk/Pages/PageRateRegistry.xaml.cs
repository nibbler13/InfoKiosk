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
	public partial class PageRateRegistry : Page {
        public PageRateRegistry() {
            InitializeComponent();
			MainWindow.Instance.SetupPage(this, ButtonBack, ButtonHome);

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("Оцените работу регистратуры");
			};

			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonSad, ButtonNeutral, ButtonGood }, 40);
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			Button button = sender as Button;
			ItemSurveyResult.Instance.RateRegistry(button.Tag as string);
			Page page;

			if (button.Name.Contains("Sad"))
				page = new PageComment();
			else
				page = new PageRateThanks();

			NavigationService.Navigate(page);
		}
	}
}
