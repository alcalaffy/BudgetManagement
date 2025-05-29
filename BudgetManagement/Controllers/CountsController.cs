using AutoMapper;
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
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public CountsController(ICountTypesRepository countTypesRepository,
                                IUserService userService,
                                ICountsRepository countsRepository,
                                IMapper mapper,
                                ITransactionRepository transactionRepository)
        {
            _countTypesRepository = countTypesRepository;
            _userService = userService;
            _countsRepository = countsRepository;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
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
                           TipoCuenta = g.Key,
                           Cuentas = g.AsEnumerable()
                       }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Create()
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
            var countType = await _countTypesRepository.GetCountTypeById(userId, count.TipoCuentaId);

            if (countType is null)
            {
                return RedirectToAction("Not Found", "NotFound");
            }

            if (!ModelState.IsValid)
            {
                count.CountTypes = await GetCountTypes(userId);
                return View(count);
            }

            await _countsRepository.Create(count);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var userId = _userService.GetUser();
            var count = await _countsRepository.GetCountById(id, userId);
            if(count is null)
            {
                return RedirectToAction("Not Found", "NotFound");
            }
            var model = _mapper.Map<CreateCountViewModel>(count);

            model.CountTypes = await GetCountTypes(userId);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CreateCountViewModel count)
        {
            var userId = _userService.GetUser();
            var countExist = await _countsRepository.GetCountById(count.Id, userId);
            if (countExist is null)
            {
                return RedirectToAction("Not Found", "NotFound");
            }
            var countType = await _countTypesRepository.GetCountTypeById(userId, count.TipoCuentaId);

            if (countType is null)
            {
                return RedirectToAction("Not Found", "NotFound");
            }
            await _countsRepository.Update(count);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult>Delete(int id)
        {
            var userId = _userService.GetUser();
            var count= await _countsRepository.GetCountById(id, userId);
            if (count is null)
            {
                return RedirectToAction("Not Found", "NotFound");
            }
            return View(count);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCount(int id)
        {
            var userId = _userService.GetUser();
            var count = await _countsRepository.GetCountById(id, userId);
            if (count is null)
            {
                return RedirectToAction("Not Found", "NotFound");
            }
            await _countsRepository.Delete(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int id,int mes,int año)
        {
            var userId = _userService.GetUser();
            var count = await _countsRepository.GetCountById(id, userId);
            if (count is null)
            {
                return RedirectToAction("Not Found", "NotFound");
            }
            DateTime fechaInicio;
            DateTime fechaFin;
            if (mes <= 0 || mes > 12 || año <= 1900)
            {
                var hoy = DateTime.Today;
                fechaInicio = new DateTime(hoy.Year, hoy.Month, 1);
            }
            else
            {
                fechaInicio = new DateTime(año, mes, 1);
            }
            fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            var obtenerTransaccionesPorCuenta = new GetTransactionsByAcount()
            {
                CuentaId = id,
                UsuarioId = userId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };
            var transactions = await _transactionRepository.GetTransactionByAcount(obtenerTransaccionesPorCuenta);
            var model = new DetailTransactionsReport();
            ViewBag.Cuenta = count.Nombre;
            var transactionsByDate = transactions.OrderByDescending(c => c.FechaTransaccion)
                                                  .GroupBy(c=>c.FechaTransaccion)
                                                  .Select(g=> new DetailTransactionsReport.TransactionsByDate(){
                                                    FechaTransaccion = g.Key,
                                                    Transacciones=g.AsEnumerable(),
                                                   });
            model.TransaccionesAgrupadas = transactionsByDate;
            model.FechaInicio = fechaInicio;
            model.FechaFin = fechaFin;
            ViewBag.mesAnterior=fechaInicio.AddMonths(-1).Month;
            ViewBag.añoAnterior = fechaInicio.AddMonths(-1).Year;
            ViewBag.mesPosterior = fechaInicio.AddMonths(1).Month;
            ViewBag.añoPosterior = fechaInicio.AddMonths(1).Year;
            return View(model);
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

