using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lab12.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Lab12.Controllers
{
    public class CalculatorController : Controller
    {
        [HttpGet]
        public IActionResult ManualSingle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ManualSingle(IFormCollection form)
        {
            var firstNumStr = form["firstNum"];
            var secondNumStr = form["secondNum"];
            var operationStr = form["operation"];

            if (double.TryParse(firstNumStr, out double firstNum) &&
                double.TryParse(secondNumStr, out double secondNum) &&
                Enum.TryParse(operationStr, out Operations operation))
            {
                double result = operation switch
                {
                    Operations.Sum => firstNum + secondNum,
                    Operations.Sub => firstNum - secondNum,
                    Operations.Mult => firstNum * secondNum,
                    Operations.Div => secondNum != 0 ? firstNum / secondNum : double.NaN,
                    _ => 0
                };

                ViewBag.firstNum = firstNum;
                ViewBag.secondNum = secondNum;
                ViewBag.operation = GetOperationSymbol(operation);
                ViewBag.result = double.IsNaN(result) ? "Error: деление на ноль" : result.ToString();

                return View("Result");
            }
            else
            {
                ViewBag.Error = "Error: введите число";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ManualSeparate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ManualSeparate(IFormCollection form)
        {
            var firstNumVal = form["firstNum"];
            var secondNumVal = form["secondNum"];
            var operationVal = form["operation"];

            // Action - Validation
            if (!Validate(firstNumVal, secondNumVal, operationVal, out double firstNumber, out double secondNumber, out Operations selectedOperation))
            {
                return View();
            }

            // Action - Calculate 
            double result = CalculateResult(firstNumber, secondNumber, selectedOperation);

            ViewBag.firstNum = firstNumber;
            ViewBag.secondNum = secondNumber;
            ViewBag.operation = GetOperationSymbol(selectedOperation);
            ViewBag.result = double.IsNaN(result) ? "Error: деление на ноль" : result.ToString();

            return View("Result");
        }

        [HttpGet]
        public IActionResult BindingParameters()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BindingParameters(string firstNum, string secondNum, Operations operation)
        {
            if (!double.TryParse(firstNum, out double number1) || !double.TryParse(secondNum, out double number2))
            {
                ViewBag.Error = "Error: введите число";
                return View();
            }

            double result = operation switch
            {
                Operations.Sum => number1 + number2,
                Operations.Sub => number1 - number2,
                Operations.Mult => number1 * number2,
                Operations.Div => number2 != 0 ? number1 / number2 : double.NaN,
                _ => 0
            };

            ViewBag.firstNum = firstNum;
            ViewBag.secondNum = secondNum;
            ViewBag.operation = GetOperationSymbol(operation);
            ViewBag.result = double.IsNaN(result) ? "Error: деление на ноль" : result.ToString();

            return View("Result");
        }

        [HttpGet]
        public IActionResult BindingSeparate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BindingSeparate(Calculate model)
        {
            if (ModelState.IsValid)
            {
                double result = model.operation switch
                {
                    Operations.Sum => model.firstNum + model.secondNum,
                    Operations.Sub => model.firstNum - model.secondNum,
                    Operations.Mult => model.firstNum * model.secondNum,
                    Operations.Div => model.secondNum != 0 ? model.firstNum / model.secondNum : double.NaN,
                    _ => 0
                };

                ViewBag.firstNum = model.firstNum;
                ViewBag.secondNum = model.secondNum;
                ViewBag.operation = GetOperationSymbol(model.operation);
                ViewBag.result = double.IsNaN(result) ? "Error: деление на ноль" : result.ToString();
                return View("Result");
            }
            else
            {
                ViewBag.Error = "Error: введите число";
                return View();
            }
        }

        private string GetOperationSymbol(Operations operation)
        {
            var displayAttribute = operation.GetType()
                                            .GetMember(operation.ToString())[0]
                                            .GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? operation.ToString();
        }

        private double CalculateResult(double firstNum, double secondNum, Operations operation)
        {
            return operation switch
            {
                Operations.Sum => firstNum + secondNum,
                Operations.Sub => firstNum - secondNum,
                Operations.Mult => firstNum * secondNum,
                Operations.Div => secondNum != 0 ? firstNum / secondNum : double.NaN,
                _ => 0
            };
        }

        private bool Validate(string num1, string num2, string operation, out double firstNumber, out double secondNumber, out Operations selectedOperation)
        {
            bool isValid = true;
            firstNumber = secondNumber = 0;
            selectedOperation = Operations.Sum; 

            if (!double.TryParse(num1, out firstNumber) || !double.TryParse(num2, out secondNumber))
            {
                ViewBag.Error = "Error: введите число";
                isValid = false;
            }

            if (!Enum.TryParse(operation, out selectedOperation))
            {
                ViewBag.Error = "Error";
                isValid = false;
            }

            return isValid;
        }
    }
}
