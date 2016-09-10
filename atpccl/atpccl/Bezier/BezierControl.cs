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
    ///     <MyNamespace:BezierControl/>
    ///
    /// </summary>
    public class BezierControl : Control
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(BezierControl), new FrameworkPropertyMetadata(new Point()));

        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point), typeof(BezierControl), new FrameworkPropertyMetadata(new Point()));

        public Point StartBezierPoint
        {
            get { return (Point)GetValue(StartBezierPointProperty); }
            set { SetValue(StartBezierPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartBezierPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartBezierPointProperty =
            DependencyProperty.Register("StartBezierPoint", typeof(Point), typeof(BezierControl), new FrameworkPropertyMetadata(new Point()));

        public Point EndBezierPoint
        {
            get { return (Point)GetValue(EndBezierPointProperty); }
            set { SetValue(EndBezierPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndBezierPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndBezierPointProperty =
            DependencyProperty.Register("EndBezierPoint", typeof(Point), typeof(BezierControl), new FrameworkPropertyMetadata(new Point()));




        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(BezierControl), new PropertyMetadata("Bezier Control"));


        static BezierControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BezierControl), new FrameworkPropertyMetadata(typeof(BezierControl)));
        }
    }
}
