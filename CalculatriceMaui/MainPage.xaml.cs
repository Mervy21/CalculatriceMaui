using System.Globalization;
namespace CalculatriceMaui;
public partial class MainPage : ContentPage
{
    private double firstNumber = 0;
    private string currentOperator = "";
    private bool isNewNumber = true;
    private bool hasError = false;
    public MainPage()
    {
        InitializeComponent();
    }
    private void NumberButton_Clicked(object sender, EventArgs e)
    {
        if (hasError)
        {
            ClearCalculator();
        }
        Button button = (Button)sender;
        string number = button.Text;

        if (isNewNumber || DisplayLabel.Text == "0")
        {
            DisplayLabel.Text = number;
            isNewNumber = false;
        }
        else
        {
            DisplayLabel.Text += number;
        }
    }
    private void DecimalButton_Clicked(object sender, EventArgs e)
    {
        if (hasError)
        {
            ClearCalculator();
        }
        if (isNewNumber)
        {
            DisplayLabel.Text = "0.";
            isNewNumber = false;
            return;
        }
        if (!DisplayLabel.Text.Contains("."))
        {
            DisplayLabel.Text += ".";
        }
    }
    private void OperatorButton_Clicked(object sender, EventArgs e)
    {
        if (hasError)
            return;
        Button button = (Button)sender;
        string selectedOperator = button.Text;

        if (!double.TryParse(
            DisplayLabel.Text,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double currentNumber))
        {
            return;
        }
        if (!isNewNumber && !string.IsNullOrEmpty(currentOperator))
        {
            Calculate(currentNumber);
            if (hasError)
                return;
        }
        else
        {
            firstNumber = currentNumber;
        }
        currentOperator = selectedOperator;

        OperationLabel.Text =
            $"{FormatNumber(firstNumber)} {currentOperator}";
        isNewNumber = true;
    }
    private void Calculate(double secondNumber)
    {
        double result = 0;
        switch (currentOperator)
        {
            case "+":
                result = firstNumber + secondNumber;
                break;
            case "-":
                result = firstNumber - secondNumber;
                break;
            case "*":
                result = firstNumber * secondNumber;
                break;
            case "/":

                if (secondNumber == 0)
                {
                    ShowError("Division par zéro impossible");
                    return;
                }
                result = firstNumber / secondNumber;
                break;
        }
        DisplayLabel.Text = FormatNumber(result);
        firstNumber = result;
    }
    private void EqualsButton_Clicked(object sender, EventArgs e)
    {
        if (hasError)
            return;

        if (string.IsNullOrEmpty(currentOperator))
            return;

        if (!double.TryParse(
            DisplayLabel.Text,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double secondNumber))
        {
            return;
        }

        string operation =
            $"{FormatNumber(firstNumber)} {currentOperator} {FormatNumber(secondNumber)}";

        Calculate(secondNumber);

        if (!hasError)
        {
            OperationLabel.Text = operation;
            currentOperator = "";
            isNewNumber = true;
        }
    }
    private void PercentButton_Clicked(object sender, EventArgs e)
    {
        if (hasError)
            return;

        if (double.TryParse(
            DisplayLabel.Text,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double number))
        {
            number = number / 100;
            DisplayLabel.Text = FormatNumber(number);
        }
    }
    private void SignButton_Clicked(object sender, EventArgs e)
    {
        if (hasError)
            return;
        if (double.TryParse(
            DisplayLabel.Text,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double number))
        {
            number = -number;
            DisplayLabel.Text = FormatNumber(number);
        }
    }
    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (hasError)
        {
            ClearCalculator();
            return;
        }
        string currentText = DisplayLabel.Text;

        if (currentText.Length <= 1)
        {
            DisplayLabel.Text = "0";
            isNewNumber = true;
        }
        else
        {
            currentText =
                currentText.Substring(0, currentText.Length - 1);

            if (currentText == "-" || currentText == "")
            {
                DisplayLabel.Text = "0";
                isNewNumber = true;
            }
            else
            {
                DisplayLabel.Text = currentText;
            }
        }
    }
    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        ClearCalculator();
    }
    private void ClearCalculator()
    {
        firstNumber = 0;
        currentOperator = "";
        isNewNumber = true;
        hasError = false;

        DisplayLabel.Text = "0";
        OperationLabel.Text = "";
    }
    private void ShowError(string message)
    {
        DisplayLabel.Text = "Erreur";
        OperationLabel.Text = message;

        hasError = true;
        currentOperator = "";
        isNewNumber = true;
    }
    private string FormatNumber(double number)
    {
        if (number == Math.Truncate(number))
        {
            return number.ToString(
                "0",
                CultureInfo.InvariantCulture);
        }

        return number.ToString(
            "0.##########",
            CultureInfo.InvariantCulture);
    }
}