# Projeto ESG - Eficiência Energética API

API RESTful desenvolvida em .NET 8 para monitoramento e otimização do consumo de energia elétrica, focada em sustentabilidade e práticas ESG (Environmental, Social, and Governance).

> 📚 **Para documentação completa, exemplos detalhados e troubleshooting**, consulte: **[TUTORIAL_COMPLETO.md](TUTORIAL_COMPLETO.md)**

## 🎯 Funcionalidades

- **Monitoramento de Consumo**: Registro e consulta de dados de consumo energético
- **Sistema de Alertas**: Detecção automática de consumos excessivos (> 1000 kWh)
- **Simulação IoT**: Endpoint para simular verificação de sensores IoT
- **Autenticação JWT**: Proteção de endpoints críticos
- **Paginação**: Listagem otimizada de dados
- **Documentação Swagger**: Interface amigável para testes da API

## 🏗️ Arquitetura

O projeto segue o padrão **MVVM** (Model-View-ViewModel) com as seguintes camadas:

```
ProjetoESG/
├── Controllers/          # Controladores da API
├── Models/              # Modelos de dados (Entity Framework)
├── ViewModels/          # DTOs para entrada/saída de dados
├── Services/            # Lógica de negócio
├── Data/                # Contexto do banco de dados
├── Middleware/          # Middleware personalizado
└── Tests/               # Testes automatizados
```

## 🚀 Como Executar

### Pré-requisitos

- .NET 8 SDK
- SQL Server LocalDB (ou SQL Server)
- Docker (opcional)

### Executar com dotnet run

1. **Clone o repositório**
```bash
git clone <repository-url>
cd ProjetoESG
```

2. **Restaurar dependências**
```bash
dotnet restore
```

3. **Executar a aplicação**
```bash
dotnet run
```

4. **Acessar a aplicação**
- API: https://localhost:7000 ou http://localhost:5000
- Swagger: https://localhost:7000 (raiz do site)

### Executar com Docker

Para instruções completas de Docker (incluindo Docker Compose com SQL Server), consulte o **[TUTORIAL_COMPLETO.md](TUTORIAL_COMPLETO.md#docker)**.

**Execução rápida:**
```bash
# Docker Compose (RECOMENDADO) - API + SQL Server
docker-compose up --build

# Acesse: http://localhost:5001
```

## 🔐 Autenticação

### Obter Token JWT

**POST** `/auth/login`

```json
{
    "username": "admin",
    "password": "esg123"
}
```

**Resposta:**
```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600,
    "tokenType": "Bearer",
    "username": "admin"
}
```

### Usar Token

Inclua o token no header Authorization:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 📋 Endpoints da API

### 1. Consumo de Energia

#### **GET** `/energy` - Listar consumos
- **Descrição**: Lista os consumos de energia com paginação
- **Parâmetros**:
  - `pageNumber` (opcional): Número da página (padrão: 1)
  - `pageSize` (opcional): Itens por página (padrão: 10, máx: 100)
- **Autenticação**: Não requerida
- **Exemplo**: `/energy?pageNumber=1&pageSize=10`

#### **POST** `/energy` - Registrar consumo
- **Descrição**: Registra um novo consumo de energia
- **Autenticação**: JWT requerido
- **Body**:
```json
{
    "companyName": "Empresa ABC Ltda",
    "consumptionKwh": 1500.50,
    "timestamp": "2024-01-15T10:30:00Z",
    "description": "Consumo do datacenter principal",
    "location": "São Paulo, SP - Prédio A"
}
```

### 2. Alertas

#### **GET** `/alerts` - Listar alertas
- **Descrição**: Lista alertas de consumo excessivo com paginação
- **Parâmetros**:
  - `pageNumber` (opcional): Número da página (padrão: 1)
  - `pageSize` (opcional): Itens por página (padrão: 10, máx: 100)
- **Autenticação**: Não requerida

#### **POST** `/alerts/check` - Verificar sensores IoT
- **Descrição**: Simula verificação de sensores IoT e gera alertas automaticamente
- **Autenticação**: JWT requerido
- **Body**: Não requerido

## 🔧 Configuração

### Connection String

Configure a string de conexão no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProjetoESGDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### JWT Settings

```json
{
  "JwtSettings": {
    "SecretKey": "ProjetoESG-SecretKey-2024-MinLength32Characters!",
    "Issuer": "ProjetoESG",
    "Audience": "ProjetoESG-Users",
    "ExpirationHours": 1
  }
}
```

## 🧪 Testes

### Executar testes automatizados

```bash
# Navegar para o diretório de testes
cd ProjetoESG.Tests

# Executar todos os testes
dotnet test

# Executar com detalhes
dotnet test --verbosity normal
```

### Testes Disponíveis

- **EnergyControllerTests**: Testa endpoints de consumo de energia
- **AlertsControllerTests**: Testa endpoints de alertas

Todos os testes verificam se os endpoints retornam **HTTP 200** conforme solicitado.

## 📊 Regras de Negócio

### Geração Automática de Alertas

- **Trigger**: Consumo > 1000 kWh
- **Severidade**:
  - `Critical`: > 5000 kWh
  - `High`: > 3000 kWh
  - `Medium`: > 1500 kWh
  - `Low`: > 1000 kWh

### Validações

- **Consumo**: Deve ser > 0 kWh
- **Nome da Empresa**: Obrigatório, máx 200 caracteres
- **Localização**: Obrigatória, máx 300 caracteres
- **Timestamp**: Obrigatório
- **Descrição**: Opcional, máx 500 caracteres

## 🚀 Testando com Postman/Insomnia

### 1. Fazer Login
```
POST http://localhost:5000/auth/login
Content-Type: application/json

{
    "username": "admin",
    "password": "esg123"
}
```

### 2. Listar Consumos
```
GET http://localhost:5000/energy?pageNumber=1&pageSize=10
```

### 3. Criar Consumo
```
POST http://localhost:5000/energy
Authorization: Bearer {seu-token-aqui}
Content-Type: application/json

{
    "companyName": "Empresa XYZ",
    "consumptionKwh": 2500,
    "timestamp": "2024-01-15T14:30:00Z",
    "location": "Rio de Janeiro, RJ",
    "description": "Consumo do setor industrial"
}
```

### 4. Verificar Sensores IoT
```
POST http://localhost:5000/alerts/check
Authorization: Bearer {seu-token-aqui}
```

## 🛠️ Tecnologias Utilizadas

- **ASP.NET Core 8**: Framework web
- **Entity Framework Core**: ORM para banco de dados
- **SQL Server**: Banco de dados relacional
- **JWT**: Autenticação stateless
- **Swagger/OpenAPI**: Documentação da API
- **xUnit**: Framework de testes
- **Docker**: Containerização

## 🌱 Sustentabilidade e ESG

Este projeto contribui para práticas ESG através de:

- **Environmental**: Monitoramento eficiente do consumo energético
- **Social**: Transparência nos dados de sustentabilidade
- **Governance**: Relatórios automatizados e auditáveis

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 📞 Suporte

Para dúvidas ou suporte:
- Email: contato@projetoesg.com
- Issues: [GitHub Issues](../../issues)

---

**Desenvolvido com ❤️ para um futuro mais sustentável** 🌍 