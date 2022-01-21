using System.Text;
using System.Text.RegularExpressions;
using Sanner.Wordle.Solver.WordList;

namespace Sanner.Wordle.Solver.Wordle;

public class WordleRecommendation
{
    private readonly IWordListProvider _wl;

    public WordleRecommendation(IWordListProvider wlProvider)
    {
        this._wl = wlProvider;
    }

    public async Task<List<Recommendation>> GetRecommendation(string requiredChars, string excludedCharacters, string positionData)
    {
        var wordList = await _wl.LoadAsync();

        var filteredWords = wordList.Words.AsQueryable();

        if (!string.IsNullOrWhiteSpace(requiredChars))
        {
            var reqCharsFilter = BuildRequiredCharsFilter(requiredChars);
            filteredWords = filteredWords.Where(w => reqCharsFilter.IsMatch(w));
        }

        if (!string.IsNullOrWhiteSpace(excludedCharacters))
        {
            var excludedCharsFilter = BuildExcludeCharsFilter(excludedCharacters);
            filteredWords = filteredWords.Where(w => !excludedCharsFilter.IsMatch(w));
        }

        if (!string.IsNullOrWhiteSpace(positionData))
        {
            var positionFilter = BuildPositionFilter(positionData);
            filteredWords = filteredWords.Where(w => positionFilter.IsMatch(w));
        }

        return filteredWords.Select(w => ConvertToRecommendation(w, wordList)).OrderByDescending(o => o.Score).ToList();

    }

    private Regex BuildRequiredCharsFilter(string requiredChars)
    {
        var exp = string.Format($"(?=.*{requiredChars})");
        return new Regex(exp, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    private Regex BuildExcludeCharsFilter(string excludedCharacters)
    {
        
        var exp = string.Join(",", excludedCharacters.Split());

        return new Regex($"[{exp}]", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    private Regex BuildPositionFilter(string positionData)
    {
        return new Regex(positionData, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    private Recommendation ConvertToRecommendation(string w, WordDictionary wl)
    {
        return new Recommendation
        {
            Word = w,
            Score = wl.ScoreWord(w)
        };
    }
}