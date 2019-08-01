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
	public partial class PageRateThanks : Page {
        public PageRateThanks() {
            InitializeComponent();

			MainWindow.Instance.SetupPage(this);

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("");
			};

			GridMain.PreviewMouseDown += MainWindow.Instance.CloseAllPages;
		}
    }
}
