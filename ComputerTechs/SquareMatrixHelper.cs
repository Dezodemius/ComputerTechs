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
    /// Получить единичную матрицу.
    /// </summary>
    /// <param name="size">Размер матрицы.</param>
    public static SquareMatrix GetIdentityMatrix(int size)
    {
      var identityMatrix = new SquareMatrix(size);
      identityMatrix.Fill((i, j) => i == j ? 1.0 : 0.0);
      return identityMatrix;
    }
  }

  /// <summary>
  /// Вспомогательный класс для работы со ColumnVector.
  /// </summary>
  public static class ColumnVectorHelper
  {
    /// <summary>
    /// Проверить равен ли вектор нулевому вектору.
    /// </summary>
    /// <param name="vector">Исходный вектор.</param>
    /// <returns><c>True</c>, если все элементы вектора равны 0.0 с некоторой точностью.</returns>
    public static bool IsZero(this ColumnVector vector)
    {
      return vector.All(i => i.IsZero());
    }
  }
}