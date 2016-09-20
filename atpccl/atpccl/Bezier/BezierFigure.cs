using System;
using System.Collections.Generic;
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
        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointCollection), typeof(BezierFigure));


        public Point[] ControlPoints
        {
            get { return (Point[])GetValue(ControlPointsProperty); }
            set { SetValue(ControlPointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlPointsProperty =
            DependencyProperty.Register("ControlPoints", typeof(Point[]), typeof(BezierFigure));


        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(BezierFigure), new FrameworkPropertyMetadata(new Point(), new PropertyChangedCallback(OnPointChanged)));

        private static void OnPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var figure = d as BezierFigure;

            figure.GetBezierApproximation();
        }

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


        public BezierFigure()
        {
            ControlPoints = new[] {
                StartPoint,
                StartBezierPoint,
                EndBezierPoint,
                EndPoint
            };
            Points = new PointCollection();
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
            ControlPoints = new[] {
                StartPoint,
                StartBezierPoint,
                EndBezierPoint,
                EndPoint
            };
            int outputSegmentCount = 256;
            Point[] points = new Point[outputSegmentCount + 1];
            for (int i = 0; i <= outputSegmentCount; i++)
            {
                double t = (double)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, ControlPoints, 0, ControlPoints.Length);
            }
            PolyLine = new PolyLineSegment(points, true);
            Points = PolyLine.Points;
        }

        Point GetBezierPoint(double t, Point[] controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index];
            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Point((1 - t) * P0.X + t * P1.X, (1 - t) * P0.Y + t * P1.Y);
        }
    }
}
