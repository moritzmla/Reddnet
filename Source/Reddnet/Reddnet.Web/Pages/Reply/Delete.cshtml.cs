using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Reply.Commands;
using System;
using System.Threading.Tasks;

namespace Reddnet.Web.Pages.Reply
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IMediator mediator;

        public DeleteModel(IMediator mediator)
            => this.mediator = mediator;

        public async Task<IActionResult> OnGet(Guid id)
        {
            await this.mediator.Send(new DeleteReplyCommand
            {
                Id = id
            });
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
