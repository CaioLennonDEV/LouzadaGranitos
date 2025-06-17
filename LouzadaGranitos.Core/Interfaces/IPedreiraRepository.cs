using LouzadaGranitos.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LouzadaGranitos.Core.Interfaces
{
    public interface IPedreiraRepository
    {
        Task<Pedreira?> GetByIdAsync(int id);
        Task<IEnumerable<Pedreira>> GetAllAsync();
        Task<int> CreateAsync(Pedreira pedreira);
        Task<bool> UpdateAsync(Pedreira pedreira);
        Task<bool> DeleteAsync(int id);
    }
}