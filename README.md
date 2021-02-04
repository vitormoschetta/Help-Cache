# Cache

## Tipos de memória

Antes de falarmos de _cache_ precisamos expor o conceito sobre os dois grupos básicos de memória:

**1. Memória Temporária**:  
São memórias voláteis, isto é, perdem seus dados com ausência de energia. Como exemplos temos a memória RAM e a memória _Cache_ (aqui estamos falando da memória _cache_ física que fica junto ao processador na placa do microcomputador e não da estratégia de _cache_ em aplicações, que falaremos mais adiante).  

**2. Memória Permanente**:
São memórias não voltáteis, isto é, não perdem seus dados na ausência de energia. Como exemplos temos o disco rígido (HD) e sua evolução, o SSD (Solid State Drive).
<br>
<br>


## Cache, em termos gerais
_Cache_ é qualquer tipo de implementação de memória cuja o objetivo seja aumentar o tempo de resposta.
Com os avanços tecnológicos, vários tipos de cache foram desenvolvidos. Atualmente há cache em processadores, discos rígidos, sistemas, servidores, nas placas-mãe, clusters de bancos de dados, entre outros. 

- No caso dos **processadores**, o cache disponibiliza alguns dados já requisitados anteriormente à memoria RAM/Disco Rígido; 

- No caso dos **navegadores web**, o _cache_ mantém o conteúdo estático (HTML, CSS, JavaScript, imagens, etc) em memória local para evitar consultas constantes à rede;

- Os **servidores de aplicação** também podem dispor de _caches_ configurados pelo administrador. Neste caso o _cache_ mantém dados em memória para melhorar o desempenho de consultas à base de dados, por exemplo.
<br>
<br>


## Cache de aplicação
É sobre _cache_ de aplicação e/ou servidores que vamos tratar.  
Como podemos melhorar o desempenho de nossas aplicações utilizando estratégias de _cache_?

Dependendo da quantidade e/ou do tipo de informações pesquisadas, essa consulta no banco de dados pode se tornar lenta. E repetir essas consultas constantemente acabará se tornando uma tarefa pesada e trará uma péssima experiência ao usuário.

### Como o cache de aplicação funciona?
Vamos montar um cenário para entender como o cache de aplicação é implementado na sua forma mais básica:

Os dados são gravados em uma memória permanente (banco de dados por exemplo). O Administrador configura que uma determinada consulta de dados precisará primeiro passar pelo _cache_. Existindo as informações nesse cache, devem elas serem retornadas ao usuário dali mesmo. Não existindo, o sistema deverá buscá-las no banco de dados, então gravá-las em cache e retornar ao usuário. 

Perceba que aquela consulta pesada à base de dados só será feita uma vez, a partir dali estes dados já estarão disponíveis no _cache_.


<a name="expiracao"></a>

### Dados passíveis de alteração
Caso estes dados sejam passíveis de alteração, um tempo deve ser configurado para que o _cache_ seja invalidado e renovado, ou seja, cada vez que o _cache_ atingir um determinado período de tempo uma nova consulta é feita à base de dados.

Essa estratégia de _cache_ é conhecida como **Write-Around**.

Perceba que desta forma as alterações efetuadas na base de dados podem não ficar imediatamente disponíveis, afinal, existe um tempo que o _cache_ deve respeitar para ser atualizado. 

Em alguns cenários isso não acarretaria em muitos problemas, e, dependendo do negócio, o tempo de _cache_ pode ser ajustado e pronto, tudo certo. Em outros casos, porém, a **inconsistência** destes dados podem causar sérios problemas. 


<a name="inconsistencia"></a>

### Inconsistência de dados
Para resolver o problema de Inconsistência de Dados descrito acima, existem outras estratégias de _cache_ que podem ser implementadas:

<a name="Write-Through"></a>

- **Write-Through**:  
Uma alteração é gravada tanto na base de dados quanto no _cache_.   
Vantagem: Alto nível de disponibilidade e consistência de dados.  
Desvantagem: Aumento de latência na escrita.


<a name="Write-Back"></a>

- **Write-Back**
Escreve apenas no _cache_. As informações são gravadas em uma base de dados de tempos em tempos, conforme configurado.  

Para o segundo cenário o risco é mais alto, uma vez que um desligamento de máquina resultaria em perda total dos dados ainda não persistidos em uma base segura. 

Por este motivo essa estratégia só possui sentido se utilizarmos **Servidores de Cache**, ou seja, o _cache_ fica na memória de servidores próprios para esta tarefa. Eles replicam a mesma informação. Se possível, estes servidores devem ser mantidos em diferentes áreas geográficas.

Perceba agora que podemos dividir em duas novas categorias os tipos/estratégias de _cache_: **Cache Local** e **Cache Distribuido**


1. **Cache Local**   
O _cache_ está na memória do mesmo servidor que está rodando a aplicação.

2. **Cache Distribuído**  
O _cache_ está na memória de servidor(s) separado(s) do servidor que está rodando a aplicação.

É importante observar que qualquer uma das estratégias de _cache_ descritas anteriormente (Write-Around, Write-Through e Write-Back) podem ser aplicadas tanto em _Cache_ Local como em _Cache_ Distribuído. Cada uma, porém, tem suas vantagens, desvantagens e cuidados a serem observados.
<br>
<br>



## Recapitulando

_Cache_ é qualquer tipo de implementação de memória cuja o objetivo seja aumentar o tempo de resposta.

O _cache_ pode ser **Local** ou **Distribuído** (Servidor(es) de _Cache_).

Se os dados forem **[passíveis de alteração](#expiracao)** o _cache_ precisa ter um **tempo de expiração** para ser renovado. 

Se a menor **[Inconsistência de Dados](#inconsistencia)** for algo prejudicial ao negócio, como alternativa ao **tempo de expiração**, podemos implementar estratégias mais avançadas de _cache_, **[Write-Through](#Write-Through)** e **[Write-Back](#Write-Back)** por exemplo.
<br>
<br>



## Cache no .NET Core
No .NET Core podemos utilizar qualquer um dos conceitos já estudados até aqui. 

### **Cache Local**  
No diretório **CacheLocal/** temos uma aplicação MVC que implementa o _cache_ local, aquele mais simples. 


### **Cache Distribuído**  

#### **Distributed SQL Server Cache**

No .NET Core, além das estratégias de _cache_ já estudadas até aqui, podemos implementar ainda um outro método de _cache_ distribuído: **Distributed SQL Server Cache**, ou Cache SQL Server distribuído.

Veja esse tipo de implementação e leia mais sobre, no diretório "**CacheDistribuido/SqlServerCache/**".
<br>


#### Distributed Redis Cache




https://docs.microsoft.com/pt-br/aspnet/core/performance/caching/memory?view=aspnetcore-5.0
https://docs.microsoft.com/pt-br/aspnet/core/performance/caching/distributed?view=aspnetcore-5.0
