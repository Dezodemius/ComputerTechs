using System;
using ComputerTechs;
using Meta.Numerics.Matrices;
using NUnit.Framework;

namespace ComputerTechsTests
{
  [TestFixture]
  public class MatrixExponentialTests
  {
    [TestCase(new[]{0.0, 0.0, 0.0, 0.0}, new[]{1.0, 0.0, 0.0, 1.0})]
    [TestCase(new[]{1.0, 0.0, 0.0, 1.0},new[]{Math.E, 0.0, 0.0, Math.E})]
    [TestCase(new[]{1.0, -3.0, 0.0, 2.0},new[]{2.7182818284590451, -14.012322811414814, 0.0, 7.3890560989306504})]
    [TestCase(new[]{1.0, -3.0, -3.0, 2.0},new[]{39.322809708033986, -46.1663014388859, -46.1663014388859,  54.711576854329266})]
    [TestCase(new[]{1.0, 0.0, 0.0, 2.0},new[]{2.7182818284590451, 0.0, 0.0, 7.3890560989306504})]
    public static void CorrectMatrixExponentialCalculations(double[] actualEntries, double[] expectedExpMEntries)
    {
      var expectedExpM = SquareMatrixHelper.ExpM(SquareMatrixHelper.SquareMatrix(actualEntries))(1.0);
      Assert.AreEqual(expectedExpMEntries, GetOriginEntries(expectedExpM));
    }

    private static double[] GetOriginEntries(SquareMatrix matrix)
    {
      var n = matrix.Dimension;
      var entries = new double[n * n];
      for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
          entries[n * i + j] = matrix[i, j];

      return entries;
    }
  }
}