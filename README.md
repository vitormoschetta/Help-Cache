# Help-Cache

O cache In Memory do Asp.NET Core ocorre na memória do servidor. Isso quer dizer que esse mesmo cache é compartilhado por todos os usuários do sistema. 
Isso pode ser um problema se os dados em cache forem formados através de parâmetros passados pelo usuário. 
Se cada usuário passa parâmetros diferentes, o cache estará sempre sendo sobrescrito, não melhorando em nada a performance da aplicação, pelo contrário. 

O exemplo a baixo apenas verifica se o cache existe, e mostra. Independente de parâmetros.

```
public class Produto
{
    private readonly IMemoryCache _cache;
    private readonly ApplicationDbContext _context;
    
    public Product(IMemoryCache cache, ApplicationDbContext context)
    {
        _context = context;
        _cache = cache;
    }
    
    public IEnumerable<Produto> ListarProdutos()
    {       
        var meuCache = _cache.TryGetValue("ChaveProdutosEmCache", out IEnumerable<Produto> produtos);
        if (meuCache != null)
        {
            if (produtos != null && produtos.Count > 0)
                return produtos;                     
        }
        
        produtos = _context.Produto.ToList();
        
        _cache.Set("ChaveProdutosEmCache", produtos, DateTime.Now.AddMinutes(30));       
        
        return produtos;        
    }
}
```


Vamos identificar as partes do nosso código:
```
_cache.Set("ChaveProdutosEmCache", produtos, DateTime.Now.AddMinutes(30));   
```
O código acima seta a nossa lista (produtos) em memória, dando a ela uma chave de identificação chamada "listaProdutosCache".
O terceiro parâmetro é o tempo que a lista deve permanecer em memória. Em nosso exemplo, 30 minutos. 


A próxima chamada que ocorrer ao método 'ListarProdutos()' nos próximos 30 minutos, vai entrar no condicional do cache, conforme a baixo:

```
var meuCache = _cache.TryGetValue("ChaveProdutosEmCache", out IEnumerable<Produto> produtos);
if (meuCache != null)
{
    if (produtos != null && produtos.Count > 0)
        return produtos;                     
}
```
Veja que a primeira ação do método é verificar se existe algum objeto em memória com a identificação 'listaProdutosCache'. Se existir, ele traz a saída desse objeto em 
memória, que na verdade é nossa lista de produtos 'IEnumerable<Produto> produtos'.
    
O Método 'TryGetValue', além de efetivar uma saída por referência a lista de produtos, retorna um _bool_ para a variável 'meuCache'. Retorna _true_ caso encontre um objeto 
em memória com o nome da chave especificado. Com esse retorno fazemos a validação, se existir e não for _null_ ou vazia, retornamos a lista em memória.


## View Component
```
<cache expires-on="@TimeSpan.FromSeconds(600)">
    @await Component.InvokeAsync("BlogPosts", new { tag = "popular" })
</cache>
```
