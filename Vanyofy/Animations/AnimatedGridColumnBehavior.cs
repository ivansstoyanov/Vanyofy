using System.Windows;
using System.Windows.Controls;

namespace Vanyofy.Animations
{
    public class AnimatedGridColumnBehavior
    {
        public static double GetAnimatedHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(AnimatedWidthProperty);
        }

        public static void SetAnimatedWidth(DependencyObject obj, double value)
        {
            obj.SetValue(AnimatedWidthProperty, value);
        }

        public static readonly DependencyProperty AnimatedWidthProperty =
        DependencyProperty.RegisterAttached("AnimatedWidth", typeof(double), typeof(AnimatedGridColumnBehavior), new UIPropertyMetadata(0d, new PropertyChangedCallback((s, e) =>
        {
            Grid sender = s as Grid;
            sender.Width = (double)e.NewValue;
        })));
    }
}
