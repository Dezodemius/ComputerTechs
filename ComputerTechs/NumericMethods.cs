using System;

namespace ComputerTechs
{
  /// <summary>
  /// Методы интегрирования.
  /// </summary>
  public enum IntegrationMethod
  {
    Trapezium,
    Rectangles
  }

  /// <summary>
  /// Отрезок, на котором производятся численные вычисления.
  /// </summary>
  public readonly struct Boundaries
  {
    /// <summary>
    /// Левая граница.
    /// </summary>
    public double Left { get; }
    
    /// <summary>
    /// Правая граница.
    /// </summary>
    public double Right { get; }

    /// <summary>
    /// Длина отрезка.
    /// </summary>
    public double Len => Right - Left;
    
    /// <summary>
    /// Количество разбиений отрезка.
    /// </summary>
    public double N { get; }

    /// <summary>
    /// Шаг на этом отрезке.
    /// </summary>
    public double Step => Len / N;
    
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="left">Левая граница.</param>
    /// <param name="right">Правая граница.</param>
    /// <param name="n">Количество разбиений отрезка.</param>
    public Boundaries(double left, double right, double n)
    {
      Left = left;
      Right = right;
      N = n;
    }
  }
  
  /// <summary>
  /// Класс для работы с численными методами.
  /// </summary>
  public static class NumericMethods
  {
    /// <summary>
    /// Численное итегрирование.
    /// </summary>
    /// <param name="func">Функция, для которой нужно вычислить интеграл.</param>
    /// <param name="boundaries">Отрезок, на котором выполняется интегрирование.</param>
    /// <param name="n">Количество разбиений отрезка.</param>
    /// <param name="integrationMethod">Метод интегрирования.</param>
    /// <returns>Результат интегрирования.</returns>
    public static double Integrate(Func<double, double> func, Boundaries boundaries, IntegrationMethod integrationMethod)
    {
      double result;
      switch (integrationMethod)
      {
        case IntegrationMethod.Rectangles:
          result = RectangleIntegration(func, boundaries);
          break;
        case IntegrationMethod.Trapezium:
          result = TrapeziumIntegration(func, boundaries);
          break;
        default:
          result = double.MinValue;
          break;
      }

      return result;
    }

    /// <summary>
    /// Метод прямоугольников.
    /// </summary>
    /// <param name="func">Функция интегрирования.</param>
    /// <param name="boundaries">Отрезок интегрирования.</param>
    /// <returns>Результат интегрирования.</returns>
    private static double RectangleIntegration(Func<double, double> func, Boundaries boundaries)
    {
      var result = 0.0;

      var t = boundaries.Left;
      for (var i = 0; i < boundaries.N; i++)
      {
        result += func(t) * boundaries.Step;
        t += boundaries.Step;
      }

      return result;
    }

    /// <summary>
    /// Метод трапеций.
    /// </summary>
    /// <param name="func">Функция интегрирования.</param>
    /// <param name="boundaries">Отрезок интегрирования.</param>
    /// <returns>Результат интегрирования.</returns>
    /// <exception cref="NotImplementedException"></exception>
    private static double TrapeziumIntegration(Func<double, double> func, Boundaries boundaries)
    {
     throw new NotImplementedException();
    }
  }
}