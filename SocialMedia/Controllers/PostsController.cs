using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using DataAccess;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Models.Context;

namespace SocialMedia.Controllers
{
    public class PostsController : Controller
    {
        private PostRepository repo;
        private readonly CommentRepository commentRepo;

        public PostsController(PostRepository repo, CommentRepository commentRepo)
        {
            this.repo = repo;
            this.commentRepo = commentRepo;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {         
            return View(await repo.GetAllAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Post post = await repo.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
           // ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Image,IsEdited,Created,UserId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Created = DateTime.Now;
                post.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await repo.AddAsync(post);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(post.UserId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Post post = await repo.GetByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            if(post.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Description,Image,IsEdited,Created,UserId")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Post originalPost = await repo.GetByIdAsync(id);

                    originalPost.IsEdited = true;

                    originalPost.Title = post.Title;
                    originalPost.Description = post.Description;
                    originalPost.Image = post.Image;

                    await repo.UpdateAsync(originalPost);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(post.UserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Post post = await repo.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Post post = await repo.GetByIdAsync(id);
            await repo.DeleteAsync(post);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Reply()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int? id, [Bind("Description,PostId,Image,IsEdited,Created,UserId")] Comment comment)
        {
            Post post = await repo.GetByIdAsync(id);
            if(ModelState.IsValid)
            {
                comment.Created = DateTime.Now;
                comment.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                comment.PostId = post.Id;
                await commentRepo.AddAsync(comment);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(post.UserId);
            return View(post);
        }

        public async Task<IActionResult> Comments(int? id)
        {
            Post post = await repo.GetByIdAsync(id);
            if(post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        private async Task<bool> PostExists(int? id)
        {
            return await repo.Exists(id);
        }
    }
}