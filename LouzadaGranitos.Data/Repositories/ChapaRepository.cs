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
    public class ChapaRepository : IChapaRepository
    {
        private readonly string _connectionString;

        public ChapaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Chapa?> GetByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Chapa>(
                "SELECT * FROM Chapas WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Chapa>> GetAllAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Chapa>("SELECT * FROM Chapas");
        }

        public async Task<IEnumerable<Chapa>> GetByBlocoAsync(int idBloco)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Chapa>(
                "SELECT * FROM Chapas WHERE IdBloco = @IdBloco", new { IdBloco = idBloco });
        }

        public async Task<int> CreateAsync(Chapa chapa)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                INSERT INTO Chapas (Codigo, IdBloco, TipoGranito, Comprimento, Largura, Espessura, Area, DataSerragem, Status, Preco) 
                VALUES (@Codigo, @IdBloco, @TipoGranito, @Comprimento, @Largura, @Espessura, @Area, @DataSerragem, @Status, @Preco);
                SELECT LAST_INSERT_ID();";
            
            return await connection.ExecuteScalarAsync<int>(sql, chapa);
        }

        public async Task<bool> UpdateAsync(Chapa chapa)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                UPDATE Chapas 
                SET Codigo = @Codigo, IdBloco = @IdBloco, TipoGranito = @TipoGranito, 
                    Comprimento = @Comprimento, Largura = @Largura, Espessura = @Espessura, 
                    Area = @Area, DataSerragem = @DataSerragem, Status = @Status, Preco = @Preco 
                WHERE Id = @Id";
            
            var rowsAffected = await connection.ExecuteAsync(sql, chapa);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM Chapas WHERE Id = @Id", new { Id = id });
            return rowsAffected > 0;
        }

        public async Task AddRangeAsync(IEnumerable<Chapa> chapas)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                INSERT INTO Chapas (Codigo, IdBloco, TipoGranito, Comprimento, Largura, Espessura, Area, DataSerragem, Status, Preco) 
                VALUES (@Codigo, @IdBloco, @TipoGranito, @Comprimento, @Largura, @Espessura, @Area, @DataSerragem, @Status, @Preco)";
            
            await connection.ExecuteAsync(sql, chapas);
        }
    }
}