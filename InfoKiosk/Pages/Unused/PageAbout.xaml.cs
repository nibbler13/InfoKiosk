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
	/// Логика взаимодействия для PageAbout.xaml
	/// </summary>
	public partial class PageAbout : Page {
		public PageAbout() {
			InitializeComponent();
			
			GridMain.PreviewMouseDown += MainWindow.Instance.CloseAllPages;

			TextBlockAbout.Text = Properties.Resources.StringContactsLeftPart;

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("О центре");
			};

			MainWindow.Instance.SetupPage(this);
			TextBlockAbout.FontSize = 34;
		}
	}
}
