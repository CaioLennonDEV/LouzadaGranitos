using LouzadaGranitos.Core.Interfaces;
using LouzadaGranitos.Core.Models;
using System.Threading.Tasks;

namespace LouzadaGranitos.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AutenticacaoService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario?> AutenticarAsync(string login, string senha)
        {
            var usuario = await _usuarioRepository.GetByLoginAsync(login);
            
            if (usuario != null && usuario.Senha == senha && usuario.Ativo)
            {
                return usuario;
            }
            
            return null;
        }
    }
}