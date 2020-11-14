using System;

namespace ComputerTechs
{
  public static class AccuracyHelper
  {
        
    /// <summary>
    /// Метод, сранивающий вещественное число с какой-то точностью с нулем.
    /// </summary>
    /// <param name="number">Вещественное число.</param>
    /// <returns><c>True</c>, если указанное число меньше eps=1e-9.</returns>
    public static bool IsZero(this double number)
    {
      return number.IsZero(1e-9);
    }
    
    /// <summary>
    /// Метод, сранивающий вещественное число с какой-то точностью с нулем.
    /// </summary>
    /// <param name="number">Вещественное число.</param>
    /// <param name="eps">Точность.</param>
    /// <returns><c>True</c>, если указанное число меньше <c>eps</c>.</returns>
    public static bool IsZero(this double number, double eps)
    {
      return Math.Abs(number - 0.0) <= eps;
    }
  }
}