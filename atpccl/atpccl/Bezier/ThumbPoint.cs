using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    ///     <MyNamespace:ThumbPoint/>
    ///
    /// </summary>
    public class ThumbPoint : Thumb
    {
        public Point Point
        {
            get { return (Point)GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Point.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointProperty =
            DependencyProperty.Register("Point", typeof(Point), typeof(ThumbPoint), new FrameworkPropertyMetadata(new Point()));

        public Guid Id { get; private set; }

        static ThumbPoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThumbPoint), new FrameworkPropertyMetadata(typeof(ThumbPoint)));
        }
        public ThumbPoint()
        {
            this.DragDelta += new DragDeltaEventHandler(this.OnDragDelta);
            Id = Guid.NewGuid();
        }
        public ThumbPoint(Point point) : this()
        {
            Point = point;
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Point = new Point(this.Point.X + e.HorizontalChange, this.Point.Y + e.VerticalChange);
        }
    }
}
