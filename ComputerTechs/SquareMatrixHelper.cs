using System;
using System.Collections.Generic;
using System.Linq;
using Meta.Numerics.Matrices;

namespace ComputerTechs
{
  /// <summary>
  /// Вспомогательный класс для работы со SquareMatrix.
  /// </summary>
  public static class SquareMatrixHelper
  {
    /// <summary>
    /// Получить функцию для расчёта матричной экспоненты в точке t.
    /// </summary>
    /// <param name="entries">ИсходныGй массив элементов.</param>
    /// <returns>Функци матричного экспоненциала.</returns>
    public static Func<double, SquareMatrix> ExpM(double[,] entries)
    {
      var matrix = new SquareMatrix(entries);
      return ExpM(matrix);
    }

    /// <summary>
    /// Получить функцию для расчёта матричной экспоненты в точке t.
    /// </summary>
    /// <param name="matrix">Исходная матрица.</param>
    /// <returns>Функци матричного экспоненциала.</returns>
    public static Func<double, SquareMatrix> ExpM(SquareMatrix matrix)
    {
      var transitionMatrix = matrix.GetEigenvectors();
      var jordanForm = matrix.GetJordanForm();
      
      return t => transitionMatrix * GetJordanFormExpM(jordanForm)(t) * transitionMatrix.GetInverse();
    }
    
    private static Func<double, SquareMatrix> GetJordanFormExpM(SquareMatrix jordanForm)
    {
      var eigenvalues = jordanForm.Eigenvalues();
      
      var l1 = eigenvalues[0].Re;
      var l2 = eigenvalues[1].Re;
      
      // Если l1 и l2 разные. 
      if ((l1 - l2).IsNotZero())
      {
        return t => { return new SquareMatrix(new [,]
          {
            { Math.Exp(l1 * t), 0.0 }, 
            { 0.0, Math.Exp(l2 * t) }
          });
        };
      }
      if (jordanForm.GetGeometricMultiplicity(l1) == 2)
      {
        return t => { return new SquareMatrix(new [,]
          {
            { Math.Exp(l1 * t), 0.0 }, 
            { 0.0, Math.Exp(l1 * t) }
          });
        };
      }
      
      return t => { return new SquareMatrix(new [,]
        {
          { Math.Exp(l1 * t), (t - 1) * Math.Exp(l1 * t) }, 
          { 0.0, Math.Exp(l1 * t) }
        });
      }; 
    }
    
    /// <summary>
    /// Получить единичную матрицу.
    /// </summary>
    /// <param name="size">Размер матрицы.</param>
    public static SquareMatrix GetIdentityMatrix(int size)
    {
      var identityMatrix = new SquareMatrix(size);
      identityMatrix.Fill((i, j) => i == j ? 1.0 : 0.0);
      return identityMatrix;
    }

    /// <summary>
    /// Получить SquareMatrix на основе одномерного массива.
    /// </summary>
    /// <param name="entries">Входящий одномерный массив.</param>
    /// <returns>Квадратная матрица.</returns>
    public static SquareMatrix SquareMatrix(double[] entries)
    {
      var n = (int)Math.Sqrt(entries.Length);
      var a = new double[n, n];
      
      for (var i = 0; i < n; i++)
      {
        for (var j = 0; j < n; j++)
        {
          a[i, j] = entries[n * i + j];
        }
      }
      
      return new SquareMatrix(a);
    }
  }

  /// <summary>
  /// Вспомогательный класс для работы со ColumnVector и RowVector.
  /// </summary>
  public static class VectorHelper
  {
    public static double Prod(ColumnVector a, ColumnVector b)
    {
      if (a.ColumnCount != b.ColumnCount)
        throw new ArgumentException("Размерности векторов не совпадают.");
      var sum = 0.0;
      for (var i = 0; i < a.ColumnCount; i++)
        sum += a[i] * b[i];
      return sum;
    }
    
    /// <summary>
    /// Проверить равен ли вектор нулевому вектору.
    /// </summary>
    /// <param name="vector">Исходный вектор.</param>
    /// <returns><c>True</c>, если все элементы вектора равны 0.0 с некоторой точностью.</returns>
    public static bool IsZero(this ColumnVector vector)
    {
      return vector.All(i => i.IsZero());
    }
    
    /// <summary>
    /// Проверить равен ли вектор нулевому вектору.
    /// </summary>
    /// <param name="vector">Исходный вектор.</param>
    /// <returns><c>True</c>, если все элементы вектора равны 0.0 с некоторой точностью.</returns>
    public static bool IsZero(this RowVector vector)
    {
      return vector.All(i => i.IsZero());
    }
  }
}