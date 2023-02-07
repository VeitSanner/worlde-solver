using Sanner.Wordle.Solver.WordList;

namespace Sanner.Wordle.Solver.WordList;

public interface IWordListProvider
{
    Task<WordDictionary> LoadAsync();
}