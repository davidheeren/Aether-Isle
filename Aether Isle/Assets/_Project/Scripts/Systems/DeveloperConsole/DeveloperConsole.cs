using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using System.Linq;

namespace DeveloperConsole
{
    [RequireComponent(typeof(DeveloperConsoleUI))]
    public class DeveloperConsole : MonoBehaviour
    {
        public readonly Dictionary<string, IDeveloperCommand> commands = new Dictionary<string, IDeveloperCommand>();
        private readonly PrefixTree prefixTree = new PrefixTree();

        DeveloperConsoleUI consoleUI;

        private const string methodName = "Invoke";

        private void Awake()
        {
            consoleUI = GetComponent<DeveloperConsoleUI>();

            RegisterAllEnabledCommands();
        }

        private void RegisterCommand(IDeveloperCommand command)
        {
            commands.Add(command.ID, command);
            prefixTree.Add(CommandInfo(command));
        }

        public void RegisterAllEnabledCommands()
        {
            // Get all types in the current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Find all non-abstract classes that implement IDebugCommand
            IEnumerable<Type> commandTypes = assembly.GetTypes()
                .Where(t => typeof(IDeveloperCommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            // Instantiate and register each found command
            //foreach (Type type in commandTypes) // No sorting
            //{
            //    ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            //    if (constructor != null) // Only instantiate if a parameterless constructor exists
            //    {
            //        if (Activator.CreateInstance(type) is IDeveloperCommand command)
            //        {
            //            if (command.Enabled)
            //                RegisterCommand(command);
            //        }
            //    }
            //    else
            //    {
            //        Debug.LogError($"Skipping {type.Name}: No parameterless constructor found.");
            //    }
            //}

            // Instantiate, filter enabled commands, and sort by ID
            List<IDeveloperCommand> commands = commandTypes
                .Select(type =>
                {
                    ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                    if (constructor != null)
                    {
                        return Activator.CreateInstance(type) as IDeveloperCommand;
                    }
                    else
                    {
                        Debug.LogError($"Skipping {type.Name}: No parameterless constructor found.");
                        return null;
                    }
                })
                .Where(command => command != null && command.Enabled) // Filter out null and disabled commands
                .OrderBy(command => command.ID) // Sort by ID
                .ToList();

            foreach (var command in commands)
            {
                RegisterCommand(command);
            }
        }

        public void UpdateConsole(string input)
        {
            string[] inputs = SplitAndFormat(input);
            if (inputs.Length == 0) return;

            if (!commands.TryGetValue(inputs[0], out IDeveloperCommand command))
            {
                consoleUI.PrintConsole("Error: No command matches");
                return;
            }

            MethodInfo method = GetMethodInfo(command);

            ParameterInfo[] parameters = method.GetParameters();

            int expectedInputsLength = parameters.Length; // the number of parameters expected to be provided by user
            object[] convertedArgs = new object[parameters.Length];

            if (parameters.Length != 0 && parameters[parameters.Length - 1].ParameterType == typeof(DeveloperConsole))
            {
                convertedArgs[parameters.Length - 1] = this;
                expectedInputsLength--;
            }

            if (inputs.Length - 1 != expectedInputsLength)
            {
                consoleUI.PrintConsole($"Error: Incorrect number of arguments for '{inputs[0]}'. Expected: {GetParametersInfoString(command)}.");
                return;
            }

            for (int i = 0; i < inputs.Length - 1; i++)
            {
                try
                {
                    convertedArgs[i] = Convert.ChangeType(inputs[i + 1], parameters[i].ParameterType);
                }
                catch (Exception)
                {
                    consoleUI.PrintConsole($"Error: Invalid argument: <{inputs[i + 1]}> for parameter <{parameters[i].ParameterType.Name}: {parameters[i].Name}>");
                    return;
                }
            }

            try
            {
                method.Invoke(command, convertedArgs);
            }
            catch (TargetInvocationException ex)
            {
                consoleUI.PrintConsole($"Error executing command '{inputs[0]}': {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                consoleUI.PrintConsole($"Unexpected error: {ex.Message}");
            }
        }

        private MethodInfo GetMethodInfo(IDeveloperCommand command)
        {
            MethodInfo method = command.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance)!;

            if (method == null)
                throw new Exception($"Command does not have the {methodName} method");

            return method;
        }

        public string GetParametersInfoString(IDeveloperCommand command)
        {
            ParameterInfo[] parameters = GetMethodInfo(command).GetParameters();

            int length = parameters.Length;
            if (parameters.Length != 0 && parameters[parameters.Length - 1].ParameterType == typeof(DeveloperConsole))
                length--;

            string output = "";
            for (int i = 0; i < length; i++)
            {
                output += $" <{parameters[i].ParameterType.Name}: {parameters[i].Name}>";
            }
            return output;
        }

        public string CommandInfo(IDeveloperCommand command)
        {
            return $"{command.ID}{GetParametersInfoString(command)} -> {command.Description}";
        }

        private string[] SplitAndFormat(string input)
        {
            input = input.Trim(); // Remove leading and trailing spaces

            // Split the string on space while keeping sub strings intact
            string[] result = Regex.Split(input, @"(?<!\\) (?=(?:[^""]*""[^""]*"")*[^""]*$)");

            // Output the result, trimming quotes from each element
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = result[i].Trim('\"');  // Remove quotes from the substring
            }

            return result;
        }

        public void TestPrintCommands()
        {
            foreach (IDeveloperCommand command in commands.Values)
            {
                consoleUI.PrintConsole($"- {CommandInfo(command)}");
            }
        }

        public List<string> GetCommandListFromPrefix(string prefix)
        {
            string p = SplitAndFormat(prefix)[0];

            return prefixTree.GetWordsFromPrefix(p);
        }


        public void PrintConsole(string message)
        {
            consoleUI.PrintConsole(message);
        }

        public void ClearConsole()
        {
            consoleUI.ClearConsole();
        }
     }

    public interface IDeveloperCommand
    {
        public string ID { get; }
        public string Description { get; }
        public bool Enabled => true; // Override this to not include on start

        // Ex: public void Invoke(string msg, int count, DebugConsole debugConsole);
        // can have any number of parameters and optional DebugConsole at very end
    }

    public class HelpDebugCommand : IDeveloperCommand
    {
        public string ID => "help";

        public string Description => "Displays all available commands";

        public void Invoke(DeveloperConsole console)
        {
            console.PrintConsole("Usage example: 'test_command(Int32: x, Int32: y)' -- Type: 'test_command 6 8'");
            console.PrintConsole("Available commands:");
            console.TestPrintCommands();
        }
    }

    public class ClearDebugCommand : IDeveloperCommand
    {
        public string ID => "clear";

        public string Description => "Clears console";

        public void Invoke(DeveloperConsole console)
        {
            console.ClearConsole();
        }
    }

    public class MathOperationDebugCommand : IDeveloperCommand
    {
        public bool Enabled => false;

        public string ID => "math";

        public string Description => "Performs a basic math operation (add, subtract, multiply, divide) on two numbers";

        public void Invoke(string operation, float num1, float num2, DeveloperConsole console)
        {
            float result;
            switch (operation.ToLower())
            {
                case "add":
                    result = num1 + num2;
                    break;
                case "subtract":
                    result = num1 - num2;
                    break;
                case "multiply":
                    result = num1 * num2;
                    break;
                case "divide":
                    if (num2 == 0)
                    {
                        console.PrintConsole("Error: Cannot divide by zero.");
                        return;
                    }
                    result = num1 / num2;
                    break;
                default:
                    console.PrintConsole($"Error: Unknown operation: {operation}. Use 'add', 'subtract', 'multiply', or 'divide'.");
                    return;
            }

            console.PrintConsole($"Result of {num1} {operation} {num2} = {result}");
        }
    }
}
