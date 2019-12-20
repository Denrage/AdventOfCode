using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var start = 372037;
        var end = 905157;
        var possibilites = new List<int>();
        for (int i = start; i < end; i++)
        {
            var valid = true;
            var stringRepresentation = i.ToString();
            for (int j = 1; j < stringRepresentation.Length; j++)
            {
                if (char.GetNumericValue(stringRepresentation[j - 1]) > char.GetNumericValue(stringRepresentation[j]))
                {
                    valid = false;
                }
            }

            var counter = new int[10];
            for (int j = 0; j < stringRepresentation.Length; j++)
            {
                counter[(int)char.GetNumericValue(stringRepresentation[j])]++;
            }

            if (!counter.Any(x => x == 2))
            {
                valid = false;
            }

            if (valid)
            {
                possibilites.Add(i);
            }
        }

        Console.WriteLine(possibilites.Count);
        Console.ReadKey();
    }
}
