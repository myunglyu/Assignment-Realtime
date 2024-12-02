using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace SignalRProject.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public IndexModel(ILogger<IndexModel> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public string? Username { get; set; }

    public async Task OnGetAsync()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var user = await _userManager.GetUserAsync(User);
            Username = user?.UserName;
        }
    }
}