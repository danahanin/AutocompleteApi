using Microsoft.AspNetCore.Mvc;
using AutocompleteApi.Services;
using AutocompleteApi.Models;

namespace AutocompleteApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutocompleteController : ControllerBase
{
    private readonly IAutocompleteService _autocompleteService;
    private readonly ILogger<AutocompleteController> _logger;

    public AutocompleteController(IAutocompleteService autocompleteService, ILogger<AutocompleteController> logger)
    {
        _autocompleteService = autocompleteService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetAutocomplete([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new { error = "Query parameter is empty, please provide a query" });
        }

        try
        {
            List<AutocompleteResult> results = await _autocompleteService.SearchAsync(query);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching autocomplete");
            return StatusCode(500, new { error = "An error occurred while processing your request" });
        }
    }
}

