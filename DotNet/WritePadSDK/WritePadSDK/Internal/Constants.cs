/* ************************************************************************************* */
/* *    PhatWare WritePad SDK                                                          * */
/* *    Copyright (c) 2008-2015 PhatWare(r) Corp. All rights reserved.                 * */
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

namespace PhatWare.WritePad
{
    internal static class Constants
    {
#if __IOS__
        internal const string LibraryName = "__Internal";
#elif __ANDROID__
        internal const string LibraryName = "libWritePadReco.so";
#elif __WINDOWS_FORMS__
        internal const string LibraryName = "WritePadReco.dll";
#endif
    }
}

//#define HW_RECINT_ID_001		0x01000002  /* Rec Interface ID */
//#define HW_MAX_SPELL_NUM_ALTS	10			/* How many variants will be out by the SpellCheck func */
//#define HW_RECID_MAXLEN			32			/* Max length of the RecID string */
//#define HW_MAX_FILENAME			128			/* Limit for filename buffer */

//// Bits of recognizer capabilities

//#define HW_CPFL_CURS			0x0001      /* Cursive capable */
//#define HW_CPFL_TRNBL			0x0002      /* Training capable */
//#define HW_CPFL_SPVSQ			0x0004      /* Speed VS Quality control capable */
//#define HW_CPFL_INTER			0x0008      /* International support capable */

//#define HW_MAXWORDLEN			50			/* maximum word length */

//#define HW_NUM_ANSWERS			1			/* Request to get number of recognized words */
//#define HW_NUM_ALTS				2			/* Request number of alternatives for given word */
//#define HW_ALT_WORD				3			/* Requestto get pointer to a given word alternative */
//#define HW_ALT_WEIGHT			4			/* Request to get weight of a give word alternative */
//#define HW_ALT_NSTR				5			/* Request to get number of strokes used for a given word alternative */
//#define HW_ALT_STROKES			6			/* Request to get a pointer to a given word alternative stroke ids */

//#define MIN_RECOGNITION_WEIGHT  51			/* Minimum recognition quality */
//#define MAX_RECOGNITION_WEIGHT  100			/* Maximum recognition quality */
//#define AVE_RECOGNITION_WEIGHT	((MIN_RECOGNITION_WEIGHT+MAX_RECOGNITION_WEIGHT)/2)

//#define LRN_WEIGHTSBUFFER_SIZE	448
//#define LRN_SETDEFWEIGHTS_OP	0			/* LEARN interface commands for RecoGetSetPictWghts func */
//#define LRN_GETCURWEIGHTS_OP	1
//#define LRN_SETCURWEIGHTS_OP	2

//#define PM_ALTSEP               1			/* Recognized word list alternatives separator */
//#define PM_LISTSEP              2			/* Recognized word list wordlist separator */
//#define PM_LISTEND              0			/* Recognized word list end */

//#define PM_NUMSEPARATOR			(-1)


//#define HW_RECINT_UNICODE        1           // NOTE: define to use Unicode (UTF-16)


//#define RW_WEIGHTMASK		0x000000FF
//#define RW_DICTIONARYWORD	0x00004000

//// Autocorrector flags
//#define WCF_IGNORECASE		0x0001
//#define WCF_ALWAYS			0x0002
//#define WCF_DISABLED		0x0004

///* ------------------------- Language ID ------------------------------------- */

//#define DEFAULT_PRESSURE        150
//#define MAX_PRESSURE            255
//#define MIN_PRESSURE            5

//#ifndef WIN32
//#define GetRValue(rgb)      ((float)((rgb)&0xFF)/255.0)
//#define GetGValue(rgb)      ((float)(((rgb)>>8)&0xFF)/255.0)
//#define GetBValue(rgb)      ((float)(((rgb)>>16)&0xFF)/255.0)
//#endif // WIN32

//#define GetAValue(rgb)      ((float)(((rgb)>>24)&0xFF)/255.0)
//#define RGBA(r,g,b,a)       ((COLORREF)(((unsigned char)(r)|((unsigned int)((unsigned char)(g))<<8))|(((unsigned int)(unsigned char)(b))<<16)|(((unsigned int)(unsigned char)(a))<<24)))
//#define CCTB(cc)			((unsigned char)(cc * (float)0xFF))


//#define MAX_TRACE_LENGTH		4096
//#define TRACE_BREAK_LENGTH		200

//#define MAX_STRING_BUFFER		2048

//#define READ_FLAG                   0x01
//#define MEM_STREAM_FLAG             0x02
//#define INK_FMT_MASK                0x3C

//#define INK_RAW                     0x01
//#define INK_CALCOMP                 0x02
//#define INK_PWCOMP                  0x03
//#define INK_JPEG                    0x04
//#define INK_DATA                    0x05
//#define INK_PNG                     0x06

//#define IGNORE_LAST_STROKE          0x0001000
//#define SORT_STROKES                0x0002000
//#define SAVE_PRESSURE				0x0004000

//#define MAKE_READ_FMT( dwDataFmt, bMemStream )  ( ((dwDataFmt) << 2L) | ((bMemStream)?MEM_STREAM_FLAG:0) | READ_FLAG )
//#define MAKE_WRITE_FMT( dwDataFmt, bMemStream ) ( ((dwDataFmt) << 2L) | ((bMemStream)?MEM_STREAM_FLAG:0) )
//#define INK_DATA_FMT( dwFlags )                 ( ((dwFlags) & INK_FMT_MASK) >> 2L )
//#define INK_READ( dwFlags )                     ( (dwFlags) & READ_FLAG )
//#define INK_WRITE( dwFlags )                    ( ((dwFlags) & READ_FLAG) == 0 )
//#define IS_MEM_STREAM( dwFlags )                ( (dwFlags) & MEM_STREAM_FLAG )
//#define IS_FILE_STREAM( dwFlags )               ( ((dwFlags) & MEM_STREAM_FLAG) == 0 )
