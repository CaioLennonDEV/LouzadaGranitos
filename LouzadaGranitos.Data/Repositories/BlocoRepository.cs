using Dapper;
using LouzadaGranitos.Core.Interfaces;
using LouzadaGranitos.Core.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace LouzadaGranitos.Data.Repositories
{
    public class BlocoRepository : IBlocoRepository
    {
        private readonly string _connectionString;

        public BlocoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Bloco?> GetByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Bloco>(
                "SELECT * FROM Blocos WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Bloco>> GetAllAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Bloco>("SELECT * FROM Blocos");
        }

        public async Task<IEnumerable<Bloco>> GetByPedreiraAsync(int idPedreira)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Bloco>(
                "SELECT * FROM Blocos WHERE IdPedreira = @IdPedreira", new { IdPedreira = idPedreira });
        }

        public async Task<int> CreateAsync(Bloco bloco)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                INSERT INTO Blocos (Codigo, IdPedreira, TipoGranito, Comprimento, Largura, Altura, Peso, DataExtracao, Status, Preco) 
                VALUES (@Codigo, @IdPedreira, @TipoGranito, @Comprimento, @Largura, @Altura, @Peso, @DataExtracao, @Status, @Preco);
                SELECT LAST_INSERT_ID();";
            
            return await connection.ExecuteScalarAsync<int>(sql, bloco);
        }

        public async Task<bool> UpdateAsync(Bloco bloco)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                UPDATE Blocos 
                SET Codigo = @Codigo, IdPedreira = @IdPedreira, TipoGranito = @TipoGranito, 
                    Comprimento = @Comprimento, Largura = @Largura, Altura = @Altura, 
                    Peso = @Peso, DataExtracao = @DataExtracao, Status = @Status, Preco = @Preco 
                WHERE Id = @Id";
            
            var rowsAffected = await connection.ExecuteAsync(sql, bloco);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM Blocos WHERE Id = @Id", new { Id = id });
            return rowsAffected > 0;
        }
    }
}