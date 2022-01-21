namespace Sanner.Wordle.Solver.Wordle;

public record Recommendation
{
    public string Word {get;init;}
    public int Score { get; init; }

    public int Frequency { get; init; }
};