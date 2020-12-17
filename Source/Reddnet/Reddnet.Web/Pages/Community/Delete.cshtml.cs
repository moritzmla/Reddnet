using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Community.Commands;
using System.Threading.Tasks;

namespace Reddnet.Web.Pages.Community
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IMediator mediator;

        public DeleteModel(IMediator mediator)
            => this.mediator = mediator;

        public async Task<IActionResult> OnGet(string name)
        {
            this.Name = name;
            await this.mediator.Send(new DeleteCommunityCommand
            {
                Name = name
            });
            return Redirect(RouteConstants.Index);
        }

        [BindProperty]
        public string Name { get; set; }
    }
}