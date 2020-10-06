using System;
using System.Collections.Generic;

namespace ComputerTechs
{
  /// <summary>
  /// Квадратная матрица.
  /// </summary>
  public class SquareMatrix
  {
    #region Поля и свойства

    /// <summary>
    /// Размер матрицы.
    /// </summary>
    public int Size { get; protected set; }
    
    public double[,] BasedArray { get; }

    #endregion

    #region Операторы

    /// <summary>
    /// Оператор сложения между матрицами.
    /// </summary>
    /// <param name="a">Матрица слева.</param>
    /// <param name="b">Матрица справа.</param>
    /// <returns>Результирующая матрица.</returns>
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

    /// <summary>
    /// Оператор умножения матриц.
    /// </summary>
    /// <param name="a">Матрица слева.</param>
    /// <param name="b">Матрица справа.</param>
    /// <returns>Результирующная матрица.</returns>
    public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b)
    {
      var c = new SquareMatrix(a.Size);
      var n = a.Size;
      for (var i = 0; i < n; i++) 
      {
        for (var j = 0; j < n; j++) 
        {
          c[i, j] = 0;
          for (var k = 0; k < n; k++) 
          {
            c[i, j] += a[i, k] * b[k, j];
          }
        }
      }

      return c;
    }

    /// <summary>
    /// Оператор умножения матрицы на вектор.
    /// </summary>
    /// <param name="a">Матрица.</param>
    /// <param name="b">Вектор.</param>
    /// <returns>Результирующий вектор.</returns>
    public static double[] operator *(SquareMatrix a, double[] b)
    {
      var n = a.Size;
      var c = new double[n];
      for (var i = 0; i < n; i++) 
      {
        for (var j = 0; j < n; j++)
        {
          c[i] += a[i, j] * b[j];
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
    /// <summary>
    /// Получить собственные значения матрицы.
    /// </summary>
    /// <param name="matrix">Матрица.</param>
    /// <returns>Массив, содержащий собственные значения.</returns>
    /// <exception cref="ArgumentException">Возникает, если размер матрицы не равен двум.</exception>
    public static double[] GetEigenvalues(this SquareMatrix matrix)
    {
      if (matrix.Size == 1)
        return new double[] { matrix[0, 0] };
      
      if (matrix.Size > 2)
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

    /// <summary>
    /// Получить собственный вектор.
    /// </summary>
    /// <param name="matrix">Матрица.</param>
    /// <returns>Список собственных векторов.</returns>
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

    /// <summary>
    /// Получить транспонированную матрицу.
    /// </summary>
    /// <param name="matrix">Исходная матрица.</param>
    /// <returns>Транспонированая матрица.</returns>
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

    /// <summary>
    /// Получить обратную матрицу.
    /// </summary>
    /// <param name="matrix">Исходная матрица.</param>
    /// <returns>Обратная матрица.</returns>
    public static SquareMatrix GetInverse(this SquareMatrix matrix)
    {
      var matrixDet = matrix.GetDeterminant();
      var inverse = new SquareMatrix(matrix.Size)
      {
        [0, 0] = matrix[1, 1] / matrixDet, [0, 1] = - matrix[0, 1] / matrixDet, 
        [1, 0] = - matrix[1, 0] / matrixDet, [1, 1] = matrix[0, 0] / matrixDet
      };

      return inverse;
    }

    /// <summary>
    /// Вывести матрицу.
    /// </summary>
    /// <param name="matrix">Матрица.</param>
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

    /// <summary>
    /// Получить определитель матрицы.
    /// </summary>
    /// <param name="matrix">Матрица.</param>
    /// <returns>Значение определителя.</returns>
    /// <exception cref="ArgumentException">Возникает, если размер матрицы не равен двум.</exception>
    public static double GetDeterminant(this SquareMatrix matrix)
    {
      if (matrix.Size != 2)
        throw new ArgumentException("Матрица размера больше 2-х не поддерживаются");
      return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
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