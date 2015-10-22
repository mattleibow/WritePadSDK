using System;
using UIKit;

using PhatWare.WritePad;
using Foundation;
using System.Collections.Generic;
using System.Linq;

namespace WritePadSDKiOSSample
{
    public partial class MainViewController : UIViewController
    {
        public Recognizer myRecognizer;

        public void CreateRecognizer(Language language = Language.English)
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

        public MainViewController(IntPtr handle)
            : base(handle)
        {
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CreateRecognizer();

            inkView.OnReturnGesture += () =>
            {
                recognizedText.Text = Recognize(true);
                inkView.cleanView(true);
            };
            inkView.OnCutGesture += () => inkView.cleanView(true);

            recognizeAllButton.Clicked += delegate
            {
                recognizedText.Text = Recognize(false);
            };

            clearButton.Clicked += delegate
            {
                inkView.cleanView(true);
            };
            languageButton.Clicked += delegate
            {
                var languages = Recognizer.SupportedLanguages;
                var names = Array.ConvertAll(languages, l => l.ToString());

                var actionSheet = new UIActionSheet("Select Language:", null, "Cancel", null, names);
                actionSheet.Clicked += (sender, args) =>
                {
                    inkView.cleanView(true);

                    var lang = languages[args.ButtonIndex];
                    CreateRecognizer(lang);
                };
                actionSheet.ShowInView(View);
            };

            optionsButton.Clicked += delegate
            {
                PerformSegue("showOptions", null);
            };
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "showOptions")
            {
                var options = (OptionsViewController)sender;
                options.myRecognizer = myRecognizer;
            }
        }

        public struct WordAlternative
        {
            public string Word;
            public int Weight;
        }

        public string Recognize(bool bLearn)
        {
            var res = "";
            var resultStringList = new List<string>();
            var wordList = new List<List<WordAlternative>>();

            string defaultResult = myRecognizer.RecognizeInkData(inkView.Ink);

            // can also use the default result
            resultStringList.Add(defaultResult);
            var wordCount = myRecognizer.GetResultWordCount();
            for (var i = 0; i < wordCount; i++)
            {
                var wordAlternativesList = new List<WordAlternative>();
                var altCount = myRecognizer.GetResultAlternativeCount(i);
                for (var j = 0; j < altCount; j++)
                {
                    String word = myRecognizer.GetResultWord(i, j);
                    if (word == "<--->")            // note: when a word is not recognized, engine returns "<--->" instead
                        word = "*Error*";
                    if (string.IsNullOrEmpty(word))
                        continue;

                    var flags = myRecognizer.RecognitionFlags;
                    var weight = myRecognizer.GetResultWeight(i, j);
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
                            emptyStr += "\t\t";
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
            // cleanView (true);
            return res;
        }
    }
}
