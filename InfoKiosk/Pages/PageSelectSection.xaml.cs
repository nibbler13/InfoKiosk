using System;
using System.Collections.Generic;
using System.IO;
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

namespace InfoKiosk {
	/// <summary>
	/// Логика взаимодействия для PageSelectSection.xaml
	/// </summary>
	public partial class PageSelectSection : Page {
		public PageSelectSection() {
			InitializeComponent();

			MainWindow.Instance.SetupPage(this);
			KeepAlive = true;

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("", "Выберите интересующий Вас раздел");
				string movieFile = Path.Combine(Directory.GetCurrentDirectory(), "Media\\Final Comp_1 (1).mp4");
				//MediaElementMain.Source = new Uri(movieFile);
				//MediaElementMain.Play();
			};

			MainWindow.ApplyStyleForButtons(new List<Button> { ButtonRateUs, ButtonSchedule, ButtonServicesAndPrice }, 50);
		}

		private void ButtonSchedule_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
			PageSelectDepartment pageSelectDepartment = new PageSelectDepartment(PageSelectDepartment.Source.Timetable);
			NavigationService.Navigate(pageSelectDepartment);
		}

		private void ButtonServicesAndPrice_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
			PageSelectDepartment pageSelectDepartment = new PageSelectDepartment(PageSelectDepartment.Source.Price);
			NavigationService.Navigate(pageSelectDepartment);
		}

		private void ButtonSpecialOffers_PreviewMouseDown(object sender, MouseButtonEventArgs e) {

		}

		private void ButtonPartnersOffering_PreviewMouseDown(object sender, MouseButtonEventArgs e) {

		}

		private void ButtonRateUs_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
			PageSelectSurvey pageSurveySelect = new PageSelectSurvey();
			NavigationService.Navigate(pageSurveySelect);
		}

		private void ButtonClinicNavigation_PreviewMouseDown(object sender, MouseButtonEventArgs e) {

		}

		private void MediaElementMain_MediaEnded(object sender, RoutedEventArgs e) {
			(sender as MediaElement).Position = new TimeSpan(0, 0, 1);
			(sender as MediaElement).Play();
		}
	}
}
