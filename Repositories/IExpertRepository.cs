using AutocompleteApi.Models;

namespace AutocompleteApi.Repositories;

public interface IExpertRepository
{
    Task<List<Expert>> SearchByNameAsync(string query);
}

