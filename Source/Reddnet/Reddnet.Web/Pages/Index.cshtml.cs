using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reddnet.Application.Community.Queries;
using Reddnet.Application.User.Queries;
using Reddnet.Domain.Entities;
using Reddnet.Web.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reddnet.Web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMediator mediator;

        public IndexModel(IMediator mediator)
            => this.mediator = mediator;

        public async Task<IActionResult> OnGet()
        {
            var communitiesResponse = await this.mediator.Send(new GetAllCommunitiesQuery());
            this.Communities = communitiesResponse.Data;

            var feedResponse = await this.mediator.Send(new GetUserFeedQuery
            {
                Id = User.GetUserId()
            });
            this.Feed = feedResponse.Data;

            return Page();
        }

        public IEnumerable<CommunityEntity> Communities { get; set; }
        public IEnumerable<PostEntity> Feed { get; set; }
    }
}