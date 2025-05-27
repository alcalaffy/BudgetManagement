using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        public DateTime FechaTransaccion { get; set; }=DateTime.Today;
        [Range(1,maximum:int.MaxValue,ErrorMessage ="Pick a category")]
        public decimal Monto { get; set; }
        [Display(Name = "Category")]
        public int CategoriaId { get; set; }
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Pick a count")]
        [Display(Name = "Count")]
        public int CuentaId { get; set; }
        [StringLength(maximumLength:1000,ErrorMessage ="The note can´t have mora than {1} characters")]
        [Display(Name = "Note")]
        public string Nota { get; set; }
        [Display(Name = "Operation Type")]
        public OperationType OperationTypeId { get; set; } = OperationType.Income;
        public string Cuenta { get; set; }
        public string Categoria { get; set; }
    }
}
