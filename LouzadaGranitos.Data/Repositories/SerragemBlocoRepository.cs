using Dapper;
using LouzadaGranitos.Core.Interfaces;
using LouzadaGranitos.Core.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks;

namespace LouzadaGranitos.Data.Repositories
{
    public class SerragemBlocoRepository : ISerragemBlocoRepository
    {
        private readonly string _connectionString;

        public SerragemBlocoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<SerragemBloco?> GetByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<SerragemBloco>(
                "SELECT * FROM SerragemBlocos WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<SerragemBloco>> GetAllAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<SerragemBloco>("SELECT * FROM SerragemBlocos");
        }

        public async Task<IEnumerable<SerragemBloco>> GetByBlocoAsync(int idBloco)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<SerragemBloco>(
                "SELECT * FROM SerragemBlocos WHERE IdBloco = @IdBloco", new { IdBloco = idBloco });
        }

        public async Task<int> CreateAsync(SerragemBloco serragem)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                INSERT INTO SerragemBlocos (IdBloco, DataSerragem, TipoSerragem, EspessuraChapa, QuantidadeChapas, Rendimento, Observacoes, Operador) 
                VALUES (@IdBloco, @DataSerragem, @TipoSerragem, @EspessuraChapa, @QuantidadeChapas, @Rendimento, @Observacoes, @Operador);
                SELECT LAST_INSERT_ID();";
            
            return await connection.ExecuteScalarAsync<int>(sql, serragem);
        }

        public async Task<bool> UpdateAsync(SerragemBloco serragem)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                UPDATE SerragemBlocos 
                SET IdBloco = @IdBloco, DataSerragem = @DataSerragem, TipoSerragem = @TipoSerragem, 
                    EspessuraChapa = @EspessuraChapa, QuantidadeChapas = @QuantidadeChapas, 
                    Rendimento = @Rendimento, Observacoes = @Observacoes, Operador = @Operador 
                WHERE Id = @Id";
            
            var rowsAffected = await connection.ExecuteAsync(sql, serragem);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM SerragemBlocos WHERE Id = @Id", new { Id = id });
            return rowsAffected > 0;
        }
    }
}