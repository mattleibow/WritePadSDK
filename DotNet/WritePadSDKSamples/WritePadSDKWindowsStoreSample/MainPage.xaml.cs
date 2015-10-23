using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using PhatWare.WritePad;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Input;
using Windows.Devices.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WritePadSDKWindowsStoreSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Recognizer myRecognizer;

        public const float GRID_GAP = 65;

        private uint _currentPointerId;

        private Point _previousContactPt;
        private Point _currentContactPt;
        private double _x1;
        private double _y1;
        private double _x2;
        private double _y2;

        private readonly Polyline _currentStroke = new Polyline
        {
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round,
            StrokeLineJoin = PenLineJoin.Round
        };

        public void CreateRecognizer(Language language = PhatWare.WritePad.Language.English)
        {
            DestroyRecognizer();

            myRecognizer = new Recognizer(language);
            var flags = myRecognizer.RecognitionFlags;
            flags = SetFlag(flags, true, RecognitionFlags.CORRECTOR);
            flags = SetFlag(flags, false, RecognitionFlags.SEPLET);
            flags = SetFlag(flags, false, RecognitionFlags.ONLYDICT);
            flags = SetFlag(flags, false, RecognitionFlags.SINGLEWORDONLY);
            flags = SetFlag(flags, true, RecognitionFlags.USERDICT);
            flags = SetFlag(flags, false, RecognitionFlags.ANALYZER);
            flags = SetFlag(flags, false, RecognitionFlags.NOSPACE);
            myRecognizer.RecognitionFlags = flags;
        }

        public static RecognitionFlags SetFlag(RecognitionFlags flags, bool value, RecognitionFlags flag)
        {
            bool isEnabled = 0 != (flags & flag);
            if (value && !isEnabled)
            {
                flags |= flag;
            }
            else if (!value && isEnabled)
            {
                flags &= ~flag;
            }
            return flags;
        }

        private void DestroyRecognizer()
        {
            if (myRecognizer != null)
            {
                myRecognizer.Dispose();
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            
            CreateRecognizer();
            Application.Current.Suspending += (sender, e) =>
            {
                DestroyRecognizer();
            };
        }

        private async void RecognizeAllClick(object sender, RoutedEventArgs e)
        {
            var result = RecognizeStrokes(InkCanvas.Children.OfType<Polyline>(), false);
            if (string.IsNullOrEmpty(result))
            {
                var messageBox = new MessageDialog("Text could not be recognized.");
                messageBox.Commands.Add(new UICommand("Close"));
                await messageBox.ShowAsync();
                result = "";
            }
            RecognizedTextBox.Text = result;
        }

        private void ClearAllClick(object sender, RoutedEventArgs e)
        {
            ClearInk();
        }

        private void StartAddingStroke(Point pt)
        {
            _previousContactPt = pt;

            _currentStroke.Points.Clear();
            var points = _currentStroke.Points;
            PixelAdder.AddPixels(pt.X, pt.Y, false, ref points);
            _currentStroke.Points = points;

            _currentStroke.StrokeThickness = 5;
            _currentStroke.Stroke = new SolidColorBrush(Colors.Blue);
            _currentStroke.Opacity = 1;
            InkCanvas.Children.Add(_currentStroke);
        }

        public double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public void DrawGrid()
        {
            for (float y = GRID_GAP; y < InkCanvas.ActualHeight; y += GRID_GAP)
            {
                var line = new Line
                {
                    Stroke = new SolidColorBrush(Colors.Red),
                    X1 = 0,
                    Y1 = y,
                    X2 = InkCanvas.ActualWidth,
                    Y2 = y
                };
                InkCanvas.Children.Add(line);
            }
        }

        private void AddPixelToStroke()
        {
            _x1 = _previousContactPt.X;
            _y1 = _previousContactPt.Y;
            _x2 = _currentContactPt.X;
            _y2 = _currentContactPt.Y;

            var color = Colors.Blue;
            var size = 10;

            if (Distance(_x1, _y1, _x2, _y2) > 2.0)
            {
                if (_currentStroke.Points.Count == 0)
                {
                    _currentStroke.StrokeThickness = size;
                    _currentStroke.Stroke = new SolidColorBrush(color);
                    try
                    {
                        InkCanvas.Children.Remove(_currentStroke);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        InkCanvas.Children.Add(_currentStroke);
                    }
                    catch (Exception)
                    {
                    }
                }
                var points = _currentStroke.Points;
                PixelAdder.AddPixels(_x2, _y2, false, ref points);
                _currentStroke.Points = points;

                _previousContactPt = _currentContactPt;
            }
        }

        /// <summary>
        /// Recognizes a collection of Polyline objects into text. Words are recognized with alternatives, eash weighted with probability. 
        /// </summary>
        /// <param name="strokes">Strokes to recognize</param>
        /// <returns></returns>
        public string RecognizeStrokes(IEnumerable<Polyline> strokes, bool bLearn)
        {
            var ink = new Ink();

            foreach (var polyline in strokes)
            {
                var points = polyline.Points.Select(p => new InkTracePoint((float)p.X, (float)p.Y));
                ink.AddStroke(points, (int)polyline.StrokeThickness - 1, 0xFF0000FF);
            }

            var res = "";
            var resultStringList = new List<string>();
            var wordList = new List<List<WordAlternative>>();
            var defaultResult = myRecognizer.RecognizeInkData(ink);
            resultStringList.Add(defaultResult);
            var wordCount = myRecognizer.GetResultWordCount();
            for (var i = 0; i < wordCount; i++)
            {
                var wordAlternativesList = new List<WordAlternative>();
                var altCount = myRecognizer.GetResultAlternativeCount(i);
                for (var j = 0; j < altCount; j++)
                {
                    var word = myRecognizer.GetResultWord(i, j);
                    if (word == Recognizer.EmptyWordString)
                        word = "*Error*";
                    if (string.IsNullOrEmpty(word))
                        continue;
                    var flags = myRecognizer.RecognitionFlags;
                    var weight = myRecognizer.GetResultWeight(i, j);
                    if (weight == 0)
                    {
                        continue;
                    }
                    if (j == 0 && bLearn && weight > 75 && 0 != (flags & RecognitionFlags.ANALYZER))
                    {
                        // if learner is enabled, learn default word(s) when the Return gesture is used
                        myRecognizer.LearnNewWord(word, weight);
                    }
                    if (wordAlternativesList.All(x => x.Word != word))
                    {
                        wordAlternativesList.Add(new WordAlternative
                        {
                            Word = word,
                            Weight = weight
                        });
                    }
                    while (resultStringList.Count < j + 2)
                    {
                        var emptyStr = "";
                        for (int k = 0; k < i; k++)
                        {
                            emptyStr += "\t";
                        }
                        resultStringList.Add(emptyStr);
                    }
                    if (resultStringList[j + 1].Length > 0)
                        resultStringList[j + 1] += "\t\t";
                    resultStringList[j + 1] += word + "\t[" + weight + "%]";
                }
                wordList.Add(wordAlternativesList);
            }

            foreach (var line in resultStringList)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                if (res.Length > 0)
                {
                    res += Environment.NewLine;
                }
                res += line;
            }

            return res;
        }

        public void ClearInk()
        {
            InkCanvas.Children.Clear();
            DrawGrid();
        }

        public void FinishStrokeDraw()
        {
            if (_currentStroke.Points.Count == 1)
            {
                var newPoint = _currentStroke.Points[0];
                _currentStroke.Points.Add(new Point(newPoint.X + 1, newPoint.Y));
            }
            if (_currentStroke.Points.Count > 0)
            {
                AddStroke(_currentStroke);
            }
        }

        public void AddStroke(Polyline currentStroke)
        {
            var points = new PointCollection();
            foreach (var point in currentStroke.Points)
            {
                points.Add(point);
            }
            var polyline = new Polyline
            {
                Stroke = currentStroke.Stroke,
                StrokeThickness = currentStroke.StrokeThickness,
                Points = points,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round
            };

            InkCanvas.Children.Add(polyline);
            InkCanvas.Children.Remove(currentStroke);
            currentStroke.Points.Clear();
        }

        public async void OnCanvasPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId != _currentPointerId)
                return;
            if (e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonReleased)
                return;

            var currentStroke = _currentStroke.Points.Select(s => new InkTracePoint((float)s.X, (float)s.Y)).ToArray();

            var gesture = PhatWare.WritePad.GestureRecognizer.CheckGesture(
                Gestures.Return | Gestures.Cut,
                currentStroke);

            switch (gesture)
            {
                case Gestures.Return:
                    InkCanvas.Children.Remove(_currentStroke);
                    _currentStroke.Points.Clear();
                    var result = RecognizeStrokes(InkCanvas.Children.OfType<Polyline>(), true);
                    if (string.IsNullOrEmpty(result))
                    {
                        var messageBox = new MessageDialog("Text could not be recognized.");
                        messageBox.Commands.Add(new UICommand("Close"));
                        await messageBox.ShowAsync();
                        result = "";
                    }
                    RecognizedTextBox.Text = result;
                    break;
                case Gestures.Cut:
                    ClearInk();
                    break;
            }

            FinishStrokeDraw();
        }

        public void OnCanvasPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId != _currentPointerId)
                return;
            var currentPoint = e.GetCurrentPoint(InkCanvas).Position;

            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                var properties = e.GetCurrentPoint(null).Properties;
                if (properties.IsRightButtonPressed || properties.IsMiddleButtonPressed || !properties.IsLeftButtonPressed)
                {
                    return;
                }
            }

            if (!e.Pointer.IsInContact)
                return;

            _currentContactPt = currentPoint;
            AddPixelToStroke();
        }

        public void OnCanvasPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _currentPointerId = e.Pointer.PointerId;

            var pressPoint = e.GetCurrentPoint(InkCanvas).Position;
            StartAddingStroke(pressPoint);
        }

        private void InkCanvas_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            OnCanvasPointerReleased(sender, e);
        }

        private void OnCanvasPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            OnCanvasPointerReleased(sender, e);
        }

        public struct WordAlternative
        {
            public string Word;
            public int Weight;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DrawGrid();
        }
    }
}
