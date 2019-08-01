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
    public partial class PageSelectSurvey : Page {
        public PageSelectSurvey() {
            InitializeComponent();
			MainWindow.Instance.SetupPage(this, buttonHome: ButtonHome);

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("Оценить нашу работу");
			};

			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonRateClinic, ButtonRateDoc, ButtonRateRegistry }, 50);
		}

		private void ButtonRateDoc_Click(object sender, RoutedEventArgs e) {
			PageSelectDepartment pageSelectDepartment = new PageSelectDepartment(PageSelectDepartment.Source.DocRate);
			NavigationService.Navigate(pageSelectDepartment);
		}

		private void ButtonRateRegistry_Click(object sender, RoutedEventArgs e) {
			PageRateRegistry pageRateRegistry = new PageRateRegistry();
			NavigationService.Navigate(pageRateRegistry);
		}

		private void ButtonRateClinic_Click(object sender, RoutedEventArgs e) {
			PageRateClinic pageRateClinic = new PageRateClinic();
			NavigationService.Navigate(pageRateClinic);
		}
	}
}
