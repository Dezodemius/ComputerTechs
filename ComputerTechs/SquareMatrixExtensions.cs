using System;
using System.Linq;
using Meta.Numerics.Matrices;

namespace ComputerTechs
{
  /// <summary>
  /// Класс для работы с методами расширений для типа SquareMatrix.
  /// </summary>
  public static class SquareMatrixExtensions
  {
    /// <summary>
    /// Получить ранг матрицы.
    /// </summary>
    /// <param name="matrix">Матрица.</param>
    /// <returns>Ранг.</returns>
    public static double GetRank(this SquareMatrix matrix)
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
        throw new ArgumentException();
      
      const double max = 1.0;
      const double min = 0.0;

      // Собственные значения равны друг другу (одно собственное значение).
      if (!eigenvalues.All(v => (v - eigenvalues[0]).Re.IsZero()))
      {
        SquareMatrix eigenvectors;
        var identityMatrix = SquareMatrixHelper.GetIdentityMatrix(originMatrix.Dimension);
      
        while (true)
        {
          var possibleAttachedVectorEntries = GetRandomColumnVector(max, min);
          var possibleAttachedVector = new ColumnVector(possibleAttachedVectorEntries);
          if ((originMatrix - eigenvalues[1].Re * identityMatrix).GetRank().IsZero())
          {
            var possibleEigenvector = (originMatrix - eigenvalues[1].Re * identityMatrix) * possibleAttachedVector;
            if (possibleEigenvector.OneNorm().IsZero()) 
              continue;
            if (((originMatrix - eigenvalues[0].Re * identityMatrix) * possibleEigenvector).OneNorm().IsZero())
            {
              eigenvectors = new SquareMatrix(new [,]
              {
                {possibleEigenvector[0], possibleAttachedVector[0]} , 
                {possibleEigenvector[1], possibleAttachedVector[1]}
              });
              break;
            }
          }
          eigenvectors = new SquareMatrix(new [,]
          {
            {1.0, 0.0} , 
            {0.0, 1.0}
          });
          break;
        }

        return eigenvectors;
      }

      return new SquareMatrix(new[,]{{1.0, 0.0},{0.0, 1.0}});
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