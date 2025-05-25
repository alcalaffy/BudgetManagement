using AutoMapper;
using BudgetManagement.Models;

namespace BudgetManagement.Services
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Count, CreateCountViewModel>();
            CreateMap <UpdateTransactionViewModel,Transaction>().ReverseMap();
        }
    }
}
