using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogCoreEngine.Core.Entities;
using BlogCoreEngine.Core.Interfaces;
using BlogCoreEngine.DataAccess.Data;
using BlogCoreEngine.DataAccess.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                return RedirectToAction("Details", "Post", new { id });
            }

            var newComment = await this.commentRepository.Add(new CommentDataModel
            {
                Content = CommentText,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                AuthorId = User.Identity.GetAuthorId(),
                PostId = id
            });

            return RedirectToAction("Details", "Post", new { id = newComment.PostId });
        }

        #endregion

        #region Delete

        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await this.commentRepository.GetById(id);

            if (!(User.Identity.GetAuthorId() == comment.AuthorId || this.User.IsInRole("Administrator")))
            {
                return RedirectToAction("NoAccess", "Home");
            }

            await this.commentRepository.Remove(id);

            return RedirectToAction("Details", "Post", new { id = comment.PostId });
        }

        #endregion

        #region Edit

        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var comment = await this.commentRepository.GetById(id);

            if (!(User.Identity.GetAuthorId() == comment.AuthorId || this.User.IsInRole("Administrator")))
            {
                return RedirectToAction("NoAccess", "Home");
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

                return RedirectToAction("Details", "Post", new { id = targetComment.PostId });
            }

            return View(comment);
        }

        #endregion
    }
}