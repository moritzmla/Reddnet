using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Community.Commands;
using Reddnet.Application.Community.Queries;
using Reddnet.Web.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Reddnet.Web.Pages.Community
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IMediator mediator;

        public EditModel(IMediator mediator)
            => this.mediator = mediator;

        public async Task<IActionResult> OnGet(string name)
        {
            var community = await mediator.Send(new GetCommunityByNameQuery
            {
                Name = name
            });
            this.Name = community.Name;
            this.Description = community.Description;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var request = new UpdateCommunityCommand
                {
                    Name = this.Name,
                    Description = this.Description,
                    Image = this.Image == null ? null : await this.Image.GetBytes()
                };

                var name = await this.mediator.Send(request);
                return RedirectToPage(RouteConstants.CommunityView, new { name });
            }
            return Page();
        }

        [Required]
        [BindProperty]
        public string Name { get; set; }
        [Required]
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
    }
}