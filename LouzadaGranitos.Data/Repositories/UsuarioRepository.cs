using Dapper;
using LouzadaGranitos.Core.Interfaces;
using LouzadaGranitos.Core.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

namespace LouzadaGranitos.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                "SELECT * FROM Usuarios WHERE Id = @Id", new { Id = id });
        }

        public async Task<Usuario?> GetByLoginAsync(string login)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                "SELECT * FROM Usuarios WHERE Login = @Login", new { Login = login });
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Usuario>("SELECT * FROM Usuarios");
        }

        public async Task<int> CreateAsync(Usuario usuario)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                INSERT INTO Usuarios (Nome, Login, Senha, Email, Ativo) 
                VALUES (@Nome, @Login, @Senha, @Email, @Ativo);
                SELECT LAST_INSERT_ID();";
            
            return await connection.ExecuteScalarAsync<int>(sql, usuario);
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                UPDATE Usuarios 
                SET Nome = @Nome, Login = @Login, Senha = @Senha, Email = @Email, Ativo = @Ativo 
                WHERE Id = @Id";
            
            var rowsAffected = await connection.ExecuteAsync(sql, usuario);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });
            return rowsAffected > 0;
        }
    }
}