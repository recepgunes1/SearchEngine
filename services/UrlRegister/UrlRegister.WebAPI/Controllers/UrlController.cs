using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Entities;
using Shared.Events;
using UrlRegister.Infrastructure.Context;

namespace UrlRegister.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController(
    AppDbContext dbContext,
    IValidator<RegisterUrlDto> validator,
    IPublishEndpoint publishEndpoint)
    : Controller
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterUrlDto urlDto)
    {
        var result = await validator.ValidateAsync(urlDto);

        if (!result.IsValid)
            return BadRequest(string.Join(Path.PathSeparator, result.Errors.Select(p => p.ErrorMessage)));

        var entity = new RegisteredUrl
        {
            Link = urlDto.Link
        };

        await dbContext.RegisteredUrls.AddAsync(entity);

        await dbContext.SaveChangesAsync();

        await publishEndpoint.Publish(new DownloadedPage
        {
            Link = urlDto.Link
        });

        return Ok("url was saved");
    }
}