using Sanner.Wordle.Solver.Wordle;
using System.Collections.ObjectModel;

namespace Sanner.Wordle.Solver.WordList;

public class WordDictionary
{
    public ReadOnlyCollection<string> Words { get; }

    private Dictionary<char, int> Scores { get; } = new Dictionary<char, int>();

    public WordDictionary() : this(new List<string>())
    { }
    public WordDictionary(IList<string> words)
    {
        Words = new ReadOnlyCollection<string>(words);
    }

    public int ScoreWord(string word)
    {
        if (!Scores.Any())
        {
            UpdateScores();
        }

        var score = 0;

        foreach (char c in GetListOfDistinctChars(word))
        {
            if (!Scores.ContainsKey(c))
            {
                continue;
            }
            score += Scores[c];
        }

        return score;
    }

    private void UpdateScores()
    {
        foreach (var word in Words)
        {
            foreach (char c in GetListOfDistinctChars(word))
            {
                if (!Scores.ContainsKey(c))
                {
                    Scores[c] = 1;
                }
                else
                {
                    Scores[c] = Scores[c] + 1;
                }
            }
        }
    }

    private IEnumerable<char> GetListOfDistinctChars(string word)
    {
        return word.ToLowerInvariant().ToCharArray().Distinct();
    }
}