using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EXAMEN_3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeFunctions();
        }

        private void InitializeFunctions()
        {
            FunctionComboBox.ItemsSource = new string[] { "sin(x)", "1/x", "x^2", "sqrt(x)", "Custom Function" };
            FunctionComboBox.SelectedIndex = 0;
        }

        private void FunctionComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FunctionComboBox.SelectedItem.ToString() == "Custom Function")
            {
                CustomFunctionTextBox.IsEnabled = true;
            }
            else
            {
                CustomFunctionTextBox.IsEnabled = false;
                CustomFunctionTextBox.Text = string.Empty;
            }
        }

        private void PlotButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedFunction = FunctionComboBox.SelectedItem?.ToString();
            string customFunction = CustomFunctionTextBox.Text.Trim();

            if (double.TryParse(XMinTextBox.Text, out double xMin) &&
                double.TryParse(XMaxTextBox.Text, out double xMax) &&
                int.TryParse(PointsTextBox.Text, out int points) && points > 0 && xMin < xMax)
            {
                PlotModel model = new PlotModel { Title = "Graph" };
                model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X", MajorGridlineStyle = LineStyle.Solid });
                model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y", MajorGridlineStyle = LineStyle.Solid });

                LineSeries series = new LineSeries();
                double step = (xMax - xMin) / (points - 1);

                var functionData = new List<FunctionData>();

                for (double x = xMin; x <= xMax; x += step)
                {
                    double y;
                    try
                    {
                        if (selectedFunction == "Custom Function" && !string.IsNullOrWhiteSpace(customFunction))
                        {
                            y = EvaluateCustomFunction(customFunction, x);
                        }
                        else
                        {
                            y = EvaluatePredefinedFunction(selectedFunction, x);
                        }

                        if (!double.IsNaN(y) && !double.IsInfinity(y))
                        {
                            series.Points.Add(new DataPoint(x, y));
                            functionData.Add(new FunctionData { X = x, Y = y });
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid custom function. Please check your syntax.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                FunctionDataGrid.ItemsSource = functionData;

                model.Series.Add(series);
                PlotView.Model = model;
            }
            else
            {
                MessageBox.Show("Invalid input. Please check your values.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private double EvaluatePredefinedFunction(string function, double x)
        {
            switch (function)
            {
                case "sin(x)":
                    return Math.Sin(x);
                case "1/x":
                    return x != 0 ? 1 / x : double.NaN;
                case "x^2":
                    return Math.Pow(x, 2);
                case "sqrt(x)":
                    return x >= 0 ? Math.Sqrt(x) : double.NaN;
                default:
                    return double.NaN;
            }
        }

        private double EvaluateCustomFunction(string function, double x)
        {
            if (function.StartsWith("y="))
            {
                function = function.Substring(2).Trim();
            }

            function = function.Replace("^", "**")
                               .Replace("sin", "Math.Sin")
                               .Replace("cos", "Math.Cos")
                               .Replace("sqrt", "Math.Sqrt")
                               .Replace("x", x.ToString(CultureInfo.InvariantCulture));

            DataTable table = new DataTable();
            return Convert.ToDouble(table.Compute(function, string.Empty));
        }
    }

    public class FunctionData
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
