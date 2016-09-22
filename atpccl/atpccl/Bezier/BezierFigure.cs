using System;
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
        public PointCollection ControlPoints { get; set; }
        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointCollection), typeof(BezierFigure));

        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(BezierFigure), new FrameworkPropertyMetadata(new Point(), new PropertyChangedCallback(OnPointChanged)));

        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point), typeof(BezierFigure), new FrameworkPropertyMetadata(new Point(), new PropertyChangedCallback(OnPointChanged)));

        public Point StartBezierPoint
        {
            get { return (Point)GetValue(StartBezierPointProperty); }
            set { SetValue(StartBezierPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartBezierPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartBezierPointProperty =
            DependencyProperty.Register("StartBezierPoint", typeof(Point), typeof(BezierFigure), new FrameworkPropertyMetadata(new Point(), new PropertyChangedCallback(OnPointChanged)));

        public Point EndBezierPoint
        {
            get { return (Point)GetValue(EndBezierPointProperty); }
            set { SetValue(EndBezierPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndBezierPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndBezierPointProperty =
            DependencyProperty.Register("EndBezierPoint", typeof(Point), typeof(BezierFigure), new FrameworkPropertyMetadata(new Point(), new PropertyChangedCallback(OnPointChanged)));


        public PolyLineSegment PolyLine
        {
            get { return (PolyLineSegment)GetValue(PolyLineProperty); }
            set { SetValue(PolyLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PolyLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PolyLineProperty =
            DependencyProperty.Register("PolyLine", typeof(PolyLineSegment), typeof(BezierFigure));

        private static void OnPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var figure = d as BezierFigure;
            figure.UpdateControlPoints();
        }
        public void UpdateControlPoints()
        {
            ControlPoints = new PointCollection {
                StartPoint,
                StartBezierPoint,
                EndBezierPoint,
                EndPoint
            };
            GetBezierApproximation();
        }
        public BezierFigure()
        {
            Points = new PointCollection();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_canvas == null)
            {
                _canvas = (Canvas)GetTemplateChild("PART_canvas");
                _canvas.PreviewMouseLeftButtonDown += _canvas_MouseUp;
            }
        }

        private void _canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var point = e.GetPosition(_canvas);
                ControlPoints = new PointCollection {
                        StartPoint,
                        StartBezierPoint,
                        EndBezierPoint,
                        EndPoint,
                        point
                    };
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
            UpdateControlPoints();
            GetBezierApproximation();
        }

        void GetBezierApproximation()
        {
            int outputSegmentCount = 256;
            Point[] points = new Point[outputSegmentCount + 1];
            for (int i = 0; i <= outputSegmentCount; i++)
            {
                double t = (double)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, ControlPoints, 0, ControlPoints.Count);
            }
            PolyLine = new PolyLineSegment(points, true);
            Points = PolyLine.Points;
        }

        Point GetBezierPoint(double t, PointCollection controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index];
            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Point((1 - t) * P0.X + t * P1.X, (1 - t) * P0.Y + t * P1.Y);
        }
    }
}
