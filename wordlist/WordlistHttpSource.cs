using System.Text.RegularExpressions;

namespace Sanner.Wordle.Solver.WordList;

public class WordlistHttpSource : IWordListProvider
{
    private HttpClient _client;

    private WordDictionary _wordList;
    private readonly Regex _wordListFilter;

    public WordlistHttpSource(IConfiguration cfg, IHttpClientFactory httpClientFactory)
    {
        this._client = httpClientFactory.CreateClient("wordlistloader");
        this._client.BaseAddress = cfg.GetSection("WordList").GetValue<Uri>("Url");

        var wordLength = cfg.GetSection("WordList").GetValue<int>("WordLength");
        this._wordListFilter = new Regex(string.Format("^[A-Za-z]{{{0}}}$", wordLength));
        
        this._wordList = new WordDictionary();
    }

    public async Task<WordDictionary> LoadAsync() 
    {
        if(!_wordList.Words.Any())
        {
            var words = await LoadFromExternalSource();
            _wordList = new WordDictionary(words);
        }

        return  _wordList;
    }

    private async Task<List<string>> LoadFromExternalSource() 
    {
        var resp = await _client.GetAsync("");
        var content = await resp.Content.ReadAsStreamAsync();

        var result = new List<string>();

        using(StreamReader sr = new StreamReader(content))
        {
            while(sr.Peek() >=0 )
            {
                var line = sr.ReadLine();
                if (line != null && _wordListFilter.IsMatch((string)line))
                {
                    result.Add((string)line);
                }
            }
        }
        return result;
    }
}