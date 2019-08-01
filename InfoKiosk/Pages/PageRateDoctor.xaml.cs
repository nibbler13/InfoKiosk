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
        public PageRateDoctor(string name) {
            InitializeComponent();
			MainWindow.Instance.SetupPage(this, ButtonBack, ButtonHome);

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("Оцените приём у врача");
			};

			foreach (UIElement uiElement in StackPanelRates.Children)
				if (uiElement is Button button)
					MainWindow.ApplyStyleForButtons(new List<Button> { uiElement as Button });

			TextBlockDocName.Text = name;
			TextBlockDocName.FontSize = 40;
			ImageDocPhoto.Source = ControlsFactory.ImageSourceForBitmap((System.Drawing.Bitmap)ControlsFactory.GetImageForDoctor(name));
		}

		private void ButtonRate_Click(object sender, RoutedEventArgs e) {
			Button button = sender as Button;
			Page page;

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
