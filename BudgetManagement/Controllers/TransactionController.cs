using AutoMapper;
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
        private readonly IMapper _mapper;
        public TransactionController( ITransactionRepository transactionRepository,
                                      IUserService userService,
                                      ICountsRepository countsRepository,
                                      ICategoryRepository categoryRepository,
                                      IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userService = userService;
            _countsRepository = countsRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;

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

        [HttpPost]
        public async Task<IActionResult> GetCategoriesByOperation([FromBody] OperationType operationType)
        {
            var user = _userService.GetUser();
            var categories= await GetCategories(user,operationType);
            return Ok(categories);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id) 
        {
            var user = _userService.GetUser();
            var currentTransaction = await _transactionRepository.GetById(id, user);
            if(currentTransaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var model = _mapper.Map<UpdateTransactionViewModel>(currentTransaction);
            model.Monto = currentTransaction.Monto;
            if(model.OperationTypeId==OperationType.Outcome)
            {
                model.MontoAnterior = model.Monto * -1;
            }
            model.CuentaAnterior = currentTransaction.CuentaId;
            model.Categorias = await GetCategories(user,model.OperationTypeId);
            model.Cuentas = await GetCounts(user);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateTransactionViewModel transaction)
        {
            var user = _userService.GetUser();
            if(!ModelState.IsValid)
            {
                transaction.Cuentas = await GetCounts(user);
                transaction.Categorias = await GetCategories(user,transaction.OperationTypeId);
                return View(transaction);
            }
            var count = await _countsRepository.GetCountById(transaction.CuentaId,user);
            if(count is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var categoty = await _categoryRepository.GetById(transaction.CategoriaId,user);
            if(categoty is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var currentTransaction=_mapper.Map<Transaction>(transaction);
            if (transaction.OperationTypeId == OperationType.Outcome)
            {
                currentTransaction.Monto *= -1;
            }
            await _transactionRepository.Update(currentTransaction,transaction.CuentaAnterior,transaction.MontoAnterior);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user= _userService.GetUser();
            var transaction = await _transactionRepository.GetById(id,user);
            if(transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await _transactionRepository.Delete(id);
            return RedirectToAction("Index");
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
        private async Task<IEnumerable<SelectListItem>> GetCounts(int userId)
        {
            var counts = await _countsRepository.SearchCounts(userId);
            return counts.Select(c => new SelectListItem(c.Nombre, c.Id.ToString()));
        }

    }
}
