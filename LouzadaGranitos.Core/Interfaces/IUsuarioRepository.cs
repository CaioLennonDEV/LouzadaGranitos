using LouzadaGranitos.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LouzadaGranitos.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByLoginAsync(string login);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<int> CreateAsync(Usuario usuario);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
    }
}