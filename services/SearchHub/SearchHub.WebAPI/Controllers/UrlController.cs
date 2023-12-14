using Microsoft.AspNetCore.Mvc;
using SearchHub.Infrastructure.Services;
using Shared.DTOs;

namespace SearchHub.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController(IElasticService elasticService) : Controller
{
    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> Search([FromQuery] SearchUrlDto searchUrlDto)
    {
        var result = await elasticService.Search(searchUrlDto);
        return Ok(result);
    }

    [HttpGet]
    [Route("suggest")]
    public async Task<IActionResult> Suggest(string input)
    {
        var result = await elasticService.SuggestTitle(input);
        return Ok(result);
    }
}