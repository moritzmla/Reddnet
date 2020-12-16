using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Post.Queries;
using Reddnet.Application.Reply.Commands;
using Reddnet.Domain.Entities;
using Reddnet.Web.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Reddnet.Web.Pages.Post
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
            => this.mediator = mediator;

        public async Task<IActionResult> OnGet(Guid id)
        {
            this.Post = await this.mediator.Send(new GetPostByIdQuery
            {
                Id = id
            });
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                await this.mediator.Send(new CreateReplyCommand
                {
                    PostId = this.Post.Id,
                    UserId = User.GetUserId(),
                    Content = this.Text
                });
            }
            return RedirectToPage();
        }

        [BindProperty]
        public PostEntity Post { get; set; }
        [Required]
        [BindProperty]
        public string Text { get; set; }
    }
}