using MySql.Data.MySqlClient;
using Dapper;

namespace LouzadaGranitos.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(string connectionString)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            // Criar tabelas
            CreateTables(connection);
            
            // Inserir usuário admin padrão
            InsertAdminUser(connection);
        }

        private static void CreateTables(MySqlConnection connection)
        {
            // Tabela Usuarios
            var createUsuariosTable = @"
                CREATE TABLE IF NOT EXISTS Usuarios (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Nome VARCHAR(100) NOT NULL,
                    Login VARCHAR(50) NOT NULL UNIQUE,
                    Senha VARCHAR(100) NOT NULL,
                    Email VARCHAR(100),
                    DataCadastro DATETIME DEFAULT CURRENT_TIMESTAMP,
                    Ativo BOOLEAN DEFAULT TRUE
                )";

            // Tabela Pedreiras
            var createPedreirasTable = @"
                CREATE TABLE IF NOT EXISTS Pedreiras (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Nome VARCHAR(100) NOT NULL,
                    Localizacao VARCHAR(200),
                    Proprietario VARCHAR(100),
                    DataCadastro DATETIME DEFAULT CURRENT_TIMESTAMP,
                    Ativa BOOLEAN DEFAULT TRUE
                )";

            // Tabela Blocos
            var createBlocosTable = @"
                CREATE TABLE IF NOT EXISTS Blocos (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Codigo VARCHAR(50) NOT NULL,
                    IdPedreira INT,
                    TipoGranito VARCHAR(50),
                    Comprimento DECIMAL(10,2),
                    Largura DECIMAL(10,2),
                    Altura DECIMAL(10,2),
                    Peso DECIMAL(10,2),
                    DataExtracao DATE,
                    Status VARCHAR(20) DEFAULT 'Disponível',
                    Preco DECIMAL(10,2),
                    FOREIGN KEY (IdPedreira) REFERENCES Pedreiras(Id)
                )";

            // Tabela Chapas
            var createChapasTable = @"
                CREATE TABLE IF NOT EXISTS Chapas (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Codigo VARCHAR(50) NOT NULL,
                    IdBloco INT,
                    TipoGranito VARCHAR(50),
                    Comprimento DECIMAL(10,2),
                    Largura DECIMAL(10,2),
                    Espessura DECIMAL(5,2),
                    Area DECIMAL(10,2),
                    DataSerragem DATETIME,
                    Status VARCHAR(20) DEFAULT 'Disponível',
                    Preco DECIMAL(10,2),
                    FOREIGN KEY (IdBloco) REFERENCES Blocos(Id)
                )";

            // Tabela SerragemBlocos
            var createSerragemBlocosTable = @"
                CREATE TABLE IF NOT EXISTS SerragemBlocos (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    IdBloco INT,
                    DataSerragem DATETIME,
                    TipoSerragem VARCHAR(50),
                    EspessuraChapa DECIMAL(5,2),
                    QuantidadeChapas INT,
                    Rendimento DECIMAL(5,2),
                    Observacoes TEXT,
                    Operador VARCHAR(100),
                    FOREIGN KEY (IdBloco) REFERENCES Blocos(Id)
                )";

            connection.Execute(createUsuariosTable);
            connection.Execute(createPedreirasTable);
            connection.Execute(createBlocosTable);
            connection.Execute(createChapasTable);
            connection.Execute(createSerragemBlocosTable);
        }

        private static void InsertAdminUser(MySqlConnection connection)
        {
            var insertAdmin = @"
                INSERT IGNORE INTO Usuarios (Nome, Login, Senha, Email, Ativo) 
                VALUES ('Administrador', 'admin', 'admin', 'admin@louzadagranitos.com', TRUE)";
            
            connection.Execute(insertAdmin);
        }

        public static void LimparERecriarAdmin(string connectionString)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            // Limpar todos os usuários
            connection.Execute("DELETE FROM Usuarios");
            
            // Recriar usuário admin
            InsertAdminUser(connection);
        }

        public static void ListarUsuarios(string connectionString)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            var usuarios = connection.Query("SELECT * FROM Usuarios");
            
            Console.WriteLine("\n📋 **Usuários Cadastrados:**");
            Console.WriteLine("┌─────┬─────────────────┬─────────────┬─────────────────────────┬─────────┐");
            Console.WriteLine("│ ID  │ Nome            │ Login       │ Email                   │ Ativo   │");
            Console.WriteLine("├─────┼─────────────────┼─────────────┼─────────────────────────┼─────────┤");
            
            foreach (var usuario in usuarios)
            {
                Console.WriteLine($"│ {usuario.Id,-3} │ {usuario.Nome,-15} │ {usuario.Login,-11} │ {usuario.Email,-23} │ {(usuario.Ativo ? "✅" : "❌"),-7} │");
            }
            
            Console.WriteLine("└─────┴─────────────────┴─────────────┴─────────────────────────┴─────────┘");
        }
    }
} 