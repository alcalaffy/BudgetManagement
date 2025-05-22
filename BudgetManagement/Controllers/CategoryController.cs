using BudgetManagement.IServices;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManagement.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoryRepository;
        private IUserService _userService;
        public CategoryController(ICategoryRepository categoryRepository,IUserService userService)
        {
                _categoryRepository = categoryRepository;
                _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var user = _userService.GetUser();
            var categories = await _categoryRepository.GetCategories(user);
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Update(int id)
        {
            var user = _userService.GetUser();
            var category = await _categoryRepository.GetById(id,user);
            if(category is null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var user = _userService.GetUser();
            var cat = await _categoryRepository.GetById(id, user);

            if (cat is null)
            {
                return RedirectToAction("Index");
            }
            return View(cat);
        }
        
        [HttpPost]
        public async Task<IActionResult>Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View(category);
            }
            var user = _userService.GetUser();
            category.UsuarioId = user;
            await _categoryRepository.Create(category);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult>Update(Category category)
        {
            var user = _userService.GetUser();
            var cat= await _categoryRepository.GetById(category.Id, user);
            if(cat is null)
            {
                return RedirectToAction("Index");
            }
            category.UsuarioId = user;
            await _categoryRepository.Update(category);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var user = _userService.GetUser();
            var cat = await _categoryRepository.GetById(id, user);

            if (cat is null)
            {
                return RedirectToAction("Index");
            }
            await _categoryRepository.Delete(id,user);
            return RedirectToAction("Index");
        }

    }
}
