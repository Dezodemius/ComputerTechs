using System;
using System.Linq;
using Meta.Numerics;
using Meta.Numerics.Matrices;

namespace ComputerTechs
{
  /// <summary>
  /// Класс для работы с методами расширений для типа SquareMatrix.
  /// </summary>
  public static class SquareMatrixExtensions
  {
    public static double Det(this SquareMatrix matrix)
    { 
      var a = matrix[0, 0];
      var b = matrix[0, 1];
      var c = matrix[1, 0];
      var d = matrix[1, 1];
      return a * d - b * c;
    }
    
    /// <summary>
    /// Вывести все значения матрицы в окно консоли.
    /// </summary>
    /// <param name="matrix">Матрица.</param>
    public static void Print(this SquareMatrix matrix)
    {
      for (var i = 0; i < matrix.Dimension; i++)
      {
        for (var j = 0; j < matrix.Dimension; j++)
          Console.Write($"{matrix[i, j]}\t");
        Console.WriteLine();
      }
      Console.WriteLine();
    }

    /// <summary>
    /// Получить Жорданову форму матрицы.
    /// </summary>
    /// <param name="matrix">Матрица.</param>
    /// <returns>Жорданова форма.</returns>
    public static SquareMatrix GetJordanForm(this SquareMatrix matrix)
    {
      var transitionMatrix = matrix.GetEigenvectors();
      return transitionMatrix.GetInverse() * matrix * transitionMatrix;
    }
    
    /// <summary>
    /// Получить ранг матрицы.
    /// </summary>
    /// <param name="matrix">Матрица.</param>
    /// <returns>Ранг.</returns>
    public static int GetRank(this SquareMatrix matrix)
    {
      var rank = 2;
      var a = matrix[0, 0];
      var b = matrix[0, 1];
      var c = matrix[1, 0];
      var d = matrix[1, 1];

      if ((b * c - a * d).IsZero())
        rank--;
      if (a.IsZero() && b.IsZero())
        rank--;  
        
      return rank;
    }

    /// <summary>
    /// Получить собственные (и присоединённые) векторы матрицы.
    /// </summary>
    /// <param name="originMatrix">Входная матрица.</param>
    /// <returns>Матрица, состоящая из комплексных чисел.</returns>
    public static SquareMatrix GetEigenvectors(this SquareMatrix originMatrix)
    {
      var eigenvalues = originMatrix.Eigenvalues();
      
      if (eigenvalues.Any(v => v.Im != 0))
        throw new ComplexEigenvaluesException("Комплексные собственные значения не поддерживаются.");
      
      const double max = 1.0;
      const double min = 0.0;
      
      var eigenvectors = new SquareMatrix(originMatrix.Dimension);
      var identityMatrix = SquareMatrixHelper.GetIdentityMatrix(originMatrix.Dimension);
      
      // Два различных собственных значения.
      if (!(eigenvalues[0] - eigenvalues[1]).Re.IsZero())
      {
        var eigenSol1 = originMatrix - eigenvalues[0].Re * identityMatrix;
        var eigenSol2 = originMatrix - eigenvalues[1].Re * identityMatrix;
        
        if (!eigenSol1.Row(0).IsZero())
          eigenvectors = new SquareMatrix(new [,]
          {
            { eigenSol1[0, 1], eigenSol2[0, 1] },
            { - eigenSol1[0, 0], - eigenSol2[0, 0] }
          });
        else
        {
          eigenvectors = new SquareMatrix(new [,]
          {
            { eigenSol1[1, 1], eigenSol2[0, 1] },
            { - eigenSol1[1, 0], - eigenSol2[0, 0] }
          });
        }
      }
      else if ((eigenvalues[0] - eigenvalues[1]).Re.IsZero())
      {
        while (true)
        {
          if ((originMatrix - eigenvalues[1].Re * identityMatrix).GetRank() != 0)
          {
            var possibleAttachedVectorEntries = GetRandomColumnVector(max, min);
            var possibleAttachedVector = new ColumnVector(possibleAttachedVectorEntries);
            var possibleEigenvector = (originMatrix - eigenvalues[1].Re * identityMatrix) * possibleAttachedVector;
            
            if (originMatrix * possibleAttachedVector == eigenvalues[1].Re * possibleAttachedVector)
              continue;
            
            if (possibleEigenvector.OneNorm().IsZero()) 
              continue;
            if (((originMatrix - eigenvalues[0].Re * identityMatrix) * possibleEigenvector).OneNorm().IsZero())
            {
              eigenvectors = new SquareMatrix(new [,]
              {
                { possibleEigenvector[0], possibleAttachedVector[0] }, 
                { possibleEigenvector[1], possibleAttachedVector[1] }
              });
              break;
            }
          }
          eigenvectors = new SquareMatrix(new [,]
          {
            { 1.0, 0.0 }, 
            { 0.0, 1.0 }
          });
          break;
        }
      }

      return eigenvectors;
    }

    /// <summary>
    /// Получить обратную матрицу. 
    /// </summary>
    /// <param name="matrix">Исходная матрица.</param>
    /// <returns>Обратная матрица.</returns>
    public static SquareMatrix GetInverse(this SquareMatrix matrix)
    {
      var a = matrix[0, 0];
      var b = matrix[0, 1];
      var c = matrix[1, 0];
      var d = matrix[1, 1];

      var det = matrix.Det();
      
      return 1 / det * new SquareMatrix(new[,] {{d, -b}, {-c, a}});
    }
    
    /// <summary>
    /// Получить геометрическую кратность собственного значения матрицы.
    /// </summary>
    /// <param name="originMatrix">Исходная матрица.</param>
    /// <param name="eigenvalue">Собственное значение.</param>
    /// <returns></returns>
    public static int GetGeometricMultiplicity(this SquareMatrix originMatrix, double eigenvalue)
    {
      var identityMatrix = SquareMatrixHelper.GetIdentityMatrix(originMatrix.Dimension);
      return originMatrix.Dimension - (originMatrix - eigenvalue * identityMatrix).GetRank();
    }

    /// <summary>
    /// Получить случайный вектор-столбец размера 2*2.
    /// </summary>
    /// <param name="max">Максимально возможное значение вектора.</param>
    /// <param name="min">Минимально возможное значение вектора.</param>
    /// <returns>Случайный вектор размером 2*2.</returns>
    private static double[] GetRandomColumnVector(double max, double min)
    {
      var random = new Random();
      var x = random.NextDouble() * (max - min) + min;
      var y = random.NextDouble() * (max - min) + min;
      return new []{ x, y };
    }
  }
}