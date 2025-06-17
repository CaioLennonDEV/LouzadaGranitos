using LouzadaGranitos.Core.Models;
using LouzadaGranitos.Core.Interfaces;
using LouzadaGranitos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LouzadaGranitos.ConsoleApp
{
    public class ConsoleApplication
    {
        private readonly AutenticacaoService _autenticacaoService;
        private readonly CadastroService _cadastroService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPedreiraRepository _pedreiraRepository;
        private readonly IBlocoRepository _blocoRepository;
        private readonly IChapaRepository _chapaRepository;
        private readonly ISerragemBlocoRepository _serragemRepository;

        public ConsoleApplication(AutenticacaoService autenticacaoService, CadastroService cadastroService, IUsuarioRepository usuarioRepository, IPedreiraRepository pedreiraRepository, IBlocoRepository blocoRepository, IChapaRepository chapaRepository, ISerragemBlocoRepository serragemRepository)
        {
            _autenticacaoService = autenticacaoService;
            _cadastroService = cadastroService;
            _usuarioRepository = usuarioRepository;
            _pedreiraRepository = pedreiraRepository;
            _blocoRepository = blocoRepository;
            _chapaRepository = chapaRepository;
            _serragemRepository = serragemRepository;
        }

        public async Task RunAsync()
        {
            Console.Title = "🏔️ LOUZADA GRANITOS - Sistema de Gestão 🏔️";
            Console.CursorVisible = false;
            
            await ExibirTelaInicial();
            
            var usuario = await AutenticarUsuario();
            if (usuario == null)
            {
                await ExibirMensagemDespedida();
                return;
            }

            await ExibirMenuPrincipal(usuario);
        }

        private async Task ExibirTelaInicial()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            // Logo animado
            string[] logo = {
                "╔══════════════════════════════════════════════════════════════════════════════╗",
                "║                                                                              ║",
                "║  ██╗     ██████╗ ██╗   ██╗███████╗██████╗  █████╗ ██████╗  █████╗ ███████╗ ║",
                "║  ██║    ██╔═══██╗██║   ██║╚══███╔╝██╔══██╗██╔══██╗██╔══██╗██╔══██╗██╔════╝ ║",
                "║  ██║    ██║   ██║██║   ██║  ███╔╝ ██║  ██║███████║██║  ██║███████║███████╗ ║",
                "║  ██║    ██║   ██║██║   ██║ ███╔╝  ██║  ██║██╔══██║██║  ██║██╔══██║╚════██║ ║",
                "║  ███████╗╚██████╔╝╚██████╔╝███████╗██████╔╝██║  ██║██████╔╝██║  ██║███████║ ║",
                "║  ╚══════╝ ╚═════╝  ╚═════╝ ╚══════╝╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝╚══════╝ ║",
                "║                                                                              ║",
                "║                    🏔️  SISTEMA DE GESTÃO DE GRANITOS 🏔️                    ║",
                "║                                                                              ║",
                "║                        💎 Qualidade e Excelência 💎                        ║",
                "║                                                                              ║",
                "╚══════════════════════════════════════════════════════════════════════════════╝"
            };

            for (int i = 0; i < logo.Length; i++)
            {
                Console.SetCursorPosition(0, i + 2);
                Console.WriteLine(logo[i]);
                await Task.Delay(100);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, logo.Length + 4);
            Console.WriteLine("🎯 Carregando sistema...");
            
            for (int i = 0; i < 50; i++)
            {
                Console.SetCursorPosition(0, logo.Length + 5);
                Console.Write("[");
                for (int j = 0; j < i; j++) Console.Write("█");
                for (int j = i; j < 50; j++) Console.Write(" ");
                Console.Write($"] {i * 2}%");
                await Task.Delay(50);
            }
            
            Console.WriteLine("\n✅ Sistema carregado com sucesso!");
            await Task.Delay(1000);
        }

        private async Task<Usuario?> AutenticarUsuario()
        {
            int tentativas = 0;
            const int maxTentativas = 3;

            while (tentativas < maxTentativas)
            {
                Console.Clear();
                await ExibirCabecalho("🔐 AUTENTICAÇÃO");
                
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                              🔐 LOGIN DO SISTEMA                            ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("👤 Usuário: ");
                Console.ForegroundColor = ConsoleColor.White;
                var usuario = Console.ReadLine() ?? "";

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("🔒 Senha: ");
                Console.ForegroundColor = ConsoleColor.White;
                var senha = LerSenha();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("🔄 Verificando credenciais...");
                
                var usuarioAutenticado = await _autenticacaoService.AutenticarAsync(usuario, senha);
                
                if (usuarioAutenticado != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" ✅");
                    Console.WriteLine($"🎉 Bem-vindo(a), {usuarioAutenticado.Nome}!");
                    await Task.Delay(1500);
                    return usuarioAutenticado;
                }
                else
                {
                    tentativas++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" ❌");
                    Console.WriteLine($"❌ Credenciais inválidas! Tentativa {tentativas} de {maxTentativas}");
                    Console.WriteLine("⚠️  Verifique seu usuário e senha.");
                    await Task.Delay(2000);
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n🚫 Número máximo de tentativas excedido. Sistema será encerrado.");
            await Task.Delay(3000);
            return null;
        }

        private string LerSenha()
        {
            var senha = "";
            ConsoleKeyInfo key;
            
            do
            {
                key = Console.ReadKey(true);
                
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    senha += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && senha.Length > 0)
                {
                    senha = senha.Substring(0, senha.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            
            Console.WriteLine();
            return senha;
        }

        private async Task ExibirMenuPrincipal(Usuario usuario)
        {
            while (true)
            {
                Console.Clear();
                await ExibirCabecalho("🏠 MENU PRINCIPAL");
                
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"👤 Usuário logado: {usuario.Nome}");
                Console.WriteLine($"🕒 Último acesso: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                Console.WriteLine();
                
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                              🎯 OPÇÕES DISPONÍVEIS                           ║");
                Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║  🏔️  1. GESTÃO DE PEDREIRAS    │  🪨  2. GESTÃO DE BLOCOS                    ║");
                Console.WriteLine("║  🔄  3. PROCESSAMENTO         │  📋  4. GESTÃO DE CHAPAS                    ║");
                Console.WriteLine("║  👥  5. GESTÃO DE USUÁRIOS    │  📊  6. RELATÓRIOS                          ║");
                Console.WriteLine("║  🚪  0. SAIR DO SISTEMA                                                      ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("🎯 Escolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;
                var opcao = Console.ReadLine();

                Console.WriteLine();
                
                switch (opcao)
                {
                    case "1":
                        await MenuPedreiras();
                        break;
                    case "2":
                        await MenuBlocos();
                        break;
                    case "3":
                        await MenuSerragem();
                        break;
                    case "4":
                        await MenuChapas();
                        break;
                    case "5":
                        await MenuUsuarios();
                        break;
                    case "6":
                        await MenuRelatorios();
                        break;
                    case "0":
                        await ExibirMensagemDespedida();
                        return;
                    default:
                        await ExibirErro("Opção inválida! Tente novamente.");
                        break;
                }
            }
        }

        private async Task ExibirCabecalho(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║  {titulo.PadRight(70)}  ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        private async Task ExibirErro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {mensagem}");
            Console.ResetColor();
            await Task.Delay(2000);
        }

        private async Task ExibirSucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ {mensagem}");
            Console.ResetColor();
            await Task.Delay(1500);
        }

        private async Task ExibirMensagemDespedida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            string[] despedida = {
                "╔══════════════════════════════════════════════════════════════════════════════╗",
                "║                                                                              ║",
                "║                           👋 OBRIGADO POR USAR! 👋                          ║",
                "║                                                                              ║",
                "║                    🏔️ LOUZADA GRANITOS - Sistema de Gestão 🏔️              ║",
                "║                                                                              ║",
                "║                              💎 Qualidade e Excelência 💎                  ║",
                "║                                                                              ║",
                "║                              🚀 Sistema encerrado com sucesso! 🚀           ║",
                "║                                                                              ║",
                "╚══════════════════════════════════════════════════════════════════════════════╝"
            };

            for (int i = 0; i < despedida.Length; i++)
            {
                Console.SetCursorPosition(0, i + 5);
                Console.WriteLine(despedida[i]);
                await Task.Delay(200);
            }
            
            await Task.Delay(3000);
        }

        private async Task MenuPedreiras()
        {
            while (true)
            {
                Console.Clear();
                await ExibirCabecalho("🏔️ GESTÃO DE PEDREIRAS");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║  1. ➕ Cadastrar Nova Pedreira                                               ║");
                Console.WriteLine("║  2. 📝 Listar Pedreiras                                                     ║");
                Console.WriteLine("║  3. ✏️  Editar Pedreira                                                     ║");
                Console.WriteLine("║  4. 🗑️  Excluir Pedreira                                                    ║");
                Console.WriteLine("║  0. 🔙 Voltar ao Menu Principal                                             ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("🎯 Escolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;
                var opcao = Console.ReadLine();
                Console.WriteLine();
                switch (opcao)
                {
                    case "1":
                        await CadastrarPedreira();
                        break;
                    case "2":
                        await ListarPedreiras();
                        break;
                    case "3":
                        await EditarPedreira();
                        break;
                    case "4":
                        await ExcluirPedreira();
                        break;
                    case "0":
                        return;
                    default:
                        await ExibirErro("Opção inválida! Tente novamente.");
                        break;
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("⏸️  Pressione qualquer tecla para continuar...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        private async Task CadastrarPedreira()
        {
            Console.WriteLine("\n➕ **CADASTRAR NOVA PEDREIRA** ➕");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            Console.Write("🏔️  Nome da Pedreira: ");
            var nome = Console.ReadLine() ?? "";

            Console.Write("📍 Localização: ");
            var localizacao = Console.ReadLine() ?? "";

            Console.Write("👤 Proprietário: ");
            var proprietario = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("\n❌ **Erro:** Nome da pedreira é obrigatório!");
                return;
            }

            var pedreira = new Pedreira
            {
                Nome = nome,
                Localizacao = localizacao,
                Proprietario = proprietario,
                DataCadastro = DateTime.Now,
                Ativa = true
            };

            var sucesso = await _cadastroService.CadastrarPedreiraAsync(pedreira);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Pedreira cadastrada com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao cadastrar pedreira!** ❌");
            }
        }

        private async Task ListarPedreiras()
        {
            Console.WriteLine("\n📝 **LISTA DE PEDREIRAS** 📝");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var pedreiras = await _cadastroService.ListarPedreiras();
            
            if (!pedreiras.Any())
            {
                Console.WriteLine("📭 Nenhuma pedreira cadastrada.");
                return;
            }

            Console.WriteLine("┌─────┬─────────────────┬─────────────────────────┬─────────────────┬─────────┬─────────────────┐");
            Console.WriteLine("│ ID  │ Nome            │ Localização             │ Proprietário    │ Ativa   │ Data Cadastro   │");
            Console.WriteLine("├─────┼─────────────────┼─────────────────────────┼─────────────────┼─────────┼─────────────────┤");
            
            foreach (var pedreira in pedreiras)
            {
                Console.WriteLine($"│ {pedreira.Id,-3} │ {pedreira.Nome,-15} │ {pedreira.Localizacao,-23} │ {pedreira.Proprietario,-15} │ {(pedreira.Ativa ? "✅" : "❌"),-7} │ {pedreira.DataCadastro:dd/MM/yyyy} │");
            }
            
            Console.WriteLine("└─────┴─────────────────┴─────────────────────────┴─────────────────┴─────────┴─────────────────┘");
        }

        private async Task EditarPedreira()
        {
            Console.WriteLine("\n✏️  **EDITAR PEDREIRA** ✏️");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            await ListarPedreiras();

            Console.Write("\n🎯 Digite o ID da pedreira a ser editada: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ **ID inválido!** ❌");
                return;
            }

            var pedreiras = await _cadastroService.ListarPedreiras();
            var pedreira = pedreiras.FirstOrDefault(p => p.Id == id);
            
            if (pedreira == null)
            {
                Console.WriteLine("\n❌ **Pedreira não encontrada!** ❌");
                return;
            }

            Console.WriteLine($"\n📝 Editando pedreira: {pedreira.Nome}");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            Console.Write($"🏔️  Nome ({pedreira.Nome}): ");
            var nome = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nome))
                pedreira.Nome = nome;

            Console.Write($"📍 Localização ({pedreira.Localizacao}): ");
            var localizacao = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(localizacao))
                pedreira.Localizacao = localizacao;

            Console.Write($"👤 Proprietário ({pedreira.Proprietario}): ");
            var proprietario = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(proprietario))
                pedreira.Proprietario = proprietario;

            Console.Write($"✅ Ativa (S/N) [{(pedreira.Ativa ? "S" : "N")}]: ");
            var ativa = Console.ReadLine()?.ToUpper();
            if (ativa == "S" || ativa == "N")
                pedreira.Ativa = ativa == "S";

            var sucesso = await _pedreiraRepository.UpdateAsync(pedreira);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Pedreira atualizada com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao atualizar pedreira!** ❌");
            }
        }

        private async Task ExcluirPedreira()
        {
            Console.WriteLine("\n🗑️  **EXCLUIR PEDREIRA** 🗑️");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            await ListarPedreiras();

            Console.Write("\n🎯 Digite o ID da pedreira a ser excluída: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ **ID inválido!** ❌");
                return;
            }

            var pedreiras = await _cadastroService.ListarPedreiras();
            var pedreira = pedreiras.FirstOrDefault(p => p.Id == id);
            
            if (pedreira == null)
            {
                Console.WriteLine("\n❌ **Pedreira não encontrada!** ❌");
                return;
            }

            Console.WriteLine($"\n⚠️  **ATENÇÃO:** Você está prestes a excluir a pedreira:");
            Console.WriteLine($"   🏔️  Nome: {pedreira.Nome}");
            Console.WriteLine($"   📍 Localização: {pedreira.Localizacao}");
            Console.WriteLine($"   👤 Proprietário: {pedreira.Proprietario}");
            
            Console.Write("\n🤔 Tem certeza? (S/N): ");
            var confirmacao = Console.ReadLine()?.ToUpper();
            
            if (confirmacao != "S")
            {
                Console.WriteLine("\n❌ **Operação cancelada!** ❌");
                return;
            }

            var sucesso = await _pedreiraRepository.DeleteAsync(id);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Pedreira excluída com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao excluir pedreira!** ❌");
            }
        }

        private async Task MenuBlocos()
        {
            while (true)
            {
                Console.Clear();
                await ExibirCabecalho("🪨 GESTÃO DE BLOCOS");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║  1. ➕ Cadastrar Novo Bloco                                                  ║");
                Console.WriteLine("║  2. 📝 Listar Blocos                                                         ║");
                Console.WriteLine("║  3. ✏️  Editar Bloco                                                         ║");
                Console.WriteLine("║  4. 🗑️  Excluir Bloco                                                        ║");
                Console.WriteLine("║  0. 🔙 Voltar ao Menu Principal                                              ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("🎯 Escolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;
                var opcao = Console.ReadLine();
                Console.WriteLine();
                switch (opcao)
                {
                    case "1":
                        await CadastrarBloco();
                        break;
                    case "2":
                        await ListarBlocos();
                        break;
                    case "3":
                        await EditarBloco();
                        break;
                    case "4":
                        await ExcluirBloco();
                        break;
                    case "0":
                        return;
                    default:
                        await ExibirErro("Opção inválida! Tente novamente.");
                        break;
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("⏸️  Pressione qualquer tecla para continuar...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        private async Task CadastrarBloco()
        {
            Console.WriteLine("\n➕ **CADASTRAR NOVO BLOCO** ➕");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            // Listar pedreiras disponíveis
            var pedreiras = await _cadastroService.ListarPedreiras();
            if (!pedreiras.Any())
            {
                Console.WriteLine("\n❌ **Erro:** Não há pedreiras cadastradas. Cadastre uma pedreira primeiro!");
                return;
            }

            Console.WriteLine("\n🏔️  **Pedreiras disponíveis:**");
            foreach (var p in pedreiras)
            {
                Console.WriteLine($"   ID: {p.Id} - {p.Nome} ({p.Localizacao})");
            }

            Console.Write("\n🎯 ID da Pedreira: ");
            if (!int.TryParse(Console.ReadLine(), out int pedreiraId))
            {
                Console.WriteLine("\n❌ **ID inválido!** ❌");
                return;
            }

            var pedreira = pedreiras.FirstOrDefault(p => p.Id == pedreiraId);
            if (pedreira == null)
            {
                Console.WriteLine("\n❌ **Pedreira não encontrada!** ❌");
                return;
            }

            Console.Write("🪨 Código do Bloco: ");
            var codigo = Console.ReadLine() ?? "";

            Console.Write("📏 Comprimento (metros): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal comprimento))
            {
                Console.WriteLine("\n❌ **Comprimento inválido!** ❌");
                return;
            }

            Console.Write("📏 Largura (metros): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal largura))
            {
                Console.WriteLine("\n❌ **Largura inválida!** ❌");
                return;
            }

            Console.Write("📏 Altura (metros): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal altura))
            {
                Console.WriteLine("\n❌ **Altura inválida!** ❌");
                return;
            }

            Console.Write("⚖️  Peso (toneladas): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal peso))
            {
                Console.WriteLine("\n❌ **Peso inválido!** ❌");
                return;
            }

            Console.Write("🎨 Tipo de Granito: ");
            var tipoGranito = Console.ReadLine() ?? "";

            Console.Write("💰 Preço: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal preco))
            {
                Console.WriteLine("\n❌ **Preço inválido!** ❌");
                return;
            }

            if (string.IsNullOrWhiteSpace(codigo))
            {
                Console.WriteLine("\n❌ **Erro:** Código do bloco é obrigatório!");
                return;
            }

            var bloco = new Bloco
            {
                IdPedreira = pedreiraId,
                Codigo = codigo,
                Comprimento = comprimento,
                Largura = largura,
                Altura = altura,
                Peso = peso,
                TipoGranito = tipoGranito,
                DataExtracao = DateTime.Now,
                Status = "Disponível",
                Preco = preco
            };

            var sucesso = await _cadastroService.CadastrarBlocoAsync(bloco);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Bloco cadastrado com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao cadastrar bloco!** ❌");
            }
        }

        private async Task ListarBlocos()
        {
            Console.WriteLine("\n📝 **LISTA DE BLOCOS** 📝");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var blocos = await _cadastroService.ListarBlocos();
            
            if (!blocos.Any())
            {
                Console.WriteLine("📭 Nenhum bloco cadastrado.");
                return;
            }

            Console.WriteLine("┌─────┬─────────────┬─────────────┬─────────┬─────────┬─────────┬─────────┬─────────────┬─────────┬─────────────────┐");
            Console.WriteLine("│ ID  │ Código      │ Pedreira    │ Comp.   │ Larg.   │ Altura  │ Peso    │ Tipo Granito│ Status  │ Data Extração   │");
            Console.WriteLine("├─────┼─────────────┼─────────────┼─────────┼─────────┼─────────┼─────────┼─────────────┼─────────┼─────────────────┤");
            
            foreach (var bloco in blocos)
            {
                var pedreiras = await _cadastroService.ListarPedreiras();
                var pedreira = pedreiras.FirstOrDefault(p => p.Id == bloco.IdPedreira);
                var nomePedreira = pedreira?.Nome ?? "N/A";
                
                Console.WriteLine($"│ {bloco.Id,-3} │ {bloco.Codigo,-11} │ {nomePedreira,-11} │ {bloco.Comprimento,-7:F2} │ {bloco.Largura,-7:F2} │ {bloco.Altura,-7:F2} │ {bloco.Peso,-7:F2} │ {bloco.TipoGranito,-11} │ {bloco.Status,-9} │ {bloco.DataExtracao:dd/MM/yyyy} │");
            }
            
            Console.WriteLine("└─────┴─────────────┴─────────────┴─────────┴─────────┴─────────┴─────────┴─────────────┴─────────┴─────────────────┘");
        }

        private async Task EditarBloco()
        {
            Console.WriteLine("\n✏️  **EDITAR BLOCO** ✏️");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            await ListarBlocos();

            Console.Write("\n🎯 Digite o ID do bloco a ser editado: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ **ID inválido!** ❌");
                return;
            }

            var blocos = await _cadastroService.ListarBlocos();
            var blocoParaEditar = blocos.FirstOrDefault(b => b.Id == id);
            
            if (blocoParaEditar == null)
            {
                Console.WriteLine("\n❌ **Bloco não encontrado!** ❌");
                return;
            }

            Console.WriteLine($"\n📝 Editando bloco: {blocoParaEditar.Codigo}");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            Console.Write($"🪨 Código ({blocoParaEditar.Codigo}): ");
            var codigo = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(codigo))
                blocoParaEditar.Codigo = codigo;

            Console.Write($"📏 Comprimento ({blocoParaEditar.Comprimento:F2}): ");
            var comprimentoStr = Console.ReadLine();
            if (decimal.TryParse(comprimentoStr, out decimal comprimento))
                blocoParaEditar.Comprimento = comprimento;

            Console.Write($"📏 Largura ({blocoParaEditar.Largura:F2}): ");
            var larguraStr = Console.ReadLine();
            if (decimal.TryParse(larguraStr, out decimal largura))
                blocoParaEditar.Largura = largura;

            Console.Write($"📏 Altura ({blocoParaEditar.Altura:F2}): ");
            var alturaStr = Console.ReadLine();
            if (decimal.TryParse(alturaStr, out decimal altura))
                blocoParaEditar.Altura = altura;

            Console.Write($"⚖️  Peso ({blocoParaEditar.Peso:F2}): ");
            var pesoStr = Console.ReadLine();
            if (decimal.TryParse(pesoStr, out decimal peso))
                blocoParaEditar.Peso = peso;

            Console.Write($"🎨 Tipo de Granito ({blocoParaEditar.TipoGranito}): ");
            var tipoGranito = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tipoGranito))
                blocoParaEditar.TipoGranito = tipoGranito;

            Console.Write($"💰 Preço ({blocoParaEditar.Preco:F2}): ");
            var precoStr = Console.ReadLine();
            if (decimal.TryParse(precoStr, out decimal preco))
                blocoParaEditar.Preco = preco;

            Console.Write($"📊 Status ({blocoParaEditar.Status}): ");
            var status = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(status))
                blocoParaEditar.Status = status;

            var sucesso = await _blocoRepository.UpdateAsync(blocoParaEditar);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Bloco atualizado com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao atualizar bloco!** ❌");
            }
        }

        private async Task ExcluirBloco()
        {
            Console.WriteLine("\n🗑️  **EXCLUIR BLOCO** 🗑️");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            await ListarBlocos();

            Console.Write("\n🎯 Digite o ID do bloco a ser excluído: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ **ID inválido!** ❌");
                return;
            }

            var blocos = await _cadastroService.ListarBlocos();
            var bloco = blocos.FirstOrDefault(b => b.Id == id);
            
            if (bloco == null)
            {
                Console.WriteLine("\n❌ **Bloco não encontrado!** ❌");
                return;
            }

            Console.WriteLine($"\n⚠️  **ATENÇÃO:** Você está prestes a excluir o bloco:");
            Console.WriteLine($"   🪨 Código: {bloco.Codigo}");
            Console.WriteLine($"   📏 Dimensões: {bloco.Comprimento:F2}m x {bloco.Largura:F2}m x {bloco.Altura:F2}m");
            Console.WriteLine($"   ⚖️  Peso: {bloco.Peso:F2} toneladas");
            Console.WriteLine($"   🎨 Tipo: {bloco.TipoGranito}");
            
            Console.Write("\n🤔 Tem certeza? (S/N): ");
            var confirmacao = Console.ReadLine()?.ToUpper();
            
            if (confirmacao != "S")
            {
                Console.WriteLine("\n❌ **Operação cancelada!** ❌");
                return;
            }

            var sucesso = await _blocoRepository.DeleteAsync(id);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Bloco excluído com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao excluir bloco!** ❌");
            }
        }

        private async Task MenuSerragem()
        {
            while (true)
            {
                Console.Clear();
                await ExibirCabecalho("🔄 PROCESSAMENTO DE SERRAGEM");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║  1. 🪚 Processar Bloco em Chapas                                            ║");
                Console.WriteLine("║  2. 📝 Listar Processamentos                                                ║");
                Console.WriteLine("║  0. 🔙 Voltar ao Menu Principal                                             ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("🎯 Escolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;
                var opcao = Console.ReadLine();
                Console.WriteLine();
                switch (opcao)
                {
                    case "1":
                        await ProcessarSerragem();
                        break;
                    case "2":
                        await ListarProcessamentos();
                        break;
                    case "0":
                        return;
                    default:
                        await ExibirErro("Opção inválida! Tente novamente.");
                        break;
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("⏸️  Pressione qualquer tecla para continuar...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        private async Task ProcessarSerragem()
        {
            Console.WriteLine("\n🔄 **PROCESSAR BLOCO EM CHAPAS** 🔄");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            // Listar blocos disponíveis
            var blocos = await _cadastroService.ListarBlocos();
            var blocosDisponiveis = blocos.Where(b => b.Status == "Disponível").ToList();
            
            if (!blocosDisponiveis.Any())
            {
                Console.WriteLine("\n❌ **Erro:** Não há blocos disponíveis para processamento!");
                return;
            }

            Console.WriteLine("\n🪨 **Blocos disponíveis:**");
            foreach (var b in blocosDisponiveis)
            {
                var volume = b.Comprimento * b.Largura * b.Altura;
                Console.WriteLine($"   ID: {b.Id} - {b.Codigo} ({b.TipoGranito}) - {volume:F2}m³");
            }

            Console.Write("\n🎯 ID do Bloco a ser processado: ");
            if (!int.TryParse(Console.ReadLine(), out int blocoId))
            {
                Console.WriteLine("\n❌ **ID inválido!** ❌");
                return;
            }

            var bloco = blocosDisponiveis.FirstOrDefault(b => b.Id == blocoId);
            if (bloco == null)
            {
                Console.WriteLine("\n❌ **Bloco não encontrado ou não disponível!** ❌");
                return;
            }

            Console.WriteLine($"\n📋 **Configuração da Serragem para Bloco {bloco.Codigo}:**");
            Console.WriteLine($"   📏 Dimensões: {bloco.Comprimento:F2}m x {bloco.Largura:F2}m x {bloco.Altura:F2}m");
            Console.WriteLine($"   ⚖️  Peso: {bloco.Peso:F2} toneladas");
            Console.WriteLine($"   🎨 Tipo: {bloco.TipoGranito}");

            Console.Write("\n📏 Espessura das chapas (centímetros): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal espessuraChapa))
            {
                Console.WriteLine("\n❌ **Espessura inválida!** ❌");
                return;
            }

            Console.Write("🎯 Tipo de Serragem: ");
            var tipoSerragem = Console.ReadLine() ?? "";

            Console.Write("👤 Operador: ");
            var operador = Console.ReadLine() ?? "";

            Console.Write("📝 Observações: ");
            var observacoes = Console.ReadLine() ?? "";

            // Calcular número de chapas
            var espessuraMetros = espessuraChapa / 100; // Converter cm para metros
            var numeroChapas = (int)(bloco.Altura / espessuraMetros);
            
            if (numeroChapas <= 0)
            {
                Console.WriteLine("\n❌ **Erro:** Espessura muito grande para o bloco!");
                return;
            }

            // Calcular rendimento
            var areaTotal = bloco.Comprimento * bloco.Largura * numeroChapas;
            var rendimento = (areaTotal / (bloco.Comprimento * bloco.Largura * bloco.Altura)) * 100;

            Console.WriteLine($"\n📊 **Resumo do Processamento:**");
            Console.WriteLine($"   📋 Número de chapas: {numeroChapas}");
            Console.WriteLine($"   📏 Dimensões das chapas: {bloco.Comprimento:F2}m x {bloco.Largura:F2}m x {espessuraChapa:F1}cm");
            Console.WriteLine($"   📐 Área total: {areaTotal:F2}m²");
            Console.WriteLine($"   📈 Rendimento: {rendimento:F1}%");

            Console.Write("\n🤔 Confirmar processamento? (S/N): ");
            var confirmacao = Console.ReadLine()?.ToUpper();
            
            if (confirmacao != "S")
            {
                Console.WriteLine("\n❌ **Processamento cancelado!** ❌");
                return;
            }

            // Criar registro de serragem
            var serragem = new SerragemBloco
            {
                IdBloco = blocoId,
                DataSerragem = DateTime.Now,
                TipoSerragem = tipoSerragem,
                EspessuraChapa = espessuraChapa,
                QuantidadeChapas = numeroChapas,
                Rendimento = rendimento,
                Observacoes = observacoes,
                Operador = operador
            };

            // Criar chapas
            var chapas = new List<Chapa>();
            for (int i = 1; i <= numeroChapas; i++)
            {
                var chapa = new Chapa
                {
                    Codigo = $"{bloco.Codigo}-CH{i:D3}",
                    IdBloco = blocoId,
                    Comprimento = bloco.Comprimento,
                    Largura = bloco.Largura,
                    Espessura = espessuraChapa,
                    Area = bloco.Comprimento * bloco.Largura,
                    TipoGranito = bloco.TipoGranito,
                    DataSerragem = DateTime.Now,
                    Status = "Disponível",
                    Preco = bloco.Preco / numeroChapas // Distribuir preço entre as chapas
                };
                chapas.Add(chapa);
            }

            // Processar serragem
            var sucesso = await _cadastroService.ProcessarSerragemAsync(serragem, chapas);
            
            if (sucesso)
            {
                // Marcar bloco como processado
                bloco.Status = "Processado";
                await _blocoRepository.UpdateAsync(bloco);

                Console.WriteLine("\n✅ **Processamento concluído com sucesso!** ✅");
                Console.WriteLine($"   📋 {numeroChapas} chapas foram criadas");
                Console.WriteLine($"   🪨 Bloco {bloco.Codigo} marcado como processado");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro no processamento!** ❌");
            }
        }

        private async Task ListarProcessamentos()
        {
            Console.WriteLine("\n📝 **LISTA DE PROCESSAMENTOS** 📝");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var serragens = await _serragemRepository.GetAllAsync();
            
            if (!serragens.Any())
            {
                Console.WriteLine("📭 Nenhum processamento registrado.");
                return;
            }

            Console.WriteLine("┌─────┬─────────────┬─────────────┬─────────────┬─────────────┬─────────────┬─────────────────┐");
            Console.WriteLine("│ ID  │ Bloco       │ Espessura   │ Nº Chapas   │ Rendimento  │ Data        │ Operador        │");
            Console.WriteLine("├─────┼─────────────┼─────────────┼─────────────┼─────────────┼─────────────┼─────────────────┤");
            
            foreach (var serragem in serragens)
            {
                var blocos = await _cadastroService.ListarBlocos();
                var bloco = blocos.FirstOrDefault(b => b.Id == serragem.IdBloco);
                var codigoBloco = bloco?.Codigo ?? "N/A";
                
                Console.WriteLine($"│ {serragem.Id,-3} │ {codigoBloco,-11} │ {serragem.EspessuraChapa,-11:F1} │ {serragem.QuantidadeChapas,-11} │ {serragem.Rendimento,-11:F1}% │ {serragem.DataSerragem:dd/MM/yyyy} │ {serragem.Operador,-15} │");
            }
            
            Console.WriteLine("└─────┴─────────────┴─────────────┴─────────────┴─────────────┴─────────────┴─────────────────┘");
        }

        private async Task RelatorioProducao()
        {
            Console.WriteLine("\n📊 **RELATÓRIO DE PRODUÇÃO** 📊");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var serragens = await _serragemRepository.GetAllAsync();
            var blocos = await _cadastroService.ListarBlocos();
            var chapas = await _cadastroService.ListarChapas();

            if (!serragens.Any())
            {
                Console.WriteLine("📭 Nenhum processamento registrado.");
                return;
            }

            // Estatísticas gerais
            var totalProcessamentos = serragens.Count();
            var totalChapas = serragens.Sum(s => s.QuantidadeChapas);
            var areaTotal = chapas.Sum(c => c.Area);
            var valorTotal = chapas.Sum(c => c.Preco);

            Console.WriteLine($"📈 **Estatísticas Gerais:**");
            Console.WriteLine($"   🔄 Total de processamentos: {totalProcessamentos}");
            Console.WriteLine($"   📋 Total de chapas produzidas: {totalChapas}");
            Console.WriteLine($"   📐 Área total produzida: {areaTotal:F2}m²");
            Console.WriteLine($"   💰 Valor total estimado: R$ {valorTotal:F2}");
            Console.WriteLine();

            // Processamentos por tipo de granito
            Console.WriteLine($"🎨 **Produção por Tipo de Granito:**");
            var producaoPorTipo = chapas
                .GroupBy(c => c.TipoGranito)
                .Select(g => new
                {
                    Tipo = g.Key,
                    Quantidade = g.Count(),
                    Area = g.Sum(c => c.Area),
                    Valor = g.Sum(c => c.Preco)
                })
                .OrderByDescending(x => x.Quantidade);

            foreach (var tipo in producaoPorTipo)
            {
                Console.WriteLine($"   {tipo.Tipo}: {tipo.Quantidade} chapas, {tipo.Area:F2}m², R$ {tipo.Valor:F2}");
            }
            Console.WriteLine();

            // Últimos processamentos
            Console.WriteLine($"🕒 **Últimos 5 Processamentos:**");
            var ultimosProcessamentos = serragens.OrderByDescending(s => s.DataSerragem).Take(5);
            
            foreach (var serragem in ultimosProcessamentos)
            {
                var bloco = blocos.FirstOrDefault(b => b.Id == serragem.IdBloco);
                var codigoBloco = bloco?.Codigo ?? "N/A";
                
                Console.WriteLine($"   {serragem.DataSerragem:dd/MM/yyyy HH:mm} - Bloco {codigoBloco}: {serragem.QuantidadeChapas} chapas");
            }
        }

        private async Task MenuRelatorios()
        {
            while (true)
            {
                Console.Clear();
                await ExibirCabecalho("📊 RELATÓRIOS E ESTATÍSTICAS");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║  1. 📈 Relatório de Produção                                                ║");
                Console.WriteLine("║  2. 💰 Relatório Financeiro                                                 ║");
                Console.WriteLine("║  3. 🏔️  Relatório de Pedreiras                                              ║");
                Console.WriteLine("║  4. 🪨 Relatório de Blocos                                                   ║");
                Console.WriteLine("║  5. 📋 Relatório de Chapas                                                  ║");
                Console.WriteLine("║  6. 📊 Dashboard Geral                                                      ║");
                Console.WriteLine("║  0. 🔙 Voltar ao Menu Principal                                             ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("🎯 Escolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;
                var opcao = Console.ReadLine();
                Console.WriteLine();
                switch (opcao)
                {
                    case "1":
                        await RelatorioProducao();
                        break;
                    case "2":
                        await RelatorioFinanceiro();
                        break;
                    case "3":
                        await RelatorioPedreiras();
                        break;
                    case "4":
                        await RelatorioBlocos();
                        break;
                    case "5":
                        await RelatorioChapas();
                        break;
                    case "6":
                        await DashboardGeral();
                        break;
                    case "0":
                        return;
                    default:
                        await ExibirErro("Opção inválida! Tente novamente.");
                        break;
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("⏸️  Pressione qualquer tecla para continuar...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        private async Task RelatorioFinanceiro()
        {
            Console.WriteLine("\n💰 **RELATÓRIO FINANCEIRO** 💰");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var blocos = await _cadastroService.ListarBlocos();
            var chapas = await _cadastroService.ListarChapas();
            var serragens = await _serragemRepository.GetAllAsync();

            // Valores dos blocos
            var valorTotalBlocos = blocos.Sum(b => b.Preco);
            var valorBlocosDisponiveis = blocos.Where(b => b.Status == "Disponível").Sum(b => b.Preco);
            var valorBlocosProcessados = blocos.Where(b => b.Status == "Processado").Sum(b => b.Preco);

            // Valores das chapas
            var valorTotalChapas = chapas.Sum(c => c.Preco);
            var valorChapasDisponiveis = chapas.Where(c => c.Status == "Disponível").Sum(c => c.Preco);
            var valorChapasVendidas = chapas.Where(c => c.Status == "Vendida").Sum(c => c.Preco);

            Console.WriteLine($"📊 **Resumo Financeiro:**");
            Console.WriteLine($"   🪨 Valor total em blocos: R$ {valorTotalBlocos:F2}");
            Console.WriteLine($"   ✅ Blocos disponíveis: R$ {valorBlocosDisponiveis:F2}");
            Console.WriteLine($"   🔄 Blocos processados: R$ {valorBlocosProcessados:F2}");
            Console.WriteLine();
            Console.WriteLine($"   📋 Valor total em chapas: R$ {valorTotalChapas:F2}");
            Console.WriteLine($"   ✅ Chapas disponíveis: R$ {valorChapasDisponiveis:F2}");
            Console.WriteLine($"   💰 Chapas vendidas: R$ {valorChapasVendidas:F2}");
            Console.WriteLine();
            Console.WriteLine($"   📈 **Valor Total do Estoque: R$ {(valorBlocosDisponiveis + valorChapasDisponiveis):F2}**");
            Console.WriteLine();

            // Análise por tipo de granito
            Console.WriteLine($"🎨 **Análise por Tipo de Granito:**");
            var analisePorTipo = blocos
                .GroupBy(b => b.TipoGranito)
                .Select(g => new
                {
                    Tipo = g.Key,
                    ValorBlocos = g.Sum(b => b.Preco),
                    QuantidadeBlocos = g.Count(),
                    Chapas = chapas.Where(c => c.TipoGranito == g.Key),
                    ValorChapas = chapas.Where(c => c.TipoGranito == g.Key).Sum(c => c.Preco)
                })
                .OrderByDescending(x => x.ValorBlocos + x.ValorChapas);

            foreach (var tipo in analisePorTipo)
            {
                Console.WriteLine($"   {tipo.Tipo}:");
                Console.WriteLine($"     🪨 Blocos: {tipo.QuantidadeBlocos} un. - R$ {tipo.ValorBlocos:F2}");
                Console.WriteLine($"     📋 Chapas: {tipo.Chapas.Count()} un. - R$ {tipo.ValorChapas:F2}");
                Console.WriteLine($"     💰 Total: R$ {(tipo.ValorBlocos + tipo.ValorChapas):F2}");
                Console.WriteLine();
            }
        }

        private async Task RelatorioPedreiras()
        {
            Console.WriteLine("\n🏔️  **RELATÓRIO DE PEDREIRAS** 🏔️");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var pedreiras = await _cadastroService.ListarPedreiras();
            var blocos = await _cadastroService.ListarBlocos();

            if (!pedreiras.Any())
            {
                Console.WriteLine("📭 Nenhuma pedreira cadastrada.");
                return;
            }

            Console.WriteLine($"📊 **Resumo Geral:**");
            Console.WriteLine($"   🏔️  Total de pedreiras: {pedreiras.Count()}");
            Console.WriteLine($"   ✅ Pedreiras ativas: {pedreiras.Count(p => p.Ativa)}");
            Console.WriteLine($"   ❌ Pedreiras inativas: {pedreiras.Count(p => !p.Ativa)}");
            Console.WriteLine();

            // Análise por pedreira
            Console.WriteLine($"📋 **Análise por Pedreira:**");
            foreach (var pedreira in pedreiras)
            {
                var blocosDaPedreira = blocos.Where(b => b.IdPedreira == pedreira.Id).ToList();
                var volumeTotal = blocosDaPedreira.Sum(b => b.Comprimento * b.Largura * b.Altura);
                var valorTotal = blocosDaPedreira.Sum(b => b.Preco);
                var blocosDisponiveis = blocosDaPedreira.Count(b => b.Status == "Disponível");
                var blocosProcessados = blocosDaPedreira.Count(b => b.Status == "Processado");

                Console.WriteLine($"   🏔️  **{pedreira.Nome}** ({pedreira.Localizacao})");
                Console.WriteLine($"     👤 Proprietário: {pedreira.Proprietario}");
                Console.WriteLine($"     📅 Data de cadastro: {pedreira.DataCadastro:dd/MM/yyyy}");
                Console.WriteLine($"     🪨 Blocos extraídos: {blocosDaPedreira.Count}");
                Console.WriteLine($"     📐 Volume total: {volumeTotal:F2}m³");
                Console.WriteLine($"     💰 Valor total: R$ {valorTotal:F2}");
                Console.WriteLine($"     ✅ Blocos disponíveis: {blocosDisponiveis}");
                Console.WriteLine($"     🔄 Blocos processados: {blocosProcessados}");
                Console.WriteLine($"     📊 Status: {(pedreira.Ativa ? "✅ Ativa" : "❌ Inativa")}");
                Console.WriteLine();
            }
        }

        private async Task RelatorioBlocos()
        {
            Console.WriteLine("\n🪨 **RELATÓRIO DE BLOCOS** 🪨");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var blocos = await _cadastroService.ListarBlocos();
            var pedreiras = await _cadastroService.ListarPedreiras();

            if (!blocos.Any())
            {
                Console.WriteLine("📭 Nenhum bloco cadastrado.");
                return;
            }

            // Estatísticas gerais
            var totalBlocos = blocos.Count();
            var blocosDisponiveis = blocos.Count(b => b.Status == "Disponível");
            var blocosProcessados = blocos.Count(b => b.Status == "Processado");
            var volumeTotal = blocos.Sum(b => b.Comprimento * b.Largura * b.Altura);
            var pesoTotal = blocos.Sum(b => b.Peso);
            var valorTotal = blocos.Sum(b => b.Preco);

            Console.WriteLine($"📊 **Estatísticas Gerais:**");
            Console.WriteLine($"   🪨 Total de blocos: {totalBlocos}");
            Console.WriteLine($"   ✅ Blocos disponíveis: {blocosDisponiveis}");
            Console.WriteLine($"   🔄 Blocos processados: {blocosProcessados}");
            Console.WriteLine($"   📐 Volume total: {volumeTotal:F2}m³");
            Console.WriteLine($"   ⚖️  Peso total: {pesoTotal:F2} toneladas");
            Console.WriteLine($"   💰 Valor total: R$ {valorTotal:F2}");
            Console.WriteLine();

            // Análise por tipo de granito
            Console.WriteLine($"🎨 **Análise por Tipo de Granito:**");
            var analisePorTipo = blocos
                .GroupBy(b => b.TipoGranito)
                .Select(g => new
                {
                    Tipo = g.Key,
                    Quantidade = g.Count(),
                    Volume = g.Sum(b => b.Comprimento * b.Largura * b.Altura),
                    Peso = g.Sum(b => b.Peso),
                    Valor = g.Sum(b => b.Preco),
                    Disponiveis = g.Count(b => b.Status == "Disponível"),
                    Processados = g.Count(b => b.Status == "Processado")
                })
                .OrderByDescending(x => x.Quantidade);

            foreach (var tipo in analisePorTipo)
            {
                Console.WriteLine($"   {tipo.Tipo}:");
                Console.WriteLine($"     📊 Quantidade: {tipo.Quantidade} blocos");
                Console.WriteLine($"     📐 Volume: {tipo.Volume:F2}m³");
                Console.WriteLine($"     ⚖️  Peso: {tipo.Peso:F2} toneladas");
                Console.WriteLine($"     💰 Valor: R$ {tipo.Valor:F2}");
                Console.WriteLine($"     ✅ Disponíveis: {tipo.Disponiveis}");
                Console.WriteLine($"     🔄 Processados: {tipo.Processados}");
                Console.WriteLine();
            }

            // Análise por pedreira
            Console.WriteLine($"🏔️  **Análise por Pedreira:**");
            var analisePorPedreira = blocos
                .GroupBy(b => b.IdPedreira)
                .Select(g => new
                {
                    PedreiraId = g.Key,
                    Quantidade = g.Count(),
                    Volume = g.Sum(b => b.Comprimento * b.Largura * b.Altura),
                    Valor = g.Sum(b => b.Preco)
                })
                .OrderByDescending(x => x.Quantidade);

            foreach (var pedreira in analisePorPedreira)
            {
                var nomePedreira = pedreiras.FirstOrDefault(p => p.Id == pedreira.PedreiraId)?.Nome ?? "N/A";
                Console.WriteLine($"   {nomePedreira}: {pedreira.Quantidade} blocos, {pedreira.Volume:F2}m³, R$ {pedreira.Valor:F2}");
            }
        }

        private async Task RelatorioChapas()
        {
            Console.WriteLine("\n📋 **RELATÓRIO DE CHAPAS** 📋");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var chapas = await _cadastroService.ListarChapas();
            var blocos = await _cadastroService.ListarBlocos();

            if (!chapas.Any())
            {
                Console.WriteLine("📭 Nenhuma chapa cadastrada.");
                return;
            }

            // Estatísticas gerais
            var totalChapas = chapas.Count();
            var chapasDisponiveis = chapas.Count(c => c.Status == "Disponível");
            var chapasVendidas = chapas.Count(c => c.Status == "Vendida");
            var areaTotal = chapas.Sum(c => c.Area);
            var valorTotal = chapas.Sum(c => c.Preco);

            Console.WriteLine($"📊 **Estatísticas Gerais:**");
            Console.WriteLine($"   📋 Total de chapas: {totalChapas}");
            Console.WriteLine($"   ✅ Chapas disponíveis: {chapasDisponiveis}");
            Console.WriteLine($"   💰 Chapas vendidas: {chapasVendidas}");
            Console.WriteLine($"   📐 Área total: {areaTotal:F2}m²");
            Console.WriteLine($"   💰 Valor total: R$ {valorTotal:F2}");
            Console.WriteLine();

            // Análise por tipo de granito
            Console.WriteLine($"🎨 **Análise por Tipo de Granito:**");
            var analisePorTipo = chapas
                .GroupBy(c => c.TipoGranito)
                .Select(g => new
                {
                    Tipo = g.Key,
                    Quantidade = g.Count(),
                    Area = g.Sum(c => c.Area),
                    Valor = g.Sum(c => c.Preco),
                    Disponiveis = g.Count(c => c.Status == "Disponível"),
                    Vendidas = g.Count(c => c.Status == "Vendida")
                })
                .OrderByDescending(x => x.Quantidade);

            foreach (var tipo in analisePorTipo)
            {
                Console.WriteLine($"   {tipo.Tipo}:");
                Console.WriteLine($"     📊 Quantidade: {tipo.Quantidade} chapas");
                Console.WriteLine($"     📐 Área: {tipo.Area:F2}m²");
                Console.WriteLine($"     💰 Valor: R$ {tipo.Valor:F2}");
                Console.WriteLine($"     ✅ Disponíveis: {tipo.Disponiveis}");
                Console.WriteLine($"     💰 Vendidas: {tipo.Vendidas}");
                Console.WriteLine();
            }

            // Análise por bloco de origem
            Console.WriteLine($"🪨 **Análise por Bloco de Origem:**");
            var analisePorBloco = chapas
                .GroupBy(c => c.IdBloco)
                .Select(g => new
                {
                    BlocoId = g.Key,
                    Quantidade = g.Count(),
                    Area = g.Sum(c => c.Area),
                    Valor = g.Sum(c => c.Preco)
                })
                .OrderByDescending(x => x.Quantidade);

            foreach (var bloco in analisePorBloco.Take(10)) // Top 10 blocos
            {
                var codigoBloco = blocos.FirstOrDefault(b => b.Id == bloco.BlocoId)?.Codigo ?? "N/A";
                Console.WriteLine($"   Bloco {codigoBloco}: {bloco.Quantidade} chapas, {bloco.Area:F2}m², R$ {bloco.Valor:F2}");
            }
        }

        private async Task DashboardGeral()
        {
            Console.WriteLine("\n📊 **DASHBOARD GERAL** 📊");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var usuarios = await _cadastroService.ListarUsuarios();
            var pedreiras = await _cadastroService.ListarPedreiras();
            var blocos = await _cadastroService.ListarBlocos();
            var chapas = await _cadastroService.ListarChapas();
            var serragens = await _serragemRepository.GetAllAsync();

            Console.WriteLine($"🎯 **Resumo Executivo:**");
            Console.WriteLine($"   👥 Usuários cadastrados: {usuarios.Count()}");
            Console.WriteLine($"   🏔️  Pedreiras ativas: {pedreiras.Count(p => p.Ativa)}");
            Console.WriteLine($"   🪨 Blocos em estoque: {blocos.Count(b => b.Status == "Disponível")}");
            Console.WriteLine($"   📋 Chapas disponíveis: {chapas.Count(c => c.Status == "Disponível")}");
            Console.WriteLine($"   🔄 Processamentos realizados: {serragens.Count()}");
            Console.WriteLine();

            // Valores
            var valorBlocos = blocos.Where(b => b.Status == "Disponível").Sum(b => b.Preco);
            var valorChapas = chapas.Where(c => c.Status == "Disponível").Sum(c => c.Preco);
            var valorTotal = valorBlocos + valorChapas;

            Console.WriteLine($"💰 **Valores em Estoque:**");
            Console.WriteLine($"   🪨 Blocos: R$ {valorBlocos:F2}");
            Console.WriteLine($"   📋 Chapas: R$ {valorChapas:F2}");
            Console.WriteLine($"   📈 **Total: R$ {valorTotal:F2}**");
            Console.WriteLine();

            // Produção recente
            var ultimasSerragens = serragens.OrderByDescending(s => s.DataSerragem).Take(5);
            if (ultimasSerragens.Any())
            {
                Console.WriteLine($"🕒 **Últimas Produções:**");
                foreach (var serragem in ultimasSerragens)
                {
                    var bloco = blocos.FirstOrDefault(b => b.Id == serragem.IdBloco);
                    var codigoBloco = bloco?.Codigo ?? "N/A";
                    Console.WriteLine($"   {serragem.DataSerragem:dd/MM/yyyy HH:mm} - {codigoBloco}: {serragem.QuantidadeChapas} chapas");
                }
                Console.WriteLine();
            }

            // Alertas
            var alertas = new List<string>();
            
            if (blocos.Count(b => b.Status == "Disponível") < 5)
                alertas.Add("⚠️  Poucos blocos disponíveis para processamento");
            
            if (chapas.Count(c => c.Status == "Disponível") < 10)
                alertas.Add("⚠️  Poucas chapas disponíveis para venda");
            
            if (pedreiras.Count(p => p.Ativa) < 2)
                alertas.Add("⚠️  Poucas pedreiras ativas");

            if (alertas.Any())
            {
                Console.WriteLine($"🚨 **Alertas:**");
                foreach (var alerta in alertas)
                {
                    Console.WriteLine($"   {alerta}");
                }
            }
        }

        private async Task MenuChapas()
        {
            while (true)
            {
                Console.Clear();
                await ExibirCabecalho("📋 GESTÃO DE CHAPAS");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║  1. ➕ Cadastrar Nova Chapa                                                  ║");
                Console.WriteLine("║  2. 📝 Listar Chapas                                                         ║");
                Console.WriteLine("║  3. ✏️  Editar Chapa                                                         ║");
                Console.WriteLine("║  4. 🗑️  Excluir Chapa                                                        ║");
                Console.WriteLine("║  0. 🔙 Voltar ao Menu Principal                                              ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("🎯 Escolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;
                var opcao = Console.ReadLine();
                Console.WriteLine();
                switch (opcao)
                {
                    case "1":
                        await CadastrarChapa();
                        break;
                    case "2":
                        await ListarChapas();
                        break;
                    case "3":
                        await EditarChapa();
                        break;
                    case "4":
                        await ExcluirChapa();
                        break;
                    case "0":
                        return;
                    default:
                        await ExibirErro("Opção inválida! Tente novamente.");
                        break;
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("⏸️  Pressione qualquer tecla para continuar...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        private async Task CadastrarChapa()
        {
            Console.WriteLine("\n➕ **CADASTRAR NOVA CHAPA** ➕");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            Console.Write("📋 Código da Chapa: ");
            var codigo = Console.ReadLine() ?? "";

            Console.Write("🎯 ID do Bloco: ");
            if (!int.TryParse(Console.ReadLine(), out int idBloco))
            {
                Console.WriteLine("\n❌ **ID do bloco inválido!** ❌");
                return;
            }

            Console.Write("📏 Comprimento (metros): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal comprimento))
            {
                Console.WriteLine("\n❌ **Comprimento inválido!** ❌");
                return;
            }

            Console.Write("📏 Largura (metros): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal largura))
            {
                Console.WriteLine("\n❌ **Largura inválida!** ❌");
                return;
            }

            Console.Write("📏 Espessura (centímetros): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal espessura))
            {
                Console.WriteLine("\n❌ **Espessura inválida!** ❌");
                return;
            }

            Console.Write("🎨 Tipo de Granito: ");
            var tipoGranito = Console.ReadLine() ?? "";

            Console.Write("💰 Preço: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal preco))
            {
                Console.WriteLine("\n❌ **Preço inválido!** ❌");
                return;
            }

            if (string.IsNullOrWhiteSpace(codigo))
            {
                Console.WriteLine("\n❌ **Erro:** Código da chapa é obrigatório!");
                return;
            }

            var area = comprimento * largura;

            var chapa = new Chapa
            {
                Codigo = codigo,
                IdBloco = idBloco,
                Comprimento = comprimento,
                Largura = largura,
                Espessura = espessura,
                Area = area,
                TipoGranito = tipoGranito,
                DataSerragem = DateTime.Now,
                Status = "Disponível",
                Preco = preco
            };

            var sucesso = await _chapaRepository.CreateAsync(chapa);
            
            if (sucesso > 0)
            {
                Console.WriteLine("\n✅ **Chapa cadastrada com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao cadastrar chapa!** ❌");
            }
        }

        private async Task ListarChapas()
        {
            Console.WriteLine("\n📝 **LISTA DE CHAPAS** 📝");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var chapas = await _cadastroService.ListarChapas();
            
            if (!chapas.Any())
            {
                Console.WriteLine("📭 Nenhuma chapa cadastrada.");
                return;
            }

            Console.WriteLine("┌─────┬─────────────┬─────────┬─────────┬──────────┬─────────┬─────────────┬─────────┬─────────┬─────────────────┐");
            Console.WriteLine("│ ID  │ Código      │ Comp.   │ Larg.   │ Espessura│ Área    │ Tipo Granito│ Preço   │ Status  │ Data Serragem   │");
            Console.WriteLine("├─────┼─────────────┼─────────┼─────────┼──────────┼─────────┼─────────────┼─────────┼─────────┼─────────────────┤");
            
            foreach (var chapa in chapas)
            {
                Console.WriteLine($"│ {chapa.Id,-3} │ {chapa.Codigo,-11} │ {chapa.Comprimento,-7:F2} │ {chapa.Largura,-7:F2} │ {chapa.Espessura,-8:F2} │ {chapa.Area,-7:F2} │ {chapa.TipoGranito,-11} │ R$ {chapa.Preco,-6:F2} │ {chapa.Status,-9} │ {chapa.DataSerragem:dd/MM/yyyy} │");
            }
            
            Console.WriteLine("└─────┴─────────────┴─────────┴─────────┴──────────┴─────────┴─────────────┴─────────┴─────────┴─────────────────┘");
        }

        private async Task EditarChapa()
        {
            Console.WriteLine("\n✏️  **EDITAR CHAPA** ✏️");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            await ListarChapas();

            Console.Write("\n🎯 Digite o ID da chapa a ser editada: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ **ID inválido!** ❌");
                return;
            }

            var chapas = await _cadastroService.ListarChapas();
            var chapa = chapas.FirstOrDefault(c => c.Id == id);
            
            if (chapa == null)
            {
                Console.WriteLine("\n❌ **Chapa não encontrada!** ❌");
                return;
            }

            Console.WriteLine($"\n📝 Editando chapa: {chapa.Codigo}");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            Console.Write($"📋 Código ({chapa.Codigo}): ");
            var codigo = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(codigo))
                chapa.Codigo = codigo;

            Console.Write($"📏 Comprimento ({chapa.Comprimento:F2}): ");
            var comprimentoStr = Console.ReadLine();
            if (decimal.TryParse(comprimentoStr, out decimal comprimento))
            {
                chapa.Comprimento = comprimento;
                chapa.Area = comprimento * chapa.Largura; // Recalcular área
            }

            Console.Write($"📏 Largura ({chapa.Largura:F2}): ");
            var larguraStr = Console.ReadLine();
            if (decimal.TryParse(larguraStr, out decimal largura))
            {
                chapa.Largura = largura;
                chapa.Area = chapa.Comprimento * largura; // Recalcular área
            }

            Console.Write($"📏 Espessura ({chapa.Espessura:F2}): ");
            var espessuraStr = Console.ReadLine();
            if (decimal.TryParse(espessuraStr, out decimal espessura))
                chapa.Espessura = espessura;

            Console.Write($"🎨 Tipo de Granito ({chapa.TipoGranito}): ");
            var tipoGranito = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tipoGranito))
                chapa.TipoGranito = tipoGranito;

            Console.Write($"💰 Preço (R$ {chapa.Preco:F2}): ");
            var precoStr = Console.ReadLine();
            if (decimal.TryParse(precoStr, out decimal preco))
                chapa.Preco = preco;

            Console.Write($"📊 Status ({chapa.Status}): ");
            var status = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(status))
                chapa.Status = status;

            var sucesso = await _chapaRepository.UpdateAsync(chapa);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Chapa atualizada com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao atualizar chapa!** ❌");
            }
        }

        private async Task ExcluirChapa()
        {
            Console.WriteLine("\n🗑️  **EXCLUIR CHAPA** 🗑️");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            await ListarChapas();

            Console.Write("\n🎯 Digite o ID da chapa a ser excluída: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ **ID inválido!** ❌");
                return;
            }

            var chapas = await _cadastroService.ListarChapas();
            var chapa = chapas.FirstOrDefault(c => c.Id == id);
            
            if (chapa == null)
            {
                Console.WriteLine("\n❌ **Chapa não encontrada!** ❌");
                return;
            }

            Console.WriteLine($"\n⚠️  **ATENÇÃO:** Você está prestes a excluir a chapa:");
            Console.WriteLine($"   📋 Código: {chapa.Codigo}");
            Console.WriteLine($"   📏 Dimensões: {chapa.Comprimento:F2}m x {chapa.Largura:F2}m x {chapa.Espessura:F2}cm");
            Console.WriteLine($"   📐 Área: {chapa.Area:F2}m²");
            Console.WriteLine($"   🎨 Tipo: {chapa.TipoGranito}");
            Console.WriteLine($"   💰 Preço: R$ {chapa.Preco:F2}");
            
            Console.Write("\n🤔 Tem certeza? (S/N): ");
            var confirmacao = Console.ReadLine()?.ToUpper();
            
            if (confirmacao != "S")
            {
                Console.WriteLine("\n❌ **Operação cancelada!** ❌");
                return;
            }

            var sucesso = await _chapaRepository.DeleteAsync(id);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Chapa excluída com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao excluir chapa!** ❌");
            }
        }

        private async Task MenuUsuarios()
        {
            while (true)
            {
                Console.Clear();
                await ExibirCabecalho("👥 GESTÃO DE USUÁRIOS");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║  1. ➕ Cadastrar Novo Usuário                                                ║");
                Console.WriteLine("║  2. 📝 Listar Usuários                                                      ║");
                Console.WriteLine("║  3. ✏️  Editar Usuário                                                      ║");
                Console.WriteLine("║  4. 🗑️  Excluir Usuário                                                     ║");
                Console.WriteLine("║  0. 🔙 Voltar ao Menu Principal                                             ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("🎯 Escolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;
                var opcao = Console.ReadLine();
                Console.WriteLine();
                switch (opcao)
                {
                    case "1":
                        await CadastrarUsuario();
                        break;
                    case "2":
                        await ListarUsuarios();
                        break;
                    case "3":
                        await EditarUsuario();
                        break;
                    case "4":
                        await ExcluirUsuario();
                        break;
                    case "0":
                        return;
                    default:
                        await ExibirErro("Opção inválida! Tente novamente.");
                        break;
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("⏸️  Pressione qualquer tecla para continuar...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        private async Task CadastrarUsuario()
        {
            Console.WriteLine("\n➕ **CADASTRAR NOVO USUÁRIO** ➕");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            Console.Write("👤 Nome completo: ");
            var nome = Console.ReadLine() ?? "";

            Console.Write("🔑 Login: ");
            var login = Console.ReadLine() ?? "";

            Console.Write("🔒 Senha: ");
            var senha = Console.ReadLine() ?? "";

            Console.Write("📧 Email: ");
            var email = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
            {
                Console.WriteLine("\n❌ **Erro:** Nome, login e senha são obrigatórios!");
                return;
            }

            var usuario = new Usuario
            {
                Nome = nome,
                Login = login,
                Senha = senha,
                Email = email,
                DataCadastro = DateTime.Now,
                Ativo = true
            };

            var sucesso = await _cadastroService.CadastrarUsuarioAsync(usuario);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Usuário cadastrado com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao cadastrar usuário!** ❌");
            }
        }

        private async Task ListarUsuarios()
        {
            Console.WriteLine("\n📝 **LISTA DE USUÁRIOS** 📝");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            var usuarios = await _cadastroService.ListarUsuarios();
            
            if (!usuarios.Any())
            {
                Console.WriteLine("📭 Nenhum usuário cadastrado.");
                return;
            }

            Console.WriteLine("┌─────┬─────────────────┬─────────────┬─────────────────────────┬─────────┬─────────────────┐");
            Console.WriteLine("│ ID  │ Nome            │ Login       │ Email                   │ Ativo   │ Data Cadastro   │");
            Console.WriteLine("├─────┼─────────────────┼─────────────┼─────────────────────────┼─────────┼─────────────────┤");
            
            foreach (var usuario in usuarios)
            {
                Console.WriteLine($"│ {usuario.Id,-3} │ {usuario.Nome,-15} │ {usuario.Login,-11} │ {usuario.Email,-23} │ {(usuario.Ativo ? "✅" : "❌"),-7} │ {usuario.DataCadastro:dd/MM/yyyy} │");
            }
            
            Console.WriteLine("└─────┴─────────────────┴─────────────┴─────────────────────────┴─────────┴─────────────────┘");
        }

        private async Task EditarUsuario()
        {
            Console.WriteLine("\n✏️  **EDITAR USUÁRIO** ✏️");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            await ListarUsuarios();

            Console.Write("\n🎯 Digite o ID do usuário a ser editado: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ **ID inválido!** ❌");
                return;
            }

            var usuarios = await _cadastroService.ListarUsuarios();
            var usuario = usuarios.FirstOrDefault(u => u.Id == id);
            
            if (usuario == null)
            {
                Console.WriteLine("\n❌ **Usuário não encontrado!** ❌");
                return;
            }

            Console.WriteLine($"\n📝 Editando usuário: {usuario.Nome}");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            Console.Write($"👤 Nome ({usuario.Nome}): ");
            var nome = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nome))
                usuario.Nome = nome;

            Console.Write($"🔑 Login ({usuario.Login}): ");
            var login = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(login))
                usuario.Login = login;

            Console.Write($"🔒 Nova senha (deixe em branco para manter): ");
            var senha = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(senha))
                usuario.Senha = senha;

            Console.Write($"📧 Email ({usuario.Email}): ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                usuario.Email = email;

            Console.Write($"✅ Ativo (S/N) [{(usuario.Ativo ? "S" : "N")}]: ");
            var ativo = Console.ReadLine()?.ToUpper();
            if (ativo == "S" || ativo == "N")
                usuario.Ativo = ativo == "S";

            var sucesso = await _cadastroService.AtualizarUsuarioAsync(usuario);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Usuário atualizado com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao atualizar usuário!** ❌");
            }
        }

        private async Task ExcluirUsuario()
        {
            Console.WriteLine("\n🗑️  **EXCLUIR USUÁRIO** 🗑️");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            await ListarUsuarios();

            Console.Write("\n🎯 Digite o ID do usuário a ser excluído: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ **ID inválido!** ❌");
                return;
            }

            var usuarios = await _cadastroService.ListarUsuarios();
            var usuario = usuarios.FirstOrDefault(u => u.Id == id);
            
            if (usuario == null)
            {
                Console.WriteLine("\n❌ **Usuário não encontrado!** ❌");
                return;
            }

            Console.WriteLine($"\n⚠️  **ATENÇÃO:** Você está prestes a excluir o usuário:");
            Console.WriteLine($"   👤 Nome: {usuario.Nome}");
            Console.WriteLine($"   🔑 Login: {usuario.Login}");
            Console.WriteLine($"   📧 Email: {usuario.Email}");
            
            Console.Write("\n🤔 Tem certeza? (S/N): ");
            var confirmacao = Console.ReadLine()?.ToUpper();
            
            if (confirmacao != "S")
            {
                Console.WriteLine("\n❌ **Operação cancelada!** ❌");
                return;
            }

            var sucesso = await _cadastroService.ExcluirUsuarioAsync(id);
            
            if (sucesso)
            {
                Console.WriteLine("\n✅ **Usuário excluído com sucesso!** ✅");
            }
            else
            {
                Console.WriteLine("\n❌ **Erro ao excluir usuário!** ❌");
            }
        }
    }
}