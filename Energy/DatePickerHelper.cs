using System.Windows;
using System.Windows.Controls;

namespace Energy
{
    public static class DatePickerHelper
    {
        // Attached property to hold the watermark text for a DatePicker.
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached(
                "Watermark",
                typeof(string),
                typeof(DatePickerHelper),
                new PropertyMetadata(string.Empty));

        public static void SetWatermark(DependencyObject element, string value)
        {
            element.SetValue(WatermarkProperty, value);
        }

        public static string GetWatermark(DependencyObject element)
        {
            return (string)element.GetValue(WatermarkProperty);
        }
    }
}
