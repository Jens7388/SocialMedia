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
        private PostRepository _repo;

        public PostsController(PostRepository repo)
        {
            _repo = repo;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {         
            return View(await _repo.GetAllAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Post post = await _repo.GetByIdAsync(id);
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
                await _repo.AddAsync(post);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(post.UserId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Post post = await _repo.GetByIdAsync(id);
            if (post == null)
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
                   await _repo.UpdateAsync(post);
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
            Post post = await _repo.GetByIdAsync(id);
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
            Post post = await _repo.GetByIdAsync(id);
            await _repo.DeleteAsync(post);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PostExists(int? id)
        {
            return await _repo.Exists(id);
        }
    }
}