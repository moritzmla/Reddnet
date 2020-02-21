using BlogCoreEngine.Core.Entities;
using BlogCoreEngine.Core.Interfaces;
using BlogCoreEngine.DataAccess.Data;
using BlogCoreEngine.DataAccess.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reddnet.Web.Extensions;
using System;
using System.Threading.Tasks;

namespace BlogCoreEngine.Controllers
{
    public class CommentController : Controller
    {
        private readonly IAsyncRepository<CommentDataModel> commentRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentController(IAsyncRepository<CommentDataModel> commentRepository, UserManager<ApplicationUser> userManager)
        {
            this.commentRepository = commentRepository;
            this.userManager = userManager;
        }

        #region New

        [Authorize, HttpPost]
        public async Task<IActionResult> New(Guid id, string CommentText)
        {
            if (string.IsNullOrWhiteSpace(CommentText))
            {
                ModelState.AddModelError("", "Text field is required!");
                return this.RedirectToAsync<PostController>(x => x.Details(id));
            }

            var newComment = await this.commentRepository.Add(new CommentDataModel
            {
                Content = CommentText,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                AuthorId = User.Identity.GetAuthorId(),
                PostId = id
            });

            return this.RedirectToAsync<PostController>(x => x.Details(newComment.PostId.Value));
        }

        #endregion

        #region Delete

        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await this.commentRepository.GetById(id);

            if (!(User.Identity.GetAuthorId() == comment.AuthorId || this.User.IsInRole("Administrator")))
            {
                return this.RedirectTo<HomeController>(x => x.NoAccess());
            }

            await this.commentRepository.Remove(id);
            return this.RedirectToAsync<PostController>(x => x.Details(comment.PostId.Value));
        }

        #endregion

        #region Edit

        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var comment = await this.commentRepository.GetById(id);

            if (!(User.Identity.GetAuthorId() == comment.AuthorId || this.User.IsInRole("Administrator")))
            {
                return this.RedirectTo<HomeController>(x => x.NoAccess());
            }

            return View(comment);
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> Edit(Guid id, CommentDataModel comment)
        {
            if (ModelState.IsValid)
            {
                var targetComment = await this.commentRepository.GetById(id);
                targetComment.Content = comment.Content;

                await this.commentRepository.Update(targetComment);

                return this.RedirectToAsync<PostController>(x => x.Details(targetComment.PostId.Value));
            }

            return View(comment);
        }

        #endregion
    }
}