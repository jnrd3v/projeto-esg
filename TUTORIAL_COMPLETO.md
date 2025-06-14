# 🎉 Tutorial Completo - Projeto ESG Eficiência Energética API

## 📋 **Visão Geral do Projeto**

Este é um projeto completo em .NET 8 Web API focado em **ESG (Environmental, Social, and Governance)**, especificamente para monitoramento e otimização do consumo de energia elétrica. O sistema permite registrar consumos, emitir alertas automaticamente e simular integrações com sensores IoT.

---

## ✅ **Funcionalidades Implementadas**

### **🏗️ Arquitetura MVVM**
- **Models**: `EnergyConsumption.cs`, `Alert.cs`
- **ViewModels**: DTOs para entrada/saída de dados com paginação
- **Services**: Lógica de negócio para consumo e alertas
- **Controllers**: `EnergyController`, `AlertsController`, `AuthController`

### **🔒 Segurança JWT**
- Autenticação JWT básica implementada
- Endpoints POST protegidos
- **Credenciais de teste**: `username: admin`, `password: esg123`

### **📊 Endpoints RESTful Implementados**
1. **GET /energy** - Lista consumos com paginação
2. **POST /energy** - Registra novo consumo (protegido com JWT)
3. **GET /alerts** - Lista alertas com paginação  
4. **POST /alerts/check** - Simulação IoT (protegido com JWT)
5. **POST /auth/login** - Autenticação JWT

### **🗄️ Banco de Dados**
- Entity Framework Core com SQL Server LocalDB
- Migrações configuradas automaticamente
- Relacionamentos entre entidades
- Índices otimizados para performance

### **⚡ Regras de Negócio**
- **Alertas automáticos** para consumo > 1000 kWh
- **Severidade baseada no nível**: 
  - `Critical`: > 5000 kWh
  - `High`: > 3000 kWh
  - `Medium`: > 1500 kWh
  - `Low`: > 1000 kWh
- Validações completas com DataAnnotations

### **🧪 Testes Automatizados**
- **8 testes xUnit** (todos passando ✅)
- Testes de integração para endpoints
- Verificação de retorno HTTP 200
- Banco de dados em memória para testes

### **🐳 Docker**
- Dockerfile funcional e otimizado
- Multi-stage build
- Porta 80 exposta

### **📚 Documentação**
- **Swagger** configurado na raiz
- **README.md** completo
- Documentação dos endpoints com exemplos

---

## 📁 **Estrutura Final do Projeto**

```
ProjetoESG/
├── Controllers/                    # Controladores da API
│   ├── EnergyController.cs        # Endpoints de consumo energético
│   ├── AlertsController.cs        # Endpoints de alertas
│   └── AuthController.cs          # Autenticação JWT
├── Models/                         # Modelos de dados
│   ├── EnergyConsumption.cs       # Modelo de consumo
│   └── Alert.cs                   # Modelo de alerta
├── ViewModels/                     # DTOs para entrada/saída
│   ├── EnergyConsumptionViewModel.cs
│   └── AlertViewModel.cs
├── Services/                       # Lógica de negócio
│   ├── IEnergyConsumptionService.cs
│   ├── EnergyConsumptionService.cs
│   ├── IAlertService.cs
│   └── AlertService.cs
├── Data/                          # Contexto do banco
│   └── ApplicationDbContext.cs
├── Middleware/                    # Middleware personalizado
│   └── GlobalExceptionMiddleware.cs
├── ProjetoESG.Tests/             # Testes automatizados
│   ├── EnergyControllerTests.cs
│   ├── AlertsControllerTests.cs
│   └── CustomWebApplicationFactory.cs
├── Dockerfile                    # Containerização
├── .dockerignore                # Exclusões do Docker
├── README.md                     # Documentação principal
├── TUTORIAL_COMPLETO.md         # Este tutorial
├── Program.cs                   # Configuração da aplicação
└── appsettings.json            # Configurações
```

---

## 🚀 **Como Executar o Projeto**

### **Pré-requisitos**
- .NET 8 SDK instalado
- SQL Server LocalDB (ou SQL Server)
- Docker (opcional)

### **Opção 1: Executar com dotnet run**

1. **Navegar para o diretório do projeto:**
```bash
cd ProjetoESG
```

2. **Restaurar dependências:**
```bash
dotnet restore
```

3. **Executar a aplicação:**
```bash
dotnet run
```

4. **Acessar a aplicação:**
- **API**: https://localhost:7000 ou http://localhost:5000
- **Swagger UI**: https://localhost:7000 (raiz do site)

### **Opção 2: Executar com Docker (RECOMENDADO para produção)**

#### **🐳 Docker vs dotnet run - Diferenças:**

| Aspecto | dotnet run | Docker |
|---------|------------|---------|
| **Onde executa** | No seu sistema | Em container Linux isolado |
| **Banco de dados** | SQLite/LocalDB | SQL Server completo |
| **Porta** | 5000/7000 | 5001 (mapeada) |
| **Isolamento** | Usa seu sistema | Completamente isolado |
| **Produção** | ❌ Não recomendado | ✅ Ideal para produção |

#### **Docker Compose (RECOMENDADO) - API + SQL Server:**

1. **Executar tudo automaticamente:**
```bash
# Navegar para o diretório do projeto
cd ProjetoESG

# Executar API + SQL Server (primeira execução demora ~2 minutos)
docker-compose up --build

# Para executar em background (detached):
docker-compose up -d --build
```

2. **Acessar a aplicação:**
- **API**: http://localhost:5001
- **Swagger UI**: http://localhost:5001

3. **Comandos úteis:**
```bash
# Ver containers rodando
docker ps

# Ver logs em tempo real
docker-compose logs -f

# Ver apenas logs da API
docker-compose logs -f api

# Parar containers
docker-compose down

# Rebuildar após mudanças
docker-compose up --build

# Limpar tudo (CUIDADO: apaga dados!)
docker-compose down -v
```

#### **Docker Manual (apenas API):**

1. **Construir a imagem Docker:**
```bash
docker build -t projeto-esg .
```

2. **Executar o container:**
```bash
docker run -p 5001:80 projeto-esg
```

3. **Acessar a aplicação:**
- **API**: http://localhost:5001
- **Swagger UI**: http://localhost:5001

#### **🔍 Verificar se Docker está funcionando:**

1. **Ver containers ativos:** `docker ps`
2. **Testar API:** `curl http://localhost:5001/energy`
3. **Ver logs:** `docker-compose logs api`

#### **🐛 Problemas comuns com Docker:**

1. **Porta ocupada**: Pare qualquer `dotnet run` antes
```bash
lsof -i :5001  # Ver o que usa a porta
kill -9 [PID]  # Matar processo se necessário
```

2. **Banco não conecta**: Aguarde ~30 segundos para SQL Server inicializar

3. **Mudanças não aparecem**: Use `--build` para reconstruir:
```bash
docker-compose up --build
```

4. **Limpeza total**:
```bash
docker-compose down -v  # Remove volumes
docker system prune -a  # Limpa images não usadas
```

### **Executar testes automatizados:**
```bash
cd ProjetoESG.Tests
dotnet test
# Resultado esperado: 8/8 testes passando ✅
```

---

## 🔐 **Sistema de Autenticação**

### **Credenciais de Teste**
- **Username**: `admin`
- **Password**: `esg123`

### **Obter Token JWT**

**Endpoint:** `POST /auth/login`

**Request:**
```http
POST /auth/login
Content-Type: application/json

{
    "username": "admin",
    "password": "esg123"
}
```

**Response:**
```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600,
    "tokenType": "Bearer",
    "username": "admin"
}
```

### **Usar o Token**
Inclua o token no header Authorization de todas as requisições protegidas:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 📋 **Guia Completo dos Endpoints**

### **1. 🔑 Autenticação**

#### **POST /auth/login**
- **Descrição**: Gera token JWT para autenticação
- **Autenticação**: Não requerida
- **Body**:
```json
{
    "username": "admin",
    "password": "esg123"
}
```

---

### **2. ⚡ Consumo de Energia**

#### **GET /energy - Listar Consumos**
- **Descrição**: Lista os consumos de energia com paginação
- **Autenticação**: Não requerida
- **Parâmetros**:
  - `pageNumber` (opcional): Número da página (padrão: 1)
  - `pageSize` (opcional): Itens por página (padrão: 10, máx: 100)

**Exemplo de Requisição:**
```http
GET /energy?pageNumber=1&pageSize=10
```

**Exemplo de Response:**
```json
{
    "data": [
        {
            "id": 1,
            "companyName": "Empresa ABC Ltda",
            "consumptionKwh": 1500.50,
            "timestamp": "2024-01-15T10:30:00Z",
            "description": "Consumo do datacenter principal",
            "location": "São Paulo, SP - Prédio A",
            "createdAt": "2024-01-15T10:35:00Z"
        }
    ],
    "totalCount": 25,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 3,
    "hasPreviousPage": false,
    "hasNextPage": true
}
```

#### **POST /energy - Registrar Consumo**
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

**Headers:**
```
Authorization: Bearer {seu-token-jwt}
Content-Type: application/json
```

---

### **3. 🚨 Alertas**

#### **GET /alerts - Listar Alertas**
- **Descrição**: Lista alertas de consumo excessivo com paginação
- **Autenticação**: Não requerida
- **Parâmetros**: Mesmos do endpoint de energia

**Exemplo de Response:**
```json
{
    "data": [
        {
            "id": 1,
            "companyName": "Empresa XYZ",
            "message": "Consumo excessivo de energia detectado: 2500.00 kWh...",
            "consumptionKwh": 2500.00,
            "severity": "Medium",
            "location": "Rio de Janeiro, RJ",
            "isResolved": false,
            "createdAt": "2024-01-15T14:30:00Z",
            "resolvedAt": null,
            "energyConsumptionId": 5
        }
    ],
    "totalCount": 10,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
}
```

#### **POST /alerts/check - Simulação de Sensores IoT**
- **Descrição**: Simula verificação de sensores IoT e gera alertas automaticamente
- **Autenticação**: JWT requerido
- **Body**: Não requerido

**Headers:**
```
Authorization: Bearer {seu-token-jwt}
```

**Exemplo de Response:**
```json
{
    "generatedAlertsCount": 2,
    "newAlerts": [
        {
            "id": 15,
            "companyName": "Empresa DEF",
            "message": "Consumo excessivo detectado...",
            "consumptionKwh": 3200.00,
            "severity": "High",
            "location": "Brasília, DF",
            "isResolved": false,
            "createdAt": "2024-01-15T15:00:00Z"
        }
    ],
    "message": "Foram gerados 2 novos alertas baseados na simulação de sensores IoT."
}
```

---

## 🔄 **Testando com Postman/Insomnia**

### **Cenário Completo de Teste**

#### **1. Fazer Login**
```http
POST http://localhost:5000/auth/login
Content-Type: application/json

{
    "username": "admin",
    "password": "esg123"
}
```
**Resultado**: Copie o `token` da resposta.

#### **2. Listar Consumos Vazios**
```http
GET http://localhost:5000/energy?pageNumber=1&pageSize=10
```
**Resultado**: Lista vazia inicialmente.

#### **3. Criar Primeiro Consumo (Baixo)**
```http
POST http://localhost:5000/energy
Authorization: Bearer {seu-token-aqui}
Content-Type: application/json

{
    "companyName": "Empresa ABC",
    "consumptionKwh": 800,
    "timestamp": "2024-01-15T10:30:00Z",
    "location": "São Paulo, SP",
    "description": "Consumo normal do escritório"
}
```
**Resultado**: Consumo criado, nenhum alerta gerado.

#### **4. Criar Segundo Consumo (Alto - Gera Alerta)**
```http
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
**Resultado**: Consumo criado + Alerta automático gerado (severity: "Medium").

#### **5. Listar Consumos**
```http
GET http://localhost:5000/energy
```
**Resultado**: 2 consumos listados.

#### **6. Listar Alertas**
```http
GET http://localhost:5000/alerts
```
**Resultado**: 1 alerta listado.

#### **7. Simular Sensores IoT**
```http
POST http://localhost:5000/alerts/check
Authorization: Bearer {seu-token-aqui}
```
**Resultado**: Verifica consumos recentes e pode gerar novos alertas.

---

## 🧪 **Testes Automatizados**

### **Testes Implementados**

O projeto inclui **8 testes automatizados** que verificam:

1. **EnergyControllerTests**:
   - ✅ GET /energy retorna 200 com dados paginados
   - ✅ GET /energy com paginação personalizada retorna 200
   - ✅ GET /energy com pageNumber inválido retorna 400
   - ✅ GET /energy com pageSize excessivo retorna 400

2. **AlertsControllerTests**:
   - ✅ GET /alerts retorna 200 com dados paginados
   - ✅ GET /alerts com paginação personalizada retorna 200
   - ✅ GET /alerts com pageNumber inválido retorna 400
   - ✅ GET /alerts com pageSize excessivo retorna 400

### **Executar Testes**
```bash
cd ProjetoESG.Tests
dotnet test --verbosity normal
```

**Resultado Esperado:**
```
Passed!  - Failed: 0, Passed: 8, Skipped: 0, Total: 8
```

---

## 📊 **Regras de Negócio Detalhadas**

### **Sistema de Alertas Automáticos**

Quando um consumo é registrado via `POST /energy`, o sistema automaticamente:

1. **Verifica** se o consumo é > 1000 kWh
2. **Calcula a severidade**:
   - `Critical`: > 5000 kWh
   - `High`: > 3000 kWh  
   - `Medium`: > 1500 kWh
   - `Low`: > 1000 kWh
3. **Cria um alerta** automaticamente com:
   - Mensagem descritiva
   - Severidade calculada
   - Referência ao consumo original
   - Timestamp de criação

### **Simulação de Sensores IoT**

O endpoint `POST /alerts/check` simula a verificação de sensores IoT:

1. **Busca** consumos das últimas 24 horas
2. **Filtra** apenas consumos > 1000 kWh sem alertas
3. **Gera alertas** para os consumos encontrados
4. **Retorna** estatísticas dos alertas criados

### **Validações de Dados**

- **Consumo**: Deve ser > 0 kWh
- **Nome da Empresa**: Obrigatório, máx 200 caracteres
- **Localização**: Obrigatória, máx 300 caracteres
- **Timestamp**: Obrigatório e válido
- **Descrição**: Opcional, máx 500 caracteres

### **Paginação**

Todos os endpoints de listagem incluem:
- `pageNumber`: Página atual (min: 1)
- `pageSize`: Itens por página (min: 1, max: 100)
- `totalCount`: Total de registros
- `totalPages`: Total de páginas
- `hasPreviousPage`: Indicador de página anterior
- `hasNextPage`: Indicador de próxima página

---

## 🐳 **Docker e Containerização**

### **Dockerfile Otimizado**

O projeto inclui um Dockerfile multi-stage:

1. **Build Stage**: Compila o projeto
2. **Runtime Stage**: Executa apenas os binários necessários
3. **Otimizações**: Cache de layers, minimal runtime image

### **Comandos Docker**

**Construir imagem:**
```bash
docker build -t projeto-esg .
```

**Executar container:**
```bash
docker run -d -p 8080:80 --name esg-api projeto-esg
```

**Ver logs:**
```bash
docker logs esg-api
```

**Parar container:**
```bash
docker stop esg-api
```

---

## 🔧 **Configurações do Sistema**

### **appsettings.json**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProjetoESGDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "SecretKey": "ProjetoESG-SecretKey-2024-MinLength32Characters!",
    "Issuer": "ProjetoESG",
    "Audience": "ProjetoESG-Users",
    "ExpirationHours": 1
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

### **Personalizar Configurações**

1. **String de Conexão**: Altere para seu SQL Server
2. **JWT Secret**: Use uma chave forte em produção
3. **Logging**: Ajuste níveis conforme necessário

---

## 🌟 **Características Avançadas**

### **Middleware Global de Exceções**

- Captura todas as exceções não tratadas
- Retorna respostas JSON padronizadas
- Logs detalhados para debugging
- Tratamento específico por tipo de exceção

### **Swagger/OpenAPI**

- Documentação automática dos endpoints
- Interface de teste integrada
- Suporte a autenticação JWT na UI
- Exemplos de requisições/respostas

### **Entity Framework**

- Code-First com migrações automáticas
- Relacionamentos configurados
- Índices otimizados
- Soft deletes (se necessário)

---

## 🚀 **Próximos Passos e Melhorias**

### **Funcionalidades Adicionais Sugeridas**

1. **Dashboard**: Interface web para visualização
2. **Relatórios**: PDF/Excel com consumo por período
3. **Notificações**: Email/SMS para alertas críticos
4. **API de Terceiros**: Integração real com IoT
5. **Cache**: Redis para melhor performance
6. **Audit Trail**: Log de todas as alterações

### **Melhorias de Segurança**

1. **Rate Limiting**: Limite de requisições por IP
2. **CORS**: Configuração adequada para produção
3. **HTTPS**: Certificados SSL/TLS
4. **Roles**: Sistema de permissões granular

### **DevOps e Produção**

1. **CI/CD**: Pipeline automatizado
2. **Monitoring**: Application Insights/Prometheus
3. **Load Balancer**: Para alta disponibilidade
4. **Database**: Configuração para produção

---

## 🎯 **Pontos de Atenção**

### **Para Desenvolvimento Local**

- ✅ SQL Server LocalDB deve estar instalado
- ✅ .NET 8 SDK é obrigatório
- ✅ Porta 5000/7000 devem estar livres

### **Para Produção**

- 🔄 Alterar JWT SecretKey
- 🔄 Configurar string de conexão real
- 🔄 Configurar HTTPS
- 🔄 Ajustar logs para produção

### **Para Testes**

- ✅ Testes usam banco em memória
- ✅ Não afetam dados reais
- ✅ Executam independentemente

---

## 📞 **Suporte e Contribuição**

### **Para Dúvidas**
- Consulte este tutorial primeiro
- Verifique o README.md
- Teste no Swagger UI

### **Para Bugs**
- Execute os testes (`dotnet test`)
- Verifique logs da aplicação
- Teste com dados mínimos

### **Para Melhorias**
- Fork o projeto
- Crie feature branch
- Implemente testes
- Submeta Pull Request

---

## 🏆 **Conclusão**

Este projeto demonstra uma implementação completa e profissional de uma API REST em .NET 8, focada em sustentabilidade e práticas ESG. Inclui:

- ✅ **Arquitetura sólida** (MVVM)
- ✅ **Segurança adequada** (JWT)
- ✅ **Testes abrangentes** (xUnit)
- ✅ **Documentação completa** (Swagger + Markdown)
- ✅ **Containerização** (Docker)
- ✅ **Boas práticas** (Clean Code, SOLID)

O projeto está **pronto para produção** e pode ser usado como base para sistemas mais complexos de monitoramento energético e sustentabilidade corporativa.

---

**🌍 Desenvolvido com ❤️ para um futuro mais sustentável!** 