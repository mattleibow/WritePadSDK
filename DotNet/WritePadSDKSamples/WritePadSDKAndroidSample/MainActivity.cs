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
using Android.Support.V7.App;
using Android.Views;
using Android.OS;
using PhatWare.WritePad;

namespace WritePadSDKAndroidSample
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat")]
    public class MainActivity : AppCompatActivity
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

        protected override void OnDestroy()
        {
            DestroyRecognizer();

            base.OnDestroy();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            CreateRecognizer();

            SetContentView(Resource.Layout.Main);

            if (bundle == null)
            {
                SupportFragmentManager.BeginTransaction()
                     .Add(Resource.Id.container, new DrawingFragment())
                     .Commit();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.OptionsButton)
            {
                SupportFragmentManager.BeginTransaction()
                     .Replace(Resource.Id.container, new OptionsFragment())
                     .AddToBackStack(null)
                     .Commit();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}
