using BudgetManagement.IServices;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Reflection;

namespace BudgetManagement.Controllers
{
    public class CountsController : Controller
    {
        private readonly ICountTypesRepository _countTypesRepository;
        private readonly IUserService _userService;
        private readonly ICountsRepository _countsRepository;
        public CountsController(ICountTypesRepository countTypesRepository,
                                IUserService userService,
                                ICountsRepository countsRepository)
        {
            _countTypesRepository = countTypesRepository;
            _userService = userService;
            _countsRepository = countsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public  async Task<IActionResult> Create()
        {
            var userId = _userService.GetUser();
            var model = new CreateCountViewModel();
            model.CountTypes = await GetCountTypes(userId);            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCountViewModel count)
        {
            var userId = _userService.GetUser();
            var countType = await _countTypesRepository.GetCountTypeById(userId,count.TipoCuentaId);

            if(countType is null)
            {
                return RedirectToAction("Not Found","NotFound");
            }

            if(!ModelState.IsValid)
            {
                count.CountTypes = await GetCountTypes(userId);
                return View(count);
            }

            await _countsRepository.Create(count);
            return RedirectToAction("Index");
        }
        private async Task<IEnumerable<SelectListItem>> GetCountTypes(int userId)
        {
            var countTypes = await _countTypesRepository.Get(userId);

            return countTypes.Select(x => new SelectListItem
            {
                Text = x.Nombre,
                Value = x.Id.ToString()
            }).ToList();
        }
    }
}

