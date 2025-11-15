using AutocompleteApi.Models;

namespace AutocompleteApi.Repositories;

public interface IStockRepository
{
    Task<List<Stock>> SearchByTickerOrNameAsync(string query);
}

