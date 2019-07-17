using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;

namespace MagicianHub.Extensions
{
    public class InAppNotificationExtension : InAppNotification
    {
        public bool IsOpened
        {
            get => (bool)GetValue(IsOpenedProperty);
            set => SetValue(IsOpenedProperty, value);
        }

        public static readonly DependencyProperty IsOpenedProperty =
            DependencyProperty.Register(
                nameof(IsOpened),
                typeof(bool),
                typeof(InAppNotificationExtension),
                new PropertyMetadata(false, (d, e) => ((InAppNotificationExtension)d).IsOpenedChanged(e))
            );

        private void IsOpenedChanged(DependencyPropertyChangedEventArgs evt)
        {
            var value = (bool)evt.NewValue;
            if (value) Show(StayDuration); else Dismiss();
        }

        public int StayDuration
        {
            get => (int)GetValue(StayDurationProperty);
            set => SetValue(StayDurationProperty, value);
        }

        public static readonly DependencyProperty StayDurationProperty =
            DependencyProperty.Register(
                nameof(StayDuration),
                typeof(int),
                typeof(InAppNotificationExtension),
                new PropertyMetadata(0)
            );
    }
}
