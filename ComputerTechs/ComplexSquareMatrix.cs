using Meta.Numerics.Matrices;

namespace ComputerTechs
{
  public class ComplexSquareMatrix
  {
    public SquareMatrix Re { get; }
    public SquareMatrix Im { get; }
    
    public ComplexSquareMatrix(SquareMatrix re, SquareMatrix im)
    {
      Re = re;
      Im = im;
    }
  }
}