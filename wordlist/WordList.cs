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

        foreach (char letter in GetListOfDistinctLetters(word))
        {
            if (!Scores.ContainsKey(letter))
            {
                continue;
            }
            score += Scores[letter];
        }

        return score;
    }

    private void UpdateScores()
    {
        foreach (var word in Words)
        {
            foreach (char letter in GetListOfDistinctLetters(word))
            {
                if (!Scores.ContainsKey(letter))
                {
                    Scores[letter] = 1;
                }
                else
                {
                    Scores[letter] = Scores[letter] + 1;
                }
            }
        }
    }

    private IEnumerable<char> GetListOfDistinctLetters(string word)
    {
        return word.ToLowerInvariant().ToCharArray().Distinct();
    }
}