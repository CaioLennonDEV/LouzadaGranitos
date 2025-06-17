using Dapper;
using LouzadaGranitos.Core.Interfaces;
using LouzadaGranitos.Core.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

namespace LouzadaGranitos.Data.Repositories
{
    public class PedreiraRepository : IPedreiraRepository
    {
        private readonly string _connectionString;

        public PedreiraRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Pedreira?> GetByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Pedreira>(
                "SELECT * FROM Pedreiras WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Pedreira>> GetAllAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Pedreira>("SELECT * FROM Pedreiras");
        }

        public async Task<int> CreateAsync(Pedreira pedreira)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                INSERT INTO Pedreiras (Nome, Localizacao, Proprietario, Ativa) 
                VALUES (@Nome, @Localizacao, @Proprietario, @Ativa);
                SELECT LAST_INSERT_ID();";
            
            return await connection.ExecuteScalarAsync<int>(sql, pedreira);
        }

        public async Task<bool> UpdateAsync(Pedreira pedreira)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                UPDATE Pedreiras 
                SET Nome = @Nome, Localizacao = @Localizacao, Proprietario = @Proprietario, Ativa = @Ativa 
                WHERE Id = @Id";
            
            var rowsAffected = await connection.ExecuteAsync(sql, pedreira);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM Pedreiras WHERE Id = @Id", new { Id = id });
            return rowsAffected > 0;
        }
    }
}