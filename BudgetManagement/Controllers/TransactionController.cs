using BudgetManagement.IServices;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BudgetManagement.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserService _userService;
        private readonly ICountsRepository _countsRepository;
        private readonly ICategoryRepository _categoryRepository;
        public TransactionController( ITransactionRepository transactionRepository,
                                      IUserService userService,
                                      ICountsRepository countsRepository,
                                      ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _userService = userService;
            _countsRepository = countsRepository;
            _categoryRepository = categoryRepository;

        }
        public IActionResult Index()
        {
            return View();
        }
        //vista para crear una transaccion
        //necesita cargar las cuentas que el usuario tiene
        //para mapearlo a selectlistitem para desplegarlo en la vista
        public async Task<IActionResult> Create()
        {
            var user = _userService.GetUser();
            var model = new CreateTransactionViewModel();
            model.Cuentas=  await GetCounts(user);
            model.Categorias = await GetCategories(user,model.OperationTypeId);
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionViewModel transaction)
        {
            var user = _userService.GetUser();
            if(!ModelState.IsValid)
            {
                transaction.Cuentas=await GetCounts(user);
                transaction.Categorias = await GetCategories(user,transaction.OperationTypeId);
                return View(transaction);
            }
            var count = await _countsRepository.GetCountById(transaction.CuentaId,user);
            if(count is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var category=await _categoryRepository.GetById(transaction.CategoriaId,user);
            if(category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            transaction.UsuarioId = user;
            if(transaction.OperationTypeId==OperationType.Outcome)
            {
                transaction.Monto *= -1;
            }
            await _transactionRepository.Create(transaction);
            return RedirectToAction("Index");
        }
        private async Task<IEnumerable<SelectListItem>> GetCounts(int userId)
        {
            var counts= await _countsRepository.SearchCounts(userId);
            return counts.Select(c => new SelectListItem(c.Nombre,c.Id.ToString()));
        }
        [HttpPost]
        public async Task<IActionResult> GetCategoriesByOperation([FromBody] OperationType operationType)
        {
            var user = _userService.GetUser();
            var categories= await GetCategories(user,operationType);
            return Ok(categories);
        }
        private async Task<IEnumerable<SelectListItem>> GetCategories(int userId, OperationType operationType)
        {
            var categories = await _categoryRepository.GetCategories(userId, operationType);

            return categories.Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.Id.ToString()
            });
        }

    }
}
