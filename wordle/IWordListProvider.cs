using Sanner.Wordle.Solver.WordList;

namespace Sanner.Wordle.Solver.Wordle;

public interface IWordListProvider
{
    Task<WordDictionary> LoadAsync();
}