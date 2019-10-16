using DirectShowLib;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InfoKiosk.Services {
	class WebCam {
		private string photoSavePath;

		public string CaptureImageFromWebCamAndSave() {
			photoSavePath = Services.Config.Instance.PathWebCamSaveTo;

			if (Debugger.IsAttached)
				return "Debug";

			DsDevice[] dsDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
			if (dsDevices.Length == 0) {
				Logging.ToLog("WebCam - CaptureImageFromWebCamAndSave: There is no video input device available");
				return "A camera isn't installed";
			}

			if (string.IsNullOrEmpty(photoSavePath))
				photoSavePath = Path.Combine(Directory.GetCurrentDirectory(), "Photos");

			if (!Directory.Exists(photoSavePath)) {
				try {
					Directory.CreateDirectory(photoSavePath);
				} catch (Exception e) {
					Logging.ToLog("WebCam - CaptureImageFromWebCamAndSave exception: " + e.Message +
						Environment.NewLine + e.StackTrace);
					return "Error";
				}
			}

			string fileName = "photo_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg";
			photoSavePath += fileName;

#pragma warning disable IDE0067 // Dispose objects before losing scope
#pragma warning disable CA2000 // Dispose objects before losing scope
			BackgroundWorker backgroundWorker = new BackgroundWorker();
#pragma warning restore CA2000 // Dispose objects before losing scope
#pragma warning restore IDE0067 // Dispose objects before losing scope
			backgroundWorker.DoWork += BackgroundWorker_DoWork;
			backgroundWorker.RunWorkerAsync();
			backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

			return photoSavePath;
		}

		private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (e.Error != null)
				Logging.ToLog("WebCam - " + e.Error.Message + Environment.NewLine + e.Error.StackTrace);

			(sender as BackgroundWorker).Dispose();
		}

		private bool IsEmpty(System.Drawing.Bitmap image) {
			var data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly, image.PixelFormat);
			var bytes = new byte[data.Height * data.Stride];
			Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
			image.UnlockBits(data);
			return bytes.All(x => x == 0);
		}

		private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
			if (!Debugger.IsAttached) {
				Logging.ToLog("WebCam - Получение изображения с веб-камеры и сохранение в файл: " + photoSavePath);

				VideoCapture videoCapture = new VideoCapture();
				System.Drawing.Bitmap bitmap = null;

				//wait for camera to be active and get non blank screen
				for (int i = 0; i < 5; i++) {
					bitmap = videoCapture.QueryFrame().Bitmap;

					if (IsEmpty(bitmap))
						continue;

					bitmap.Save(photoSavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
					break;
				}

				bitmap.Dispose();
				videoCapture.Dispose();
			}
		}
	}
}
