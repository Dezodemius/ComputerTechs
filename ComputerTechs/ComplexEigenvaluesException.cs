using System;

namespace ComputerTechs
{
  /// <summary>
  /// Исключение, возникающее, если были получены комплексные собственные значения.
  /// </summary>
  public class ComplexEigenvaluesException : Exception
  {
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="message">Сообщение исключения.</param>
    public ComplexEigenvaluesException(string message) : base(message) { }
  }
}