using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PhatWare.WritePad;

namespace WritePadSDKWindowsFormsSample
{
    public partial class Form1 : Form
    {
        public static Point _previousContactPt;
        public static Point _currentContactPt;

        public Recognizer myRecognizer;

        public List<Point> points = new List<Point>();
        public LinkedList<List<Point>> polylineList = new LinkedList<List<Point>>();

        public void CreateRecognizer(Language language = Language.English)
        {
            DestroyRecognizer();

            myRecognizer = new Recognizer(language, AppDomain.CurrentDomain.BaseDirectory);
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

        public Form1()
        {
            InitializeComponent();

            var names = Array.ConvertAll(Recognizer.SupportedLanguages, l => l.ToString());
            LanguagesCombo.Items.AddRange(names);

            panelCanvas.polylineList = polylineList;

            CreateRecognizer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = RecognizeStrokes(polylineList, false);
            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Text could not be recognized.");
                result = "";
            }

            textBox1.Text = result;

        }

        private void ClearInk()
        {
            polylineList.Clear();
            if (panelCanvas != null)
                panelCanvas.Invalidate();
        }

        public struct WordAlternative
        {
            public string Word;
            public int Weight;
        }

        /// <summary>
        /// Recognizes a collection of Polyline objects into text. Words are recognized with alternatives, each
        /// weighted with probability. 
        /// </summary>
        /// <param name="strokes">Strokes to recognize</param>
        /// <returns></returns>
        public string RecognizeStrokes(LinkedList<List<Point>> strokes, bool bLearn)
        {
            var ink = new Ink();

            foreach (var polyline in strokes)
            {
                var line = polyline.Select(l => new InkTracePoint(l.X, l.Y));
                ink.AddStroke(line, 3, 0xFFFF0000);
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
                        }
                        );
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

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _currentContactPt = e.Location;
                panelCanvas.AddPixelToStroke();

                panelCanvas.Invalidate();
            }
        }

        private void panelCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            polylineList.AddLast(new List<Point>());
        }

        private void panelCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            var currentStroke = polylineList.Last().Select(s => new InkTracePoint(s.X, s.Y)).ToArray();

            var gesture = GestureRecognizer.CheckGesture(
                Gestures.Return | Gestures.Cut | Gestures.Back | Gestures.BackLong,
                currentStroke);

            switch (gesture)
            {
                case Gestures.Return:
                    polylineList.RemoveLast();
                    Refresh();
                    var result = RecognizeStrokes(polylineList, true);
                    if (string.IsNullOrEmpty(result))
                    {
                        MessageBox.Show("Text could not be recognized.");
                        result = "";
                    }
                    textBox1.Text = result;
                    break;
                case Gestures.Cut:
                    ClearInk();
                    Refresh();
                    break;
                case Gestures.Back:
                case Gestures.BackLong:
                    polylineList.RemoveLast();
                    if (polylineList.Count > 0)
                    {
                        polylineList.RemoveLast();
                    }
                    Refresh();
                    break;
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearInk();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LanguagesCombo.SelectedItem = myRecognizer.Language.ToString();
        }

        private void LanguagesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LanguagesCombo.SelectedIndex != -1)
            {
                ClearInk();

                var lang = (Language)Enum.Parse(typeof(Language), (string)LanguagesCombo.SelectedItem);
                CreateRecognizer(lang);
            }
        }

        private void buttonOptions_Click(object sender, EventArgs e)
        {
            new Options(myRecognizer).ShowDialog(this);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ClearInk();
            DestroyRecognizer();

            base.OnFormClosing(e);
        }
    }

    public class CustomPanel : Panel
    {
        private const int GRID_GAP = 65;

        public LinkedList<List<Point>> polylineList = new LinkedList<List<Point>>();

        public CustomPanel()
        {
            DoubleBuffered = true;
        }

        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public void AddPixelToStroke()
        {
            var _x1 = Form1._previousContactPt.X;
            var _y1 = Form1._previousContactPt.Y;
            var _x2 = Form1._currentContactPt.X;
            var _y2 = Form1._currentContactPt.Y;
            var currentStroke = polylineList.Last();

            if (Distance(_x1, _y1, _x2, _y2) > 2.0)
            {
                PixelAdder.AddPixels(_x2, _y2, false, ref currentStroke);
                Form1._previousContactPt = Form1._currentContactPt;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(e);

            var bluePen = new Pen(Color.Blue) { Width = 3.0F };
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            // Call methods of the System.Drawing.Graphics object.
            foreach (var polyline in polylineList.Where(polyline => polyline.Count > 2))
            {
                e.Graphics.DrawLines(bluePen, polyline.ToArray());
            }
            for (int y = GRID_GAP; y < Height; y += GRID_GAP)
            {
                var gridLine = new[] { new Point(0, y), new Point(Width, y) };
                e.Graphics.DrawLines(Pens.Red, gridLine.ToArray());
            }
        }
    }
}
