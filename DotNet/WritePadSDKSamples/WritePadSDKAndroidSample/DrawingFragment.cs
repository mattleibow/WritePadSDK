/* ************************************************************************************* */
/* *    PhatWare WritePad SDK                                                          * */
/* *    Copyright (c) 2008-2015 PhatWare(r) Corp. All rights reserved.                 * */
/* ************************************************************************************* */

/* ************************************************************************************* *
 *
 * WritePad SDK Xamarin Sample for Android
 *
 * Unauthorized distribution of this code is prohibited. For more information
 * refer to the End User Software License Agreement provided with this
 * software.
 *
 * This source code is distributed and supported by PhatWare Corp.
 * http://www.phatware.com
 *
 * THIS SAMPLE CODE CAN BE USED  AS A REFERENCE AND, IN ITS BINARY FORM,
 * IN THE USER'S PROJECT WHICH IS INTEGRATED WITH THE WRITEPAD SDK.
 * ANY OTHER USE OF THIS CODE IS PROHIBITED.
 *
 * THE MATERIAL EMBODIED ON THIS SOFTWARE IS PROVIDED TO YOU "AS-IS"
 * AND WITHOUT WARRANTY OF ANY KIND, EXPRESS, IMPLIED OR OTHERWISE,
 * INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. IN NO EVENT SHALL PHATWARE CORP.
 * BE LIABLE TO YOU OR ANYONE ELSE FOR ANY DIRECT, SPECIAL, INCIDENTAL,
 * INDIRECT OR CONSEQUENTIAL DAMAGES OF ANY KIND, OR ANY DAMAGES WHATSOEVER,
 * INCLUDING WITHOUT LIMITATION, LOSS OF PROFIT, LOSS OF USE, SAVINGS
 * OR REVENUE, OR THE CLAIMS OF THIRD PARTIES, WHETHER OR NOT PHATWARE CORP.
 * HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH LOSS, HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, ARISING OUT OF OR IN CONNECTION WITH THE
 * POSSESSION, USE OR PERFORMANCE OF THIS SOFTWARE.
 *
 * US Government Users Restricted Rights
 * Use, duplication, or disclosure by the Government is subject to
 * restrictions set forth in EULA and in FAR 52.227.19(c)(2) or subparagraph
 * (c)(1)(ii) of the Rights in Technical Data and Computer Software
 * clause at DFARS 252.227-7013 and/or in similar or successor
 * clauses in the FAR or the DOD or NASA FAR Supplement.
 * Unpublished-- rights reserved under the copyright laws of the
 * United States.  Contractor/manufacturer is PhatWare Corp.
 * 1314 S. Grand Blvd. Ste. 2-175 Spokane, WA 99202
 *
 * ************************************************************************************* */

using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Android.OS;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Fragment = Android.Support.V4.App.Fragment;
using Environment = System.Environment;

using PhatWare.WritePad;

namespace WritePadSDKAndroidSample
{
    public class DrawingFragment : Fragment
    {
        private MainActivity MainActivity
        {
            get { return (MainActivity)Activity; }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Drawing, container, false);

            var button = view.FindViewById<Button>(Resource.Id.RecognizeButton);
            var inkView = view.FindViewById<InkView>(Resource.Id.ink_view);
            var readyText = view.FindViewById<TextView>(Resource.Id.ready_text);
            var languageBtn = view.FindViewById<Button>(Resource.Id.LanguageButton);
            var optionsBtn = view.FindViewById<Button>(Resource.Id.OptionsButton);

            readyText.MovementMethod = new ScrollingMovementMethod();

            button.Click += delegate
            {
                readyText.Text = Recognize(inkView.Ink, false);
            };

            languageBtn.Click += delegate
            {
                var languages = Recognizer.SupportedLanguages;
                var selection = Array.IndexOf(languages, MainActivity.myRecognizer.Language);
                var names = Array.ConvertAll(languages, l => l.ToString());

                var builder = new AlertDialog.Builder(Activity);
                builder.SetTitle("Select language");
                builder.SetSingleChoiceItems(names, selection, (sender, args) =>
                {
                    inkView.CleanView(true);

                    if (args.Which >= 0 && args.Which < languages.Length)
                    {
                        var lang = languages[args.Which];
                        MainActivity.CreateRecognizer(lang);
                    }
                });
                builder.Show();
            };
            inkView.OnReturnGesture += () =>
            {
                readyText.Text = Recognize(inkView.Ink, true);
                inkView.CleanView(true);
            };
            inkView.OnCutGesture += () => inkView.CleanView(true);
            var clearbtn = view.FindViewById<Button>(Resource.Id.ClearButton);
            clearbtn.Click += delegate
            {
                readyText.Text = "";
                inkView.CleanView(true);
            };

            return view;
        }

        public struct WordAlternative
        {
            public string Word;
            public int Weight;
        }

        public string Recognize(Ink ink, bool bLearn)
        {
            var recognizer = MainActivity.myRecognizer;

            var res = "";
            var resultStringList = new List<string>();
            var wordList = new List<List<WordAlternative>>();
            string defaultResult = recognizer.RecognizeInkData(ink);
            // can also use the default result
            resultStringList.Add(defaultResult);
            var wordCount = recognizer.GetResultWordCount();
            for (var i = 0; i < wordCount; i++)
            {
                var wordAlternativesList = new List<WordAlternative>();
                var altCount = recognizer.GetResultAlternativeCount(i);
                for (var j = 0; j < altCount; j++)
                {
                    var word = recognizer.GetResultWord(i, j);
                    if (word == Recognizer.EmptyWordString)
                        word = "*Error*";
                    if (string.IsNullOrEmpty(word))
                        continue;
                    var weight = recognizer.GetResultWeight(i, j);
                    var flags = recognizer.RecognitionFlags;
                    if (j == 0 && bLearn && weight > 75 && 0 != (flags & RecognitionFlags.ANALYZER))
                    {
                        recognizer.LearnNewWord(word, weight);
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
                        resultStringList.Add("");
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
    }
}
