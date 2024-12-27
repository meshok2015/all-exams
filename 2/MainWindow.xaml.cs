using System;
using System.Collections.Generic;
using System.Windows;

namespace UnitConverter
{
    public partial class MainWindow : Window
    {

        private Dictionary<string, double> distanceUnits = new Dictionary<string, double>
        {
            { "Метры (m)", 1 },
            { "Километры (km)", 1000 },
            { "Мили (mi)", 1609.34 },
            { "Ярды (yd)", 0.9144 }
        };

        private Dictionary<string, double> volumeUnits = new Dictionary<string, double>
        {
            { "Литры (L)", 1 },
            { "Миллилитры (mL)", 0.001 },
            { "Кубические метры (m³)", 1000 },
            { "Галлоны (gal)", 3.78541 }
        };

        private Dictionary<string, double> weightUnits = new Dictionary<string, double>
        {
            { "Граммы (g)", 1 },
            { "Килограммы (kg)", 1000 },
            { "Фунты (lb)", 453.592 },
            { "Унции (oz)", 28.3495 }
        };

        private Dictionary<string, double> currencyRates = new Dictionary<string, double>
        {
            { "USD - Доллар США", 1 },
            { "EUR - Евро", 0.9 },
            { "RUB - Рубль", 100 },
            { "JPY - Йена", 130 }
        };

        public MainWindow()
        {
            InitializeComponent();
            LoadUnits("Distance");
        }


        private void LoadUnits(string category)
        {
            FromUnitCombo.Items.Clear();
            ToUnitCombo.Items.Clear();

            Dictionary<string, double> units = null;

            if (category == "Distance")
                units = distanceUnits;
            else if (category == "Volume")
                units = volumeUnits;
            else if (category == "Weight")
                units = weightUnits;
            else if (category == "Currency")
                units = currencyRates;

            if (units != null)
            {
                foreach (var unit in units.Keys)
                {
                    FromUnitCombo.Items.Add(unit);
                    ToUnitCombo.Items.Add(unit);
                }
                FromUnitCombo.SelectedIndex = 0;
                ToUnitCombo.SelectedIndex = 1;
            }
        }


        private void CategoryChanged(object sender, RoutedEventArgs e)
        {
            if (DistanceRadio.IsChecked == true)
                LoadUnits("Distance");
            else if (VolumeRadio.IsChecked == true)
                LoadUnits("Volume");
            else if (WeightRadio.IsChecked == true)
                LoadUnits("Weight");
            else if (CurrencyRadio.IsChecked == true)
                LoadUnits("Currency");
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double inputValue = double.Parse(InputValue.Text);
                string fromUnit = FromUnitCombo.SelectedItem.ToString();
                string toUnit = ToUnitCombo.SelectedItem.ToString();

                double result = CurrencyRadio.IsChecked == true
                    ? ConvertCurrency(inputValue, fromUnit, toUnit)
                    : ConvertUnits(inputValue, fromUnit, toUnit);

                ResultBox.Text = result.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private double ConvertUnits(double value, string fromUnit, string toUnit)
        {
            Dictionary<string, double> currentUnits = null;

            if (DistanceRadio.IsChecked == true)
                currentUnits = distanceUnits;
            else if (VolumeRadio.IsChecked == true)
                currentUnits = volumeUnits;
            else if (WeightRadio.IsChecked == true)
                currentUnits = weightUnits;

            if (currentUnits != null)
            {
                double fromFactor = currentUnits[fromUnit];
                double toFactor = currentUnits[toUnit];

                return value * fromFactor / toFactor;
            }

            throw new InvalidOperationException("Неверная категория конверсии.");
        }


        private double ConvertCurrency(double value, string fromCurrency, string toCurrency)
        {
            double fromRate = currencyRates[fromCurrency];
            double toRate = currencyRates[toCurrency];

            return value * fromRate / toRate;
        }
    }
}
