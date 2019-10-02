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
    public partial class PageRateDoctor : Page {
		private readonly ItemDoctor doctor;

        public PageRateDoctor(ItemDoctor doctor) {
            InitializeComponent();
			this.doctor = doctor;
			MainWindow.Instance.SetupPage(this, ButtonBack, ButtonHome);

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("Оцените приём у врача", doctor.Department + " @ " + doctor.Name);
			};

			foreach (UIElement uiElement in StackPanelRates.Children)
				if (uiElement is Button button)
					MainWindow.ApplyStyleForButtons(new List<Button> { uiElement as Button });

			TextBlockDocName.Text = doctor.Name;
			TextBlockDocName.FontSize = 40;
			ImageDocPhoto.Source = ControlsFactory.ImageSourceForBitmap(
				(System.Drawing.Bitmap)ControlsFactory.GetImageForDoctor(doctor.Name));
		}

		private void ButtonRate_Click(object sender, RoutedEventArgs e) {
			Button button = sender as Button;
			Page page;

			string mark = (button.Tag ?? string.Empty).ToString();

			ItemSurveyResult.Instance.RateDoctor(doctor, mark);

			if (button.Name.Contains("Angry") || button.Name.Contains("Sad"))
				page = new PageComment();
			else
				page = new PageRateThanks();
			
			NavigationService.Navigate(page);
		}

		private void ImageDocPhoto_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
			PageDocInfo pageDocInfo = new PageDocInfo();
			NavigationService.Navigate(pageDocInfo);
		}
	}
}
