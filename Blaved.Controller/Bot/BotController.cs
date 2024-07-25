using Blaved.Controllers.Attributes;
using Blaved.Controllers.Bot.RequestHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Telegram.Bot.Types;

namespace Blaved.Controllers.Bot;

public class BotController : Controller
{
    private readonly ILogger<BotController> _logger;

    public BotController(ILogger<BotController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [RateLimit(key: "Bot", tokens: 50, seconds: 1)]
    [ValidateTelegramBot]
    [IgnoreDuplicateUpdate]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandler handleUpdateService,
        CancellationToken cancellationToken)
    {
        LogContext.PushProperty("UpdateId", update.Id);
        _logger.LogInformation($"----->  Receipt of a request for API BOT");

        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);

        _logger.LogInformation($"Completed processing request of the API BOT  <-----");

        return Ok();
    }
}
