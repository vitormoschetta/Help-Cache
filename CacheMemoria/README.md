## Cache local

Neste exemplo usamos _cache_ local. Ou seja, os dados ficam na memória do mesmo servidor que roda a aplicação.

Estamos utilizando uma base de dados SQLite com 50 mil pedidos (Orders) cadastrados. 

Quando navegamos para a página 'Orders' através do Menu, todos estes pedidos são retornados e paginados.

Se navegarmos para a página 'OrdersInCache' estes mesmos 50 mil pedidos são consultados, porém após a primeira consulta os dados ficam 
armazenados em cache. **Observe o tempo de resposta** a partir do segundo request feito na página 'OrdersInCache'.