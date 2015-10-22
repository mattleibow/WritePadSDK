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
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;

using PhatWare.WritePad;

namespace WritePadSDKAndroidSample
{
    public class InkView : View
    {
        private Ink ink;
        private InkStroke mCurrStroke;

        private Path mPath;
        private Paint mPaint;
        private Paint mResultPaint;
        private Path gridpath;

        private LinkedList<Path> mPathList;
        private float mX, mY;
        private const float TOUCH_TOLERANCE = 2;
        private bool mMoved;
        private List<InkTracePoint> currentStroke;
        private int strokeLen = 0;

        private InkPoint _lastPoint;
        private InkPoint _previousLocation;

        private const int SEGMENT2 = 2;
        private const int SEGMENT3 = 3;

        private const float GRID_GAP = 65;
        private const int SEGMENT4 = 4;

        private const float SEGMENT_DIST_1 = 3;
        private const float SEGMENT_DIST_2 = 6;
        private const float SEGMENT_DIST_3 = 12;

        protected InkView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            Setup();
        }

        public InkView(Context context)
            : base(context)
        {
            Setup();
        }

        public InkView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Setup();
        }

        public InkView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Setup();
        }

        private void Setup()
        {
            ink = new Ink();

            mPath = new Path();
            mPathList = new LinkedList<Path>();
            mCurrStroke = null;
            mPaint = new Paint();
            mPaint.AntiAlias = true;
            mPaint.Dither = true;
            mPaint.Color = new Color(0, 0, 255);
            mPaint.SetStyle(Paint.Style.Stroke);
            mPaint.StrokeJoin = Paint.Join.Round;
            mPaint.StrokeCap = Paint.Cap.Round;
            mPaint.StrokeWidth = 3;

            mResultPaint = new Paint();
            mResultPaint.TextSize = 32;
            mResultPaint.AntiAlias = true;
            mResultPaint.SetARGB(0xff, 0x00, 0x00, 0x00);

            gridpath = new Path();
        }

        public Ink Ink
        {
            get { return ink; }
        }

        // this method called from inkCollectorThread
        private int AddPixelsXY(float x, float y, bool bLastPoint)
        {
            float xNew, yNew, x1, y1;
            int nSeg = SEGMENT3;

            if (mCurrStroke == null)
                return 0;

            if (strokeLen < 1)
            {
                _lastPoint.X = _previousLocation.X = x;
                _lastPoint.Y = _previousLocation.Y = y;
                mCurrStroke.AddPixel(x, y);
                AddCurrentPoint(x, y);
                strokeLen = 1;
                return 1;
            }

            float dx = Math.Abs(x - _lastPoint.X);
            float dy = Math.Abs(y - _lastPoint.Y);
            if ((dx + dy) < SEGMENT_DIST_1)
            {
                _lastPoint.X = _previousLocation.X = x;
                _lastPoint.Y = _previousLocation.Y = y;
                mCurrStroke.AddPixel(x, y);
                AddCurrentPoint(x, y);
                strokeLen++;
                return 1;
            }

            if ((dx + dy) < SEGMENT_DIST_2)
                nSeg = SEGMENT2;
            else if ((dx + dy) < SEGMENT_DIST_3)
                nSeg = SEGMENT3;
            else
                nSeg = SEGMENT4;
            int nPoints = 0;
            for (int i = 1; i < nSeg; i++)
            {
                x1 = _previousLocation.X + ((x - _previousLocation.X) * i) / nSeg;  //the point "to look at"
                y1 = _previousLocation.Y + ((y - _previousLocation.Y) * i) / nSeg;  //the point "to look at"

                xNew = _lastPoint.X + (x1 - _lastPoint.X) / nSeg;
                yNew = _lastPoint.Y + (y1 - _lastPoint.Y) / nSeg;

                if (xNew != _lastPoint.X || yNew != _lastPoint.Y)
                {
                    _lastPoint.X = xNew;
                    _lastPoint.Y = yNew;
                    mCurrStroke.AddPixel(xNew, yNew);
                    AddCurrentPoint(x, y);
                    strokeLen++;
                    nPoints++;
                }
            }

            if (bLastPoint)
            {
                // add last point
                if (x != _lastPoint.X || y != _lastPoint.Y)
                {
                    _lastPoint.X = x;
                    _lastPoint.Y = y;
                    mCurrStroke.AddPixel(x, y);
                    AddCurrentPoint(x, y);
                    strokeLen++;
                    nPoints++;
                }
            }

            _previousLocation.X = x;
            _previousLocation.Y = y;
            return nPoints;
        }

        protected override void OnDraw(Canvas canvas)
        {
            // draw grid lines
            mPaint.Color = new Color(255, 0, 0);
            mPaint.StrokeWidth = 1;

            for (float y = GRID_GAP; y < canvas.Height; y += GRID_GAP)
            {
                gridpath.Reset();
                gridpath.MoveTo(0, y);
                gridpath.LineTo(canvas.Width, y);
                canvas.DrawPath(gridpath, mPaint);
            }
            mPaint.Color = new Color(0, 0, 255);
            mPaint.StrokeWidth = 3;

            // draw strokes
            foreach (var aMPathList in mPathList)
            {
                canvas.DrawPath(aMPathList, mPaint);
            }
            canvas.DrawPath(mPath, mPaint);
        }

        private void AddCurrentPoint(float mX, float mY)
        {
            currentStroke.Add(new InkTracePoint(mX, mY));
        }

        private void TouchStart(float x, float y)
        {
            mPath.Reset();
            currentStroke = new List<InkTracePoint>();
            mPath.MoveTo(x, y);
            mX = x;
            mY = y;
            AddCurrentPoint(mX, mY);
            mMoved = false;
            mCurrStroke = ink.AddEmptyStroke(3, 0xFFFF0000);
            strokeLen = 0;
            AddPixelsXY(mX, mY, false);
        }

        private void TouchMove(float x, float y)
        {
            float dx = Math.Abs(x - mX);
            float dy = Math.Abs(y - mY);
            if (dx >= TOUCH_TOLERANCE || dy >= TOUCH_TOLERANCE)
            {
                mPath.QuadTo(mX, mY, (x + mX) / 2, (y + mY) / 2);
                mMoved = true;
                mX = x;
                mY = y;
                AddCurrentPoint(mX, mY);
                AddPixelsXY(mX, mY, false);
            }
        }

        private void TouchEnd(float x, float y)
        {
            var gesture = GestureRecognizer.CheckGesture(
                Gestures.Return | Gestures.Cut | Gestures.Back | Gestures.BackLong,
                currentStroke);

            AddPixelsXY(x, y, true);
            AddCurrentPoint(mX, mY);
            mCurrStroke = null;
            mMoved = false;

            if (!mMoved)
                mX++;
            mPath.LineTo(mX, mY);
            mPathList.AddLast(mPath);
            mPath = new Path();
            Invalidate();

            switch (gesture)
            {
                case Gestures.Return:
                    if (OnReturnGesture != null)
                    {
                        mPathList.RemoveLast();
                        ink.DeleteLastStroke();
                        Invalidate();
                        OnReturnGesture();
                        return;
                    }
                    break;
                case Gestures.Cut:
                    if (OnCutGesture != null)
                    {
                        mPathList.RemoveLast();
                        ink.DeleteLastStroke();
                        Invalidate();
                        OnCutGesture();
                        return;
                    }
                    break;
                case Gestures.Back:
                case Gestures.BackLong:
                    mPathList.RemoveLast();
                    ink.DeleteLastStroke();
                    if (ink.StrokeCount > 0)
                    {
                        mPathList.RemoveLast();
                        ink.DeleteLastStroke();
                    }
                    Invalidate();
                    return;
            }
        }

        public void CleanView(bool emptyAll)
        {
            ink.Erase();

            mCurrStroke = null;
            mPathList.Clear();
            mPath.Reset();
            Invalidate();
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            float x = ev.GetX();
            float y = ev.GetY();

            switch (ev.Action)
            {
                case MotionEventActions.Down:
                    TouchStart(x, y);
                    Invalidate();
                    break;

                case MotionEventActions.Move:
                    for (int i = 0, n = ev.HistorySize; i < n; i++)
                    {
                        TouchMove(ev.GetHistoricalX(i), ev.GetHistoricalY(i));
                    }
                    TouchMove(x, y);
                    Invalidate();
                    break;

                case MotionEventActions.Up:
                    TouchEnd(x, y);
                    Invalidate();
                    break;
            }
            return true;
        }

        public event Action OnReturnGesture;
        public event Action OnCutGesture;
    }
}
