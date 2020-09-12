using System;
using System.IO;

namespace ComputerTechs
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      var data = CalculateSupportingFunction(10);
      using (var writer = new StreamWriter("data.txt"))
      {
        for (var i = 0; i < data.GetLength(0); i++)
          writer.Write($"{data[i]}\n");
      }
    }

    private static double SpecificSupportingFunction(double[] psi)
    {
      var psi1 = psi[0];
      var psi2 = psi[1];
      
      if (2 * psi2 <= psi1 && psi2 >= - psi1 / 2.0)
        return -psi1 * psi1 / psi2 / 4.0;
      if (psi1 >= 0 && 2 * psi2 >= - psi1)
        return psi1 + psi2;
      if (psi1 <= 0 && 2 * psi2 >= psi1)
        return psi2 - psi1;
      
      return double.NaN;
    }

    private static double[] CalculateSupportingFunction(int n)
    {
      var result = new double[n];

      var rand = new Random();
      for (var i = 0; i < n; i++)
      {
        result[i] = SpecificSupportingFunction(new[]
        {
          Math.Sin(rand.NextDouble()),
          Math.Cos(rand.NextDouble())
        });
      }

      return result;
    }
  }
}