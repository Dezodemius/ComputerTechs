using System;
using System.Collections.Generic;

namespace ComputerTechs
{
  public class SquareMatrix
  {
    #region Поля и свойства

    public int Size { get; protected set; }
    
    public double[,] BasedArray { get; }

    #endregion

    #region Операторы

    public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b)
    {
      var n = a.Size;
      var c = new SquareMatrix(n);
      for (var i = 0; i < n; i++)
      {
        for (var j = 0; j < n; j++)
        {
          c[i, j] = a[i, j] + b[i, j];
        } 
      }

      return c;
    }

    #endregion

    #region Индексатор
    
    public double this[int i, int j]
    {
      get => BasedArray[i, j];
      set => BasedArray[i, j] = value;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="n">Размер матрицы.</param>
    public SquareMatrix(int n)
    {
      Size = n;
      BasedArray = new double[n, n];
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="basedArray">Исходный массив.</param>
    public SquareMatrix(double[,] basedArray)
    {
      BasedArray = basedArray;
      Size = basedArray.GetLength(0);
    }

    #endregion
  }

  /// <summary>
  /// Класс с методами расширения класса SquareMatrix.
  /// </summary>
  public static class SquareMatrixExtensions
  {
    public static double[] GetEigenvalues(this SquareMatrix matrix)
    {
      if (matrix.Size != 2)
        throw new ArgumentException("Матрица размера больше 2-х не поддерживаются");
      
      var eigenvalues = new double[matrix.Size];
      var a = matrix[0, 0];
      var b = matrix[0, 1];
      var c = matrix[1, 0];
      var d = matrix[1, 1];
        
      var discriminant = Math.Pow(a + d, 2) - 4 * (a * d - b * c);
      
      eigenvalues[0] = ((a + d) + Math.Sqrt(discriminant)) * 0.5;
      eigenvalues[1] = ((a + d) - Math.Sqrt(discriminant)) * 0.5;
      
      return eigenvalues;
    }

    public static List<double[]> GetEigenvectors(this SquareMatrix matrix)
    {
      var eigenvectors = new List<double[]>(new [] {new double[2], new double[2] });
      
      var eigenvalues = matrix.GetEigenvalues();

      var a = matrix[0, 0];
      var b = matrix[0, 1];

      for (var i = 0; i < matrix.Size; i++)
      {
        var tmp = -b / (a - eigenvalues[i]);
        eigenvectors[i][1] = tmp > 0 ? 1.0 : -1.0;
        eigenvectors[i][0] = tmp * eigenvectors[i][1];
      }
      
      return eigenvectors;
    }

    public static SquareMatrix GetTransposed(this SquareMatrix matrix)
    {
      var transposedMatrix = new SquareMatrix(matrix.Size);
      for (var i = 0; i < matrix.Size; i++)
      {
        for (var j = 0; j < matrix.Size; j++)
        {
          transposedMatrix[i, j] = matrix[j, i];
        }
      }

      return transposedMatrix;
    }

    public static void Print(this SquareMatrix matrix)
    {
      for (var i = 0; i < matrix.Size; i++)
      {
        for (var j = 0; j < matrix.Size; j++)
        {
          Console.Write($"{matrix[i, j]}\t");
        }
        Console.WriteLine();
      }
    }
  }
  
  /// <summary>
  /// Единичная матрица.
  /// </summary>
  public class IdentityMatrix : SquareMatrix
  {
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="n"></param>
    public IdentityMatrix(int n) : base(n)
    {
      for (var i = 0; i < n; i++)
        BasedArray[i, i] = 1.0;
    }
  }
}