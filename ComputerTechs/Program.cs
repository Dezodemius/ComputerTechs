using System;
using Meta.Numerics.Matrices;

namespace ComputerTechs
{
  internal static class Program
  {
    public static void Main(string[] args)
    {
      // var entries = new[,]
      // {
      //   {25.0, 17.0}, 
      //   {-5.0, 1.0}
      // };
      //
      // var A = new SquareMatrix(entries);
      //
      // var S = A.GetEigenvectors();
      // // S.Print();
      // var J = A.GetJordanForm();
      // // J.Print();
      // ().Print();
      // var expJ = J.GetMatrixExponentialFunc()(1.0);
      // expJ.Print();
      //
      // var expM = S * expJ * S.Inverse();
      // expM.Print();
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
  }
}