#Region ;**** Directives created by AutoIt3Wrapper_GUI ****
#AutoIt3Wrapper_Icon=InfoKioskAutoRestartIcon.ico
#EndRegion ;**** Directives created by AutoIt3Wrapper_GUI ****
#pragma compile(ProductVersion, 1.0)
#pragma compile(UPX, true)
#pragma compile(CompanyName, 'ООО Клиника ЛМС')
#pragma compile(FileDescription, Приложение для перезапуска процесса инфокиоска)
#pragma compile(LegalCopyright, Грашкин Павел Павлович - nibble@yande.ru)
#pragma compile(ProductName, InfoKioskRestart)

Local $sProcessName = "InfoKiosk.exe"
Local $sWindowTitle = "InfoKiosk"

While 1
	If Not ProcessExists($sProcessName) Then
		ShellExecute($sProcessName)
	Else
		If Not WinActive($sWindowTitle) Then
			WinActivate($sWindowTitle)
		EndIf
	EndIf

	Sleep(30 * 1000)
WEnd