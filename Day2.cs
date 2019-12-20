using System;
using System.Linq;
					
public class Program
{
	public static void Main()
	{
		var input = "1,12,2,3,1,1,2,3,1,3,4,3,1,5,0,3,2,6,1,19,1,19,9,23,1,23,9,27,1,10,27,31,1,13,31,35,1,35,10,39,2,39,9,43,1,43,13,47,1,5,47,51,1,6,51,55,1,13,55,59,1,59,6,63,1,63,10,67,2,67,6,71,1,71,5,75,2,75,10,79,1,79,6,83,1,83,5,87,1,87,6,91,1,91,13,95,1,95,6,99,2,99,10,103,1,103,6,107,2,6,107,111,1,13,111,115,2,115,10,119,1,119,5,123,2,10,123,127,2,127,9,131,1,5,131,135,2,10,135,139,2,139,9,143,1,143,2,147,1,5,147,0,99,2,0,14,0";
		var splitted = input.Split(',').Select(x => int.Parse(x)).ToArray();
		for(int j = 0; j <= 99; j++)
		{
			for(int k = 0 ;k <= 99; k++)
			{
				var tempInput = splitted.ToArray();
				tempInput[2] = k;
				tempInput[1] = j;
				for(int i = 0; i + 3 < tempInput.Length; i += 4)
				{
					var op = tempInput[i];
					var param1 = tempInput[i+1];
					var param2 = tempInput[i+2];
					var param3 = tempInput[i+3];
					
					if(param1 >= 0 && param1 < tempInput.Length)
					{
						if(param2 >= 0 && param2 < tempInput.Length)
						{
							if(param3 >= 0 && param3 < tempInput.Length)
							{
								if(op == 1)
								{
									var temp = tempInput[param1] + tempInput[param2];
									tempInput[param3] = temp;
								}
								else if(op == 2)
								{
									var temp = tempInput[param1] * tempInput[param2];
									tempInput[param3] = temp;
								}
							}
						}
					}
					if(op == 99)
					{
						break;
					}
				}
				if(tempInput[0] == 19690720)
				{
					Console.WriteLine("Verb: " + k);
					Console.WriteLine("Noun: " + j);
					break;
				}
			}
		}
	}
}
