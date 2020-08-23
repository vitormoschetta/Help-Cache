# Help-Cache

### Exemplo básico:
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

###### Vamos identificar as partes do nosso código:
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



### Cache formado a partir de parâmetros
O cache deve mudar sempre que os parâmetros mudarem. Logo, implemente uma verificação no código. No exemplo abaixo, verificamos se a categoria informada é a mesma 
da informada na última vez (guardada em cache). Em caso positivo retorna a lista em cache, ou seja, nem faz a consulta na base de dados:
```
public IEnumerable<Produto> ListarProdutos(string grupo)
{       
    var meuCache = _cache.TryGetValue("ChaveProdutosEmCache", out IEnumerable<Produto> produtos);
    if (meuCache != null)
    {
        if (produtos != null && produtos.Count > 0) {
            for (var i = 0; i <= 1; i++) {
                if (produtos[i].Grupo == grupo)
                    return produtos;    
            }
        }

    }

    produtos = _context.Produto.Where(x => x.Grupo == grupo).ToList();

    _cache.Set("ChaveProdutosEmCache", produtos, DateTime.Now.AddMinutes(30));       

    return produtos;        
}

```


## Cache Individual

O cache In Memory do Asp.NET Core ocorre na memória do servidor. Isso quer dizer que esse mesmo cache é compartilhado por todos os usuários do sistema. 

Isso não seria um problema, a não ser que os dados sejam gerados a partir de parâmetros informados pelo usuário. Nesse caso o cache deve mudar sempre que 
os parâmetros mudarem.  

Para que cada usuário tenha seu próprio cache, basta dar um nome próprio à chave do cache. Você pode usar o nome do usuário logado como a chave do cache.

No exemplo a baixo (com base no exemplo anterior) ccncatenamos o nome do usuário logado com outra palavra para formar a chave do cache:
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
    
    public IEnumerable<Produto> ListarProdutos(decimal preco, string usuario)
    {       
        var meuCache = _cache.TryGetValue("ChaveProdutosEmCache"+usuario, out IEnumerable<Produto> produtos);
        if (meuCache != null)
        {
            if (produtos != null && produtos.Count > 0)
                return produtos;                     
        }
        
        produtos = _context.Produto.ToList();
        
        _cache.Set("ChaveProdutosEmCache"+usuario, produtos, DateTime.Now.AddMinutes(30));       
        
        return produtos;        
    }
}
```




## View Component
```
<cache expires-on="@TimeSpan.FromSeconds(600)">
    @await Component.InvokeAsync("BlogPosts", new { tag = "popular" })
</cache>
```
