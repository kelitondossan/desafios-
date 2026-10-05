using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Desafio2Estoque
{
    class Program
    {
        static int _proximoId = 1;
        static readonly List<Movimentacao> _movimentacoes = new List<Movimentacao>();

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            string caminhoJson = Path.Combine(AppContext.BaseDirectory, "estoque.json");
            string json = File.ReadAllText(caminhoJson);

            RegistroEstoque? dados = JsonSerializer.Deserialize<RegistroEstoque>(json);

            if (dados?.Estoque == null || dados.Estoque.Count == 0)
            {
                Console.WriteLine("Nenhum produto encontrado no arquivo de estoque.");
                return;
            }

            List<Produto> produtos = dados.Estoque;

            Console.WriteLine("===== MOVIMENTACAO DE ESTOQUE =====");

            while (true)
            {
                ExibirProdutos(produtos);

                Console.Write("\nCodigo do produto (0 para sair): ");
                string? entrada = Console.ReadLine();

                if (entrada == null || entrada.Trim() == "0")
                    break;

                if (!int.TryParse(entrada, out int codigo))
                {
                    Console.WriteLine("Codigo invalido.");
                    continue;
                }

                Produto? produto = produtos.Find(p => p.CodigoProduto == codigo);
                if (produto == null)
                {
                    Console.WriteLine("Produto nao encontrado.");
                    continue;
                }

                Console.Write("Tipo da movimentacao (E = entrada, S = saida): ");
                string? tipo = Console.ReadLine()?.Trim().ToUpperInvariant();
                if (tipo != "E" && tipo != "S")
                {
                    Console.WriteLine("Tipo invalido. Use E ou S.");
                    continue;
                }

                Console.Write("Quantidade: ");
                if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
                {
                    Console.WriteLine("Quantidade invalida. Informe um numero inteiro maior que zero.");
                    continue;
                }

                if (tipo == "S" && quantidade > produto.Estoque)
                {
                    Console.WriteLine($"Saldo insuficiente: '{produto.DescricaoProduto}' possui apenas {produto.Estoque} unidade(s) em estoque.");
                    continue;
                }

                Console.Write("Descricao da movimentacao: ");
                string descricao = Console.ReadLine()?.Trim() ?? string.Empty;
                if (descricao.Length == 0)
                    descricao = tipo == "E" ? "Entrada de mercadoria" : "Saida de mercadoria";

                int estoqueFinal = RegistrarMovimentacao(produto, tipo, quantidade, descricao);

                Console.WriteLine("\nMovimentacao registrada com sucesso!");
                Console.WriteLine($"Produto: {produto.CodigoProduto} - {produto.DescricaoProduto}");
                Console.WriteLine($"Quantidade final em estoque: {estoqueFinal}");
            }

            if (_movimentacoes.Count > 0)
            {
                Console.WriteLine("\n===== RESUMO DAS MOVIMENTACOES =====");
                Console.WriteLine($"{"ID",4} | {"Descricao",-40} | {"Produto",7} | {"Qtde",5} | {"Estoque final",13}");
                Console.WriteLine(new string('-', 82));
                foreach (Movimentacao m in _movimentacoes)
                {
                    Console.WriteLine($"{m.Id,4} | {m.Descricao,-40} | {m.CodigoProduto,7} | {m.Quantidade,5} | {m.EstoqueFinal,13}");
                }
            }

            Console.WriteLine("Programa encerrado.");
        }
        static int RegistrarMovimentacao(Produto produto, string tipo, int quantidade, string descricao)
        {
            if (tipo == "E")
                produto.Estoque += quantidade;
            else
                produto.Estoque -= quantidade;

            var movimentacao = new Movimentacao
            {
                Id = _proximoId++,
                Descricao = $"{(tipo == "E" ? "ENTRADA" : "SAIDA")} - {descricao}",
                CodigoProduto = produto.CodigoProduto,
                Quantidade = quantidade,
                EstoqueFinal = produto.Estoque
            };

            _movimentacoes.Add(movimentacao);

            return produto.Estoque;
        }

        static void ExibirProdutos(List<Produto> produtos)
        {
            Console.WriteLine($"\n{"Codigo",6} | {"Produto",-28} | {"Estoque",7}");
            Console.WriteLine(new string('-', 50));
            foreach (Produto p in produtos)
            {
                Console.WriteLine($"{p.CodigoProduto,6} | {p.DescricaoProduto,-28} | {p.Estoque,7}");
            }
        }
    }

    class Movimentacao
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int CodigoProduto { get; set; }
        public int Quantidade { get; set; }
        public int EstoqueFinal { get; set; }
    }

    class RegistroEstoque
    {
        [JsonPropertyName("estoque")]
        public List<Produto> Estoque { get; set; } = new List<Produto>();
    }

    class Produto
    {
        [JsonPropertyName("codigoProduto")]
        public int CodigoProduto { get; set; }

        [JsonPropertyName("descricaoProduto")]
        public string DescricaoProduto { get; set; } = string.Empty;

        [JsonPropertyName("estoque")]
        public int Estoque { get; set; }
    }
}
