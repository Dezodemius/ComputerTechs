using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using CenterSpace.NMath.Charting.Microsoft;
using CenterSpace.NMath.Core;
using CommandLine;
using CommandLine.Text;
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

    private static CommandLineOptions commandLineOptions;
    
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    public static void Main(string[] args)
    {  
      if (args.Length == 0)
      {
        Console.WriteLine("Run application with --help");
        Environment.Exit(-1);
      }

      var parser = new Parser(with => with.HelpWriter = null);
      var parserResult = parser.ParseArguments<CommandLineOptions>(args);
      parserResult
        .WithParsed(options => commandLineOptions = options)
        .WithNotParsed(errs => DisplayHelp(parserResult));

      if (args.Any(a => a.Contains("help") || a.Contains("--version")))
        Environment.Exit(-1);

      Console.WriteLine("#### Program started ####\n");
      var matrix =  SquareMatrixHelper.SquareMatrix(commandLineOptions.SquareMatrixEntries.ToArray());

      var x0 = new ColumnVector(commandLineOptions.InitVector.ToArray());
      var t0 = commandLineOptions.InitTime.ToList()[0];
      var t1 = commandLineOptions.InitTime.ToList()[1];
      var n = commandLineOptions.N;

      var frames = commandLineOptions.Frames;
      var dt = (t1 - t0) / frames;
      
      PlotByNMath(frames, matrix, x0, n, t0, dt);
      // PlotByPython(frames, matrix, x0, n, t0, dt);
      Console.WriteLine("#### Program finished ####");
    }

    static void DisplayHelp<T>(ParserResult<T> result)
    {  
      var helpText = HelpText.AutoBuild(result, h =>
      {
        h.AdditionalNewLineAfterOption = false;
        h.Heading = "ComputerTechs 1.0.0";
        h.Copyright = "Gladkov Yegor (c) 2020";
        h.AutoHelp = true;
        h.AutoVersion = true;
        h.AddPostOptionsLine("В списках разделителем выступаем ','.");
        return HelpText.DefaultParsingErrorsHandler(result, h);
      }, e => e);
      Console.WriteLine(helpText);
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
      var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, commandLineOptions.PicsPath);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      const ChartImageFormat format = ChartImageFormat.Png;

      for (var i = 0; i < m; i++)
      {
        Console.Write($"Plotting {i}-th frame . . . ");
        CalculateSupportingFunctionOfReachableSet(matrix, x0, n, t0, t0 + dt * i,
          x => new ColumnVector(Cos(x), Sin(x)), out var psi, out var supportingFunctionOfReachableSet);
        CalculatePointsForPlot(n, psi, supportingFunctionOfReachableSet, out var data);

        var chart = NMathChart.ToChart(new FloatMatrix(data), 0, 1);
        chart.Size = new Size(3840, 2160);
        NMathChart.Save(chart, $@"{path}\{i}.{format}", format);
        Console.WriteLine($"{i}-th frame saved\n");
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