using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PostFixOperation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press q to quit or c to continue the program!");

            string filename = null;
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            var directory = new FileInfo(location.AbsolutePath).Directory;

            

            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                Console.WriteLine();
                if (args.Length == 0)
                {                    
                    Console.WriteLine("Command argument is not found to read input.txt. Type input.txt which should be along with .exe in same location");
                    filename = Console.ReadLine();
                }
                else
                {
                    filename = args[0].ToLower();
                }

                if (!string.IsNullOrEmpty(filename) && filename != "input.txt")
                {
                    Console.WriteLine("entered invalid file name. please pass file name as command argument");
                    Environment.Exit(0);
                }
                else
                {
                    string[] expressions = Operations.getContentInFile(directory + "\\" + filename);

                    if (expressions == null || expressions.Length == 0)
                    {
                        Console.WriteLine("File cannot be empty or void");
                    }
                    else
                    {
                        var newexpressions = expressions.Where(x => !string.IsNullOrEmpty(x));
                        foreach (string expression in newexpressions)
                        {
                            Console.WriteLine(expression);

                            if (expression.Length == 1 && Convert.ToChar(expression.ToLower()) == 'q')
                            {
                                Console.WriteLine("file contains quit statment. hence exiting the application");
                                Environment.Exit(0);
                            }

                            Operations.processExpression(expression);
                        }
                    }
                }
            }
        }
    }

    internal class Operations
    {
        internal static string[] getContentInFile(string filename)
        {
            string[] lines = null;
            try
            {
                if (filename.Contains("input.txt") && File.Exists(filename))
                {
                    lines = File.ReadAllLines(filename);
                }
                else
                {
                    Console.WriteLine("File not found");
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            return lines;

        }

        internal static void processExpression(string expression)
        {
            Stack<double> myStack = new Stack<double>();
            string[] tokens = expression.Trim().Split(' ');
            double resultdouble;

            foreach (var token in tokens)
            {
                if (Double.TryParse(token, out resultdouble))
                {
                    myStack.Push(resultdouble);
                }
                else
                {
                    var rightvalue = myStack.Pop();
                    var leftvalue = myStack.Pop();
                    double result;

                    result = Operations.OperationSwitch(token, leftvalue, rightvalue);
                    myStack.Push(result);
                }
            }
            Console.WriteLine("expression result after posfix operation is : " + myStack.Pop());
        }

        internal static double OperationSwitch(string operation, double leftvalue, double rightvalue)
        {
            //float result;
            switch (operation)
            {
                case "+": return leftvalue + rightvalue;
                case "-": return leftvalue - rightvalue;
                case "*": return leftvalue * rightvalue;
                case "/": return leftvalue / rightvalue;
                default: throw new InvalidOperationException("invalid operation");
            }
        }
    }
}
