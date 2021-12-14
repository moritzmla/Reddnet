using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Community.Commands;

namespace Reddnet.Web.Pages.Community;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly IMediator mediator;

    public DeleteModel(IMediator mediator)
        => this.mediator = mediator;

    public async Task<IActionResult> OnGet(string name)
    {
        var response = await this.mediator.Send(new DeleteCommunityCommand
        {
            Name = name
        });

        if (response.IsError)
        {
            return Redirect(RouteConstants.Error);
        }

        this.Name = name;
        return Redirect(RouteConstants.Index);
    }

    [BindProperty]
    public string Name { get; set; }
}
