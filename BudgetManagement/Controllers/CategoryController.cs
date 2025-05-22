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
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var user = _userService.GetUser();
            var categories = await _categoryRepository.GetCategories(user);
            return View(categories);
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

    }
}
