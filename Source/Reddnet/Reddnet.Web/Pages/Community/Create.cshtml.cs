using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Community.Commands;
using Reddnet.Web.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Reddnet.Web.Pages.Community
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IMediator mediator;

        public CreateModel(IMediator mediator) => this.mediator = mediator;

        public void OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var Community = await this.mediator.Send(new CreateCommunityCommand
                {
                    UserId = User.GetUserId(),
                    Name = this.Name,
                    Description = this.Description,
                    Image = await this.Image.GetBytes()
                });

                return RedirectToPage("/Community/View", new { Community.Name });
            }

            return Page();
        }

        [Required]
        [BindProperty]
        public string Name { get; set; }
        [Required]
        [BindProperty]
        public string Description { get; set; }
        [Required]
        [BindProperty]
        public IFormFile Image { get; set; }
    }
}