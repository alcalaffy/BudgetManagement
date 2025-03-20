using BudgetManagement.Models.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class CountType
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Campo {0} Obligatorio")]
        [FirstLetterInUpperCase]
        [Remote(action: "ValidateRepeatedCountType",controller: "CountTypes")]
        public string? Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }
    }
}
