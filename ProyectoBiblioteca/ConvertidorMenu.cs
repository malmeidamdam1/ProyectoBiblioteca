using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ProyectoBiblioteca
{
    public class ConvertidorMenu : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedMenuItem = value as string;
            var menuItem = parameter as string;

            if (selectedMenuItem == menuItem)
            {
                return Brushes.LightBlue; // Color para el botón seleccionado
            }
            else
            {
                return Brushes.Transparent; // Color para otros botones
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
