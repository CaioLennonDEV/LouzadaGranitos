using LouzadaGranitos.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LouzadaGranitos.Core.Interfaces
{
    public interface IBlocoRepository
    {
        Task<Bloco?> GetByIdAsync(int id);
        Task<IEnumerable<Bloco>> GetAllAsync();
        Task<IEnumerable<Bloco>> GetByPedreiraAsync(int idPedreira);
        Task<int> CreateAsync(Bloco bloco);
        Task<bool> UpdateAsync(Bloco bloco);
        Task<bool> DeleteAsync(int id);
    }
}