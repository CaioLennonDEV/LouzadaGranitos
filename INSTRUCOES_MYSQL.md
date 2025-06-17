# 🗄️ Instalação e Configuração do MySQL para CGB Granitos

## 📥 **Passo 1: Baixar MySQL**

1. Acesse: https://dev.mysql.com/downloads/installer/
2. Baixe o **"MySQL Installer for Windows"**
3. Escolha a versão **"Windows (x86, 32-bit), MSI Installer"** (mais estável)

## 🚀 **Passo 2: Instalar MySQL**

1. **Execute o instalador** como administrador
2. **Escolha "Developer Default"** (inclui MySQL Server + Workbench)
3. **Clique "Next"** em todas as telas
4. **Configure a senha do root:**
   - Digite uma senha (ex: `123456`)
   - **ANOTE ESSA SENHA!**
5. **Finalize a instalação**

## ⚙️ **Passo 3: Configurar String de Conexão**

### **Opção A: Sem senha (mais simples)**
Edite o arquivo `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "FirebirdConnection": "Server=localhost;Port=3306;Database=cgbgranitos;Uid=root;Pwd=;"
  }
}
```

### **Opção B: Com senha (mais seguro)**
Edite o arquivo `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "FirebirdConnection": "Server=localhost;Port=3306;Database=cgbgranitos;Uid=root;Pwd=SUA_SENHA_AQUI;"
  }
}
```

## 🗃️ **Passo 4: Criar o Banco de Dados**

### **Método 1: MySQL Workbench (Recomendado)**
1. Abra o **MySQL Workbench**
2. Conecte com `root` e sua senha
3. Execute o script `setup_database.sql` que criamos

### **Método 2: Linha de Comando**
1. Abra o **Command Prompt**
2. Execute:
```bash
mysql -u root -p
```
3. Digite sua senha
4. Execute:
```sql
CREATE DATABASE cgbgranitos;
```

## 🧪 **Passo 5: Testar o Sistema**

Execute no terminal:
```bash
dotnet run --project CGBGranitos.ConsoleApp
```

## 🔧 **Solução de Problemas**

### **Erro: "Access denied for user 'root'@'localhost'"**
- Verifique se a senha está correta na string de conexão
- Ou remova a senha se instalou sem senha

### **Erro: "Can't connect to MySQL server"**
- Verifique se o MySQL está rodando
- Abra "Serviços" do Windows e procure por "MySQL80"

### **Erro: "Unknown database 'cgbgranitos'"**
- Execute o script `setup_database.sql`
- Ou crie manualmente: `CREATE DATABASE cgbgranitos;`

## 📞 **Precisa de Ajuda?**

Se tiver problemas, me envie:
1. O erro exato que aparece
2. Se conseguiu instalar o MySQL
3. Se conseguiu conectar no MySQL Workbench 