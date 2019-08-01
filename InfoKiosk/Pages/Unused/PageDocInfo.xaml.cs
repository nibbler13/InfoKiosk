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
	public partial class PageDocInfo : Page {
        public PageDocInfo() {
            InitializeComponent();
			MainWindow.Instance.SetupPage(this, ButtonBack, ButtonHome);

			Loaded += (s, e) => {
				(Application.Current.MainWindow as MainWindow).SetupTitle("Информация о враче");
			};

			TextBlockInfo.FontSize = 30;
		}
	}
}
