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

		protected double _leftCornerShadow = 0;
		protected double _rightCornerShadow = 5;

		protected ItemSurveyResult _surveyResult = null;











		public static void FillPanelWithElements(WrapPanel wrapPanel,
										   object[] elements,
										   ControlsFactory.ElementType type,
										   RoutedEventHandler eventHandler,
										   int elementsInRow) {
			double elementsCreated = 0;
			double totalElementsCreated = 0;
			double linesCreated = 0;
			double totalLines = Math.Ceiling(elements.Length / (double)elementsInRow);

			double currentX;// = 0;
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
			foreach (object element in elements) {
				if (element is string && string.IsNullOrEmpty((string)element))
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
	}
}
