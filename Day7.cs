using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var input = "3,8,1001,8,10,8,105,1,0,0,21,34,55,68,85,106,187,268,349,430,99999,3,9,1001,9,5,9,1002,9,5,9,4,9,99,3,9,1002,9,2,9,1001,9,2,9,1002,9,5,9,1001,9,2,9,4,9,99,3,9,101,3,9,9,102,3,9,9,4,9,99,3,9,1002,9,5,9,101,3,9,9,102,5,9,9,4,9,99,3,9,1002,9,4,9,1001,9,2,9,102,3,9,9,101,3,9,9,4,9,99,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,1001,9,1,9,4,9,99,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,99";

        var phaseSettings = Enumerable.Range(0, 44444)
            .Select(i => i.ToString().PadLeft(5, '0'))
            .Where(i => i.Distinct().Count() == 5 && !i.Any(x => (int)char.GetNumericValue(x) > 4))
            .ToList();

        var maxAmplification = int.MinValue;

        foreach (var phaseSetting in phaseSettings)
        {
            Computer previousAmplifier = null;
            for (int i = 0; i < 5; i++)
            {
                var outputProvider = previousAmplifier != null ? previousAmplifier.OutputProvider : new ZeroAmplifierOutputProvider();
                var amplifier = new Computer(input, new AmplifierOutputProvider(), new AmplifierInputProvider((int)char.GetNumericValue(phaseSetting[i]), outputProvider.Output));
                amplifier.Start();
                previousAmplifier = amplifier;
            }
            if (previousAmplifier.OutputProvider.Output > maxAmplification)
            {
                maxAmplification = previousAmplifier.OutputProvider.Output;
            }
        }

        Console.WriteLine(maxAmplification);
        Console.WriteLine("Done");
        Console.ReadKey();
    }

    public class Computer
    {
        private readonly string program;

        private readonly Operation[] operations;

        public IOutputProvider OutputProvider { get; }

        public Computer(string program, IOutputProvider outputProvider, IInputProvider inputProvider)
        {
            this.program = program;
            this.OutputProvider = outputProvider;
            this.operations = new Operation[]
            {
                new Addition(),
                new Multiplication(),
                new Halt(),
                new Input(inputProvider),
                new Output(this.OutputProvider),
                new JumpTrue(),
                new JumpFalse(),
                new Less(),
                new Equals(),
            };
        }

        public void Start()
        {
            var splitted = this.program.Split(',').Select(x => int.Parse(x)).ToArray();

            var tempInput = splitted.ToArray();
            for (int i = 0; i < tempInput.Length;)
            {
                var tempI = i;
                var stringRepresentation = tempInput[i].ToString();
                var operationCode = stringRepresentation.Length >= 2 ? int.Parse(stringRepresentation.Substring(stringRepresentation.Length - 2)) : tempInput[i];
                var parameterModes = stringRepresentation.Length > 2 ? stringRepresentation.Substring(0, stringRepresentation.Length - 2).ToCharArray().Select(x => char.GetNumericValue(x)).Select(x => (ParameterMode)x).Reverse().ToList() : new List<ParameterMode>();
                var operation = this.operations.FirstOrDefault(x => x.OperationCode == operationCode);
                if (operation is null || operation is Halt)
                {
                    break;
                }

                while (parameterModes.Count != operation.ParameterCount)
                {
                    parameterModes.Add(ParameterMode.Position);
                }

                if (i + operation.ParameterCount < tempInput.Length)
                {
                    operation.Operate(ref tempInput, ref i, parameterModes.ToArray());
                }
                if (tempI == i)
                {
                    i += operation.ParameterCount + 1;
                }
            }
        }
    }

    public abstract class Operation
    {
        public abstract int OperationCode { get; }

        public abstract int ParameterCount { get; }

        public abstract void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes);

        protected int GetValue(ref int[] tape, int index, ParameterMode mode)
        {
            switch (mode)
            {
                case ParameterMode.Position:
                    return tape[tape[index]];

                case ParameterMode.Immediate:
                    return tape[index];
            }

            throw new NotImplementedException();
        }
    }

    public interface IOutputProvider
    {
        int Output { get; }

        void Set(int output);
    }

    public class AmplifierOutputProvider : IOutputProvider
    {
        public int Output { get; private set; }

        public void Set(int output)
        {
            this.Output = output;
            Console.WriteLine(this.Output);
        }
    }

    public class ZeroAmplifierOutputProvider : IOutputProvider
    {
        public int Output => 0;

        public void Set(int output) => throw new NotImplementedException();
    }

    public interface IInputProvider
    {
        int Get();
    }

    public class AmplifierInputProvider : IInputProvider
    {
        private readonly int phaseSetting;
        private readonly int input;
        private int counter = 0;

        public AmplifierInputProvider(int phaseSetting, int input)
        {
            this.phaseSetting = phaseSetting;
            this.input = input;
        }

        public int Get()
        {
            if (this.counter == 0)
            {
                this.counter++;
                return this.phaseSetting;
            }
            else if (this.counter == 1)
            {
                this.counter++;
                return this.input;
            }

            throw new NotImplementedException();
        }
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

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes) => tape[tape[operationCodeIndex + 3]] = this.GetValue(ref tape, operationCodeIndex + 1, modes[0]) + this.GetValue(ref tape, operationCodeIndex + 2, modes[1]);
    }

    public class Multiplication : Operation
    {
        public override int OperationCode => 2;

        public override int ParameterCount => 3;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes) => tape[tape[operationCodeIndex + 3]] = this.GetValue(ref tape, operationCodeIndex + 1, modes[0]) * this.GetValue(ref tape, operationCodeIndex + 2, modes[1]);
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
        private readonly IInputProvider inputProvider;

        public override int OperationCode => 3;

        public override int ParameterCount => 1;

        public Input(IInputProvider inputProvider)
        {
            this.inputProvider = inputProvider;
        }

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes) => tape[tape[operationCodeIndex + 1]] = this.inputProvider.Get();
    }

    public class Output : Operation
    {
        private readonly IOutputProvider outputProvider;

        public override int OperationCode => 4;

        public override int ParameterCount => 1;

        public Output(IOutputProvider outputProvider)
        {
            this.outputProvider = outputProvider;
        }

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes) => this.outputProvider.Set(tape[tape[operationCodeIndex + 1]]);
    }

    public class JumpTrue : Operation
    {
        public override int OperationCode => 5;

        public override int ParameterCount => 2;

        public override void Operate(ref int[] tape, ref int operationCodeIndex, ParameterMode[] modes)
        {
            if (this.GetValue(ref tape, operationCodeIndex + 1, modes[0]) != 0)
            {
                operationCodeIndex = this.GetValue(ref tape, operationCodeIndex + 2, modes[1]);
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

            if (this.GetValue(ref tape, operationCodeIndex + 1, modes[0]) == 0)
            {
                operationCodeIndex = this.GetValue(ref tape, operationCodeIndex + 2, modes[1]);
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

            if (this.GetValue(ref tape, operationCodeIndex + 1, modes[0]) < this.GetValue(ref tape, operationCodeIndex + 2, modes[1]))
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
            if (this.GetValue(ref tape, operationCodeIndex + 1, modes[1]) == this.GetValue(ref tape, operationCodeIndex + 2, modes[1]))
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
