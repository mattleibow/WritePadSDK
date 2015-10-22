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

using Android.App;
using Android.OS;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Views;
using Android.Widget;

using PhatWare.WritePad;

namespace WritePadSDKAndroidSample
{
    public class OptionsFragment : Fragment
    {
        private MainActivity MainActivity
        {
            get { return (MainActivity)Activity; }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Options, container, false);

            var seplet = view.FindViewById<CheckBox>(Resource.Id.separate_letters);
            var singleword = view.FindViewById<CheckBox>(Resource.Id.single_word);
            var corrector = view.FindViewById<CheckBox>(Resource.Id.autocorrector);
            var learner = view.FindViewById<CheckBox>(Resource.Id.autolearner);
            var userdict = view.FindViewById<CheckBox>(Resource.Id.user_dictionary);
            var dictwords = view.FindViewById<CheckBox>(Resource.Id.dict_words);

            var recognizer = MainActivity.myRecognizer;

            var recoFlags = recognizer.RecognitionFlags;
            seplet.Checked = recoFlags.HasFlag(RecognitionFlags.SEPLET);
            singleword.Checked = recoFlags.HasFlag(RecognitionFlags.SINGLEWORDONLY);
            learner.Checked = recoFlags.HasFlag(RecognitionFlags.ANALYZER);
            userdict.Checked = recoFlags.HasFlag(RecognitionFlags.USERDICT);
            dictwords.Checked = recoFlags.HasFlag(RecognitionFlags.ONLYDICT);
            corrector.Checked = recoFlags.HasFlag(RecognitionFlags.CORRECTOR);

            seplet.Click += delegate
            {
                recognizer.RecognitionFlags = MainActivity.SetFlag(recognizer.RecognitionFlags, seplet.Checked, RecognitionFlags.SEPLET);
            };
            singleword.Click += delegate
            {
                recognizer.RecognitionFlags = MainActivity.SetFlag(recognizer.RecognitionFlags, singleword.Checked, RecognitionFlags.SINGLEWORDONLY);
            };
            learner.Click += delegate
            {
                recognizer.RecognitionFlags = MainActivity.SetFlag(recognizer.RecognitionFlags, learner.Checked, RecognitionFlags.ANALYZER);
            };
            userdict.Click += delegate
            {
                recognizer.RecognitionFlags = MainActivity.SetFlag(recognizer.RecognitionFlags, userdict.Checked, RecognitionFlags.USERDICT);
            };
            dictwords.Click += delegate
            {
                recognizer.RecognitionFlags = MainActivity.SetFlag(recognizer.RecognitionFlags, dictwords.Checked, RecognitionFlags.ONLYDICT);
            };
            corrector.Click += delegate
            {
                recognizer.RecognitionFlags = MainActivity.SetFlag(recognizer.RecognitionFlags, corrector.Checked, RecognitionFlags.CORRECTOR);
            };

            return view;
        }
    }
}
