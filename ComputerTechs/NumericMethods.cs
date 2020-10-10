using System;

namespace ComputerTechs
{
  public enum IntegrationMethod
  {
    Trapezium,
    Rectangles
  }

  public readonly struct Boundaries
  {
    public double Left { get; }
    public double Right { get; }

    public double Len => Right - Left;

    public Boundaries(double left, double right)
    {
      Left = left;
      Right = right;
    }
  }
  
  public static class NumericMethods
  {
    public static double Integrate(Func<double, double> func, Boundaries boundaries, int n, IntegrationMethod integrationMethod)
    {
      double result;
      switch (integrationMethod)
      {
        case IntegrationMethod.Rectangles:
          result = RectangleIntegration(func, boundaries, n);
          break;
        case IntegrationMethod.Trapezium:
          result = TrapeziumIntegration(func, boundaries, n);
          break;
        default:
          result = double.MinValue;
          break;
      }

      return result;
    }

    private static double RectangleIntegration(Func<double, double> func, Boundaries boundaries, int n)
    {
      var result = 0.0;
      
      var h = boundaries.Len / n;

      var t = boundaries.Left;
      for (var i = 0; i < n; i++)
      {
        result += func(t) * h;
        t += h;
      }

      return result;
    }

    private static double TrapeziumIntegration(Func<double, double> func, Boundaries boundaries, int n)
    {
      var result = 0.0;
      
      var h = boundaries.Len / n;

      var t = boundaries.Left;
      for (var i = 0; i < n; i++)
      {
        result += func(t) * h;
        t += h;
      }

      return result;
    }
  }
}