using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Community.Queries;
using Reddnet.Application.Post.Commands;
using Reddnet.Domain.Entities;
using Reddnet.Web.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reddnet.Web.Pages.Post
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IMediator mediator;

        public CreateModel(IMediator mediator) => this.mediator = mediator;

        public async Task<IActionResult> OnGet()
        {
            this.Communities = await this.mediator.Send(new GetAllCommunitiesQuery());

            if (!this.Communities.Any())
            {
                return Redirect("/");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var post = await this.mediator.Send(new CreatePostCommand
                {
                    UserId = this.User.GetUserId(),
                    CommunityId = this.Community,
                    Image = await this.Image.GetBytes(),
                    Title = this.Title,
                    Content = this.Text
                });
                return RedirectToPage("/Post/View", new { post.Id });
            }
            return RedirectToPage();
        }

        [BindProperty]
        public IEnumerable<CommunityEntity> Communities { get; set; }
        [Required]
        [BindProperty]
        public Guid Community { get; set; }
        [Required]
        [BindProperty]
        public string Title { get; set; }
        [Required]
        [BindProperty]
        public string Text { get; set; }
        [Required]
        [BindProperty]
        public IFormFile Image { get; set; }
    }
}