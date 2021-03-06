using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var operations = new Operation[]
        {
            new Addition(),
            new Multiplication(),
            new Halt(),
            new Input(),
            new Output(),
            new JumpTrue(),
            new JumpFalse(),
            new Less(),
            new Equals(),
        };

        var input = "3,225,1,225,6,6,1100,1,238,225,104,0,1101,11,91,225,1002,121,77,224,101,-6314,224,224,4,224,1002,223,8,223,1001,224,3,224,1,223,224,223,1102,74,62,225,1102,82,7,224,1001,224,-574,224,4,224,102,8,223,223,1001,224,3,224,1,224,223,223,1101,28,67,225,1102,42,15,225,2,196,96,224,101,-4446,224,224,4,224,102,8,223,223,101,6,224,224,1,223,224,223,1101,86,57,225,1,148,69,224,1001,224,-77,224,4,224,102,8,223,223,1001,224,2,224,1,223,224,223,1101,82,83,225,101,87,14,224,1001,224,-178,224,4,224,1002,223,8,223,101,7,224,224,1,223,224,223,1101,38,35,225,102,31,65,224,1001,224,-868,224,4,224,1002,223,8,223,1001,224,5,224,1,223,224,223,1101,57,27,224,1001,224,-84,224,4,224,102,8,223,223,1001,224,7,224,1,223,224,223,1101,61,78,225,1001,40,27,224,101,-89,224,224,4,224,1002,223,8,223,1001,224,1,224,1,224,223,223,4,223,99,0,0,0,677,0,0,0,0,0,0,0,0,0,0,0,1105,0,99999,1105,227,247,1105,1,99999,1005,227,99999,1005,0,256,1105,1,99999,1106,227,99999,1106,0,265,1105,1,99999,1006,0,99999,1006,227,274,1105,1,99999,1105,1,280,1105,1,99999,1,225,225,225,1101,294,0,0,105,1,0,1105,1,99999,1106,0,300,1105,1,99999,1,225,225,225,1101,314,0,0,106,0,0,1105,1,99999,1008,677,226,224,1002,223,2,223,1006,224,329,101,1,223,223,8,226,677,224,102,2,223,223,1005,224,344,101,1,223,223,1107,226,677,224,102,2,223,223,1006,224,359,101,1,223,223,1007,226,226,224,102,2,223,223,1006,224,374,101,1,223,223,7,677,677,224,102,2,223,223,1005,224,389,1001,223,1,223,108,677,677,224,1002,223,2,223,1005,224,404,101,1,223,223,1008,226,226,224,102,2,223,223,1005,224,419,1001,223,1,223,1107,677,226,224,102,2,223,223,1005,224,434,1001,223,1,223,1108,677,677,224,102,2,223,223,1006,224,449,1001,223,1,223,7,226,677,224,102,2,223,223,1005,224,464,101,1,223,223,1008,677,677,224,102,2,223,223,1005,224,479,101,1,223,223,1007,226,677,224,1002,223,2,223,1006,224,494,101,1,223,223,8,677,226,224,1002,223,2,223,1005,224,509,101,1,223,223,1007,677,677,224,1002,223,2,223,1006,224,524,101,1,223,223,107,226,226,224,102,2,223,223,1006,224,539,101,1,223,223,107,226,677,224,102,2,223,223,1005,224,554,1001,223,1,223,7,677,226,224,102,2,223,223,1006,224,569,1001,223,1,223,107,677,677,224,1002,223,2,223,1005,224,584,101,1,223,223,1107,677,677,224,102,2,223,223,1005,224,599,101,1,223,223,1108,226,677,224,102,2,223,223,1006,224,614,101,1,223,223,8,226,226,224,102,2,223,223,1006,224,629,101,1,223,223,108,226,677,224,102,2,223,223,1005,224,644,1001,223,1,223,108,226,226,224,102,2,223,223,1005,224,659,101,1,223,223,1108,677,226,224,102,2,223,223,1006,224,674,1001,223,1,223,4,223,99,226";
        var splitted = input.Split(',').Select(x => int.Parse(x)).ToArray();

        var tempInput = splitted.ToArray();
        for (int i = 0; i < tempInput.Length;)
        {
            var tempI = i;
            var stringRepresentation = tempInput[i].ToString();
            var operationCode = stringRepresentation.Length >= 2 ? int.Parse(stringRepresentation.Substring(stringRepresentation.Length - 2)) : tempInput[i];
            var parameterModes = stringRepresentation.Length > 2 ? stringRepresentation.Substring(0, stringRepresentation.Length - 2).ToCharArray().Select(x => char.GetNumericValue(x)).Select(x => (ParameterMode)x).Reverse().ToList() : new List<ParameterMode>();
            var operation = operations.First(x => x.OperationCode == operationCode);
            if (operation is Halt)
            {
                break;
            }

            while (parameterModes.Count != operation.ParameterCount)
            {
                parameterModes.Add(ParameterMode.Position);
            }

            if (i + operation.ParameterCount < tempInput.Length)
            {
                //var outOfRange = false;
                //for (int j = 1; j < operation.ParameterCount + 1; j++)
                //{
                //    if (tempInput[i + j] < 0 || tempInput[i + j] >= tempInput.Length)
                //    {
                //        outOfRange = true;
                //    }
                //}

                //if (outOfRange)
                //{
                //    break;
                //}

                operation.Operate(ref tempInput, ref i, parameterModes.ToArray());
            }
            if (tempI == i)
            {
                i += operation.ParameterCount + 1;
            }
        }

        Console.WriteLine(tempInput[0]);
        Console.WriteLine("Done");

        Console.ReadKey();
    }

    public abstract class Operation
    {
        public abstract int OperationCode { get; }

        public abstract int ParameterCount { get; }

        public abstract void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes);
    }

    public enum ParameterMode
    {
        Position = 0,
        Immediate = 1,
    }

    public class Addition : Operation
    {
        public override int OperationCode => 1;

        public override int ParameterCount => 3;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes)
        {
            int parameter1 = 0;
            int parameter2 = 0;

            switch (modes[0])
            {
                case ParameterMode.Position:
                    parameter1 = tape[tape[operationCodeIndex + 1]];
                    break;

                case ParameterMode.Immediate:
                    parameter1 = tape[operationCodeIndex + 1];
                    break;

                default:
                    break;
            }

            switch (modes[1])
            {
                case ParameterMode.Position:
                    parameter2 = tape[tape[operationCodeIndex + 2]];
                    break;

                case ParameterMode.Immediate:
                    parameter2 = tape[operationCodeIndex + 2];
                    break;

                default:
                    break;
            }
            //Console.WriteLine(parameter1 + " + " + parameter2);

            tape[tape[operationCodeIndex + 3]] = parameter1 + parameter2;
        }
    }

    public class Multiplication : Operation
    {
        public override int OperationCode => 2;

        public override int ParameterCount => 3;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes)
        {
            int parameter1 = 0;
            int parameter2 = 0;

            switch (modes[0])
            {
                case ParameterMode.Position:
                    parameter1 = tape[tape[operationCodeIndex + 1]];
                    break;

                case ParameterMode.Immediate:
                    parameter1 = tape[operationCodeIndex + 1];
                    break;

                default:
                    break;
            }

            switch (modes[1])
            {
                case ParameterMode.Position:
                    parameter2 = tape[tape[operationCodeIndex + 2]];
                    break;

                case ParameterMode.Immediate:
                    parameter2 = tape[operationCodeIndex + 2];
                    break;

                default:
                    break;
            }

            //Console.WriteLine(parameter1 + " * " + parameter2);

            tape[tape[operationCodeIndex + 3]] = parameter1 * parameter2;
        }
    }

    public class Halt : Operation
    {
        public override int OperationCode => 99;

        public override int ParameterCount => 0;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes)
        {
        }
    }

    public class Input : Operation
    {
        public override int OperationCode => 3;

        public override int ParameterCount => 1;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes)
        {
            Console.Write("Input: ");
            tape[tape[operationCodeIndex + 1]] = int.Parse(Console.ReadLine());
        }
    }

    public class Output : Operation
    {
        public override int OperationCode => 4;

        public override int ParameterCount => 1;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes) => Console.WriteLine("Output: " + tape[tape[operationCodeIndex + 1]]);
    }

    public class JumpTrue : Operation
    {
        public override int OperationCode => 5;

        public override int ParameterCount => 2;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes)
        {
            int parameter1 = 0;
            int parameter2 = 0;

            switch (modes[0])
            {
                case ParameterMode.Position:
                    parameter1 = tape[tape[operationCodeIndex + 1]];
                    break;

                case ParameterMode.Immediate:
                    parameter1 = tape[operationCodeIndex + 1];
                    break;
            }

            switch (modes[1])
            {
                case ParameterMode.Position:
                    parameter2 = tape[tape[operationCodeIndex + 2]];
                    break;

                case ParameterMode.Immediate:
                    parameter2 = tape[operationCodeIndex + 2];
                    break;
            }

            if (parameter1 != 0)
            {
                operationCodeIndex = parameter2;
            }
        }
    }

    public class JumpFalse : Operation
    {
        public override int OperationCode => 6;

        public override int ParameterCount => 2;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes)
        {
            int parameter1 = 0;
            int parameter2 = 0;

            switch (modes[0])
            {
                case ParameterMode.Position:
                    parameter1 = tape[tape[operationCodeIndex + 1]];
                    break;

                case ParameterMode.Immediate:
                    parameter1 = tape[operationCodeIndex + 1];
                    break;
            }

            switch (modes[1])
            {
                case ParameterMode.Position:
                    parameter2 = tape[tape[operationCodeIndex + 2]];
                    break;

                case ParameterMode.Immediate:
                    parameter2 = tape[operationCodeIndex + 2];
                    break;
            }

            if (parameter1 == 0)
            {
                operationCodeIndex = parameter2;
            }
        }
    }

    public class Less : Operation
    {
        public override int OperationCode => 7;

        public override int ParameterCount => 3;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes)
        {
            int parameter1 = 0;
            int parameter2 = 0;

            switch (modes[0])
            {
                case ParameterMode.Position:
                    parameter1 = tape[tape[operationCodeIndex + 1]];
                    break;

                case ParameterMode.Immediate:
                    parameter1 = tape[operationCodeIndex + 1];
                    break;
            }

            switch (modes[1])
            {
                case ParameterMode.Position:
                    parameter2 = tape[tape[operationCodeIndex + 2]];
                    break;

                case ParameterMode.Immediate:
                    parameter2 = tape[operationCodeIndex + 2];
                    break;
            }

            if (parameter1 < parameter2)
            {
                tape[tape[operationCodeIndex + 3]] = 1;
            }
            else
            {
                tape[tape[operationCodeIndex + 3]] = 0;
            }
        }
    }

    public class Equals : Operation
    {
        public override int OperationCode => 8;

        public override int ParameterCount => 3;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes)
        {
            int parameter1 = 0;
            int parameter2 = 0;

            switch (modes[0])
            {
                case ParameterMode.Position:
                    parameter1 = tape[tape[operationCodeIndex + 1]];
                    break;

                case ParameterMode.Immediate:
                    parameter1 = tape[operationCodeIndex + 1];
                    break;
            }

            switch (modes[1])
            {
                case ParameterMode.Position:
                    parameter2 = tape[tape[operationCodeIndex + 2]];
                    break;

                case ParameterMode.Immediate:
                    parameter2 = tape[operationCodeIndex + 2];
                    break;
            }

            if (parameter1 == parameter2)
            {
                tape[tape[operationCodeIndex + 3]] = 1;
            }
            else
            {
                tape[tape[operationCodeIndex + 3]] = 0;
            }
        }
    }
}
