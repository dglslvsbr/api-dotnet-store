<h1>Store API</h1>

<p>Store API é uma API RESTful construída em ASP.NET Core com Entity Framework, permitindo gerenciamento de Clientes, Categorias, Produtos e Pedidos.
O sistema cobre todas as principais funcionalidades, como: Criar uma conta, criar uma nova categoria, criar um novo produto e criar uma nova permissão
para o sistema. Atualmente, há duas permissões: Permissão de usuário comum, e permissão para usuários com privilégios mais altos, administrador.
O usuário comum é capaz de executar ações específicas, por exemplo: Buscar clientes, buscar categorias, buscar produtos, buscar pedidos. Há várias
outras ações que requerem privilégios mais altos, como por exemplo, modificar ou deletar um recurso.</p>

<h2>Versão .NET 8.0</h2>

<h2>Descrição mais detalhada das funcionalidades</h2>
<pre>
  ▪️ Cadastro de Cliente
  ▪️ Logar na Conta (Autenticação Jwt Bearer)
  ▪️ Cadastro de Categoria ou Produto
  ▪️ Buscar Cliente, Categoria ou Produto
  ▪️ Criar um Pedido
  ▪️ Criar uma nova permissão
  ▪️ Adicionar uma permissão a um usuário
  ▫️ Cada recurso pode ser modificado ou removido (Requer Permissão de Administrador)
</pre>

<h2>Ferramentas e bibliotecas utilizadas</h2>
<pre>
  ▪️ AutoMapper
  ▪️ BCrypt-Net-Next
  ▪️ Authentication.JwtBearer
  ▪️ EntityFrameworkCore
  ▪️ EntityFrameworkCore.Tools
  ▪️ EntityFramework.SqlServer
  ▪️ Swagger
  ▪️ SwaggerGen
  ▪️ SwaggerUI
</pre>

<h2>Implementações de segurança e desempenho</h2>
<pre>
  ▪️ Autenticação Jwt
  ▪️ Sistema de cache (IMemoryCache)
  ▪️ CORS (Cross-Origin Resource Sharing)
  ▪️ Rate Limiter
  ▪️ Políticas de autorização
  ▪️ Tratamento Global de erro
</pre>

<h2>Endpoints</h2>

<h3>Controlador de Autorização</h3>
<pre>
  ▪️ https://localhost:xxxx/api/Auth/Login
  ▪️ https://localhost:xxxx/api/Auth/CreateRole
  ▪️ https://localhost:xxxx/api/Auth/AddRole/email/roleName
  ▪️ https://localhost:xxxx/api/Auth/DeleteRole/id
</pre>

<h3>Controlador de Cliente</h3>
<pre>
  ▪️ https://localhost:xxxx/api/Client/GetAll
  ▪️ https://localhost:xxxx/api/Client/Get/id
  ▪️ https://localhost:xxxx/api/Client/GetByEmail/email
  ▪️ https://localhost:xxxx/api/Client/Create
  ▪️ https://localhost:xxxx/api/Client/Update/id
  ▪️ https://localhost:xxxx/api/Client/Delete/id
</pre>

<h3>Controlador de Categoria</h3>
<pre>
  ▪️ https://localhost:xxxx/api/Category/GetAllCategoriesPaginated
  ▪️ https://localhost:xxxx/api/Category/GetCategoryWithProducts/id
  ▪️ https://localhost:xxxx/api/Category/Get/id
  ▪️ https://localhost:xxxx/api/Category/Create
  ▪️ https://localhost:xxxx/api/Category/Update/id
  ▪️ https://localhost:xxxx/api/Category/Delete/id
</pre>

<h3>Controlador de Produto</h3>
<pre>
  ▪️ https://localhost:xxxx/api/Product/GetAllProductsByCategory/id
  ▪️ https://localhost:xxxx/api/Product/GetAllPaginated
  ▪️ https://localhost:xxxx/api/Product/Get/id
  ▪️ https://localhost:xxxx/api/Product/Create
  ▪️ https://localhost:xxxx/api/Product/Update/id
  ▪️ https://localhost:xxxx/api/Product/Delete/id
</pre>

<h3>Controlador de Pedido</h3>
<pre>
  ▪️ https://localhost:xxxx/api/Order/GetAllOrdersPaginated
  ▪️ https://localhost:xxxx/api/Order/Get/id
  ▪️ https://localhost:xxxx/api/Order/Create
  ▪️ https://localhost:xxxx/api/Order/Update/id
  ▪️ https://localhost:xxxx/api/Order/Delete/id
</pre>

<h3>Importante</h3>
<p>Alguns endpoints requerem dados no corpo da requisição.</p>
<p>Troque o <strong>xxxx</strong> pela porta correta.</p>

<h2>Como usar</h2>
<p>A primeira coisa que você deve fazer para conseguir usar a Store API é fazer um pull
para a sua máquina local. Depois, basta renomear o arquivo <strong>example-appsettings.json</strong>
para <strong>appsettings.json</strong>. Após isso, dentro desse arquivo você vai encontrar instruções que você
deve seguir para que tudo funcione corretamente. Exemplo, configurar uma string de conexão com o banco
de dados e a chave secreta pra assinatura do token.</p>

<h2>Considerações</h2>
<p>É extremamente importante que você tenha o SQL Server instalado em sua máquina.
Sem isso, você não conseguirá rodar o Migrations, uma ferramenta do Entity Framework Core
para aplicar esquemas no banco de dados a partir do código-fonte. Então, algumas sugestões práticas:</p>

<pre>
  1. Instale o SQL Server
  2. Abra a solução do Projeto
  3. Vá em Tools > NuGet Package Manager > Package Manager Console
  4. Digite o comando: Add-Migration "CreateDb"
  5. Digite o próximo comando: Update-Database
</pre>

<p>Seguindo esses passos corretamente, você irá criar um esquema no banco de dados
a partir do código-fonte. Após isso, basta rodar a aplicação apertando F5.</p>

<p>Finalmente, esse é o resumo geral do projeto. Se você chegou até aqui, saiba que essa Store API
está sendo consumida a partir de um projeto frontend utilizando HTML, CSS e Javascript puro. Dê uma olhada!
Se assim como eu, você é aquele tipo de pessoa que está aprendendo e se interessa em entender de forma mais profunda
como um sistema funciona, faça um pull nesse projeto frontend e veja toda a comunicação acontecendo. 
<a href='https://github.com/dglslvsbr/frontend-store'>Clique aqui para ser redirecionado ao projeto frontend</a></p>

<h2>Contatos</h2>
<a href='https://www.instagram.com/dglslvsbr' target='_blank'><img src='https://img.shields.io/badge/Instagram-%23E4405F.svg?style=for-the-badge&logo=Instagram&logoColor=white'></a>
<a href='https://www.linkedin.com/in/dglslvsbr' target='_blank'><img src='https://img.shields.io/badge/linkedin-%230077B5.svg?style=for-the-badge&logo=linkedin&logoColor=white'></a>
