using LouzadaGranitos.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LouzadaGranitos.Core.Interfaces
{
    public interface IChapaRepository
    {
        Task<Chapa?> GetByIdAsync(int id);
        Task<IEnumerable<Chapa>> GetAllAsync();
        Task<IEnumerable<Chapa>> GetByBlocoAsync(int idBloco);
        Task<int> CreateAsync(Chapa chapa);
        Task<bool> UpdateAsync(Chapa chapa);
        Task<bool> DeleteAsync(int id);
        Task AddRangeAsync(IEnumerable<Chapa> chapas); // Para o processo de serragem
    }
}