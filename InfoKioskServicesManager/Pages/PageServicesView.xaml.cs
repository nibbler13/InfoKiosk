using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace InfoKioskServicesManager.Pages {
	/// <summary>
	/// Interaction logic for PageServicesView.xaml
	/// </summary>
	public partial class PageServicesView : Page {
		public ObservableCollection<InfoKiosk.ItemService> Services { get; set; } 
			= new ObservableCollection<InfoKiosk.ItemService>();
		public ICollectionView ServicesView { get; set; }
		private string department;

		public PageServicesView(string department) {
			InitializeComponent();

			this.department = department;

			TextBlockSpecTitle.Text += department;
			DataGridServices.DataContext = this;
			InfoKiosk.Services.DataProvider.Services[department].ForEach(Services.Add);
			ServicesView = CollectionViewSource.GetDefaultView(Services);
			DataGridServices.Sorting += DataGridServices_Sorting;
			DataGridServices.LoadingRow += (s, e) => {
				e.Row.Header = (e.Row.GetIndex() + 1).ToString();
			};

			TextBlockHint.Text = "На одну страницу с услугами помещается максимум 10 услуг. " +
				"Приоритет можно задать числом, он может совпадать для нескольких услуг. Терминал сперва отсортирует список " +
				"услуг по алфавиту, далее по заданному приоритету. На первой странице будут отображены услуги, у которых приоритет ниже, " +
				"т.е. \"1, 2, 3...\", далее услуги без приоритета.";

			IsVisibleChanged += (s, e) => {
				if (!(bool)e.NewValue) {
					Application.Current.MainWindow.Closing -= MainWindow_Closing;
					(Application.Current.MainWindow as MainWindow).FrameMain.Navigating -= NavigationService_Navigating;
				}
			};

			Loaded += (s, e) => {
				Application.Current.MainWindow.Closing += MainWindow_Closing;
				(Application.Current.MainWindow as MainWindow).FrameMain.Navigating += NavigationService_Navigating;
				DataGridServices_Sorting(null, null);
			};
		}

		private void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e) {
			CheckUnsavedChanges(e);
		}

		private void MainWindow_Closing(object sender, CancelEventArgs e) {
			CheckUnsavedChanges(cancel: e);
		}

		private void CheckUnsavedChanges(NavigatingCancelEventArgs navCancel = null, CancelEventArgs cancel = null) {
			if (Services.Where(x => x.HasChanged).Count() == 0)
				return;

			MessageBoxResult result = MessageBox.Show(
				Application.Current.MainWindow,
				"Имеются несохраненные изменения. Сохранить их перед выходом?",
				"",
				MessageBoxButton.YesNoCancel,
				MessageBoxImage.Question);

			if (result == MessageBoxResult.No) {
				foreach (InfoKiosk.ItemService item in Services) 
					if (item.HasChanged) 
						item.RestorePriority();

				return;
			} else if (result == MessageBoxResult.Cancel) {
				if (navCancel != null)
					navCancel.Cancel = true;
				if (cancel != null)
					cancel.Cancel = true;

				return;
			}

			SaveChanges();
		}

		private void SaveChanges() {
			bool result = false;
			string message = string.Empty;
			IsEnabled = false;
			Cursor = Cursors.Wait;

			result = InfoKiosk.Services.DataProvider.WriteItemServiceToDb(department, out message);

			IsEnabled = true;
			Cursor = Cursors.Arrow;

			if (result)
				MessageBox.Show(Application.Current.MainWindow, message, "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
			else
				MessageBox.Show(Application.Current.MainWindow, message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void DataGridServices_Sorting(object sender, DataGridSortingEventArgs e) {
			IComparer comparer = null;

			if (e == null) {
				comparer = new ResultSort(ListSortDirection.Ascending, "LikeTerminal");
				(ServicesView as ListCollectionView).CustomSort = comparer;
				return;
			} 

			DataGridColumn column = e.Column;

			//i do some custom checking based on column to get the right comparer
			//i have different comparers for different columns. I also handle the sort direction
			//in my comparer

			// prevent the built-in sort from sorting
			e.Handled = true;

			ListSortDirection direction = (column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;

			//set the sort order on the column
			column.SortDirection = direction;

			//use a ListCollectionView to do the sort.
			//ListCollectionView lcv = (ListCollectionView)CollectionViewSource.GetDefaultView(this.ItemsSource);

			//this is my custom sorter it just derives from IComparer and has a few properties
			//you could just apply the comparer but i needed to do a few extra bits and pieces
			comparer = new ResultSort(direction, column.Header as string);

			//apply the sort
			(ServicesView as ListCollectionView).CustomSort = comparer;
		}

		private class ResultSort : IComparer {
			private ListSortDirection direction;
			private string columnHeader;
			public ResultSort(ListSortDirection direction, string columnHeader) {
				this.direction = direction;
				this.columnHeader = columnHeader;
			}
			public int Compare(object x, object y) {
				bool isNumbers = false;
				object o1;
				object o2;

				InfoKiosk.ItemService itemService1 = x as InfoKiosk.ItemService;
				InfoKiosk.ItemService itemService2 = y as InfoKiosk.ItemService;

				if (columnHeader.Equals("Код услуги")) {
					o1 = itemService1.Kodoper;
					o2 = itemService2.Kodoper;
				} else if (columnHeader.Equals("Наименование услуги")) {
					o1 = itemService1.Name;
					o2 = itemService2.Name;
				} else if (columnHeader.Equals("Стоимость")) {
					o1 = itemService1.Cost;
					o2 = itemService2.Cost;
					isNumbers = true;
				} else if (columnHeader.Equals("LikeTerminal")) {
					o1 = itemService1.PriorityToSortInTerminal;
					o2 = itemService2.PriorityToSortInTerminal;
				} else {
					o1 = itemService1.PriorityToSort;
					o2 = itemService2.PriorityToSort;

					isNumbers = true;
				}

				if (direction == ListSortDirection.Ascending) {
					if (isNumbers) {
						return ((double)o1).CompareTo((double)o2);
					} else {
						return (o1 as string).CompareTo(o2 as string);
					}
				} else {
					if (isNumbers) {
						return ((double)o2).CompareTo((double)o1);
					} else {
						return (o2 as string).CompareTo(o1 as string);
					}
				}
			}
		}

		private void ButtonBack_Click(object sender, RoutedEventArgs e) {
			if (NavigationService.CanGoBack)
				NavigationService.GoBack();
		}

		private void ButtonSave_Click(object sender, RoutedEventArgs e) {
			SaveChanges();
		}

		private void ButtonSort_Click(object sender, RoutedEventArgs e) {
			CheckboxFilterKodoper.IsChecked = false;
			CheckboxFilterName.IsChecked = false;

			DataGridServices_Sorting(null, null);
		}

		private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e) {
			SetUpFilter();
		}

		private void CheckboxFilterKodoper_Checked(object sender, RoutedEventArgs e) {
			if (CheckboxFilterKodoper.IsChecked.HasValue && CheckboxFilterKodoper.IsChecked.Value)
				CheckboxFilterName.IsChecked = false;

			SetUpFilter();
		}

		private void CheckboxFilterName_Checked(object sender, RoutedEventArgs e) {
			if (CheckboxFilterName.IsChecked.HasValue && CheckboxFilterName.IsChecked.Value)
				CheckboxFilterKodoper.IsChecked = false;

			SetUpFilter();
		}

		private void TextBoxFilterKodoper_GotFocus(object sender, RoutedEventArgs e) {
			CheckboxFilterKodoper.IsChecked = true;
			SetUpFilter();
		}

		private void TextBoxFilterName_GotFocus(object sender, RoutedEventArgs e) {
			CheckboxFilterName.IsChecked = true;
			SetUpFilter();
		}

		private void SetUpFilter() {
			if (!IsLoaded)
				return;

			if (CheckboxFilterKodoper.IsChecked.HasValue && CheckboxFilterKodoper.IsChecked.Value) {
				if (string.IsNullOrEmpty(TextBoxFilterKodoper.Text))
					ServicesView.Filter = null;
				else
					ServicesView.Filter = w => (w as InfoKiosk.ItemService).Kodoper.ToUpper().Contains(TextBoxFilterKodoper.Text.ToUpper());
			} else if (CheckboxFilterName.IsChecked.HasValue && CheckboxFilterName.IsChecked.Value) {
				if (string.IsNullOrEmpty(TextBoxFilterName.Text))
					ServicesView.Filter = null;
				else
					ServicesView.Filter = w => (w as InfoKiosk.ItemService).Name.ToUpper().Contains(TextBoxFilterName.Text.ToUpper());
			} else
				ServicesView.Filter = null;

			ServicesView.Refresh();
		}
	}
}
