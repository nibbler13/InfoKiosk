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
	public partial class PageRateClinic : Page {
        public PageRateClinic() {
            InitializeComponent();
			MainWindow.Instance.SetupPage(this, ButtonBack, ButtonHome);

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("Оцените клинику");
			};

			foreach (UIElement uiElement in StackPanelRates.Children)
				if (uiElement is Button button)
					MainWindow.ApplyStyleForButtons(new List<Button> { uiElement as Button }, 50);
		}

		private void ButtonRate_Click(object sender, RoutedEventArgs e) {
			Button button = sender as Button;

			Page page;
			if (button.Name.Equals("Button0"))
				page = new PageComment();
			else
				page = new PageRateThanks();

			NavigationService.Navigate(page);
		}
	}
}
