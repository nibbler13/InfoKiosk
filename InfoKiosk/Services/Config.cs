using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace InfoKiosk.Services {
	[Serializable]
	public class Config : INotifyPropertyChanged {
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			IsNeedToSave = true;
		}
		public bool IsConfigReadedSuccessfull { get; set; } = false;
		public bool IsNeedToSave { get; set; } = false;

		private string configFilePath;
		public string ConfigFilePath {
			get { return configFilePath; }
			set {
				if (value != configFilePath) {
					configFilePath = value;
					NotifyPropertyChanged();
				}
			}
		}

		#region DB
		private string misDbAddress;
		public string MisDbAddress {
			get { return misDbAddress; }
			set {
				if (value != misDbAddress) {
					misDbAddress = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string misDbName;
		private int misDbPort;
		public int MisDbPort {
			get { return misDbPort; }
			set { 
				if (value != misDbPort) {
					misDbPort = value;
					NotifyPropertyChanged();
				} 
			}
		}
		public string MisDbName {
			get { return misDbName; }
			set {
				if (value != misDbName) {
					misDbName = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string misDbUserName;
		public string MisDbUserName {
			get { return misDbUserName; }
			set {
				if (value != misDbUserName) {
					misDbUserName = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string misDbUserPassword;
		public string MisDbUserPassword {
			get { return misDbUserPassword; }
			set {
				if (value != misDbUserPassword) {
					misDbUserPassword = value;
					NotifyPropertyChanged();
				}
			}
		}
		#endregion

		#region Queries
		private string sqlGetSurveyInfo;
		public string SqlGetSurveyInfo {
			get { return sqlGetSurveyInfo; }
			set {
				if (value != sqlGetSurveyInfo) {
					sqlGetSurveyInfo = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string sqlGetScheduleInfo;
		public string SqlGetScheduleInfo {
			get { return sqlGetScheduleInfo; }
			set {
				if (value != sqlGetScheduleInfo) {
					sqlGetScheduleInfo = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string sqlGetPriceInfo;
		public string SqlGetPriceInfo {
			get { return sqlGetPriceInfo; }
			set {
				if (value != sqlGetPriceInfo) {
					sqlGetPriceInfo = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string sqlGetPriceInfoFilialID;
		public string SqlGetPriceInfoFilialID {
			get { return sqlGetPriceInfoFilialID; }
			set {
				if (value != sqlGetPriceInfoFilialID) {
					sqlGetPriceInfoFilialID = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string sqlInsertLoyaltySurveyResult;
		public string SqlInsertLoyaltySurveyResult {
			get { return sqlInsertLoyaltySurveyResult; }
			set {
				if (value != sqlInsertLoyaltySurveyResult) {
					sqlInsertLoyaltySurveyResult = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string sqlInsertServicePriority;
		public string SqlInsertServicePriority {
			get {
				return sqlInsertServicePriority;
			} 
			set {
				if (value != sqlInsertServicePriority) {
					sqlInsertServicePriority = value;
					NotifyPropertyChanged();
				}
			}
		}
		#endregion

		#region Mail
		private string mailSmtpServer;
		public string MailSmtpServer {
			get { return mailSmtpServer; }
			set {
				if (value != mailSmtpServer) {
					mailSmtpServer = value;
					NotifyPropertyChanged();
				}
			}
		}
		private uint mailSmtpPort;
		public uint MailSmtpPort {
			get { return mailSmtpPort; }
			set {
				if (value != mailSmtpPort) {
					mailSmtpPort = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string mailUser;
		public string MailUser {
			get { return mailUser; }
			set {
				if (value != mailUser) {
					mailUser = value;
					NotifyPropertyChanged();
				}
			}
		}
		private bool mailEnableSSL;
		public bool MailEnableSSL {
			get { return mailEnableSSL; }
			set {
				if (value != mailEnableSSL) {
					mailEnableSSL = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string mailPassword;
		public string MailPassword {
			get { return mailPassword; }
			set {
				if (value != mailPassword) {
					mailPassword = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string mailUserDomain;
		public string MailUserDomain {
			get { return mailUserDomain; }
			set {
				if (value != mailUserDomain) {
					mailUserDomain = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string mailClinicName;
		public string MailClinicName {
			get { return mailClinicName; }
			set {
				if (value != mailClinicName) {
					mailClinicName = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string mailAdminAddress;
		public string MailAdminAddress {
			get { return mailAdminAddress; }
			set {
				if (value != mailAdminAddress) {
					mailAdminAddress = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string mailRecipientsNegativeMarksDoctor;
		public string MailRecipientsNegativeMarksDoctor {
			get { return mailRecipientsNegativeMarksDoctor; }
			set {
				if (value != mailRecipientsNegativeMarksDoctor) {
					mailRecipientsNegativeMarksDoctor = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string mailRecipientsNegativeMarksClinic;
		public string MailRecipientsNegativeMarksClinic {
			get { return mailRecipientsNegativeMarksClinic; }
			set {
				if (value != mailRecipientsNegativeMarksClinic) {
					mailRecipientsNegativeMarksClinic = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string mailRecipientsNegativeMarksRegistry;
		public string MailRecipientsNegativeMarksRegistry {
			get { return mailRecipientsNegativeMarksRegistry; }
			set {
				if (value != mailRecipientsNegativeMarksRegistry) {
					mailRecipientsNegativeMarksRegistry = value;
					NotifyPropertyChanged();
				}
			}
		}
		private bool shouldSendMail;
		public bool ShouldSendMail {
			get { return shouldSendMail; }
			set {
				if (value != shouldSendMail) {
					shouldSendMail = value;
					NotifyPropertyChanged();
				}
			}
		}
		private bool shouldAddAdminToCopy;
		public bool ShouldAddAdminToCopy {
			get { return shouldAddAdminToCopy; }
			set {
				if (value != shouldAddAdminToCopy) {
					shouldAddAdminToCopy = value;
					NotifyPropertyChanged();
				}
			}
		}
		#endregion

		#region General
		private uint maxLogfilesQuantity;
		public uint MaxLogfilesQuantity {
			get { return maxLogfilesQuantity; }
			set {
				if (value != maxLogfilesQuantity) {
					maxLogfilesQuantity = value;
					NotifyPropertyChanged();
				}
			}
		}
		public uint AutoCloseTImerInfoPageMultiplier { get; set; }
		private uint autoCloseTimerIntervalInSeconds;
		public uint AutoCloseTimerIntervalInSeconds {
			get { return autoCloseTimerIntervalInSeconds; }
			set {
				if (value != autoCloseTimerIntervalInSeconds) {
					autoCloseTimerIntervalInSeconds = value;
					NotifyPropertyChanged();
				}
			}
		}
		private string pathWebCamSaveTo;
		public string PathWebCamSaveTo {
			get { return pathWebCamSaveTo; }
			set {
				if (value != pathWebCamSaveTo) {
					pathWebCamSaveTo = value;
					NotifyPropertyChanged();
				}
			}
		}
		private bool isDebug;
		public bool IsDebug {
			get { return isDebug; }
			set {
				if (value != isDebug) {
					isDebug = value;
					NotifyPropertyChanged();
				}
			}
		}
		#endregion

		#region Visual
		public Color ColorButtonBackground { get => colorButtonBackground; set => colorButtonBackground = value; }
		public Color ColorDisabled { get => colorDisabled; set => colorDisabled = value; }
		public Color ColorLabelForeground { get => colorLabelForeground; set => colorLabelForeground = value; }
		#endregion



		private static Config instance = null;
		private static readonly object padlock = new object();

		public static Config Instance {
			get {
				lock (padlock) {
					if (instance == null)
						instance = LoadConfiguration();

					return instance;
				}
			}
		}

		[NonSerialized()] private ICommand buttonClick;
		[NonSerialized()] private Color colorButtonBackground;
		[NonSerialized()] private Color colorDisabled;
		[NonSerialized()] private Color colorLabelForeground;

		[IgnoreDataMember]
		public ICommand ButtonClick {
			get {
				return buttonClick ??
					(buttonClick = new CommandHandler((object parameter) =>
					Action(parameter)));
			}
		}

		public void Action(object parameter) {
			if (parameter == null)
				return;

			string param = parameter.ToString();

			if (param.Equals("SelectPathWebCamSaveTo")) {
				SelectPathWebCamSaveTo();
			} else if (param.Equals("ClearPathWebCamSaveTo")) {
				PathWebCamSaveTo = string.Empty;
			} else if (param.Equals("ResetToDefaults")) {
				ResetConfiguration();
			} else if (param.Equals("SaveConfig")) {
				SaveConfiguration();
			} else if (param.Equals("CheckDbConnection")) {
				CheckDbConnection();
			} else if (param.Equals("CheckMailSettings")) {
				CheckMailServer();
			}
		}

		private async void CheckMailServer() {
			List<string> emptyStrings = new List<string>();

			if (string.IsNullOrEmpty(MailSmtpServer))
				emptyStrings.Add("Адрес SMTP-сервера");
			if (MailSmtpPort == 0)
				emptyStrings.Add("Порт");
			if (string.IsNullOrEmpty(MailUser))
				emptyStrings.Add("Имя пользователя");
			if (string.IsNullOrEmpty(MailPassword))
				emptyStrings.Add("Пароль");

			if (emptyStrings.Count > 0) {
				string msg = "Для проверки подключения к почтовому серверу необходимо заполнить:" + Environment.NewLine +
					string.Join(Environment.NewLine, emptyStrings);
				MessageBox.Show(msg, string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			if (string.IsNullOrEmpty(MailAdminAddress)) {
				MessageBox.Show("Необходимо задать как минимум один адрес в списке 'Получатели системных уведомлений'",
					string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			try {
				await Task.Run(() => {
					ClientMail.SendTestMail("Проверка подключения", "Проверка подключения", MailAdminAddress);
					MessageBox.Show("Проверка подключения: Успешно", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
				}).ConfigureAwait(false);
			} catch (Exception e) {
				MessageBox.Show("Проверка подключения: Ошибка" +
					Environment.NewLine + e.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private async void CheckDbConnection() {
			List<string> emptyStrings = new List<string>();

			if (string.IsNullOrEmpty(MisDbAddress))
				emptyStrings.Add("Адрес БД МИС Инфоклиника");
			if (string.IsNullOrEmpty(MisDbName))
				emptyStrings.Add("Имя базы");
			if (string.IsNullOrEmpty(MisDbUserName))
				emptyStrings.Add("Имя пользователя");
			if (string.IsNullOrEmpty(MisDbUserPassword))
				emptyStrings.Add("Пароль");

			if (emptyStrings.Count > 0) {
				string msg = "Для проверки подключения к БД необходимо заполнить:" + Environment.NewLine +
					string.Join(Environment.NewLine, emptyStrings);
				MessageBox.Show(msg, string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			await Task.Run(() => {
				using (FirebirdClient firebirdClient = new FirebirdClient(
					MisDbAddress,
					MisDbPort,
					MisDbName,
					MisDbUserName,
					MisDbUserPassword)) {
					DataTable dt = firebirdClient.GetDataTable("select date 'Now' from rdb$database");

					string msg = "Подключение к БД: ";
					MessageBoxImage image;
					if (dt.Rows.Count == 1) {
						msg += "Успешно";
						image = MessageBoxImage.Information;
					} else {
						msg += "Ошибка";
						image = MessageBoxImage.Error;
					}

					MessageBox.Show(msg, string.Empty, MessageBoxButton.OK, image);
				};
			}).ConfigureAwait(false);
		}

		private void ResetConfiguration() {
			MessageBoxResult result = MessageBox.Show(
				"Вы уверены, что хотите сбросить значения текущей конфигурации?",
				string.Empty,
				MessageBoxButton.YesNo,
				MessageBoxImage.Question);

			if (result == MessageBoxResult.No)
				return;

			LoadDefaults();
		}

		private void SelectPathWebCamSaveTo() {
			using (CommonOpenFileDialog dlg = new CommonOpenFileDialog {
				Title = "Выбор папки для сохранения снимков с веб-камеры",
				IsFolderPicker = true,
				InitialDirectory = Directory.GetCurrentDirectory(),
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				DefaultDirectory = Directory.GetCurrentDirectory(),
				EnsureFileExists = true,
				EnsurePathExists = true,
				EnsureReadOnly = false,
				EnsureValidNames = true,
				Multiselect = false,
				ShowPlacesList = true
			}) {
				if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
					PathWebCamSaveTo = dlg.FileName;
			};
		}

		private static Config LoadConfiguration() {
			Config configuration = new Config();
			Logging.ToLog("Configuration - Считывание файла настроек: " + configuration.configFilePath);

			if (!File.Exists(configuration.configFilePath)) {
				Logging.ToLog("Configuration - !!! Не удается найти файл");
				return configuration;
			}

			try {

				byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8 };
				byte[] iv = { 1, 2, 3, 4, 5, 6, 7, 8 };
				using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
				using (var fs = new FileStream(configuration.ConfigFilePath, FileMode.Open, FileAccess.Read))
				using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(key, iv), CryptoStreamMode.Read)) {
					BinaryFormatter formatter = new BinaryFormatter();
					configuration = (Config)formatter.Deserialize(cryptoStream);
					configuration.IsConfigReadedSuccessfull = true;
					configuration.ColorButtonBackground = (Color)ColorConverter.ConvertFromString("#FFF0F0F0");
					configuration.ColorDisabled = (Color)ColorConverter.ConvertFromString("#FFF0F0F0");
					configuration.ColorLabelForeground = (Color)ColorConverter.ConvertFromString("#FF1E1E1E");
				};
			} catch (Exception e) {
				Logging.ToLog("Configuration - !!! " + e.Message + Environment.NewLine + e.StackTrace);
			}

			configuration.IsNeedToSave = false;

			return configuration;
		}

		public static bool SaveConfiguration() {
			byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8 };
			byte[] iv = { 1, 2, 3, 4, 5, 6, 7, 8 };
			try {
				using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
				using (var fs = new FileStream(Instance.ConfigFilePath, FileMode.Create, FileAccess.Write))
				using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(key, iv), CryptoStreamMode.Write)) {
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(cryptoStream, Instance);
				};

				Instance.IsNeedToSave = false;
				MessageBox.Show("Изменения сохранены", string.Empty,
					MessageBoxButton.OK, MessageBoxImage.Information);

				return true;
			} catch (Exception e) {
				Logging.ToLog(e.Message + Environment.NewLine + e.StackTrace);
				MessageBox.Show("Не удалось сохранить конфигурацию: " + e.Message,
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

				return false;
			}
		}

		private Config() {
			ConfigFilePath = Logging.AssemblyDirectory + "InfoKioskConfig.obj";
			LoadDefaults();
		}

		public static string[] GetSplittedAddresses(string addresses) {
			if (addresses == null)
				throw new ArgumentNullException(nameof(addresses));

			return addresses.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
		}

		private void LoadDefaults() {
			#region DB
			MisDbAddress = "172.16.225.2";
			MisDbPort = 3050;
			MisDbName = "mssu";
			MisDbUserName = "sysdba";
			MisDbUserPassword = "masterkey";
			#endregion

			#region Queries
			SqlGetSurveyInfo = "SELECT distinct q1.* FROM " + Environment.NewLine +
				"(SELECT distinct dp.depname AS department, " + Environment.NewLine +
				"dp.depnum AS depnum, " + Environment.NewLine +
				"d.fullname AS docname, " + Environment.NewLine +
				"d.dcode AS dcode, " + Environment.NewLine +
				"d.doctpost AS docposition, " + Environment.NewLine +
				"r.RAZDNAME AS sectionname " + Environment.NewLine +
				"FROM doctshedule doc " + Environment.NewLine +
				"LEFT JOIN doctor d on doc.dcode = d.dcode and doc.daytype = 1 " + Environment.NewLine +
				"LEFT JOIN departments dp on d.depnum = dp.depnum " + Environment.NewLine +
				"LEFT JOIN razdel r on dp.RAZDEL = r.RAZDID " + Environment.NewLine +
				"where doc.WDATE between dateadd(-30 day to current_date) AND " + Environment.NewLine +
				"'today' and doc.schedlocked = 0 AND " + Environment.NewLine +
				"coalesce(d.locked, 0) = 0  AND " + Environment.NewLine +
				"d.viewinsched != 0) q1 " + Environment.NewLine +
				" JOIN treat t on t.dcode = q1.dcode and " + Environment.NewLine +
				"t.treatdate between dateadd(-30 day to current_date) and 'today'";

			SqlGetScheduleInfo = "-- ver 1.0 " + Environment.NewLine +
				"select depname, fullname, d0, d1, d2, d3, d4, d5, d6 " + Environment.NewLine +
				"from " + Environment.NewLine +
				"( " + Environment.NewLine +
				"select " + Environment.NewLine +
				"	upper(dp.depname) depname " + Environment.NewLine +
				"  , d.fullname " + Environment.NewLine +
				"  , ds.depnum " + Environment.NewLine +
				"  , ds.dcode " + Environment.NewLine +
				"  , (select lpad(dateadd(minute, min(ds0.beghour * 60 + ds0.begmin), cast('00:00' as time)), 5) || ' - ' || lpad(dateadd(minute, max(ds0.endhour * 60 + ds0.endmin), cast('00:00' as time)), 5) from doctshedule ds0 " + Environment.NewLine +
				"	 where ds0.depnum = ds.depnum and ds0.dcode = ds.dcode and ds0.wdate = dateadd(day, 0, current_date)) d0 " + Environment.NewLine +
				"  , (select lpad(dateadd(minute, min(ds0.beghour * 60 + ds0.begmin), cast('00:00' as time)), 5) || ' - ' || lpad(dateadd(minute, max(ds0.endhour * 60 + ds0.endmin), cast('00:00' as time)), 5) from doctshedule ds0 " + Environment.NewLine +
				"	 where ds0.depnum = ds.depnum and ds0.dcode = ds.dcode and ds0.wdate = dateadd(day, 1, current_date)) d1 " + Environment.NewLine +
				"  , (select lpad(dateadd(minute, min(ds0.beghour * 60 + ds0.begmin), cast('00:00' as time)), 5) || ' - ' || lpad(dateadd(minute, max(ds0.endhour * 60 + ds0.endmin), cast('00:00' as time)), 5) from doctshedule ds0 " + Environment.NewLine +
				"	 where ds0.depnum = ds.depnum and ds0.dcode = ds.dcode and ds0.wdate = dateadd(day, 2, current_date)) d2 " + Environment.NewLine +
				"  , (select lpad(dateadd(minute, min(ds0.beghour * 60 + ds0.begmin), cast('00:00' as time)), 5) || ' - ' || lpad(dateadd(minute, max(ds0.endhour * 60 + ds0.endmin), cast('00:00' as time)), 5) from doctshedule ds0 " + Environment.NewLine +
				"	 where ds0.depnum = ds.depnum and ds0.dcode = ds.dcode and ds0.wdate = dateadd(day, 3, current_date)) d3 " + Environment.NewLine +
				"  , (select lpad(dateadd(minute, min(ds0.beghour * 60 + ds0.begmin), cast('00:00' as time)), 5) || ' - ' || lpad(dateadd(minute, max(ds0.endhour * 60 + ds0.endmin), cast('00:00' as time)), 5) from doctshedule ds0 " + Environment.NewLine +
				"	 where ds0.depnum = ds.depnum and ds0.dcode = ds.dcode and ds0.wdate = dateadd(day, 4, current_date)) d4 " + Environment.NewLine +
				"  , (select lpad(dateadd(minute, min(ds0.beghour * 60 + ds0.begmin), cast('00:00' as time)), 5) || ' - ' || lpad(dateadd(minute, max(ds0.endhour * 60 + ds0.endmin), cast('00:00' as time)), 5) from doctshedule ds0 " + Environment.NewLine +
				"	 where ds0.depnum = ds.depnum and ds0.dcode = ds.dcode and ds0.wdate = dateadd(day, 5, current_date)) d5 " + Environment.NewLine +
				"  , (select lpad(dateadd(minute, min(ds0.beghour * 60 + ds0.begmin), cast('00:00' as time)), 5) || ' - ' || lpad(dateadd(minute, max(ds0.endhour * 60 + ds0.endmin), cast('00:00' as time)), 5) from doctshedule ds0 " + Environment.NewLine +
				"	 where ds0.depnum = ds.depnum and ds0.dcode = ds.dcode and ds0.wdate = dateadd(day, 6, current_date)) d6 " + Environment.NewLine +
				"		from " + Environment.NewLine +
				"  doctshedule ds " + Environment.NewLine +
				"  , (select outdate from getdate(current_date, dateadd(day, 6, current_date))) x " + Environment.NewLine +
				"  , departments dp " + Environment.NewLine +
				"  , doctor d " + Environment.NewLine +
				"where " + Environment.NewLine +
				"  x.outdate = ds.wdate " + Environment.NewLine +
				"  and ds.depnum = dp.depnum " + Environment.NewLine +
				"  and ds.dcode = d.dcode " + Environment.NewLine +
				"  and ds.depnum is not null " + Environment.NewLine +
				"  and coalesce(ds.CHAIR,-1) <> -1 " + Environment.NewLine +
				"  -- Исключения-- " + Environment.NewLine +
				"  and ds.depnum + 0 not in " + Environment.NewLine +
				"  ( " + Environment.NewLine +
				"  991325710, --ВРАЧИ ОФИСА " + Environment.NewLine +
				"  992112048, --КАБИНЕТЫ ДРС " + Environment.NewLine +
				"  991317990, --ЛИЧНЫЙ ВРАЧ " + Environment.NewLine +
				"  991306861, --МАССАЖ НА ДОМУ " + Environment.NewLine +
				"  990386480, --ОБЩЕЕ " + Environment.NewLine +
				"  992120358, --ОБЩЕЕ ПЕДИАТРИЯ " + Environment.NewLine +
				"  991491433, --ОТДЕЛЕНИЕ ЛВ " + Environment.NewLine +
				"  990833277, --ОТДЕЛЕНИЕ ПНД " + Environment.NewLine +
				"  10029098,  --ПОМОЩЬ НА ДОМУ " + Environment.NewLine +
				"  991330975, --ПРЕДРЕЙСОВЫЙ ОСМОТР " + Environment.NewLine +
				"  821,       --ПРОЦЕДУРНЫЙ " + Environment.NewLine +
				"  991382360, --ПРОЦЕДУРНЫЙ(ДЕТСКИЙ) " + Environment.NewLine +
				"  991330843, --СКОРАЯ МЕДИЦИНСКАЯ ПОМОЩЬ " + Environment.NewLine +
				"  992092102, --ТЕЛЕМЕДИЦИНА " + Environment.NewLine +
				"  991135669--УЗКИЕ СПЕЦИАЛИСТЫ НА ДОМУ " + Environment.NewLine +
				"  ) " + Environment.NewLine +
				"group by 1,2,3,4 " + Environment.NewLine +
				")";

			SqlGetPriceInfo = "select " + Environment.NewLine +
				" s.sname as \"GROUP\", " + Environment.NewLine +
				" w.SCHID as \"SCHID\", " + Environment.NewLine +
				" pr.infokiosk_priority as \"PRIORITY\", " + Environment.NewLine +
				" w.KODOPER as \"SERVICE_CODE\", " + Environment.NewLine +
				" w.SCHNAME as \"SERVICE_NAME\", " + Environment.NewLine +
				"(select sprice from get_pricebyid(current_date, 13, @filialID, 38, 0, w.schid)) as \"COST\" " + Environment.NewLine +
				" from WSCHEMA W " + Environment.NewLine +
				" join speciality s on s.scode = w.speccode and s.scode <> 990010000000001 " + Environment.NewLine +
				" left join wschema wcap on wcap.schid = w.PARENTSCHID and wcap.ISCAPTION = 1 " + Environment.NewLine +
				" left join bz_infokiosk_services_priority pr on pr.schid = w.schid " + Environment.NewLine +
				" where " + Environment.NewLine +
				"   (W.STRUCTID = 4) and " + Environment.NewLine +
				"   (w.ISCAPTION = 0) and " + Environment.NewLine +
				"   (select sprice from get_pricebyid (current_date, 13, @filialID, 38, 0, w.schid)) > 10 " + Environment.NewLine +
				" order by s.sortorder, wcap.scode, w.scode ";

			SqlGetPriceInfoFilialID = "12";

			SqlInsertLoyaltySurveyResult = "insert into bz_loyalitysurvey ( " + Environment.NewLine +
				"dcode, docrate, commentary, phone, photo_path, depnum) " + Environment.NewLine +
				"values(@dcode, @docrate, @comment, @phonenumber, @photopath, @depnum)";

			SqlInsertServicePriority = "update or insert into bz_infokiosk_services_priority values (@schid, @priority)";
			#endregion

			#region Mail
			MailSmtpServer = "smtp.yandex.ru";
			MailSmtpPort = 25;
			MailUser = "infokiosk.askona@yandex.ru";
			MailEnableSSL = true;
			MailPassword = "!@#QWEasd";
			MailUserDomain = string.Empty;
			MailClinicName = "Сущевская";
			MailAdminAddress = "nibble@yandex.ru";
			MailRecipientsNegativeMarksDoctor = "nibble@yandex.ru";
			MailRecipientsNegativeMarksClinic = "nibble@yandex.com";
			MailRecipientsNegativeMarksRegistry = "nibble@yandex.kz";
			ShouldSendMail = true;
			ShouldAddAdminToCopy = true;
			#endregion

			#region General
			MaxLogfilesQuantity = 14;
			AutoCloseTImerInfoPageMultiplier = 2;
			AutoCloseTimerIntervalInSeconds = 30;
			PathWebCamSaveTo = string.Empty;
			IsDebug = true;
			#endregion

			#region Visual
			ColorButtonBackground = (Color)ColorConverter.ConvertFromString("#FFF0F0F0");
			ColorDisabled = (Color)ColorConverter.ConvertFromString("#FFF0F0F0");
			ColorLabelForeground = (Color)ColorConverter.ConvertFromString("#FF1E1E1E");
			#endregion
		}
	}
}
