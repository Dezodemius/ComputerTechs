using System;
using System.Collections.Generic;
using System.IO;

namespace ComputerTechs
{
  internal static class Program
  {
    public static void Main(string[] args)
    {
      var A = new SquareMatrix(new double[,]
      {
        {2, 3},
        {3, 2}
      });
      
      var B = new SquareMatrix(new double[,]
      {
        {-10}
      });
      var expB = MatrixExponential(B);
      
      using (var writer = new StreamWriter($"{Environment.CurrentDirectory}/../../../Plotter/data.txt"))
      {
        const double a = 0;
        const double b = 2;
        const double n = 100;
        const double step = (b - a) / n;
        var t = a;
        for (var i = 0; i < n; i++)
        {
          writer.WriteLine(expB(t)[0, 0]);
          t += step;
        }
      }
    }

    /// <summary>
    /// Получить Жорданову форму матрицы.
    /// </summary>
    /// <param name="matrix">Матрица.</param>
    /// <returns>Жорданова форма матрицы.</returns>
    private static SquareMatrix GetJordanFrom(SquareMatrix matrix)
    {
      var aEigenvectors = matrix.GetEigenvectors();
      var H = new SquareMatrix(new double[,]
      {
        {aEigenvectors[0][0], aEigenvectors[0][1]},
        {aEigenvectors[1][0], aEigenvectors[1][1]}
      });
      
      return H.GetInverse() * matrix * H;;
    }

    /// <summary>
    /// Опорная функция множества.
    /// </summary>
    /// <param name="psi"></param>
    /// <returns></returns>
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

    private static double[] CalculateSupportingFunction(Func<double[], double> func, int n)
    {
      var result = new double[n];

      var rand = new Random();
      for (var i = 0; i < n; i++)
      {
        result[i] = func(new[]
        {
          Math.Sin(rand.NextDouble()),
          Math.Cos(rand.NextDouble())
        });
      }

      return result;
    }

    /// <summary>
    /// Матричный экспоненциал.
    /// </summary>
    /// <remarks>На вход приходит матрица, а возвращается функция, которая зависит от некоторого t.</remarks>
    /// <param name="matrix">Матрица.</param>
    /// <returns>Функция, которая может вычислять значения матричного экспоненциала.</returns>
    private static Func<double, SquareMatrix> MatrixExponential(SquareMatrix matrix)
    {
      if (matrix.Size == 2)
      {
        var eigenvalues = matrix.GetEigenvalues();
      
        var eps = 1e-6;
        if (Math.Abs(eigenvalues[0] - eigenvalues[1]) < eps)
        {
          return t =>
          {
            return new SquareMatrix(new double[,]
            {
              { Math.Exp(eigenvalues[0] * t), 1 },
              { 0, Math.Exp(eigenvalues[0] * t) }
            });
          };
        }

        return t =>
        {
          return new SquareMatrix(new double[,]
          {
            { Math.Exp(eigenvalues[0] * t), t },
            { 0, Math.Exp(eigenvalues[0] * t) }
          });
        };
      }

      if (matrix.Size > 2)
        throw new ArgumentException("Матрица размера больше 2-х не поддерживаются");

      return t =>
      {
        return new SquareMatrix(new double[,]
        {
          { Math.Exp(matrix[0, 0] * t)}
        });
      };
    }
  }
}