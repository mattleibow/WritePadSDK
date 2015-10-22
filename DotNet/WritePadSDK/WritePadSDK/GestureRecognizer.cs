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
using System.Runtime.InteropServices;

namespace PhatWare.WritePad
{
    [Flags]
    public enum Gestures : uint
    {
        None = 0x00000000,

        Delete = 0x00000001,
        ScrollUp = 0x00000002,
        Back = 0x00000004,
        Space = 0x00000008,
        Return = 0x00000010,
        Correct = 0x00000020,
        SpellCheck = 0x00000040,
        SelectAll = 0x00000080,
        Undo = 0x00000100,
        SmallPt = 0x00000200,
        Copy = 0x00000400,
        Cut = 0x00000800,
        Paste = 0x00001000,
        Tab = 0x00002000,
        Menu = 0x00004000,
        Loop = 0x00008000,
        Redo = 0x00010000,
        ScrollDown = 0x00020000,
        Save = 0x00040000,
        SendMain = 0x00080000,
        Options = 0x00100000,
        SendToDevices = 0x00200000,
        BackLong = 0x00400000,

        LeftArc = 0x10000000,
        RightArc = 0x20000000,
        Arcs = 0x30000000,

        Timeout = 0x40000000,
        Custom = 0x80000000,

        All = 0x0FFFFFFF
    }

    public static class GestureRecognizer
    {
        public const int LONG_STROKE_MINLENGTH = 200;

        public static Gestures CheckGesture(Gestures type, InkTracePoint[] stroke)
        {
            return CheckGesture(type, stroke, stroke.Length, 1, LONG_STROKE_MINLENGTH);
        }

        public static Gestures CheckGesture(Gestures type, InkTracePoint[] stroke, int len, int nScale, int nMinLen)
        {
            return GestureApi.HWR_CheckGesture(type, stroke, len, nScale, nMinLen);
        }
    }
}
