using BudgetManagement.IServices;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManagement.Controllers
{
    public class CountsController : Controller
    {
        private readonly ICountTypesRepository _countTypesRepository;
        private readonly IUserService _userService;

        public CountsController(ICountTypesRepository countTypesRepository,
                                IUserService userService)
        {
            _countTypesRepository = countTypesRepository;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public  async Task<IActionResult> Create()
        {
            var userId = _userService.GetUser();
            var countTypes = await _countTypesRepository.Get(userId);
            var model = new CreateCountViewModel();

            model.CountTypes = countTypes.Select(x => new SelectListItem
            {
                Text = x.Nombre,  
                Value = x.Id.ToString()
            }).ToList();

            return View(model);
        }
    }
}

