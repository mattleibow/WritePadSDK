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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhatWare.WritePad
{
    //#if __IOS__
    //    using CGFloat = System.nfloat;
    //#else
    //    using CGFloat = System.Single;
    //#endif

    public struct InkPoint
    {
        public float X;
        public float Y;
    }

    public struct InkSize
    {
        public float Width;
        public float Height;
    }

    public struct InkRect
    {
        public InkPoint Location;
        public float Size;
    }

    /// <summary>
    /// Represents digital ink.
    /// </summary>
    public struct InkTracePoint
    {
        public InkTracePoint(float x, float y)
        {
            Location = new InkPoint { X = x, Y = y };
            Pressure = Ink.DefaultPressure;
        }

        /// <summary>
        /// The x and y coordinates. [0...16000]
        /// </summary>
        public InkPoint Location;

        /// <summary>
        /// The optional pressure. [1...255]
        /// The pressure value is not required and ignored by the recognition engine.
        /// </summary>
        public int Pressure;
    }

    public class InkStroke
    {
        private readonly Ink ink;
        private readonly int index;
        private int width;
        private uint color;

        internal InkStroke(Ink ink, int index, int width, uint color)
        {
            this.ink = ink;
            this.index = index;

            this.width = width;
            this.color = color;
        }

        public int Index
        {
            get { return index; }
        }

        public int Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    width = value;
                    ink.SetStrokeWidthAndColor(this, Width, Color);
                }
            }
        }

        public uint Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    ink.SetStrokeWidthAndColor(this, Width, Color);
                }
            }
        }

        public int AddPixel(float x, float y)
        {
            return ink.AddPixel(this, x, y);
        }

        public int AddPixel(float x, float y, int pressure)
        {
            return ink.AddPixel(this, x, y, pressure);
        }
    }

    public class Ink : IDisposable
    {
        public const int DefaultPressure = 150;
        public const int MaximumPressure = 255;
        public const int MinimumPressure = 5;

        internal readonly IntPtr nativeHandle;

        private bool disposed = false;

        public Ink()
        {
            nativeHandle = InkApi.INK_InitData();
        }

        ~Ink()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (nativeHandle != null && !disposed)
            {
                InkApi.INK_Erase(nativeHandle);
                InkApi.INK_FreeData(nativeHandle);

                disposed = true;
            }
        }

        public InkStroke AddStroke(IEnumerable<InkTracePoint> strokeData, int width, uint color)
        {
            return AddStroke(strokeData.ToArray(), width, color);
        }

        public InkStroke AddStroke(InkTracePoint[] strokeData, int width, uint color)
        {
            var strokeDataArray = strokeData.ToArray();
            var newIndex = StrokeCount;
            InkApi.INK_AddStroke(nativeHandle, strokeDataArray, strokeDataArray.Length, width, color);
            return new InkStroke(this, newIndex, width, color);
        }

        public InkStroke AddEmptyStroke(int width, uint color)
        {
            var index = InkApi.INK_AddEmptyStroke(nativeHandle, width, color);
            return new InkStroke(this, index, width, color);
        }

        public bool SetStrokeWidthAndColor(InkStroke stroke, int width, uint color)
        {
            return SetStrokeWidthAndColor(stroke.Index, width, color);
        }

        public bool SetStrokeWidthAndColor(int strokeIndex, int width, uint color)
        {
            return InkApi.INK_SetStrokeWidthAndColor(nativeHandle, strokeIndex, color, width);
        }

        public int AddPixel(InkStroke stroke, float x, float y, int pressure)
        {
            return AddPixel(stroke.Index, x, y, pressure);
        }

        public int AddPixel(InkStroke stroke, float x, float y)
        {
            return AddPixel(stroke.Index, x, y, DefaultPressure);
        }

        public int AddPixel(int strokeIndex, float x, float y)
        {
            return AddPixel(strokeIndex, x, y, DefaultPressure);
        }

        public int AddPixel(int strokeIndex, float x, float y, int pressure)
        {
            return InkApi.INK_AddPixelToStroke(nativeHandle, strokeIndex, x, y, pressure);
        }

        public int StrokeCount
        {
            get { return InkApi.INK_StrokeCount(nativeHandle, false); }
        }

        public int SelectedStrokeCount
        {
            get { return InkApi.INK_StrokeCount(nativeHandle, true); }
        }

        public InkTracePoint[] GetStrokePoints(InkStroke stroke, out int width, out uint color)
        {
            return GetStrokePoints(stroke.Index, out width, out color);
        }

        public InkTracePoint[] GetStrokePoints(int strokeIndex, out int width, out uint color)
        {
            return InkApi.INK_GetStrokeP_Managed(nativeHandle, strokeIndex, out width, out color);
        }

        public InkPoint[] GetStrokeCoordinates(InkStroke stroke, out int width, out uint color)
        {
            return GetStrokeCoordinates(stroke.Index, out width, out color);
        }

        public InkPoint[] GetStrokeCoordinates(int strokeIndex, out int width, out uint color)
        {
            return InkApi.INK_GetStroke_Managed(nativeHandle, strokeIndex, out width, out color);
        }

        public bool GetStrokePoint(InkStroke stroke, int pointIndex, out InkPoint point)
        {
            return GetStrokePoint(stroke.Index, pointIndex, out point);
        }

        public bool GetStrokePoint(InkStroke stroke, int pointIndex, out float x, out float y)
        {
            return GetStrokePoint(stroke.Index, pointIndex, out x, out y);
        }

        public bool GetStrokePoint(int strokeIndex, int pointIndex, out InkPoint point)
        {
            float x;
            float y;
            var result = GetStrokePoint(strokeIndex, pointIndex, out x, out y);
            point = new InkPoint { X = x, Y = y };
            return result;
        }

        public bool GetStrokePoint(int strokeIndex, int pointIndex, out float x, out float y)
        {
            return InkApi.INK_GetStrokePoint(nativeHandle, strokeIndex, pointIndex, out x, out y);
        }

        public bool DeleteLastStroke()
        {
            return DeleteStroke(-1);
        }

        public bool DeleteStroke(InkStroke stroke)
        {
            return DeleteStroke(stroke.Index);
        }

        public bool DeleteStroke(int strokeIndex)
        {
            return InkApi.INK_DeleteStroke(nativeHandle, strokeIndex);
        }

        public void Erase()
        {
            InkApi.INK_Erase(nativeHandle);
        }
    }
}
