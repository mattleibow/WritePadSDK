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
    internal struct ImageAttributes
    {
        public InkRect imagerect;
        public int iZOrder;
        public int nIndex;
        public IntPtr pData;
        public uint nDataSize;
        public IntPtr userData;
        public uint flags;
    }

    internal struct TextAttributes
    {
        public InkRect textrect;
        public int iZOrder;
        public int nIndex;
        public string pUnicodeText;
        public uint nTextLength;
        public string pFontName;
        public int fontSize;
        public uint fontAttributes;
        public uint alignment;
        public uint fontColor;
        public uint backColor;
        public IntPtr userData;
        public uint flags;
    }

    internal enum Shapes
    {
        UNKNOWN = 0,

        TRIANGLE = 0x0001,
        CIRCLE = 0x0002,
        ELLIPSE = 0x0004,
        RECTANGLE = 0x0008,
        LINE = 0x0010,
        ARROW = 0x0020,
        SCRATCH = 0x0040,

        ALL = 0x00FF
    }

    //#define LF_FONT_BOLD		            0x00000001
    //#define LF_FONT_ITALIC		        0x00000002
    //#define LF_FONT_UNDERSCORE	        0x00000004
    //#define LF_FONT_STRIKE		        0x00000008

    //#define OBJECTFLAG_POSITIONLOCKED     0x00010000
    //#define OBJECTFLAG_SIZELOCKED         0x00020000
    //#define OBJECTFLAG_CONTENTLOCKED      0x00040000
    //#define OBJECTFLAG_LOCKED             0x00070000
    //#define OBJECTFLAG_GROUPED            0x00100000

    internal static class InkApi
    {
        // Ink data API

        [DllImport(Constants.LibraryName, EntryPoint = "INK_InitData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr INK_InitData();

        [DllImport(Constants.LibraryName, EntryPoint = "INK_FreeData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_FreeData(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_Erase", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_Erase(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_StrokeCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_StrokeCount(IntPtr pData, bool selectedOnly);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_DeleteStroke", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_DeleteStroke(IntPtr pData, int nStroke);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_RecognizeShape", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern Shapes INK_RecognizeShape(InkTracePoint[] pStroke, int nStrokeCnt, Shapes inType);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_AddStroke", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_AddStroke(IntPtr pData, InkTracePoint[] pStroke, int nStrokeCnt, int iWidth, uint color);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetStroke", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_GetStroke(IntPtr pData, int nStroke, out IntPtr ppoints, out int nWidth, out uint color);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetStrokeP", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_GetStrokeP(IntPtr pData, int nStroke, out IntPtr ppoints, out int pnWidth, out uint pColor);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetStrokeRect", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_GetStrokeRect(IntPtr pData, int nStroke, out InkRect rect, bool bAddWidth);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetDataRect", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_GetDataRect(IntPtr pData, out InkRect rect, bool selectedOnly);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_AddEmptyStroke", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_AddEmptyStroke(IntPtr pData, int iWidth, uint color);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_AddPixelToStroke", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_AddPixelToStroke(IntPtr pData, int nStroke, float x, float y, int p);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetStrokePointP", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_GetStrokePointP(IntPtr pData, int nStroke, int nPoint, out float pX, out float pY, out int pP);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetStrokePoint", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_GetStrokePoint(IntPtr pData, int nStroke, int nPoint, out float pX, out float pY);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_CreateCopy", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr INK_CreateCopy(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SortInk", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_SortInk(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_Undo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_Undo(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_Redo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_Redo(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_CanRedo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_CanRedo(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_CanUndo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_CanUndo(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SelectAllStrokes", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_SelectAllStrokes(IntPtr pData, bool bSelect);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_DeleteSelectedStrokes", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_DeleteSelectedStrokes(IntPtr pData, bool bAll);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetStrokesRecognizable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_SetStrokesRecognizable(IntPtr pData, bool bSet, bool bSelectedOnly);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetStrokeRecognizable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_SetStrokeRecognizable(IntPtr pData, int nStroke, bool bSet);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SelectStroke", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_SelectStroke(IntPtr pData, int nStroke, bool bSelect);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_IsStrokeRecognizable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_IsStrokeRecognizable(IntPtr pData, int nStroke);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_IsStrokeSelected", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_IsStrokeSelected(IntPtr pData, int nStroke);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetUndoLevels", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_SetUndoLevels(IntPtr pData, int levels);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_Serialize", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_Serialize(IntPtr pData, bool bWrite, IntPtr pFile, out IntPtr ppData, out long pcbSize, bool skipImages, bool savePressure);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_Paste", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_Paste(IntPtr pData, IntPtr pRawData, long cbSize, InkPoint atPosition);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_Copy", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_Copy(IntPtr pData, out IntPtr ppRawData, out long pcbSize);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_MoveStroke", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_MoveStroke(IntPtr pData, int nStroke, float xOffset, float yOffset, out InkRect pRect, bool recordUndo);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_ChangeSelZOrder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_ChangeSelZOrder(IntPtr pData, int iDepth, bool bFwd);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_IsShapeRecognitionEnabled", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_IsShapeRecognitionEnabled(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_EnableShapeRecognition", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_EnableShapeRecognition(IntPtr pData, bool bEnable);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_FindStrokeByPoint", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_FindStrokeByPoint(IntPtr pData, InkPoint thePoint, float proximity);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SelectStrokesInRect", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_SelectStrokesInRect(IntPtr pData, InkRect selRect);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_EmptyUndoBuffer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void INK_EmptyUndoBuffer(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_CurveIntersectsStroke", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_CurveIntersectsStroke(IntPtr pData, int nStroke, InkTracePoint[] points, int nPointCount);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetStrokeWidthAndColor", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_SetStrokeWidthAndColor(IntPtr pData, int nStroke, uint color, int nWidth);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_DeleteIntersectedStrokes", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_DeleteIntersectedStrokes(IntPtr pData, InkTracePoint[] points, int nPointCount);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_ResizeStroke", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_ResizeStroke(IntPtr pData, int nStroke, float x0, float y0, float scalex, float scaley, bool bReset, out InkRect pRect, bool recordUndo);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetStrokeZOrder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_GetStrokeZOrder(IntPtr pData, int nStroke);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetStrokeZOrder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_SetStrokeZOrder(IntPtr pData, int nStroke, int iZOrder);


        // image support

        [DllImport(Constants.LibraryName, EntryPoint = "INK_AddImage", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_AddImage(IntPtr pData, ImageAttributes pImage);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetImage", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_SetImage(IntPtr pData, int nImageIndex, ImageAttributes pImage);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetImageUserData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_SetImageUserData(IntPtr pData, int nImageIndex, IntPtr userData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_DeleteImage", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_DeleteImage(IntPtr pData, int nImageIndex);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetImage", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_GetImage(IntPtr pData, int nImageIndex, out ImageAttributes pAttrib);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetImageFromPoint", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_GetImageFromPoint(IntPtr pData, InkPoint point, out ImageAttributes pAttrib);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_DeleteAllImages", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_DeleteAllImages(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_CountImages", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_CountImages(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetImageFrame", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_SetImageFrame(IntPtr pData, int nImageIndex, InkRect frame);

        // text support

        [DllImport(Constants.LibraryName, EntryPoint = "INK_AddText", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_AddText(IntPtr pData, TextAttributes pText);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetText", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_SetText(IntPtr pData, int nTextIndex, TextAttributes pText);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetTextUserData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_SetTextUserData(IntPtr pData, int nTextIndex, IntPtr userData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_DeleteText", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_DeleteText(IntPtr pData, int nTextIndex);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetText", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_GetText(IntPtr pData, int nTextIndex, out TextAttributes pText);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_GetTextFromPoint", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_GetTextFromPoint(IntPtr pData, InkPoint point, out TextAttributes pText);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_DeleteAllTexts", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_DeleteAllTexts(IntPtr pData, bool bRecordUndo);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_CountTexts", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int INK_CountTexts(IntPtr pData);

        [DllImport(Constants.LibraryName, EntryPoint = "INK_SetTextFrame", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool INK_SetTextFrame(IntPtr pData, int nTextIndex, InkRect frame);


        // managed wrappers for IntPtr values

        internal static InkPoint[] INK_GetStroke_Managed(IntPtr pData, int nStroke, out int nWidth, out uint color)
        {
            IntPtr ppointsPtr;
            var size = INK_GetStroke(pData, nStroke, out ppointsPtr, out nWidth, out color);
            if (size == -1)
            {
                // TODO (possibly): Marshal.FreeHGlobal(ppointsPtr);
                return null;
            }
            else if (size == 0)
            {
                // TODO (possibly: Marshal.FreeHGlobal(ppointsPtr);
                return new InkPoint[0];
            }
            var itemSize = Marshal.SizeOf(typeof(InkPoint));
            InkPoint[] ppoints = new InkPoint[size];
            for (int i = 0; i < size; i++)
            {
                var itemPtr = ppointsPtr + (i * itemSize);
                ppoints[i] = (InkPoint)Marshal.PtrToStructure(itemPtr, typeof(InkPoint));
            }
            Marshal.FreeHGlobal(ppointsPtr);
            return ppoints;
        }

        internal static InkTracePoint[] INK_GetStrokeP_Managed(IntPtr pData, int nStroke, out int nWidth, out uint color)
        {
            IntPtr ppointsPtr;
            var size = INK_GetStrokeP(pData, nStroke, out ppointsPtr, out nWidth, out color);
            if (size == -1)
            {
                // TODO (possibly): Marshal.FreeHGlobal(ppointsPtr);
                return null;
            }
            else if (size == 0)
            {
                // TODO (possibly: Marshal.FreeHGlobal(ppointsPtr);
                return new InkTracePoint[0];
            }
            var itemSize = Marshal.SizeOf(typeof(InkTracePoint));
            InkTracePoint[] ppoints = new InkTracePoint[size];
            for (int i = 0; i < size; i++)
            {
                var itemPtr = ppointsPtr + (i * itemSize);
                ppoints[i] = (InkTracePoint)Marshal.PtrToStructure(itemPtr, typeof(InkTracePoint));
            }
            Marshal.FreeHGlobal(ppointsPtr);
            return ppoints;
        }
    }
}
