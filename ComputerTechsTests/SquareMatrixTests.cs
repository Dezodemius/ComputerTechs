using ComputerTechs;
using Meta.Numerics;
using Meta.Numerics.Matrices;
using NUnit.Framework;

namespace ComputerTechsTests
{
  [TestFixture]
  public class SquareMatrixTests
  {
    [TestCase(new []{0.0, 0.0, 0.0, 0.0}, ExpectedResult = new[] {0.0, 0.0})]
    [TestCase(new []{1e9, 2e9, 0.0, 3e9}, ExpectedResult = new[] {1e9, 3e9})]
    [TestCase(new []{1.0, 0.0, 0.0, 1.0}, ExpectedResult = new[] {1.0, 1.0})]
    [TestCase(new []{1.0, -3.0, -2.0, 2.0}, ExpectedResult = new[] {-1.0, 4.0})]
    [TestCase(new []{1.0, -3.0, 0.0, 2.0}, ExpectedResult = new[] {1.0, 2.0})]
    [TestCase(new []{1.0, 0.0, 0.0, 2.0}, ExpectedResult = new[] {1.0, 2.0})]
    public static double[] CorrectEigenvaluesTest(double[] a)
    {
      return GetEigenvaluesArray(SquareMatrixHelper.SquareMatrix(a).Eigenvalues());
    }

    [TestCase(new []{0.0, 0.0, 0.0, 0.0})]
    [TestCase(new []{1e9, 2e9, 0.0, 3e9})]
    [TestCase(new []{1.0, 0.0, 0.0, 1.0})]
    [TestCase(new []{1.0, -3.0, -2.0, 2.0})]
    [TestCase(new []{1.0, -3.0, 0.0, 2.0})]
    [TestCase(new []{1.0, 0.0, 0.0, 2.0})]
    public static void CorrectEigenvectorsTest(double[] a)
    {
      var matrix = SquareMatrixHelper.SquareMatrix(a);
      var eigenvalues = matrix.Eigenvalues();
      var eigenvectors = matrix.GetEigenvectors();

      Assert.AreEqual(matrix * eigenvectors.Column(0), eigenvalues[0].Re * eigenvectors.Column(0));
      if ((eigenvalues[0].Re - eigenvalues[1].Re).IsZero() && matrix.GetGeometricMultiplicity(eigenvalues[0].Re) == 1)
        Assert.AreNotEqual(matrix * eigenvectors.Column(1), eigenvalues[1].Re * eigenvectors.Column(1));
      else
        Assert.AreEqual(matrix * eigenvectors.Column(1), eigenvalues[1].Re * eigenvectors.Column(1));
    }
    
    private static double[] GetEigenvaluesArray(Complex[] eigenvalues)
    {
      return new [] {eigenvalues[0].Re, eigenvalues[1].Re};
    }
  }
}