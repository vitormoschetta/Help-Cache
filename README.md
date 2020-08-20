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


