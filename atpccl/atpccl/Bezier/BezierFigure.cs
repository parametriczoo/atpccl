﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace atpccl.Bezier
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:atpccl.Bezier"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:atpccl.Bezier;assembly=atpccl.Bezier"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:BezierFigure/>
    ///
    /// </summary>
    public class BezierFigure : Control
    {
        Canvas _canvas;
        List<ThumbPoint> ControlPoints { get; set; }
        List<ThumbPoint> OrderedControlPoints { get; set; }
        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointCollection), typeof(BezierFigure));

        public PolyLineSegment PolyLine
        {
            get { return (PolyLineSegment)GetValue(PolyLineProperty); }
            set { SetValue(PolyLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PolyLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PolyLineProperty =
            DependencyProperty.Register("PolyLine", typeof(PolyLineSegment), typeof(BezierFigure));


        public BezierFigure()
        {
            Points = new PointCollection();
            ControlPoints = new List<ThumbPoint>();
            ControlPoints.Add(new ThumbPoint(10, 200));
            ControlPoints.Add(new ThumbPoint(30, 40));
            ControlPoints.Add(new ThumbPoint(300, 40));
            ControlPoints.Add(new ThumbPoint(350, 250));
            OrderControlPoints();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_canvas == null)
            {
                _canvas = (Canvas)GetTemplateChild("PART_canvas");
                _canvas.PreviewMouseLeftButtonDown += _canvas_MouseUp;
                foreach (var item in OrderedControlPoints)
                {
                    item.DragDelta += Thumb_DragDelta;
                    item.DragCompleted += Item_DragCompleted;
                    _canvas.Children.Add(item);
                }
            }
        }

        private void Item_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            GetBezierApproximation();
        }

        void OrderControlPoints()
        {
            if (OrderedControlPoints == null)
            {
                OrderedControlPoints = new List<ThumbPoint>();
            }
            foreach (var item in ControlPoints)
            {
                if (OrderedControlPoints.Find(p => p.Id == item.Id) != null)
                {
                    continue;
                }
                if (OrderedControlPoints.Count == 0)
                {
                    OrderedControlPoints.Add(item);
                }
                else if (item.Point.X > OrderedControlPoints.Last().Point.X)
                {
                    OrderedControlPoints.Add(item);
                }
                else
                {
                    for (int i = 0; i < OrderedControlPoints.Count; i++)
                    {
                        if (item.Point.X < OrderedControlPoints[i].Point.X)
                        {
                            OrderedControlPoints.Insert(i, item);
                            break;
                        }
                    }
                }
            }
        }
        private void _canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var point = e.GetPosition(_canvas);
                var thumb = new ThumbPoint(point);
                thumb.DragDelta += Thumb_DragDelta;
                thumb.DragCompleted += Item_DragCompleted;
                ControlPoints.Add(thumb);
                _canvas.Children.Add(thumb);
                OrderControlPoints();
                GetBezierApproximation();
            }
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (OrderedControlPoints.Count < 10)
            {
                GetBezierApproximation();
            }
        }

        static BezierFigure()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BezierFigure), new FrameworkPropertyMetadata(typeof(BezierFigure)));
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            GetBezierApproximation();
        }

        void GetBezierApproximation()
        {
            int outputSegmentCount = 256;
            Point[] points = new Point[outputSegmentCount + 1];
            for (int i = 0; i <= outputSegmentCount; i++)
            {
                double t = (double)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, OrderedControlPoints, 0, OrderedControlPoints.Count);
            }
            PolyLine = new PolyLineSegment(points, true);
            Points = PolyLine.Points;
        }

        Point GetBezierPoint(double t, List<ThumbPoint> controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index].Point;
            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Point((1 - t) * P0.X + t * P1.X, (1 - t) * P0.Y + t * P1.Y);
        }
    }
}
