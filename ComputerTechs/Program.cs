namespace ComputerTechs
{
  /// <summary>
  /// Класс программы.
  /// </summary>
  internal static class Program
  {
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    public static void Main()
    {
      var entries = new[,]
      {
        {1.0, 0.0}, 
        {0.0, 2.0}
      };

      var expM = SquareMatrixHelper.ExpM(entries);
      expM(1.0).Print();
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