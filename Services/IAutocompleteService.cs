using AutocompleteApi.Models;

namespace AutocompleteApi.Services;

public interface IAutocompleteService
{
    Task<List<AutocompleteResult>> SearchAsync(string query);
}

