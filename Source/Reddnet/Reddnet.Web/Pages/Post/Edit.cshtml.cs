using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Post.Commands;
using Reddnet.Application.Post.Queries;
using Reddnet.Web.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Reddnet.Web.Pages.Post
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IMediator mediator;

        public EditModel(IMediator mediator)
            => this.mediator = mediator;

        public async Task<IActionResult> OnGet(Guid id)
        {
            this.Id = id;
            var post = await this.mediator.Send(new GetPostByIdQuery
            {
                Id = id
            });
            this.Title = post.Title;
            this.Text = post.Content;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var request = new UpdatePostCommand
                {
                    Id = this.Id,
                    Title = this.Title,
                    Content = this.Text,
                    Image = this.Image == null ? null : await this.Image.GetBytes()
                };

                var id = await this.mediator.Send(request);
                return RedirectToPage(RouteConstants.PostView, new { id });
            }
            return Page();
        }

        [Required]
        [BindProperty]
        public Guid Id { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
        [Required]
        [BindProperty]
        public string Title { get; set; }
        [Required]
        [BindProperty]
        public string Text { get; set; }
    }
}