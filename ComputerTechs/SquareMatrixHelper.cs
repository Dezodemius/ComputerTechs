using Meta.Numerics.Matrices;

namespace ComputerTechs
{
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
}