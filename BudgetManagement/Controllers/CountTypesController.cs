using BudgetManagement.IServices;
using BudgetManagement.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;


namespace BudgetManagement.Controllers
{
    public class CountTypesController : Controller
    {
        private readonly ICountTypesRepository _repository;
        private readonly IUserService _userService;
        public CountTypesController(ICountTypesRepository countTypesRepository,
                                    IUserService userService)
        {
            _repository = countTypesRepository;
            _userService = userService;
        }

        public IActionResult CreateCountType()
        {
                return View();
        }
        public IActionResult Update()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUser();
            var countTypes = await _repository.Get(userId);
            return View(countTypes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCountType(CountType countType)
        {
            if (!ModelState.IsValid)
            {
                return View(countType);
            }
            countType.UsuarioId = _userService.GetUser();
            var exist=await _repository.Validate(countType.Nombre, countType.UsuarioId);
            if(exist)
            {
                ModelState.AddModelError(nameof(countType.Nombre),
                                        $"The name {countType.Nombre} exist already.");
            }

            await _repository.Create(countType);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ValidateRepeatedCountType(string nombre)
        {
            var userId = _userService.GetUser();
            var exist = await _repository.Validate(nombre, userId);
            if(exist)
            {
                return Json($"The {nombre} already exist");
            }
            return Json(true);
        }
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var userId = _userService.GetUser();

            //se valida si el tipo de cuenta existe relacionada a ese usuario
            //de lo contrario cualquier usuario podria modificar el registro
            var count = await _repository.GetCountTypeById(id,userId);

            if (count is null)
            {
                RedirectToAction("Recurso no encontrado", "NotFound");
            }
            //await update
            return View(count);
        }
        [HttpPost]
        public async Task<ActionResult> Update(CountType countType)
        {
            var userId = _userService.GetUser();

            //se valida si el tipo de cuenta existe relacionada a ese usuario
            //de lo contrario cualquier usuario podria modificar el registro
            var count = await _repository.GetCountTypeById(countType.Id, userId);

            if (count is null)
            {
                RedirectToAction("Recurso no encontrado", "NotFound");
            }
            //await update
            await _repository.Update(countType);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userService.GetUser();

            var count = await _repository.GetCountTypeById(id, userId);

            if (count is null)
            {
                RedirectToAction("Recurso no encontrado", "NotFound");
            }

            return View(count);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCount(int id)
        {
            var userId = _userService.GetUser();

            var count = await _repository.GetCountTypeById(id, userId);

            if (count is null)
            {
                RedirectToAction("Recurso no encontrado", "NotFound");
            }
            await _repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
