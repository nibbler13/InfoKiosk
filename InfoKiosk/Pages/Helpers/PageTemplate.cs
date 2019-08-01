using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace InfoKiosk {
	public partial class PageTemplate : Page {
		protected double ScreenWidth { get; private set; }
		protected double ScreenHeight { get; private set; }
		protected double Gap { get; private set; }
		protected double HeaderHeight { get; private set; }
		protected double FontSizeMain { get; private set; }
		protected double StartX { get; private set; }
		protected double StartY { get; private set; }
		protected double AvailableWidth { get; private set; }
		protected double AvailableHeight { get; private set; }
		protected Canvas CanvasMain { get; private set; }

		protected FontFamily FontFamilyMain { get; private set; }
		protected FontFamily FontFamilySub { get; private set; }
		protected double DefaultButtonWidth { get; private set; }
		protected double DefaultButtonHeight { get; private set; }
		protected bool IsDebug { get; private set; }

		protected ScrollViewer ScrollViewer { get; private set; }
		protected WrapPanel CanvasForElements { get; private set; }
		protected double ElementWidth { get; private set; }
		protected double ElementHeight { get; private set; }

		//private Label _labelTitle;
		//private Label _labelSubtitle;
		//private Image _imageLogo;
		//private Image _imageBottomLineSolid;
		//private Image _imageBottomLineColors;
		//private Button _buttonBack;

		private Button _buttonScrollUp;
		private Button _buttonScrollDown;
		private Button _buttonScrollLeft;
		private Button _buttonScrollRight;
		
		private Image _imageQuestion;
		private Label _labelQuestion;
		private Button _buttonNoQuestion;
		private Button _buttonYesQuestion;

		protected double _leftCornerShadow = 0;
		protected double _rightCornerShadow = 5;
		private double _elementsInLine = 0;
		private double _elementsLineCount = 0;
		private double _elementsGap = 0;

		protected ItemSurveyResult _surveyResult = null;
		private DispatcherTimer _dispatcherTimerPageAutoClose;



		//public PageTemplate() {
		//	//SystemLogging.ToLog("---> Создание страницы " + this.GetType().Name);

		//	ScreenWidth = SystemParameters.PrimaryScreenWidth;
		//	ScreenHeight = SystemParameters.PrimaryScreenHeight;
		//	Gap = ScreenHeight * 0.03;
		//	FontSizeMain = ScreenHeight * 0.033;
		//	HeaderHeight = FontSizeMain * 3;
		//	DefaultButtonHeight = FontSizeMain * 2;
		//	DefaultButtonWidth = DefaultButtonHeight;

		//	FontFamilyMain = new FontFamily(Properties.Settings.Default.FontMain.Name);
		//	FontFamilySub = new FontFamily(Properties.Settings.Default.FontSub.Name);

		//	CanvasMain = new Canvas {
		//		Width = ScreenWidth,
		//		Height = ScreenHeight
		//	};
		//	Content = CanvasMain;

		//	CreateMainControls();

		//	StartX = Gap;
		//	StartY = (double)(_labelTitle.Height + Gap);
		//	AvailableWidth = ScreenWidth - Gap * 2;
		//	AvailableHeight = (double)(Canvas.GetTop(_labelSubtitle) - HeaderHeight - Gap * 2);

		//	IsVisibleChanged += ClassPageTemplate_IsVisibleChanged;
		//}

		//protected void HideLogo() {
		//	_imageLogo.Visibility = Visibility.Hidden;
		//}

		//protected void HideButtonBack() {
		//	_buttonBack.Visibility = Visibility.Hidden;
		//}

		//protected void HideTitlesLabel() {
		//	_labelTitle.Visibility = Visibility.Hidden;
		//	_labelSubtitle.Visibility = Visibility.Hidden;
		//}

		//protected void SetLabelsContent(string title, string subtitle) {
		//	double fontTitleScale = 1.2;
		//	if (title.Contains(Environment.NewLine))
		//		fontTitleScale = 1.0;

		//	_labelTitle.Content = ControlsFactory.CreateTextBlock(
		//		title,
		//		FontFamilySub, 
		//		FontSizeMain * fontTitleScale, 
		//		FontWeights.DemiBold);

		//	_labelSubtitle.Content = ControlsFactory.CreateTextBlock(
		//		subtitle, 
		//		FontFamilySub, 
		//		FontSizeMain, 
		//		FontWeights.Normal);
		//}

		//protected void SetLabelSubtitleText(string text) {
		//	_labelSubtitle.Content = ControlsFactory.CreateTextBlock(
		//		text, 
		//		FontFamilySub, 
		//		FontSizeMain, 
		//		FontWeights.Normal);
		//}

		//protected void SetElementsWidthAndHeight(double width, double height) {
		//	ElementWidth = width;
		//	ElementHeight = height;
		//}



		private void CreateMainControls() {
			//double logoWidth = Properties.Resources.logo.Width;
			//double logoHeight = Properties.Resources.logo.Height;
			//double logoScale = logoWidth / logoHeight;
			//logoHeight = ScreenHeight * 0.15f;
			//logoWidth = logoHeight * logoScale;

			//double colorLineWidth = Properties.Resources.BottomLineContinuesClear.Width;
			//double colorLineHeight = Properties.Resources.BottomLineContinuesClear.Height;
			//double colorLineScale = colorLineWidth / colorLineHeight;
			//colorLineHeight = logoHeight * 0.058f;
			//colorLineWidth = colorLineHeight * colorLineScale;

			//_imageBottomLineColors = ControlsFactory.CreateImage(
			//	Properties.Resources.BottomLineContinuesClear,
			//	colorLineWidth,
			//	colorLineHeight,
			//	ScreenWidth - colorLineWidth,
			//	ScreenHeight - colorLineHeight,
			//	CanvasMain,
			//	false);

			//_imageBottomLineSolid = ControlsFactory.CreateImage(
			//	Properties.Resources.BottomLineTemplate,
			//	ScreenWidth - colorLineWidth,
			//	colorLineHeight,
			//	0,
			//	ScreenHeight - colorLineHeight,
			//	CanvasMain,
			//	false);
			//_imageBottomLineSolid.Stretch = Stretch.Fill;
			//_imageBottomLineSolid.HorizontalAlignment = HorizontalAlignment.Left;

			//_imageLogo = ControlsFactory.CreateImage(
			//	Properties.Resources.logo,
			//	logoWidth,
			//	logoHeight,
			//	ScreenWidth - Gap - logoWidth,
			//	ScreenHeight - Gap - logoHeight,
			//	CanvasMain,
			//	false);
			//Canvas.SetZIndex(_imageLogo, 99);
			
			//_labelTitle = ControlsFactory.CreateLabel(
			//	"Title" + Environment.NewLine + "Title",
			//	//(this is PageError) ? 
			//	//Properties.Settings.Default.ColorPageErrorHeaderBackground : 
			//	Properties.Settings.Default.ColorHeaderBackground,
			//	Properties.Settings.Default.ColorHeaderForeground,
			//	FontFamilySub,
			//	FontSizeMain,
			//	FontWeights.Normal,
			//	ScreenWidth,
			//	HeaderHeight,
			//	0,
			//	0,
			//	CanvasMain);

			//_labelSubtitle = ControlsFactory.CreateLabel(
			//	"Subtitle",
			//	Colors.Transparent,
			//	Properties.Settings.Default.ColorDisabled,
			//	FontFamilySub,
			//	FontSizeMain,
			//	FontWeights.Normal,
			//	ScreenWidth,
			//	DefaultButtonHeight,
			//	0,
			//	ScreenHeight - DefaultButtonHeight - Gap,
			//	CanvasMain);

			//_buttonBack = ControlsFactory.CreateButtonWithImageOnly(
			//	Properties.Resources.ButtonBack,
			//	DefaultButtonWidth,
			//	DefaultButtonHeight,
			//	Gap,
			//	Canvas.GetTop(_labelSubtitle),
			//	CanvasMain);

			//_buttonBack = ControlsFactory.CreateButtonWithImageAndText(
			//	"Назад",
			//	DefaultButtonWidth * 3,
			//	DefaultButtonHeight,
			//	ControlsFactory.ElementType.Custom,
			//	FontFamilySub,
			//	FontSizeMain,
			//	FontWeights.Normal,
			//	Properties.Resources.BackButton,
			//	Gap,
			//	Canvas.GetTop(_labelSubtitle),
			//	CanvasMain);

			//_buttonBack.Click += ButtonBack_Click;
			
			_dispatcherTimerPageAutoClose = new DispatcherTimer();

			int timeoutSeconds = Properties.Settings.Default.PageAutocloseTimeoutInSeconds;
			//if (this is PageThanks)
			//	timeoutSeconds /= 2;

			_dispatcherTimerPageAutoClose.Interval = new TimeSpan(0, 0, timeoutSeconds);
			_dispatcherTimerPageAutoClose.Tick += DispatcherTimerPageAutoClose_Tick;
			PreviewMouseLeftButtonDown += ClassPageTemplate_PreviewMouseLeftButtonDown;
		}

		//protected Rect CreateFirstOrLastPageControls(string leftTopText, string leftBottomText, string rightText, string subtitleText, bool isFirstPage) {
		//	CanvasMain.Children.Clear();

		//	double leftTopFontScale = 2.9;
		//	double leftBottomFontScale = 1.4;
		//	double rightFontScale = 3.9;

		//	if (!isFirstPage) {
		//		leftTopFontScale = 1.9;
		//		leftBottomFontScale = 1.4;
		//		rightFontScale = 2.4;
		//	}

		//	ControlsFactory.CreateLabel(
		//		string.Empty, 
		//		Properties.Settings.Default.ColorHeaderFirstLastBackground, 
		//		Colors.Transparent, 
		//		FontFamily, 
		//		FontSize, 
		//		FontWeights.Normal, 
		//		ScreenWidth, 
		//		StartY * 1.5, 
		//		0, 
		//		0, 
		//		CanvasMain);

		//	StackPanel stackPanelInner = new StackPanel {
		//		Orientation = Orientation.Vertical,
		//		VerticalAlignment = VerticalAlignment.Center,
		//		Margin = new Thickness(0, 0, 15, 0)
		//	};

		//	Grid gridTitle = new Grid {
		//		Width = ScreenWidth,
		//		Height = StartY * 1.5,
		//		Background = Brushes.Transparent
		//	};
		//	CanvasMain.Children.Add(gridTitle);
		//	Canvas.SetLeft(gridTitle, 0);
		//	Canvas.SetTop(gridTitle, 0);

		//	TextBlock leftTop = ControlsFactory.CreateTextBlock(
		//		leftTopText, 
		//		FontFamilySub, 
		//		FontSizeMain * leftTopFontScale, 
		//		FontWeights.Normal, 
		//		Properties.Settings.Default.ColorHeaderForeground,
		//		FontStretches.UltraExpanded);
		//	leftTop.HorizontalAlignment = HorizontalAlignment.Right;
		//	stackPanelInner.Children.Add(leftTop);

		//	TextBlock leftBottom = ControlsFactory.CreateTextBlock(
		//		leftBottomText, 
		//		FontFamilySub, 
		//		FontSizeMain * leftBottomFontScale, 
		//		FontWeights.UltraLight, 
		//		Properties.Settings.Default.ColorHeaderForeground,
		//		FontStretches.UltraExpanded);
		//	stackPanelInner.Children.Add(leftBottom);

		//	StackPanel stackPanelRoot = new StackPanel {
		//		Orientation = Orientation.Horizontal,
		//		HorizontalAlignment = HorizontalAlignment.Center,
		//		VerticalAlignment = VerticalAlignment.Center
		//	};
		//	stackPanelRoot.Children.Add(stackPanelInner);
		//	stackPanelRoot.Margin = new Thickness(10);
			
		//	TextBlock right = ControlsFactory.CreateTextBlock(
		//		rightText, 
		//		FontFamilyMain, 
		//		FontSizeMain * rightFontScale, 
		//		FontWeights.Heavy, 
		//		Properties.Settings.Default.ColorHeaderForeground,
		//		FontStretches.UltraExpanded);
		//	right.VerticalAlignment = VerticalAlignment.Center;
		//	right.Margin = new Thickness(15, 0, 0, 0);
		//	stackPanelRoot.Children.Add(right);

		//	gridTitle.Children.Add(stackPanelRoot);

		//	double logoBzFullWidth = Properties.Resources.logo.Width;
		//	double logoBzFullHeight = Properties.Resources.logo.Height;
		//	double logoBzFullScale = logoBzFullWidth / logoBzFullHeight;
		//	logoBzFullHeight = ScreenHeight * 0.25f;
		//	logoBzFullWidth = logoBzFullHeight * logoBzFullScale;

		//	Image imageLogoBzFull = ControlsFactory.CreateImage(
		//		Properties.Resources.logo,
		//		logoBzFullWidth,
		//		logoBzFullHeight,
		//		ScreenWidth - logoBzFullWidth - Gap,
		//		ScreenHeight - logoBzFullHeight - Gap,
		//		CanvasMain,
		//		false);

		//	Rect rect = new Rect(
		//		imageLogoBzFull.Width + Gap * 2,
		//		gridTitle.Height + Gap,
		//		ScreenWidth - imageLogoBzFull.Width * 2 - Gap * 4,
		//		ScreenHeight - gridTitle.Height - Gap * 2);

		//	if (!string.IsNullOrEmpty(subtitleText)) {
		//		rect.Height = rect.Height - DefaultButtonHeight;
		//		ControlsFactory.CreateLabel(
		//			subtitleText,
		//			Colors.Transparent,
		//			Properties.Settings.Default.ColorDisabled,
		//			FontFamilySub,
		//			FontSizeMain,
		//			FontWeights.Normal,
		//			ScreenWidth,
		//			DefaultButtonHeight,
		//			0,
		//			ScreenHeight - DefaultButtonHeight,
		//			CanvasMain);
		//	}
			
		//	Label labelToHandleMousePreview = ControlsFactory.CreateLabel(
		//		"",
		//		Colors.Transparent,
		//		Colors.Transparent,
		//		FontFamily,
		//		FontSize,
		//		FontWeights.Normal,
		//		ScreenWidth,
		//		ScreenHeight,
		//		0,
		//		0,
		//		CanvasMain);
		//	Canvas.SetZIndex(labelToHandleMousePreview, -1);


		//	//int day = DateTime.Now.Day;
		//	//int month = DateTime.Now.Month;
		//	//if ((month == 12 && day >= 10) || (month == 1 && day < 10)) {
		//	//	//ControlsFactory.AddDropShadow(viewboxLeftBottom, true);
		//	//	//ControlsFactory.AddDropShadow(viewboxLeftTop, true);
		//	//	//ControlsFactory.AddDropShadow(viewboxRight, true);
		//	//	ControlsFactory.AddDropShadow(stackPanelRoot);

		//	//	Image imageNewYearTree = ControlsFactory.CreateImage(
		//	//		Properties.Resources.NewYearTree,
		//	//		logoBzFullWidth,
		//	//		logoBzFullHeight,
		//	//		Gap,
		//	//		ScreenHeight - logoBzFullHeight - Gap,
		//	//		CanvasMain,
		//	//		false);
		//	//	imageNewYearTree.HorizontalAlignment = HorizontalAlignment.Left;

		//	//	Image imageNewYearSnowflakes = ControlsFactory.CreateImage(
		//	//		Properties.Resources.NewYearSnowflakes,
		//	//		ScreenWidth,
		//	//		StartY * 1.5,
		//	//		0,
		//	//		0,
		//	//		CanvasMain,
		//	//		false);
		//	//	imageNewYearSnowflakes.Opacity = 0.8;
		//	//	Canvas.SetZIndex(gridTitle, 1);

		//	//	Image imageNewYearTinsel = ControlsFactory.CreateImage(
		//	//		Properties.Resources.NewYearTinsel,
		//	//		ScreenWidth,
		//	//		80,
		//	//		0,
		//	//		StartY * 1.5 - 20,
		//	//		CanvasMain,
		//	//		false);
		//	//} else {
		//	//	string devInfo = "Разработка приложения:" + Environment.NewLine +
		//	//		"ООО \"Клиника ЛМС\"" + Environment.NewLine +
		//	//		"dev@bzklinika.ru";


		//	//	Label label = ControlsFactory.CreateLabel(
		//	//		devInfo,
		//	//		Colors.Transparent,
		//	//		Colors.LightGray,
		//	//		FontFamilySub,
		//	//		FontSizeMain * 0.7,
		//	//		FontWeights.Normal,
		//	//		-1,
		//	//		-1,
		//	//		Gap * 0.7,
		//	//		ScreenHeight - (FontSizeMain * 0.7) * 3 - Gap,
		//	//		CanvasMain);
		//	//	(label.Content as TextBlock).TextAlignment = TextAlignment.Left;
		//	//}

		//	return rect;
		//}

		//protected void CreateRootPanel(double elementsInLine, double elementsLineCount, double totalElements, 
		//	Orientation orientation = Orientation.Vertical, double width = 0, double height = 0, double left = 0, double top = 0, 
		//	ControlsFactory.ElementType? type = null) {

		//	_elementsGap = Gap;
		//	if (type == ControlsFactory.ElementType.Department &&
		//		Properties.Settings.Default.PageDepartmentUseSmallDistanceBetweenElements)
		//		_elementsGap = _rightCornerShadow;

		//	_elementsInLine = elementsInLine;
		//	_elementsLineCount = elementsLineCount;

		//	ElementWidth = (AvailableWidth - _elementsGap * (elementsInLine - 1)) / elementsInLine;
		//	ElementHeight = (AvailableHeight - DefaultButtonHeight - Gap - _elementsGap * (elementsLineCount - 1)) / elementsLineCount;

		//	VerticalAlignment verticalAlignment = VerticalAlignment.Bottom;
		//	HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;

		//	if (totalElements > elementsInLine * elementsLineCount)
		//		CreateUpDownButtons();
		//	else {
		//		verticalAlignment = VerticalAlignment.Center;
		//		horizontalAlignment = HorizontalAlignment.Center;
		//	}

		//	if (width == 0 & height == 0) {
		//		width = AvailableWidth + _leftCornerShadow + _rightCornerShadow;
		//		height = AvailableHeight + _leftCornerShadow + _rightCornerShadow - DefaultButtonHeight - Gap;
		//		left = StartX - _leftCornerShadow;
		//		top = StartY - _leftCornerShadow + DefaultButtonWidth + Gap;
		//	}

		//	ScrollViewer = new ScrollViewer {
		//		Width = width,
		//		Height = height
		//	};
		//	CanvasMain.Children.Add(ScrollViewer);
		//	Canvas.SetLeft(ScrollViewer, left);
		//	Canvas.SetTop(ScrollViewer, top);

		//	ScrollViewer.PanningMode = PanningMode.Both;
		//	ScrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;

		//	CanvasForElements = new WrapPanel {
		//		VerticalAlignment = verticalAlignment,
		//		HorizontalAlignment = horizontalAlignment
		//	};
		//	ScrollViewer.Content = CanvasForElements;

		//	if (IsDebug)
		//		ScrollViewer.Background = new SolidColorBrush(Colors.Yellow);

		//	if (orientation == Orientation.Horizontal) {
		//		CreateLeftRightButtons();
		//		CanvasForElements.Orientation = Orientation.Horizontal;
		//		CanvasForElements.VerticalAlignment = VerticalAlignment.Top;
		//		ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
		//		ScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
		//	} else {
		//		ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
		//	}
		//}

		//private void CreateLeftRightButtons() {
		//	_buttonScrollLeft = ControlsFactory.CreateButtonWithImageOnly(
		//		Properties.Resources.ButtonLeft,
		//		DefaultButtonHeight,
		//		DefaultButtonHeight,
		//		StartX,
		//		Canvas.GetTop(ScrollViewer) + ScrollViewer.Height + Gap,
		//		CanvasMain);
		//	_buttonScrollLeft.Style = Application.Current.MainWindow.FindResource("RoundCorner") as Style;
		//	_buttonScrollLeft.Click += ButtonScrollLeft_Click;
		//	_buttonScrollLeft.Visibility = Visibility.Hidden;
		//	//_buttonScrollLeft.Background = new SolidColorBrush(Properties.Settings.Default.ColorScrollButton);

		//	_buttonScrollRight = ControlsFactory.CreateButtonWithImageOnly(
		//		Properties.Resources.ButtonRight,
		//		DefaultButtonHeight,
		//		DefaultButtonHeight,
		//		StartX + AvailableWidth - DefaultButtonHeight,
		//		Canvas.GetTop(_buttonScrollLeft),
		//		CanvasMain);
		//	_buttonScrollRight.Style = Application.Current.MainWindow.FindResource("RoundCorner") as Style;
		//	_buttonScrollRight.Click += ButtonScrollRight_Click;
		//	_buttonScrollRight.Visibility = Visibility.Hidden;
		//	//_buttonScrollRight.Background = new SolidColorBrush(Properties.Settings.Default.ColorScrollButton);

		//	//AddBlickingEffectToButton(new List<Button>() { _buttonScrollLeft, _buttonScrollRight });
		//}

		//private void CreateUpDownButtons() {
		//	_buttonScrollUp = ControlsFactory.CreateButtonWithImageOnly(
		//		Properties.Resources.UpButton,
		//		DefaultButtonHeight,
		//		DefaultButtonHeight,
		//		StartX + AvailableWidth - DefaultButtonHeight,
		//		StartY,
		//		CanvasMain);
		//	_buttonScrollUp.Style = Application.Current.MainWindow.FindResource("RoundCornerBlue") as Style;
		//	_buttonScrollUp.Click += ButtonScrollUp_Click;
		//	_buttonScrollUp.Visibility = Visibility.Hidden;
		//	//_buttonScrollUp.Background = new SolidColorBrush(Properties.Settings.Default.ColorScrollButton);

		//	_buttonScrollDown = ControlsFactory.CreateButtonWithImageOnly(
		//		Properties.Resources.DownButton,
		//		DefaultButtonHeight,
		//		DefaultButtonHeight,
		//		StartX + AvailableWidth - DefaultButtonHeight,
		//		Canvas.GetTop(_buttonBack),
		//		CanvasMain);
		//	_buttonScrollDown.Style = Application.Current.MainWindow.FindResource("RoundCornerBlue") as Style;
		//	_buttonScrollDown.Click += ButtonScrollDown_Click;
		//	//_buttonScrollDown.Background = new SolidColorBrush(Properties.Settings.Default.ColorScrollButton);

		//	//AddBlickingEffectToButton(new List<Button>() { _buttonScrollUp, _buttonScrollDown });
		//}

		//private void AddBlickingEffectToButton(List<Button> buttons) {
		//	ColorAnimation animation = new ColorAnimation {
		//		From = Properties.Settings.Default.ColorButtonBackground,
		//		To = Properties.Settings.Default.ColorScrollButton,
		//		Duration = new TimeSpan(0, 0, 1),
		//		RepeatBehavior = RepeatBehavior.Forever,
		//		AutoReverse = true
		//	};

		//	foreach (Button button in buttons)
		//		button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
		//}

		//protected void CreateQuestionControlsAnd(string question, System.Drawing.Bitmap image, 
		//	RoutedEventHandler buttonNoHandler, RoutedEventHandler buttonYesHandler) {
		//	double buttonWidth = DefaultButtonWidth * 3;

		//	_buttonYesQuestion = ControlsFactory.CreateButtonWithImageAndText(
		//		"Да",
		//		buttonWidth,
		//		DefaultButtonHeight,
		//		ControlsFactory.ElementType.Custom,
		//		FontFamilySub,
		//		FontSizeMain,
		//		FontWeights.Normal,
		//		Properties.Resources.ButtonOk,
		//		StartX + AvailableWidth / 2 - Gap * 2 - buttonWidth,
		//		StartY + AvailableHeight - DefaultButtonHeight,
		//		CanvasMain);
		//	_buttonYesQuestion.Style = Application.Current.MainWindow.FindResource("RoundCornerGreen") as Style;

		//	_buttonNoQuestion = ControlsFactory.CreateButtonWithImageAndText(
		//		"Нет",
		//		_buttonYesQuestion.Width,
		//		_buttonYesQuestion.Height,
		//		ControlsFactory.ElementType.Custom,
		//		FontFamilySub,
		//		FontSizeMain,
		//		FontWeights.Normal,
		//		Properties.Resources.ButtonClose,
		//		Canvas.GetLeft(_buttonYesQuestion) + _buttonYesQuestion.Width + Gap * 4,
		//		Canvas.GetTop(_buttonYesQuestion),
		//		CanvasMain);
		//	_buttonYesQuestion.Background = new SolidColorBrush(Properties.Settings.Default.ColorHeaderBackground);
		//	_buttonYesQuestion.Foreground = new SolidColorBrush(Properties.Settings.Default.ColorHeaderForeground);

		//	_buttonNoQuestion.Click += buttonNoHandler;
		//	_buttonYesQuestion.Click += buttonYesHandler;
		//	_buttonYesQuestion.Click += ButtonYesQuestion_Click;

		//	_labelQuestion = ControlsFactory.CreateLabel(
		//		question,
		//		Colors.Transparent,
		//		Properties.Settings.Default.ColorLabelForeground,
		//		FontFamilySub,
		//		FontSizeMain,
		//		FontWeights.Normal,
		//		AvailableWidth,
		//		DefaultButtonHeight,
		//		StartX,
		//		Canvas.GetTop(_buttonNoQuestion) - Gap - DefaultButtonHeight,
		//		CanvasMain);

		//	_imageQuestion = ControlsFactory.CreateImage(
		//		image,
		//		AvailableWidth,
		//		Canvas.GetTop(_labelQuestion) - StartY - Gap,
		//		StartX,
		//		StartY,
		//		CanvasMain,
		//		false);
		//}




		private void ButtonScrollRight_Click(object sender, RoutedEventArgs e) {
			double newOffset = ScrollViewer.HorizontalOffset + ScrollViewer.Width + Gap - _leftCornerShadow - _rightCornerShadow;
			ScrollHorizontalWithAnimation(newOffset);
			_buttonScrollLeft.Visibility = Visibility.Visible;
		}

		private void ButtonScrollLeft_Click(object sender, RoutedEventArgs e) {
			double newOffset = ScrollViewer.HorizontalOffset - ScrollViewer.Width - Gap + _leftCornerShadow + _rightCornerShadow;
			ScrollHorizontalWithAnimation(newOffset);
			_buttonScrollRight.Visibility = Visibility.Visible;
		}

		private void ButtonScrollDown_Click(object sender, RoutedEventArgs e) {
			double newOffset = ScrollViewer.VerticalOffset + ScrollViewer.Height + Gap - _leftCornerShadow - _rightCornerShadow;
			ScrollVerticalWithAnimation(newOffset);
			_buttonScrollUp.Visibility = Visibility.Visible;
		}

		private void ButtonScrollUp_Click(object sender, RoutedEventArgs e) {
			double newOffset = ScrollViewer.VerticalOffset - ScrollViewer.Height - Gap + _leftCornerShadow + _rightCornerShadow;
			ScrollVerticalWithAnimation(newOffset);
			_buttonScrollDown.Visibility = Visibility.Visible;
		}




		private void ScrollVerticalWithAnimation(double to) {
			DoubleAnimation verticalAnimation = new DoubleAnimation {
				From = ScrollViewer.VerticalOffset,
				To = to,
				Duration = new Duration(new TimeSpan(0, 0, 0))
			};

			Storyboard storyboard = new Storyboard();
			storyboard.Children.Add(verticalAnimation);
			Storyboard.SetTarget(verticalAnimation, ScrollViewer);
			Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(PageScrollAnimationBehavior.VerticalOffsetProperty));
			storyboard.Begin();
		}

		private void ScrollHorizontalWithAnimation(double to) {
			DoubleAnimation horizontalAnimation = new DoubleAnimation {
				From = ScrollViewer.HorizontalOffset,
				To = to,
				Duration = new Duration(new TimeSpan(0, 0, 0))
			};

			Storyboard storyboard = new Storyboard();
			storyboard.Children.Add(horizontalAnimation);
			Storyboard.SetTarget(horizontalAnimation, ScrollViewer);
			Storyboard.SetTargetProperty(horizontalAnimation, new PropertyPath(PageScrollAnimationBehavior.HorizontalOffsetProperty));
			storyboard.Begin();
		}

		private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) {
			if (_buttonScrollDown != null || _buttonScrollUp != null) {
				if ((int)ScrollViewer.ScrollableHeight == 0) {
					_buttonScrollUp.Visibility = Visibility.Hidden;
					_buttonScrollDown.Visibility = Visibility.Hidden;
					return;
				}

				if ((int)e.VerticalOffset >= (int)ScrollViewer.ScrollableHeight) {
					_buttonScrollDown.Visibility = Visibility.Hidden;
					_buttonScrollUp.Visibility = Visibility.Visible;
				} else if ((int)e.VerticalOffset <= 0) {
					_buttonScrollUp.Visibility = Visibility.Hidden;
					_buttonScrollDown.Visibility = Visibility.Visible;
				}
			}

			if (_buttonScrollLeft != null || _buttonScrollRight != null) {
				if ((int)ScrollViewer.ScrollableWidth == 0) {
					_buttonScrollLeft.Visibility = Visibility.Hidden;
					_buttonScrollRight.Visibility = Visibility.Hidden;
					return;
				}

				if ((int)e.HorizontalOffset >= (int)ScrollViewer.ScrollableWidth) {
					_buttonScrollRight.Visibility = Visibility.Hidden;
					_buttonScrollLeft.Visibility = Visibility.Visible;
				} else if ((int)e.HorizontalOffset <= 0) {
					_buttonScrollLeft.Visibility = Visibility.Hidden;
					_buttonScrollRight.Visibility = Visibility.Visible;
				}
			}
		}



		//private void ButtonBack_Click(object sender, RoutedEventArgs e) {
		//	//SystemLogging.ToLog("<-- Нажатие кнопки назад");
		//	NavigationService.GoBack();
		//}

		//private void ButtonYesQuestion_Click(object sender, RoutedEventArgs e) {
		//	HideLogo();

		//	List<Control> controlsToRemove = new List<Control>() {
		//		_labelQuestion,
		//		_buttonNoQuestion,
		//		_buttonYesQuestion,
		//	};

		//	CanvasMain.Children.Remove(_imageQuestion);

		//	foreach (Control control in controlsToRemove)
		//		CanvasMain.Children.Remove(control);
		//}



		public static void FillPanelWithElements(WrapPanel wrapPanel,
										   string[] elements,
										   ControlsFactory.ElementType type,
										   RoutedEventHandler eventHandler,
										   int elementsInRow) {
			double elementsCreated = 0;
			double totalElementsCreated = 0;
			double linesCreated = 0;
			double totalLines = Math.Ceiling(elements.Length / (double)elementsInRow);

			double currentX = 0;
			double currentY = 0;
			double _elementsGap = 15;
			double rowsCount = 6;

			if (type == ControlsFactory.ElementType.Doctor)
				rowsCount = 2;
			else if (type == ControlsFactory.ElementType.Search)
				rowsCount = 1;

			double ElementWidth = (wrapPanel.ActualWidth - _elementsGap * (elementsInRow - 1)) / elementsInRow;
			double ElementHeight = (wrapPanel.ActualHeight - _elementsGap * (rowsCount - 1)) / rowsCount;
			ScrollViewer ScrollViewer = new ScrollViewer {
				Width = wrapPanel.ActualWidth,
				Height = wrapPanel.ActualHeight
			};

			bool isLastLineCentered = false;
			foreach (string element in elements) {
				if (string.IsNullOrEmpty(element))
					continue;

				Button innerButton = ControlsFactory.CreateButtonWithImageAndText(
					element, 
					ElementWidth, 
					ElementHeight,
					type,
					new FontFamily("FuturaLightC"),
					30,
					FontWeights.Normal);

				double bottomMargin = _elementsGap;
				double rightMargin = _elementsGap;
				double leftMargin = 0;
				double _rightCornerShadow = 0;
				double _leftCornerShadow = 0;

				if (elementsCreated == elementsInRow - 1)
					rightMargin = 0;

				if (linesCreated == totalLines - 1) {
					bottomMargin = _rightCornerShadow;

					if (totalLines > 1 && !isLastLineCentered) {
						double lastElements = elements.Length - totalElementsCreated;
						double lastElementsWidth = lastElements * ElementWidth + _elementsGap * (lastElements - 1);
						leftMargin = (ScrollViewer.Width - _rightCornerShadow - lastElementsWidth) / 2.0d;
						isLastLineCentered = true;
					}
				}

				innerButton.Margin = new Thickness(leftMargin, 0, rightMargin, bottomMargin);

				wrapPanel.Children.Add(innerButton);
				innerButton.Click += eventHandler;
				elementsCreated++;
				totalElementsCreated++;

				if (elementsCreated >= elementsInRow) {
					elementsCreated = 0;
					linesCreated++;
					currentX = _leftCornerShadow;
					currentY += ElementHeight + _elementsGap;
				}
			}
		}

		protected void CloseAllPagesExceptSplashScreen(bool showDepartmentSelect = false) {
			//SystemLogging.ToLog("<<< Возвращение к стартовой странице");

			try {
				while (NavigationService.CanGoBack)
					NavigationService.GoBack();

				if (showDepartmentSelect && NavigationService.CanGoForward)
					NavigationService.GoForward();

				((MainWindow)Application.Current.MainWindow).previousThankPageCloseTime = DateTime.Now;
			} catch (Exception e) {
				//SystemLogging.ToLog("CloseAllFormsExceptMain exception: " + e.Message + 
				//	Environment.NewLine + e.StackTrace);
				Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
			}
		}

		//protected void WriteSurveyResultToDb(ItemSurveyResult surveyResult) {
		//	if (((MainWindow)Application.Current.MainWindow).previousRatesDcodes.Contains(surveyResult.DCode))
		//		surveyResult.DocRate = "Duplicate";
		//	else
		//		((MainWindow)Application.Current.MainWindow).previousRatesDcodes.Add(surveyResult.DCode);

		//	SystemLogging.ToLog("Запись результата в базу данных: " + surveyResult.ToString());

		//	SystemFirebirdClient fBClient = new SystemFirebirdClient(
		//		Properties.Settings.Default.MisInfoclinicaDbAddress,
		//		Properties.Settings.Default.MisInfoclinicaDbName,
		//		Properties.Settings.Default.MisInfoclinicaDbUser,
		//		Properties.Settings.Default.MisInfoclinicaDbPassword);

		//	Dictionary<string, object> surveyResults = new Dictionary<string, object>() {
		//		{ "@dcode", surveyResult.DCode },
		//		{ "@docrate", surveyResult.DocRate },
		//		{ "@comment", surveyResult.Comment },
		//		{ "@phonenumber", surveyResult.PhoneNumber },
		//		{ "@clinicrate", surveyResult.ClinicRecommendMark },
		//		{ "@photopath", surveyResult.PhotoLink },
		//		{ "@depnum", surveyResult.DocDeptCode }
		//	};

		//	string query = Properties.Settings.Default.SqlInsertSurveyResultWithoutEmotion;

		//	//if (surveyResult.EmotionObject != null) {
		//	//	surveyResults.Add("@em_anger", surveyResult.EmotionObject.Scores.Anger);
		//	//	surveyResults.Add("@em_contempt", surveyResult.EmotionObject.Scores.Contempt);
		//	//	surveyResults.Add("@em_disgust", surveyResult.EmotionObject.Scores.Disgust);
		//	//	surveyResults.Add("@em_fear", surveyResult.EmotionObject.Scores.Fear);
		//	//	surveyResults.Add("@em_happiness", surveyResult.EmotionObject.Scores.Happiness);
		//	//	surveyResults.Add("@em_neutral", surveyResult.EmotionObject.Scores.Neutral);
		//	//	surveyResults.Add("@em_sadness", surveyResult.EmotionObject.Scores.Sadness);
		//	//	surveyResults.Add("@em_surprice",  surveyResult.EmotionObject.Scores.Surprise);
		//	//	query = Properties.Settings.Default.SqlInsertSurveyResultWithEmotion;
		//	//}
			
		//	surveyResult.IsInsertedToDb = fBClient.ExecuteUpdateQuery(query, surveyResults);

		//	SystemLogging.ToLog("Результат выполнения: " + surveyResult.IsInsertedToDb);
		//}



		private void ClassPageTemplate_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
			//SystemLogging.ToLog("Видимость страницы " + sender.GetType().Name +
			//	" изменилась с " + e.OldValue + " на " + e.NewValue);

			//if (this is PageSplashScreen ||
			//	this is PageError)
			//	return;

			if ((bool)e.NewValue)
				ResetTimer();
			else
				DisablePageAutoCloseTimer();
		}

		private void DispatcherTimerPageAutoClose_Tick(object sender, EventArgs e) {
			//SystemLogging.ToLog("Истекло время таймера автозакрытия страницы");
			FireUpTimerPageAutoClose();
		}

		private void ClassPageTemplate_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			ResetTimer();
		}

		protected void DisablePageAutoCloseTimerResetByClick() {
			PreviewMouseLeftButtonDown -= ClassPageTemplate_PreviewMouseLeftButtonDown;
		}

		protected void DisablePageAutoCloseTimer() {
			_dispatcherTimerPageAutoClose.Stop();
		}

		private void ResetTimer() {
			_dispatcherTimerPageAutoClose.Stop();
			_dispatcherTimerPageAutoClose.Start();
		}

		protected void FireUpTimerPageAutoClose(bool showDepartmentSelect = false) {
			//if (_surveyResult != null) {
			//	if (this is PageCallback)
			//		_surveyResult.PhoneNumber = "Timeout";
			//	else if (this is PageClinicRate)
			//		_surveyResult.ClinicRecommendMark = "Timeout";
			//	else if (this is PageComment)
			//		_surveyResult.Comment = "Timeout";

			//	WriteSurveyResultToDb(_surveyResult);
			//}

			_dispatcherTimerPageAutoClose.Stop();
			CloseAllPagesExceptSplashScreen(showDepartmentSelect);
		}
	}
}
