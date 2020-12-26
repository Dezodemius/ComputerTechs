using System.Collections.Generic;
using CommandLine;

namespace ComputerTechs
{
  public class CommandLineOptions
  {
    [Option('m', "matrix", Separator = ',', Required = true, HelpText = "Входная матрица")]
    public IEnumerable<double> SquareMatrixEntries { get; set; }
    
    [Option('i', "init", Separator = ',', Required = true, HelpText = "Начальное и конечное время")]
    public  IEnumerable<double> InitTime { get; set; }
    
    [Option('x', "vector", Separator = ',', Required = true, HelpText = "Вектор начального положения")]
    public IEnumerable<double> InitVector { get; set; }
    
    [Option('n', "number", Default = 1000, Required = false, HelpText = "Количечество строчек для построения")]
    public int N { get; set; }
    
    [Option('f', "frames", Default = 24, Required = false, HelpText = "Количество кадров для отображения")]
    public int Frames { get; set; }
    
    [Option('p', "path", Default = "..\\..\\pics", Required = false, HelpText = "Путь для сохранения итоговых картинок")]
    public string PicsPath { get; set; }
  }
}