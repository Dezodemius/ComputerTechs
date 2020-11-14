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
    /// Метод Гаусса (Решение СЛАУ)
    /// </summary>
    /// <param name="matrix">Начальная матрица</param>
    /// <returns></returns>
    public static double[] Gauss(double[,] matrix)
    {
        var n = matrix.GetLength(0); //Размерность начальной матрицы (строки)
        var matrixClone = new double[n, n + 1]; //Матрица-дублер
        for (var i = 0; i < n; i++)
            for (var j = 0; j < n + 1; j++)
                matrixClone[i, j] = matrix[i, j];

        //Прямой ход (Зануление нижнего левого угла)
        for (var k = 0; k < n; k++) //k-номер строки
        {
            for (var i = 0; i < n + 1; i++) //i-номер столбца
                matrixClone[k, i] = matrixClone[k, i] / matrix[k, k]; //Деление k-строки на первый член !=0 для преобразования его в единицу
            for (var i = k + 1; i < n; i++) //i-номер следующей строки после k
            {
                var K = matrixClone[i, k] / matrixClone[k, k]; //Коэффициент
                for (var j = 0; j < n + 1; j++) //j-номер столбца следующей строки после k
                    matrixClone[i, j] = matrixClone[i, j] - matrixClone[k, j] * K; //Зануление элементов матрицы ниже первого члена, преобразованного в единицу
            }
            for (var i = 0; i < n; i++) //Обновление, внесение изменений в начальную матрицу
                for (var j = 0; j < n + 1; j++)
                    matrix[i, j] = matrixClone[i, j];
        }

        //Обратный ход (Зануление верхнего правого угла)
        for (var k = n - 1; k > -1; k--) //k-номер строки
        {
            for (var i = n; i > -1; i--) //i-номер столбца
                matrixClone[k, i] = matrixClone[k, i] / matrix[k, k];
            for (var i = k - 1; i > -1; i--) //i-номер следующей строки после k
            {
                var K = matrixClone[i, k] / matrixClone[k, k];
                for (var j = n; j > -1; j--) //j-номер столбца следующей строки после k
                    matrixClone[i, j] = matrixClone[i, j] - matrixClone[k, j] * K;
            }
        }

        //Отделяем от общей матрицы ответы
        var answer = new double[n];
        for (var i = 0; i < n; i++)
            answer[i] = matrixClone[i, n];

        return answer;
    }

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