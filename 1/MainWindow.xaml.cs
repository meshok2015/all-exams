using System;
using System.Windows;
using System.Windows.Controls;

namespace _1_EXAM
{
    public partial class MainWindow : Window
    {
        private double currentValue = 0;  
        private double lastValue = 0;     
        private string lastOperator = "";
        private double memoryValue = 0;   
        private bool isDegreeMode = true; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                DisplayTextBox.Text += button.Content.ToString();
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                lastValue = double.Parse(DisplayTextBox.Text);
                lastOperator = button.Content.ToString();
                DisplayTextBox.Clear();
            }
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            double result = 0;
            double newValue = double.Parse(DisplayTextBox.Text);

            switch (lastOperator)
            {
                case "+":
                    result = lastValue + newValue;
                    break;
                case "-":
                    result = lastValue - newValue;
                    break;
                case "*":
                    result = lastValue * newValue;
                    break;
                case "/":
                    if (newValue != 0)
                        result = lastValue / newValue;
                    else
                        MessageBox.Show("Ошибка: Деление на ноль!");
                    break;
                default:
                    result = newValue;
                    break;
            }

            DisplayTextBox.Text = result.ToString();
            lastValue = result;
        }

        private void Point_Click(object sender, RoutedEventArgs e)
        {
            if (!DisplayTextBox.Text.Contains(","))
                DisplayTextBox.Text += ",";
        }

        private void PlusMinus_Click(object sender, RoutedEventArgs e)
        {
            double current = double.Parse(DisplayTextBox.Text);
            current = -current;
            DisplayTextBox.Text = current.ToString();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DisplayTextBox.Clear();
        }

        private void Constant_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                double constantValue = button.Content.ToString() == "PI" ? Math.PI : Math.E;
                DisplayTextBox.Text = constantValue.ToString();
            }
        }

        private void MemoryButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                switch (button.Content.ToString())
                {
                    case "MC":
                        memoryValue = 0;
                        break;
                    case "MR":
                        DisplayTextBox.Text = memoryValue.ToString();
                        break;
                    case "M+":
                        memoryValue += double.Parse(DisplayTextBox.Text);
                        break;
                    case "M-":
                        memoryValue -= double.Parse(DisplayTextBox.Text);
                        break;
                }
            }
        }

        private void Function_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                double inputValue = double.Parse(DisplayTextBox.Text);
                double result = 0;

                switch (button.Content.ToString())
                {
                    case "Sin":
                        result = isDegreeMode ? Math.Sin(DegreesToRadians(inputValue)) : Math.Sin(inputValue);
                        break;
                    case "Cos":
                        result = isDegreeMode ? Math.Cos(DegreesToRadians(inputValue)) : Math.Cos(inputValue);
                        break;
                    case "Tan":
                        result = isDegreeMode ? Math.Tan(DegreesToRadians(inputValue)) : Math.Tan(inputValue);
                        break;
                    case "√":
                        result = Math.Sqrt(inputValue);
                        break;
                }

                DisplayTextBox.Text = result.ToString();
            }
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private void DegreeRadian_CheckedChanged(object sender, RoutedEventArgs e)
        {
            isDegreeMode = DegreeRadioButton.IsChecked == true;
        }
    }
}
