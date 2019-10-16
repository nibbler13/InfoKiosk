using System.Net.Mail;
using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Diagnostics;

namespace InfoKiosk {
	public static class ClientMail {
		public static async void SendMail (string subject, string body, string receiver, string attachmentPath = "") {
			Logging.ToLog("Mail - Отправка сообщения, тема: " + subject + ", текст: " + body);
			Logging.ToLog("Mail - Получатели: " + receiver);

			if (string.IsNullOrEmpty(receiver) ||
				!Services.Config.Instance.ShouldSendMail) {
				Logging.ToLog("Mail - Пропуск отправки в соответствии с настройками");
				return;
			}

			try { 
				SmtpClient client = CreateClientAndMessage(subject, body, receiver, out MailMessage message, attachmentPath);
				await Task.Run(() => { client.Send(message); }).ConfigureAwait(false);
				Logging.ToLog("Mail - Письмо отправлено успешно");
				DisposeResources(client, message);
			} catch (Exception e) {
				Logging.ToLog("Mail - SendMail exception: " + e.Message + Environment.NewLine + e.StackTrace);
			}
		}

		public static void SendTestMail(string subject, string body, string receiver) {
			if (receiver == null)
				receiver = string.Empty;

			SmtpClient client = CreateClientAndMessage(subject, body, receiver, out MailMessage message);
			client.Send(message);
			DisposeResources(client, message);
		}

		private static SmtpClient CreateClientAndMessage(string subject, string body, string receiver, out MailMessage message, string attachmentPath = "") {
			string appName = Assembly.GetExecutingAssembly().GetName().Name;

			MailAddress from = new MailAddress(Services.Config.Instance.MailUser, appName);
			List<MailAddress> mailAddressesTo = new List<MailAddress>();

			if (receiver.Contains(" | ")) {
				string[] receivers = Services.Config.GetSplittedAddresses(receiver);
				foreach (string address in receivers)
					try {
						mailAddressesTo.Add(new MailAddress(address));
					} catch (Exception e) {
						Logging.ToLog("Mail - Не удалось разобрать адрес: " + address + Environment.NewLine + e.Message);
					}
			} else
				try {
					mailAddressesTo.Add(new MailAddress(receiver));
				} catch (Exception e) {
					Logging.ToLog("Mail - Не удалось разобрать адрес: " + receiver + Environment.NewLine + e.Message);
				}

			body += Environment.NewLine + Environment.NewLine +
				"___________________________________________" + Environment.NewLine +
				"Это автоматически сгенерированное сообщение" + Environment.NewLine +
				"Просьба не отвечать на него" + Environment.NewLine +
				 "Имя системы: " + Environment.MachineName;

			message = new MailMessage();

			foreach (MailAddress mailAddress in mailAddressesTo)
				message.To.Add(mailAddress);

			message.IsBodyHtml = body.Contains("<") && body.Contains(">");

			if (message.IsBodyHtml)
				body = body.Replace(Environment.NewLine, "<br>");

			if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath)) {
#pragma warning disable IDE0068 // Use recommended dispose pattern
#pragma warning disable CA2000 // Dispose objects before losing scope
				Attachment attachment = new Attachment(attachmentPath);
#pragma warning restore CA2000 // Dispose objects before losing scope
#pragma warning restore IDE0068 // Use recommended dispose pattern

				if (message.IsBodyHtml && attachmentPath.EndsWith(".jpg")) {
					attachment.ContentDisposition.Inline = true;

					LinkedResource inline = new LinkedResource(attachmentPath, MediaTypeNames.Image.Jpeg) {
						ContentId = Guid.NewGuid().ToString()
					};

					body = body.Replace("Фотография с камеры терминала:", "Фотография с камеры терминала:<br>" +
						string.Format(@"<img src=""cid:{0}"" />", inline.ContentId));

					AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
					avHtml.LinkedResources.Add(inline);

					message.AlternateViews.Add(avHtml);
				} else
					message.Attachments.Add(attachment);
			}

			message.From = from;
			message.Subject = subject;
			message.Body = body;

			if (Services.Config.Instance.ShouldAddAdminToCopy) {
				string adminAddress = Services.Config.Instance.MailAdminAddress;
				if (!string.IsNullOrEmpty(adminAddress))
					if (adminAddress.Contains(" | ")) {
						string[] adminAddresses = Services.Config.GetSplittedAddresses(adminAddress);
						foreach (string address in adminAddresses)
							try {
								message.CC.Add(new MailAddress(address));
							} catch (Exception e) {
								Logging.ToLog("Mail - Не удалось разобрать адрес: " + address + Environment.NewLine + e.Message);
							}
					} else
						try {
							message.CC.Add(new MailAddress(adminAddress));
						} catch (Exception e) {
							Logging.ToLog("Mail - Не удалось разобрать адрес: " + adminAddress + Environment.NewLine + e.Message);
						}
			}

			SmtpClient client = new SmtpClient(Services.Config.Instance.MailSmtpServer, (int)Services.Config.Instance.MailSmtpPort) {
				UseDefaultCredentials = false,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				EnableSsl = Services.Config.Instance.MailEnableSSL,
				Credentials = new System.Net.NetworkCredential(
				Services.Config.Instance.MailUser,
				Services.Config.Instance.MailPassword)
			};

			if (!string.IsNullOrEmpty(Services.Config.Instance.MailUserDomain))
				client.Credentials = new System.Net.NetworkCredential(
				Services.Config.Instance.MailUser,
				Services.Config.Instance.MailPassword,
				Services.Config.Instance.MailUserDomain);

			return client;
		}

		private static void DisposeResources(SmtpClient client, MailMessage message) {
			client.Dispose();
			foreach (Attachment attach in message.Attachments)
				attach.Dispose();

			message.Dispose();
		}
	}
}
