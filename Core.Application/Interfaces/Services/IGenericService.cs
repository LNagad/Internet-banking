using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class

    {
        Task<SaveViewModel> Add(SaveViewModel vm);
        Task Update(SaveViewModel vm, string id);
        Task Delete(string id);
        Task<SaveViewModel> GetByIdSaveViewModel(string id);
        Task<List<ViewModel>> GetAllViewModel();

    }
}
