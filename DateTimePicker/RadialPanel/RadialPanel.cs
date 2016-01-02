using System;
using System.Windows;
using System.Windows.Controls;

namespace PeteEvans.RadialPanel
{
    /// <summary>
    ///     Radial panel to lay out Child items around a circle)
    /// </summary>
    public class RadialPanel : Panel
    {
        #region Private Member Variables

        private const double RadiansPerDegree = Math.PI/180;

        private double maxChildRadius;

        #endregion Private Member Variables

        #region Dependency Properties

        #region RotationOffset

        public static readonly DependencyProperty RotationOffsetProperty
            = DependencyProperty.Register("RotationOffset",
                typeof (double),
                typeof (RadialPanel),
                new FrameworkPropertyMetadata(0d,
                    OnRotationOffsetPropertyChanged));

        public double RotationOffset
        {
            get { return (double) GetValue(RotationOffsetProperty); }
            set { SetValue(RotationOffsetProperty, value); }
        }

        private static void OnRotationOffsetPropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as RadialPanel;
            control.UpdateLayout();
        }

        #endregion RotationOffset

        #region MaxControlRadius

        public static readonly DependencyProperty MaxControlRadiusProperty
            = DependencyProperty.Register("MaxControlRadius",
                typeof (double),
                typeof (RadialPanel),
                new FrameworkPropertyMetadata(0d,
                    OnMaxControlRadiusPropertyChanged));

        public double MaxControlRadius
        {
            get { return (double) GetValue(MaxControlRadiusProperty); }
            set { SetValue(MaxControlRadiusProperty, value); }
        }

        private static void OnMaxControlRadiusPropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as RadialPanel;
            control.UpdateLayout();
        }

        #endregion MaxControlRadius

        #endregion Dependency Properties

        #region Layout Implementation

        protected override Size MeasureOverride(Size availableSize)
        {
            // Call measure for each child
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);

                // Work out the largets control 'radius'.
                var childHeight = child.DesiredSize.Height/2;
                var childWidth = child.DesiredSize.Width/2;
                var childRadius = Math.Sqrt(childHeight*childHeight + childWidth*childWidth);
                maxChildRadius = Math.Max(maxChildRadius, childRadius);

                // Limit the max radius if set
                if (MaxControlRadius > 0 && maxChildRadius > MaxControlRadius)
                {
                    maxChildRadius = MaxControlRadius;
                }
            }
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // If no children use the available space to avoid div by zero
            if (Children.Count == 0)
            {
                return finalSize;
            }

            var currentAngle = RotationOffset*RadiansPerDegree;

            // Work out spacing  
            var xRadius = finalSize.Width/2 - maxChildRadius;
            var yRadius = finalSize.Height/2 - maxChildRadius;

            // Find the angle to space each control
            var deltaDegrees = 360.0/Children.Count;
            var delta = deltaDegrees*RadiansPerDegree;

            var panelCentre = new Size(finalSize.Width/2, finalSize.Height/2);

            // Set each controls
            foreach (UIElement child in Children)
            {
                // Find the centre point for the control
                var childCentre = new Size(panelCentre.Width + Math.Sin(currentAngle)*xRadius,
                    panelCentre.Height - Math.Cos(currentAngle)*yRadius);

                // Offset by half width and height to centre the control

                var actualChildPoint = new Point(childCentre.Width - child.DesiredSize.Width/2,
                    childCentre.Height - child.DesiredSize.Height/2);

                // Arrange the child
                child.Arrange(new Rect(actualChildPoint.X, actualChildPoint.Y,
                    child.DesiredSize.Width, child.DesiredSize.Height));

                // Increment the new angle
                currentAngle += delta;
            }
            return finalSize;
        }

        #endregion Layout Implementation
    }
}