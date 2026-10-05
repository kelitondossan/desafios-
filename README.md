# Desafios — Processo Seletivo (C#)

Repositório com a resolução dos 3 desafios propostos, implementados em **C# (.NET 8)**.
Cada desafio é um projeto de console independente, que pode ser executado separadamente.

## Estrutura do repositório

```
├── Desafio1-Comissao/
│   ├── Desafio1-Comissao.csproj
│   ├── Program.cs
│   └── vendas.json              # dados de vendas fornecidos no enunciado
├── Desafio2-Estoque/
│   ├── Desafio2-Estoque.csproj
│   ├── Program.cs
│   └── estoque.json             # dados de estoque fornecidos no enunciado
├── Desafio3-Juros/
│   ├── Desafio3-Juros.csproj
│   └── Program.cs
└── README.md
```

## Pré-requisitos

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)

## Como executar

Entre na pasta do desafio desejado e execute `dotnet run`:

```bash
cd Desafio1-Comissao && dotnet run
cd Desafio2-Estoque  && dotnet run
cd Desafio3-Juros    && dotnet run
```

---

## Desafio 1 — Comissão de vendas

Programa que lê o arquivo `vendas.json` (registros de vendas do time comercial) e
calcula a comissão total de cada vendedor, aplicando a regra **por venda**:

| Faixa do valor da venda      | Comissão |
|------------------------------|----------|
| Abaixo de R$ 100,00          | 0%       |
| De R$ 100,00 até R$ 499,99   | 1%       |
| A partir de R$ 500,00        | 5%       |

### Saída esperada

```
========== RELATORIO DE COMISSOES ==========

Vendedor               Total vendido    Comissao
--------------------------------------------------
João Silva             R$ 10.754,70     R$ 495,68
Maria Souza            R$ 9.874,30      R$ 465,95
Carlos Oliveira        R$ 7.928,35      R$ 379,37
Ana Lima               R$ 8.763,95      R$ 404,98
```

### Observações da implementação

- O JSON é desserializado com `System.Text.Json` (nativo do .NET, sem dependências externas).
- Valores monetários usam `decimal` para evitar erros de arredondamento de ponto flutuante.
- O arquivo `vendas.json` é copiado automaticamente para a pasta de saída no build.

---

## Desafio 2 — Movimentação de estoque

Programa de console interativo para lançar **entradas** e **saídas** de mercadoria
nos produtos do arquivo `estoque.json`.

Cada movimentação possui:

- **Identificador único** — número sequencial gerado automaticamente (1, 2, 3, ...);
- **Descrição** — informada pelo usuário para identificar o tipo da movimentação
  (ex.: "ENTRADA - Compra de fornecedor", "SAIDA - Venda balcão");
- **Retorno da quantidade final** — ao final de cada lançamento o programa exibe
  o saldo atualizado do produto movimentado.

### Fluxo de uso

1. O programa lista os produtos com código, descrição e saldo atual.
2. Informe o **código do produto** (ou `0` para encerrar).
3. Informe o **tipo**: `E` (entrada) ou `S` (saída).
4. Informe a **quantidade** e uma **descrição** para a movimentação.
5. O programa registra a movimentação e exibe a quantidade final em estoque.
6. Ao encerrar (`0`), é exibido um resumo de todas as movimentações realizadas.

### Exemplo de execução

```
===== MOVIMENTACAO DE ESTOQUE =====

Codigo | Produto                      | Estoque
--------------------------------------------------
   101 | Caneta Azul                  |     150
   102 | Caderno Universitário        |      75
   103 | Borracha Branca              |     200
   104 | Lápis Preto HB               |     320
   105 | Marcador de Texto Amarelo    |      90

Codigo do produto (0 para sair): 102
Tipo da movimentacao (E = entrada, S = saida): S
Quantidade: 10
Descricao da movimentacao: Venda balcao

Movimentacao registrada com sucesso!
Produto: 102 - Caderno Universitário
Quantidade final em estoque: 65
```

### Validações implementadas

- Código de produto inexistente é rejeitado;
- Quantidade deve ser inteiro maior que zero;
- Saída com quantidade maior que o saldo em estoque é bloqueada
  (o estoque nunca fica negativo);
- Tipo de movimentação diferente de `E`/`S` é rejeitado.

---

## Desafio 3 — Cálculo de juros por atraso

Programa que recebe um **valor** e uma **data de vencimento** e calcula os juros
devidos na data de hoje, considerando **multa de 2,5% ao dia** de atraso
(juros simples sobre o valor original).

### Fórmula

```
juros = valor × 0,025 × dias em atraso
dias em atraso = data de hoje − data de vencimento
```

Se o vencimento for hoje ou uma data futura, nenhum juros é cobrado.

### Exemplo de execução

```
===== CALCULO DE JUROS POR ATRASO =====

Informe o valor: R$ 1000
Informe a data de vencimento (dd/mm/aaaa): 20/09/2026

Data de hoje:        05/10/2026
Data de vencimento:  20/09/2026
Dias em atraso:      15
Valor original:      R$ 1.000,00
Juros (2,5% a.d.):   R$ 375,00
Valor total a pagar: R$ 1.375,00
```

---

## Decisões técnicas

- **Linguagem:** C# com .NET 8 (LTS);
- **JSON:** `System.Text.Json` — biblioteca nativa, sem pacotes NuGet externos;
- **Valores monetários:** tipo `decimal` (precisão exata para dinheiro);
- **Cultura:** `pt-BR` fixada no início de cada programa para exibir valores no
  formato `R$ 1.234,56` independente da configuração da máquina;
- **Estrutura:** cada desafio em seu próprio projeto para permitir execução
  isolada com `dotnet run`.
