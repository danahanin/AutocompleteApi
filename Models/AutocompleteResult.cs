using AutocompleteApi.Constants;

namespace AutocompleteApi.Models;

public abstract class AutocompleteResult
{
    public string Type { get; set; } = string.Empty;
}

public class StockResult : AutocompleteResult
{
    public StockResult() => Type = AppConstants.ResultTypes.Stock;
    public string Ticker { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long MarketCap { get; set; }
}

public class ExpertResult : AutocompleteResult
{
    public ExpertResult() => Type = AppConstants.ResultTypes.Expert;
    public string Name { get; set; } = string.Empty;
    public string ExpertType { get; set; } = string.Empty;
}

