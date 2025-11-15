namespace AutocompleteApi.Services;

public interface IAutocompleteService
{
    Task<List<object>> SearchAsync(string query);
}

