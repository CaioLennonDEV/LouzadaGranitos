using LouzadaGranitos.Core.Interfaces;
using LouzadaGranitos.Core.Models;

namespace LouzadaGranitos.Services
{
    public class CadastroService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPedreiraRepository _pedreiraRepository;
        private readonly IBlocoRepository _blocoRepository;
        private readonly IChapaRepository _chapaRepository;
        private readonly ISerragemBlocoRepository _serragemRepository;

        public CadastroService(
            IUsuarioRepository usuarioRepository,
            IPedreiraRepository pedreiraRepository,
            IBlocoRepository blocoRepository,
            IChapaRepository chapaRepository,
            ISerragemBlocoRepository serragemRepository)
        {
            _usuarioRepository = usuarioRepository;
            _pedreiraRepository = pedreiraRepository;
            _blocoRepository = blocoRepository;
            _chapaRepository = chapaRepository;
            _serragemRepository = serragemRepository;
        }

        public async Task<bool> CadastrarUsuarioAsync(Usuario usuario)
        {
            try
            {
                await _usuarioRepository.CreateAsync(usuario);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AtualizarUsuarioAsync(Usuario usuario)
        {
            try
            {
                return await _usuarioRepository.UpdateAsync(usuario);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ExcluirUsuarioAsync(int id)
        {
            try
            {
                return await _usuarioRepository.DeleteAsync(id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CadastrarPedreiraAsync(Pedreira pedreira)
        {
            try
            {
                await _pedreiraRepository.CreateAsync(pedreira);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CadastrarBlocoAsync(Bloco bloco)
        {
            try
            {
                await _blocoRepository.CreateAsync(bloco);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ProcessarSerragemAsync(SerragemBloco serragem, List<Chapa> chapas)
        {
            try
            {
                // Salvar a serragem
                await _serragemRepository.CreateAsync(serragem);
                
                // Salvar as chapas geradas
                await _chapaRepository.AddRangeAsync(chapas);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Usuario>> ListarUsuarios()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Pedreira>> ListarPedreiras()
        {
            return await _pedreiraRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Bloco>> ListarBlocos()
        {
            return await _blocoRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Chapa>> ListarChapas()
        {
            return await _chapaRepository.GetAllAsync();
        }
    }
} 