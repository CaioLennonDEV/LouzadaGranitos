using LouzadaGranitos.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LouzadaGranitos.Core.Interfaces
{
    public interface ISerragemBlocoRepository
    {
        Task<SerragemBloco?> GetByIdAsync(int id);
        Task<IEnumerable<SerragemBloco>> GetAllAsync();
        Task<IEnumerable<SerragemBloco>> GetByBlocoAsync(int idBloco);
        Task<int> CreateAsync(SerragemBloco serragem);
        Task<bool> UpdateAsync(SerragemBloco serragem);
        Task<bool> DeleteAsync(int id);
    }
}