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

using System;

namespace PhatWare.WritePad
{
    [Flags]
    internal enum RecognizerControlFlags : uint
    {
        /// <summary>
        /// Do not perform segmentation at all
        /// </summary>
        NSEG = 0x0001,

        /// <summary>
        /// Do not allow segm not waiting for final stroke. (No results on the go)
        /// </summary>
        NCSEG = 0x0002,

        /// <summary>
        /// Perform read-ahead of tentative segmented words
        /// </summary>
        TTSEG = 0x0004,

        /// <summary>
        /// Enables international charsets
        /// </summary>
        INTL_CS = 0x0010,

        /// <summary>
        /// Enables international charsets
        /// </summary>
        ALPHAONLY = 0x0020,

        /// <summary>
        /// Alpha with custom punctuation
        /// </summary>
        CUSTOM_WITH_ALPHA = 0x0040,

        /// <summary>
        /// Enables separate letter mode
        /// </summary>
        SEPLET = 0x0100,

        /// <summary>
        /// Restricts dictionary words only recognition
        /// </summary>
        DICTONLY = 0x0200,

        /// <summary>
        /// NUMBERS only
        /// </summary>
        NUMONLY = 0x0400,

        /// <summary>
        /// CAPITALS only
        /// </summary>
        CAPSONLY = 0x0800,

        /// <summary>
        /// NUMBERS and CAPITALS modes do not use any other chars
        /// </summary>
        PURE = 0x1000,

        /// <summary>
        /// Internet address mode
        /// </summary>
        INTERNET = 0x2000,

        /// <summary>
        /// Static segmentation
        /// </summary>
        STATICSEG = 0x4000,

        /// <summary>
        /// use custom charset
        /// </summary>
        CUSTOM = 0x8000
    }
}
