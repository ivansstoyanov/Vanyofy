using System.Windows;
using System.Windows.Controls;

namespace Vanyofy.Animations
{
    public class AnimatedGridRowBehavior
    {
        public static double GetAnimatedHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(AnimatedHeightProperty);
        }

        public static void SetAnimatedHeight(DependencyObject obj, double value)
        {
            obj.SetValue(AnimatedHeightProperty, value);
        }

        public static readonly DependencyProperty AnimatedHeightProperty =
        DependencyProperty.RegisterAttached("AnimatedHeight", typeof(double), typeof(AnimatedGridRowBehavior), new UIPropertyMetadata(0d, new PropertyChangedCallback((s, e) =>
        {
            RowDefinition sender = s as RowDefinition;
            sender.Height = new GridLength((double)e.NewValue);
        })));
    }
}
