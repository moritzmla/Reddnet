using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Post.Queries;
using Reddnet.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reddnet.Web.Pages
{
    [Authorize]
    public class SearchModel : PageModel
    {
        private readonly IMediator mediator;

        public SearchModel(IMediator mediator)
            => this.mediator = mediator;

        public async Task<IActionResult> OnGet(string query)
        {
            this.Query = query;
            this.Posts = await this.mediator.Send(new GetPostsByFilter
            {
                Query = query
            });
            return Page();
        }

        [BindProperty]
        public string Query { get; set; }
        [BindProperty]
        public IEnumerable<PostEntity> Posts { get; set; }
    }
}