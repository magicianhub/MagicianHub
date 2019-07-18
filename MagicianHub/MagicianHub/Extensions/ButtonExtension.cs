using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MagicianHub.Extensions
{
    public class ButtonExtension : Button
    {
        public bool IsFocused
        {
            get => (bool)GetValue(IsFocusedProperty);
            set => SetValue(IsFocusedProperty, value);
        }
        
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.Register(
                nameof(IsFocused),
                typeof(bool),
                typeof(ButtonExtension),
                new PropertyMetadata(
                    false,
                    (d, e) => ((ButtonExtension)d).IsFocusedChanged(e)
                )
            );

        public void IsFocusedChanged(DependencyPropertyChangedEventArgs evt)
        {
            var value = (bool)evt.NewValue;
            if (value) Focus(FocusState.Keyboard);
        }
    }
}
