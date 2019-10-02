using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InfoKiosk {
	public class ItemSurveyResult {
		private static ItemSurveyResult instance = null;
		private static readonly object padlock = new object();

		public enum SurveyType { Doctor, Registry, Clinic }

		public static ItemSurveyResult Instance {
			get {
				lock (padlock) {
					if (instance == null)
						instance = new ItemSurveyResult();

					return instance;
				}
			}
		}

		public DateTime? SurveyDateTime { get; private set; }
		public string DCode { get; private set; }
		public string DocName { get; private set; }
		public string DocDepartment { get; private set; }
		public string DocDeptCode { get; private set; }
		public string Mark { get; private set; }
		public string PhotoLink { get; private set; }
		public string Comment { get; private set; }
		public string PhoneNumber { get; private set; }
		public SurveyType Type { get; private set; }

		public bool NeedToWriteToDb { get; private set; }

		public void ClearValues() {
			SurveyDateTime = null;
			DCode = null;
			DocName = null;
			DocDepartment = null;
			DocDeptCode = null;
			Mark = null;
			PhotoLink = null;
			Comment = null;
			PhoneNumber = null;
			NeedToWriteToDb = false;
		}

		private void Initialize() {
			SurveyDateTime = DateTime.Now;
			NeedToWriteToDb = true;
			Services.WebCam webCam = new Services.WebCam();
			PhotoLink = webCam.CaptureImageFromWebCamAndSave();
		}

		public void RateDoctor(ItemDoctor doctor, string mark) {
			Initialize();
			DCode = doctor.Code;
			DocName = doctor.Name;
			DocDepartment = doctor.Department;
			DocDeptCode = doctor.DeptCode;
			Mark = mark;
			Type = SurveyType.Doctor;
		}

		public void RateClinic(string mark) {
			Initialize();
			DCode = "-1";
			Mark = mark;
			DocName = "Клиника";
			Type = SurveyType.Clinic;
		}

		public void RateRegistry(string mark) {
			Initialize();
			DCode = "-2";
			Mark = mark;
			DocName = "Регистратура";
			Type = SurveyType.Registry;
		}

		public void SetComment(string comment) {
			Comment = comment;
		}

		public void SetPhoneNumber(string phoneNumber) {
			PhoneNumber = phoneNumber;
		}



		private static readonly List<string> previousRatesDcodes = new List<string>();
		private static DateTime previousRateTime = DateTime.Now;

		public override string ToString() {
			return base.ToString() + Environment.NewLine +
				"SurveyType: " + Type + Environment.NewLine +
				"SurveyDateTime: " + (SurveyDateTime.HasValue ? SurveyDateTime.Value.ToString() : string.Empty) + Environment.NewLine +
				"DCode: " + (DCode ?? string.Empty) + Environment.NewLine +
				"DocName: " + (DocName ?? string.Empty) + Environment.NewLine +
				"DocDepartment: " + (DocDepartment ?? string.Empty) + Environment.NewLine +
				"DocDeptCode: " + (DocDeptCode ?? string.Empty) + Environment.NewLine +
				"DocRate: " + (Mark ?? string.Empty) + Environment.NewLine +
				"PhotoLink: " + (PhotoLink ?? string.Empty) + Environment.NewLine +
				"Comment: " + (Comment ?? string.Empty) + Environment.NewLine +
				"PhoneNumber: " + (PhoneNumber ?? string.Empty);
		}



		public static async void WriteSurveyResultToDb() {
			await Task.Run(() => {
				ItemSurveyResult surveyResult = Instance;
				SendMailAboutNegativeMark(surveyResult);

				if ((DateTime.Now - previousRateTime).TotalSeconds >= 120)
					previousRatesDcodes.Clear();

				string mark = surveyResult.Mark;
				if (previousRatesDcodes.Contains(surveyResult.DCode))
					mark = "Duplicate";
				else
					previousRatesDcodes.Add(surveyResult.DCode);

				Logging.ToLog("ItemSurveyResult - Запись результата опроса в базу данных: " + surveyResult.ToString());

				using (Services.FirebirdClient fBClient = new Services.FirebirdClient(
					Services.Configuration.Instance.MisDbAddress,
					Services.Configuration.Instance.MisDbName,
					Services.Configuration.Instance.MisDbUserName,
					Services.Configuration.Instance.MisDbUserPassword)) {
					Dictionary<string, object> surveyResults = new Dictionary<string, object>() {
						{ "@dcode", surveyResult.DCode },
						{ "@docrate", mark },
						{ "@comment", surveyResult.Comment },
						{ "@phonenumber", surveyResult.PhoneNumber },
						{ "@photopath", surveyResult.PhotoLink },
						{ "@depnum", surveyResult.DocDeptCode }
					};

					string query = Services.Configuration.Instance.SqlInsertLoyaltySurveyResult;
					Logging.ToLog("ItemSurveyResult - Результат выполнения: " + fBClient.ExecuteUpdateQuery(query, surveyResults));

					previousRateTime = surveyResult.SurveyDateTime.Value;
				}
			}).ConfigureAwait(false);
		}

		public static void SendMailAboutNegativeMark(ItemSurveyResult surveyResult) {
			if (surveyResult == null)
				return;

			string header = "";

			string recipients = string.Empty;
			string mark;
			switch (surveyResult.Type) {
				case SurveyType.Doctor:
					recipients = Services.Configuration.Instance.MailRecipientsNegativeMarksDoctor;
					break;
				case SurveyType.Registry:
					recipients = Services.Configuration.Instance.MailRecipientsNegativeMarksRegistry;
					break;
				case SurveyType.Clinic:
					recipients = Services.Configuration.Instance.MailRecipientsNegativeMarksClinic;
					break;
				default:
					break;
			}

			if (surveyResult.Mark.Equals("0"))
				mark = "Крайне не буду рекомендовать";
			else if (surveyResult.Mark.Equals("1"))
				mark = "Очень плохо";
			else if (surveyResult.Mark.Equals("2"))
				mark = "Плохо";
			else
				return;

			if (surveyResult.PhoneNumber.Length == 10 &&
				!surveyResult.PhoneNumber.Equals("skipped"))
				header = "Пациент указал, что ему можно позвонить для уточнения подробностей " +
				"о его негативной оценке.";
			else if (!string.IsNullOrEmpty(surveyResult.Comment) &&
				!string.IsNullOrWhiteSpace(surveyResult.Comment) &&
				!surveyResult.Comment.Equals("skipped"))
				header = "Пациент оставил комментарий к своей негативной оценке";

			if (string.IsNullOrEmpty(header)) {
				Logging.ToLog("ItemSurveyResult - Пропуск отправки сообщения об обратной связи - " +
					"неверный формат номера телефона и отсутствует комментарий");
				return;
			}

			string subject = Services.Configuration.Instance.MailClinicName + " - обратная связь с пациентом через монитор лояльности";
			string body =
				header + "<br><br>" +
				"<table border=\"1\">" +
				"<tr><td>Сотрудник</td><td><b>" + surveyResult.DocName + "</b></td></tr>" +
				"<tr><td>Отделение</td><td><b>" + surveyResult.DocDepartment + "</b></td></tr>" +
				"<tr><td>Оценка</td><td><b>" + mark + "</b></td></tr>" +
				"<tr><td>Комментарий</td><td><b>" +
				(surveyResult.Comment.Equals("skipped") ? "отказался" : surveyResult.Comment) + "</b></td></tr>" +
				"<tr><td>Номер телефона для связи</td><td><b>" +
				(surveyResult.PhoneNumber.Equals("skipped") ? "отказался" : surveyResult.PhoneNumber) + "</b></td></tr>" +
				"</table><br>";

			string attachmentPath = surveyResult.PhotoLink;

			if (File.Exists(attachmentPath))
				body += "Фотография с камеры терминала:";
			else
				body += "Фотография отсутствует";
			body += "</b>";

			Mail.SendMail(subject, body, recipients, attachmentPath);
		}
	}
}
