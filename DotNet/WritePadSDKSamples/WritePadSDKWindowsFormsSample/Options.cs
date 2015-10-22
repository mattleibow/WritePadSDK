/* ************************************************************************************* */
/* *    PhatWare WritePad SDK                                                           * */
/* ************************************************************************************* */

/* ************************************************************************************* *
 *
 * Unauthorized distribution of this code is prohibited. For more information
 * refer to the End User Software License Agreement provided with this 
 * software.
 *
 * This source code is distributed and supported by PhatWare Corp.
 * http://www.phatware.com
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
 * 530 Showers Drive Suite 7 #333 Mountain View, CA 94040
 *
 * ************************************************************************************* */

using System;
using System.Windows.Forms;

using PhatWare.WritePad;

namespace WritePadSDKWindowsFormsSample
{
    public partial class Options : Form
    {
        private Recognizer myRecognizer;

        public Options(Recognizer recognizer)
        {
            InitializeComponent();

            myRecognizer = recognizer;
        }

        private void Options_Load(object sender, EventArgs e)
        {
            var flags = myRecognizer.RecognitionFlags;
            SeparateLetters.Checked = flags.HasFlag(RecognitionFlags.SEPLET);
            DisableSegmentation.Checked = flags.HasFlag(RecognitionFlags.SINGLEWORDONLY);
            AutoLearner.Checked = flags.HasFlag(RecognitionFlags.ANALYZER);
            AutoCorrector.Checked = flags.HasFlag(RecognitionFlags.CORRECTOR);
            UserDictionary.Checked = flags.HasFlag(RecognitionFlags.USERDICT);
            DictionaryOnly.Checked = flags.HasFlag(RecognitionFlags.ONLYDICT);
        }

        private void SeparateLetters_CheckedChanged(object sender, EventArgs e)
        {
            myRecognizer.RecognitionFlags = Form1.SetFlag(myRecognizer.RecognitionFlags, SeparateLetters.Checked, RecognitionFlags.SEPLET);
        }

        private void DisableSegmentation_CheckedChanged(object sender, EventArgs e)
        {
            myRecognizer.RecognitionFlags = Form1.SetFlag(myRecognizer.RecognitionFlags, DisableSegmentation.Checked, RecognitionFlags.SINGLEWORDONLY);
        }

        private void AutoLearner_CheckedChanged(object sender, EventArgs e)
        {
            myRecognizer.RecognitionFlags = Form1.SetFlag(myRecognizer.RecognitionFlags, AutoLearner.Checked, RecognitionFlags.ANALYZER);
        }

        private void AutoCorrector_CheckedChanged(object sender, EventArgs e)
        {
            myRecognizer.RecognitionFlags = Form1.SetFlag(myRecognizer.RecognitionFlags, AutoCorrector.Checked, RecognitionFlags.CORRECTOR);
        }

        private void UserDictionary_CheckedChanged(object sender, EventArgs e)
        {
            myRecognizer.RecognitionFlags = Form1.SetFlag(myRecognizer.RecognitionFlags, UserDictionary.Checked, RecognitionFlags.USERDICT);
        }

        private void DictionaryOnly_CheckedChanged(object sender, EventArgs e)
        {
            myRecognizer.RecognitionFlags = Form1.SetFlag(myRecognizer.RecognitionFlags, DictionaryOnly.Checked, RecognitionFlags.ONLYDICT);
        }
    }
}
