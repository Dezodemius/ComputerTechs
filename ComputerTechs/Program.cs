using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using CenterSpace.NMath.Charting.Microsoft;
using CenterSpace.NMath.Core;
using Meta.Numerics.Matrices;
using static System.Math;

namespace ComputerTechs
{
  /// <summary>
  /// Класс программы.
  /// </summary>
  internal static class Program
  {
    /// <summary>
    /// Путь для файла с данными.
    /// </summary>
    private const string DataFilePath = "..\\..\\data.txt";
    
    /// <summary>
    /// Путь к построителю графиков.
    /// </summary>
    private const string PlotScriptFilePath = "..\\..\\..\\Plotter\\Plotter.py";

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    public static void Main(string[] args)
    {
      var entries = new[,]
      {
        {1.0, 6.0}, 
        {0.0, 2.0}
      };

      var matrix = new SquareMatrix(entries);
      var x0 = new ColumnVector(1.0, 0.0);
      const double t0 = 0.0;
      const double t1 = 1.0;
      const int n = 1000;

      const int m = 24;
      const double dt = (t1 - t0) / m;
      
      PlotByNMath(m, matrix, x0, n, t0, dt);
      PlotByPython(m, matrix, x0, n, t0, dt);
    }

    /// <summary>
    /// Построить графики с помощью Python.
    /// </summary>
    /// <param name="m">Количество рисунков.</param>
    /// <param name="matrix">Исходная матрица.</param>
    /// <param name="x0">Начальное значение.</param>
    /// <param name="n">Количество точек.</param>
    /// <param name="t0">Начальное время.</param>
    /// <param name="dt">Шаг по времени.</param>
    /// <summary />
    private static void PlotByPython(int m, SquareMatrix matrix, ColumnVector x0, int n, double t0, double dt)
    {
      for (var i = 0; i < m; i++)
      {
        CalculateSupportingFunctionOfReachableSet(matrix, x0, n, t0, t0 + dt * i,
          x => new ColumnVector(Cos(x), Sin(x)), out var psi, out var supportingFunctionOfReachableSet);

        CalculatePointsForPlot(n, psi, supportingFunctionOfReachableSet, out var data);
        SaveCalculationsToFile(data);
        StartPlotProcess(new[] {i.ToString()});
      }
    }

    /// <summary>
    /// Построить графики с помощью NMathChart.
    /// </summary>
    /// <param name="m">Количество рисунков.</param>
    /// <param name="matrix">Исходная матрица.</param>
    /// <param name="x0">Начальное значение.</param>
    /// <param name="n">Количество точек.</param>
    /// <param name="t0">Начальное время.</param>
    /// <param name="dt">Шаг по времени.</param>
    private static void PlotByNMath(int m, SquareMatrix matrix, ColumnVector x0, int n, double t0, double dt)
    {
      var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\pics");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      const ChartImageFormat format = ChartImageFormat.Png;

      for (var i = 0; i < m; i++)
      {
        CalculateSupportingFunctionOfReachableSet(matrix, x0, n, t0, t0 + dt * i,
          x => new ColumnVector(Cos(x), Sin(x)), out var psi, out var supportingFunctionOfReachableSet);
        CalculatePointsForPlot(n, psi, supportingFunctionOfReachableSet, out var data);

        var chart = NMathChart.ToChart(new FloatMatrix(data), 0, 1);
        NMathChart.Save(chart, $@"{path}\{i}.{format}", format);
      }
    }

    /// <summary>
    /// Выполнить процесс построения графика.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    private static void StartPlotProcess(string[] args)
    {
      var plotProcess = new ProcessStartInfo
      {
        FileName = "python",
        Arguments = $"{PlotScriptFilePath} {DataFilePath} {string.Join(" ", args)}",
        UseShellExecute = false
      };
      using (var process = Process.Start(plotProcess))
      {
        while (process.HasExited) { }
      }
    }

    /// <summary>
    /// Вычислить значения опорной функции для множества достижимости.
    /// </summary>
    /// <param name="matrix">Исходная матрица</param>
    /// <param name="x0">Начальное положение.</param>
    /// <param name="n">Количество точек для расчёта.</param>
    /// <param name="t0">Начальный момент времени.</param>
    /// <param name="t">Конечный момент времени.</param>
    /// <param name="psiFunc">Фунция для вычисления значений векторов направлений.</param>
    /// <param name="psi">Массив векторов.</param>
    /// <param name="supportingFunctionValues">Массив значений опорной функции множества достижимости.</param>
    private static void CalculateSupportingFunctionOfReachableSet(SquareMatrix matrix, ColumnVector x0, int n, 
      double t0, double t, Func<double, ColumnVector> psiFunc, out ColumnVector[] psi, out double[] supportingFunctionValues)
    {
      supportingFunctionValues = new double[n];

      var dx = 2 * PI / n;
      psi = new ColumnVector[n];
      for (var i = 0; i < n; i++)
        psi[i] = psiFunc(i * dx);
      var expMFunc = SquareMatrixHelper.ExpM(matrix);

      for (var i = 0; i < n; i++)
      {
        supportingFunctionValues[i] = VectorHelper.Prod(expMFunc(t - t0) * x0, psi[i])
                                      + Integrate(expMFunc, psi[i], t0, t, n);
      }
    }

    /// <summary>
    /// Вычислить точки для построения опорной функции множества достижимости.
    /// </summary>
    /// <param name="n">Количество точек.</param>
    /// <param name="psi">Список векторов направлений.</param>
    /// <param name="supportingFunctionValues">Массив значений опорной функции.</param>
    /// <param name="data">Данные для построения. [X, Y]</param>
    private static void CalculatePointsForPlot(int n, IReadOnlyList<ColumnVector> psi, IReadOnlyList<double> 
    supportingFunctionValues, out float[,] data)
    {
      data = new float[n, 2];
      for (var i = 0; i < n; i++)
      {
        var phi1 = psi[i][0];
        var phi2 = psi[i][1];
        var c1 = supportingFunctionValues[i];

        // Значение (i + 1). Нужно для пар типа (n, 0).
        var nextI = (i + 1) % n;
        var psi1 = psi[nextI][0];
        var psi2 = psi[nextI][1];
        var c2 = supportingFunctionValues[nextI];

        var det = phi1 * psi2 - phi2 * psi1;

        data[i, 0] = (float) ((c1 * psi2 - phi2 * c2) / det);
        data[i, 1] = (float) ((phi1 * c2 - c1 * psi1) / det);
      }
    }

    /// <summary>
    /// Сохранить вычисления в файл.
    /// </summary>
    /// <param name="data"></param>
    private static void SaveCalculationsToFile(float[,] data)
    {
      using (var writer = new StreamWriter(DataFilePath))
      {
        for (var i = 0; i < data.GetLength(0); i++)
        {
          for (var j = 0; j < data.GetLength(1); j++)
            writer.Write($"{data[i, j]}\t");
          writer.WriteLine();
        }
      }
    }

    /// <summary>
    /// Интегрировать матричный экспоненциал с учётом вектора psi.
    /// </summary>
    /// <param name="expMFunc">Функция для вычисления матричного экспоненциала.</param>
    /// <param name="psi">Вектор направления.</param>
    /// <param name="t0">Начальное время.</param>
    /// <param name="t">Конечное время.</param>
    /// <param name="n">Количество точек.</param>
    /// <returns>Значение интеграла.</returns>
    private static double Integrate(Func<double, SquareMatrix> expMFunc, ColumnVector psi, double t0, double t, double n)
    {
      var result = 0.0;
      var s = t0;
      var step = (t - t0) / n;

      for (var i = 0; i < n; i++)
      {
        result += SpecificSupportingFunction(expMFunc(t - s).Transpose * psi) * step;
        s += step;
      }

      return result;
    }
    
    /// <summary>
    /// Опорная функция множества.
    /// </summary>
    /// <param name="psi"></param>
    /// <returns></returns>
    private static double SpecificSupportingFunction(IReadOnlyList<double> psi)
    {
      var psi1 = psi[0];
      var psi2 = psi[1];

      if (psi1 >= 0 && psi2 >= -(psi1 / 2.0))
        return psi1 + psi2;
      if (psi1 < 0 && psi2 >= psi1 / 2.0)
        return -psi1 + psi2;
      return -psi1 * psi1 / (4.0 * psi2);
    }
  }
}