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
using System.Windows.Shapes;

namespace InfoKioskConfigManager {
	/// <summary>
	/// Interaction logic for WindowSqlQueryView.xaml
	/// </summary>
	public partial class WindowSqlQueryView : Window {
		public enum Type {
			DoctorRate,
			Schedule,
			Services,
			InsertSurveyResult
		}

		public WindowSqlQueryView(Type type, string query, string filialID = "") {
			InitializeComponent();

			if (type == Type.Services)
				GridFilialID.Visibility = Visibility.Visible;

			TextBoxQuery.Text = query;
			TextBoxFilialID.Text = filialID;

			Closed += (s, e) => {
				string queryEntered = TextBoxQuery.Text;

				switch (type) {
					case Type.DoctorRate:
						InfoKiosk.Services.Configuration.Instance.SqlGetSurveyInfo = queryEntered;
						break;
					case Type.Schedule:
						InfoKiosk.Services.Configuration.Instance.SqlGetScheduleInfo = queryEntered;
						break;
					case Type.Services:
						InfoKiosk.Services.Configuration.Instance.SqlGetPriceInfo = queryEntered;
						InfoKiosk.Services.Configuration.Instance.SqlGetPriceInfoFilialID = TextBoxFilialID.Text;
						break;
					case Type.InsertSurveyResult:
						InfoKiosk.Services.Configuration.Instance.SqlInsertLoyaltySurveyResult = queryEntered;
						break;
					default:
						break;
				}
			};

			Loaded += (s, e) => {
				using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("InfoKioskConfigManager.sql.xshd")) {
					using (var reader = new System.Xml.XmlTextReader(stream)) {
						TextBoxQuery.SyntaxHighlighting =
							ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
							ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
					}
				}
			};
		}

		private void ButtonClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
