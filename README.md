# Help-Cache

```
public class Product
{
    private readonly IMemoryCache _cache;
    private readonly ApplicationDbContext _context;
    
    public Product(IMemoryCache cache, ApplicationDbContext context)
    {
        _context = context;
        _cache = cache;
    }
    
    public IEnumerable<Product> GetProducts()
    {            
        if (_cache.TryGetValue("listaProdutosCache", out IEnumerable<Product> products))
        {
            if (products != null && products.Count > 0)
                return products;                     
        }
        
        products = _context.Product.ToList();
        
        _cache.Set("listaProdutosCache", products, DateTime.Now.AddMinutes(30));       
        
        return products        
    }
}
```

Vamos identificar as partes do nosso código:
```
_cache.Set("listaProdutosCache", products, DateTime.Now.AddMinutes(30));  
```
O código acima seta a nossa lista (products) em memória, dando a ela uma chave de identificação chamada "listaProdutosCache".
O terceiro parâmetro é o tempo que a lista deve permanecer em memória. Em nosso exemplo, 30 minutos. 

