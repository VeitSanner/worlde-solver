using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sanner.Wordle.Solver.Wordle;

namespace Sanner.Wordle.Solver.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly WordleRecommendation _wr;

    public IndexModel(ILogger<IndexModel> logger, Wordle.WordleRecommendation wr)
    {
        _logger = logger;
        _wr = wr;
    }

    [BindProperty]
    public string RequiredCharacters { get; set; } = "";

    [BindProperty]
    public string ExcludedCharacters { get; set; } = "";
    
    [BindProperty]
    public string PositionData { get; set; } = "";

    [BindProperty]
    public List<Recommendation> RecommendedWords {get;set;} = new List<Recommendation>();

    public void OnGet()
    {
        _logger.LogInformation(new EventId(1212), null, $"{RequiredCharacters}");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogInformation(new EventId(1212), null, $"{RequiredCharacters}");

        RecommendedWords = (await _wr.GetRecommendation(RequiredCharacters, ExcludedCharacters, PositionData)).ToList();

        return Page();
    }
}
