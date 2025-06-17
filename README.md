# Louzada Granitos

Sistema de gerenciamento para a empresa Louzada Granitos, desenvolvido em .NET Core.

## 📋 Descrição

Sistema de console para gerenciamento de:
- 👥 Usuários
- 🏔️ Pedreiras
- 🗿 Blocos
- 📏 Chapas
- ⚙️ Serragem

## 🚀 Tecnologias Utilizadas

- .NET Core
- MySQL
- Entity Framework Core
- Console Application

## ⚙️ Pré-requisitos

- .NET Core SDK 7.0 ou superior
- MySQL Server
- Visual Studio 2022 ou VS Code

## 🔧 Instalação

1. Clone o repositório
```bash
git clone https://github.com/CaioLennonDEV/louzada-granitos.git
```

2. Navegue até o diretório do projeto
```bash
cd louzada-granitos
```

3. Restaure as dependências
```bash
dotnet restore
```

4. Configure a string de conexão no arquivo `appsettings.json`

5. Execute as migrações do banco de dados
```bash
dotnet ef database update
```

6. Execute o projeto
```bash
dotnet run --project LouzadaGranitos.ConsoleApp
```


## 👥 Autores

- CAIO LENNON VANDERMUREN
- MARIA EDUARDA LOUZADA
- VINICIO MENDES
- LIVIA LOUZADA
