namespace Sanner.Wordle.Solver.Wordle;

public class Recommendation
{
    public string Word {get;set;}
    public int Score { get; set; }

    public int Frequency { get; set; }
}