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

        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUser();
            var countsWithType = await _countsRepository.SearchCounts(userId);

            //the list of counts with types need to transfer in a indexcountsviewmodel class
            //first we agrupeated the list with the field TipoCuenta and then we do the
            // convertion
            var model = countsWithType
                       .GroupBy(c => c.TipoCuenta)
                       .Select(g => new IndexCountsViewModel
                       {
                           TipoCuenta=g.Key,
                           Cuentas=g.AsEnumerable()
                       }).ToList();

            return View(model);
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

