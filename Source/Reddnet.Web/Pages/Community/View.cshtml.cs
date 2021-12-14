using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Community.Queries;
using Reddnet.Domain.Entities;

namespace Reddnet.Web.Pages.Community;

[Authorize]
public class ViewModel : PageModel
{
    private readonly IMediator mediator;

    public ViewModel(IMediator mediator) => this.mediator = mediator;

    public async Task<IActionResult> OnGet()
    {
        var response = await this.mediator.Send(new GetCommunityByNameQuery
        {
            Name = this.Name
        });

        if (response.IsError)
        {
            return Redirect(RouteConstants.Error);
        }

        this.Community = response.Data;
        return Page();
    }

    [BindProperty(SupportsGet = true)]
    public string Name { get; set; }
    [BindProperty]
    public CommunityEntity Community { get; set; }
}
