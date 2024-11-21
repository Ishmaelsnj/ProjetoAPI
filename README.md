
# Documentação da API KML

## Visão Geral

Esta API permite que os usuários interajam e filtrem dados de um arquivo KML (DIRECIONADORES1.kml). Ela suporta filtragem com base em critérios específicos e oferece a funcionalidade de exportar os dados filtrados, recuperá-los no formato JSON ou visualizar as opções disponíveis para filtragem.

## Configuração

### AppSettings

A API requer configurações específicas no arquivo appsettings.json para definir os diretórios do arquivo KML. Atualize as configurações conforme abaixo:

```json
{
   "KmlConfig": {
    "DiretorioLeitura": "C:\\Caminho\\Para\\DIRECIONADORES1.kml",
    "DiretorioEscrita": "C:\\Caminho\\Para\\Saida\\KmlFiltrado.kml"
  }
}
```

## DiretorioLeitura
Caminho do arquivo KML de entrada.

## DiretorioEscrita
Caminho onde o arquivo KML filtrado será exportado. Este caminho deve incluir o nome do arquivo (ex.: `KmlFiltrado.kml`).

### Endpoints

1. **Exportar Arquivo KML Filtrado**

    **URL:** `/api/KML/export`

    **Método:** POST

    **Descrição:** Exporta um novo arquivo KML com base nos filtros aplicados.

    **Corpo da Requisição:**

    ```json
    {
      "Cliente": "Valor",
      "Situacao": "Valor",
      "Bairro": "Valor",
      "Referencia": "TextoParcial",
      "RuaCruzamento": "TextoParcial"
    }
    ```

    **Respostas:**

    - `200 OK`: Arquivo KML exportado com sucesso.
    - `400 Bad Request`: Caso os critérios de filtragem sejam inválidos.

2. **Listar Elementos Filtrados em JSON**

    **URL:** `/api/KML/list`

    **Método:** POST

    **Descrição:** Retorna uma lista de elementos filtrados no formato JSON.

    **Corpo da Requisição:**

    ```json
    {
      "Cliente": "Valor",
      "Situacao": "Valor",
      "Bairro": "Valor",
      "Referencia": "TextoParcial",
      "RuaCruzamento": "TextoParcial"
    }
    ```

    **Respostas:**

    - `200 OK`: Array JSON contendo os elementos filtrados.
    - `400 Bad Request`: Caso os critérios de filtragem sejam inválidos.

3. **Obter Opções de Filtragem Disponíveis**

    **URL:** `/api/KML/filters`

    **Método:** GET

    **Descrição:** Recupera os valores únicos para os seguintes campos:
    
    - CLIENTE
    - SITUAÇÃO
    - BAIRRO

    **Respostas:**

    - `200 OK`: Objeto JSON contendo os valores únicos de cada campo filtrável.

### Critérios de Filtragem

#### Campos Filtráveis

- **CLIENTE:** Lista predefinida de valores.
- **SITUAÇÃO:** Lista predefinida de valores.
- **BAIRRO:** Lista predefinida de valores.
- **REFERENCIA:** Busca por texto parcial (mínimo de 3 caracteres).
- **RUA/CRUZAMENTO:** Busca por texto parcial (mínimo de 3 caracteres).

#### Validação

- **Campos de pré-seleção (CLIENTE, SITUAÇÃO, BAIRRO):** Devem conter apenas valores disponíveis nas opções de filtragem. Caso sejam inválidos, a API retorna um `400 Bad Request` com uma mensagem de erro descritiva.

- **Campos de busca por texto (REFERENCIA, RUA/CRUZAMENTO):** Requerem no mínimo 3 caracteres. Caso não atendam a esse requisito, a API retorna um `400 Bad Request` com uma mensagem de erro descritiva.

#### Respostas de Erro

- **400 Bad Request:**
  
  Retornado quando os filtros são inválidos ou mal formatados.
  
  **Exemplo de resposta:**

  ```json
  {
    "erro": "O filtro REFERENCIA deve ter pelo menos 3 caracteres."
  }
  ```

### Controller: KMLController.cs

A controller é responsável por todas as operações relacionadas ao KML, garantindo validação e execução adequadas para filtragem e exportação.

## Exemplos de Uso

### Exportar Arquivo KML Filtrado

**Requisição:**

- **POST /api/KML/export**
- **Content-Type:** `application/json`

```json
{
  "Cliente": "COESI",
  "Situacao": "ATIVO",
  "Bairro": "Jardins",
  "Referencia": "CANTE",
  "RuaCruzamento": "AV. JORN"
}
```

**Resposta:**

- `200 OK`: Arquivo KML filtrado exportado no diretório especificado.

### Listar Elementos Filtrados

**Requisição:**

- **POST /api/KML/list**
- **Content-Type:** `application/json`

```json
{
  "Cliente": "COESI",
  "Situacao": "ATIVO",
  "Bairro": "Jardins",
  "Referencia": "CANTE",
  "RuaCruzamento": "AV. JORN"
}
```

**Resposta:**

```json
[
  {
    "Name": "Ponto 11",
    "Cliente": "COESI",
    "Situacao": "ATIVO",
    "Bairro": "Jardins",
    "Referencia": "CANTEIRO",
    "RuaCruzamento": "AV. JORNALISTA SANTOS SANTANA X AV. PEDRO VALADARES"
  }
]
```

### Obter Opções de Filtragem

**Requisição:**

- **GET /api/KML/filters**

**Resposta:**

```json
{
  "Cliente": ["COESI", "OUTRO_CLIENTE"],
  "Situacao": ["ATIVO", "INATIVO"],
  "Bairro": ["Jardins", "Centro"]
}
```
