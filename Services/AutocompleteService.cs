using AutocompleteApi.Constants;
using AutocompleteApi.Models;
using AutocompleteApi.Repositories;

namespace AutocompleteApi.Services;

public class AutocompleteService : IAutocompleteService
{
    private readonly IStockRepository _stockRepository;
    private readonly IExpertRepository _expertRepository;
    private readonly ILogger<AutocompleteService> _logger;

    public AutocompleteService(
        IStockRepository stockRepository,
        IExpertRepository expertRepository,
        ILogger<AutocompleteService> logger)
    {
        _stockRepository = stockRepository;
        _expertRepository = expertRepository;
        _logger = logger;
    }

    public async Task<List<AutocompleteResult>> SearchAsync(string query)
    {
        var normalizedQuery = query.Trim();
        var allResults = new List<(AutocompleteResult Result, int Priority, string SortKey)>();

        var allStocks = await _stockRepository.SearchByTickerOrNameAsync(normalizedQuery);
        var allExperts = await _expertRepository.SearchByNameAsync(normalizedQuery);

        allResults.AddRange(allStocks.Select(stock => {
            var (result, priority, sortKey) = MapStockToResult(stock, normalizedQuery);
            return ((AutocompleteResult)result, priority, sortKey);
        }));

        allResults.AddRange(allExperts.Select(expert => {
            var (result, priority, sortKey) = MapExpertToResult(expert, normalizedQuery);
            return ((AutocompleteResult)result, priority, sortKey);
        }));

        return allResults
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.SortKey)
            .Take(AppConstants.Search.MaxResults)
            .Select(x => x.Result)
            .ToList();
    }

    private static (StockResult Result, int Priority, string SortKey) MapStockToResult(Stock stock, string query)
    {
        var tickerExact = stock.Ticker.Equals(query, StringComparison.OrdinalIgnoreCase);
        var nameExact = stock.Name.Equals(query, StringComparison.OrdinalIgnoreCase);
        var tickerStartsWith = stock.Ticker.StartsWith(query, StringComparison.OrdinalIgnoreCase);
        var nameStartsWith = stock.Name.StartsWith(query, StringComparison.OrdinalIgnoreCase);

        var priority = CalculatePriority(tickerExact, nameExact, tickerStartsWith, nameStartsWith);
        var sortKey = (tickerExact || tickerStartsWith) ? stock.Ticker : stock.Name;

        return (new StockResult
        {
            Ticker = stock.Ticker,
            Name = stock.Name,
            MarketCap = stock.MarketCap
        }, priority, sortKey);
    }

    private static (ExpertResult Result, int Priority, string SortKey) MapExpertToResult(Expert expert, string query)
    {
        var nameExact = expert.Name.Equals(query, StringComparison.OrdinalIgnoreCase);
        var nameStartsWith = expert.Name.StartsWith(query, StringComparison.OrdinalIgnoreCase);

        var priority = CalculatePriority(false, nameExact, false, nameStartsWith);

        return (new ExpertResult
        {
            Name = expert.Name,
            ExpertType = expert.Type.ToLower()
        }, priority, expert.Name);
    }

    private static int CalculatePriority(bool tickerExact, bool nameExact, bool tickerStartsWith, bool nameStartsWith)
    {
        if (tickerExact) return 1;
        if (nameExact) return 2;
        if (tickerStartsWith || nameStartsWith) return 3;
        return 4;
    }
}

