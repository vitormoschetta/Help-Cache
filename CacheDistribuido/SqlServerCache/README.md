## SqlServerCache

Neste método de _cache_ os dados NÃO ficam armazenados na Memória de qualquer servidor, mas em uma TABELA com tipo de campo especial (varbinary)  no banco de dados Sql Server. 

Além das informações ficarem em uma única tabela (desnormalização de dados) ainda são salvos em uma única coluna, com um tipo de dados mais fácil de ser lido pela máquina. Essa técnica possibilita uma consulta bastante performática para uma requisição ao banco de dados, possilitando usá-la como um tipo de _cache_.

Neste diretório existem duas aplicações MVC (app01 e app02), idênticas à aplicação do diretório '../CacheLocal', com exceção de que estas estão configuradas para gravar o cache em uma tabela específica de um banco de dados específico no SqlServer:

Acesse o diretório **App01/** e veja o conteúdo do arquivo _appsettings.json_:
```
"ConnectionStrings": {
    "SQLite": "Data Source=Cache.db",
    "SqlServerCache": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DistCache;Integrated Security=True;"
}
```
Perceba que além da conexão com o banco de dados SQLite, que usamos para de fato gravar nossos pedidos (orders), existe uma conexão com o Sql Server LocalDB, direcionado a uma tabela chamada "DistCache".

Agora veja o método _ConfigureServices_ da classe _Startup.cs_:
```
services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = Configuration.GetConnectionString("SqlServerCache");
    options.SchemaName = "dbo";
    options.TableName = "TestCache";
});
```
O método define um serviço de _cache_ distribuído, onde usamos o banco de dados Sql Server LocalDB, cuja string de conexão é recuperada de _appsettings.json_, e setamos uma tabela chamada "TestCache" para armazenar as nossas informações.

Por último, veja o conteúdo do método _OrdersInCache_ da classe "Controllers/OrderController":
```
public async Task<IActionResult> OrdersInCache()
{
    PaginatedList<Order> listaPaginada;
    var itensPorPagina = 5;

    string ordersCache = _cache.GetString("Orders");
    if (ordersCache != null)
    {                
        var listOrders = JsonConvert.DeserializeObject<List<Order>>(ordersCache);
        listaPaginada = PaginatedList<Order>.Create(listOrders, 1, itensPorPagina);
        return View(listaPaginada);
    }                 

    List<Order> orders = await _context.Order
        .Include(x => x.OrderProducts)
        .ThenInclude(x => x.Product)
        .ToListAsync();

    string stringOrders = JsonConvert.SerializeObject(orders);
    _cache.SetString("Orders", stringOrders);

    listaPaginada = PaginatedList<Order>.Create(orders, 1, itensPorPagina);
    return View(listaPaginada);
}
```
Perceba que antes de salvar os dados na tabela de _cache_ (TestCache), precisamos serializá-los para um JSON/string. Em seguida usar o método _SetString_ da interface _IDistributedCache_ para setar esses dados em _cache_.

O mesmo vale para o processo inverso: Ao efetuar o _GetString_ da interface _IDistributedCache_, precisamos converter essa string/JSON para o tipo de objeto que estamos trabalhando.
<br>


### Criação da tabela 'TestCache'
Podemos criar essa tabela com os tipos necessários para se trabalhar com a inteface _IDistributedCache_ através da seguinte linha de comando:
```
dotnet sql-cache create "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DistCache;Integrated Security=True;" dbo TestCache
```
Obs: Deve existir um banco no Sql Server LocalDB com o nome especificado no comando acima (DistCache). 

O comando acima seta/cria uma tabela chamada 'TestCache' no banco dedados 'DistCache'.


### Executando a aplicação
Execute os dois apps (App01 e App02) e veja que após o primeiro acesso o retorno será muito mais rápido. E mais, você pode reiniciar sua aplicação que o cache não se perde, afinal, este _cache_ está distribuído.


### Recomendações da Microsoft:
> É recomendável usar uma instância de SQL Server dedicada para o armazenamento de backup e outra para a recuperação do _cache_ distribuído.

Ou seja, quando a mesma tabela está sendo usada para a escrita e a leitura do cache pode haver perdade de desemepnho ou algum tipo de conflito durante o processo.


### Referências
<https://docs.microsoft.com/pt-br/aspnet/core/performance/caching/distributed?view=aspnetcore-5.0>