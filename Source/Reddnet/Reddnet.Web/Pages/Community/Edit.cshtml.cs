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
            var response = await mediator.Send(new GetCommunityByNameQuery
            {
                Name = name
            });

            if (response.IsError)
            {
                return Redirect(RouteConstants.Error);
            }
            else if (response.Data.UserId != User.GetUserId())
            {
                return Redirect(RouteConstants.Error);
            }

            this.Name = response.Data.Name;
            this.Description = response.Data.Description;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var response = await this.mediator.Send(new UpdateCommunityCommand
                {
                    Name = this.Name,
                    Description = this.Description,
                    Image = this.Image == null ? null : await this.Image.GetBytes()
                });

                if (response.IsError)
                {
                    return Redirect(RouteConstants.Error);
                }

                return RedirectToPage(RouteConstants.CommunityView, new { response.Data });
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