using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Desafio1Comissao
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            string caminhoJson = Path.Combine(AppContext.BaseDirectory, "vendas.json");
            string json = File.ReadAllText(caminhoJson);

            RegistroVendas? dados = JsonSerializer.Deserialize<RegistroVendas>(json);

            if (dados?.Vendas == null || dados.Vendas.Count == 0)
            {
                Console.WriteLine("Nenhuma venda encontrada no arquivo.");
                return;
            }

            var comissoes = new Dictionary<string, decimal>();
            var totaisVendidos = new Dictionary<string, decimal>();
            var vendedores = new List<string>();

            foreach (Venda venda in dados.Vendas)
            {
                if (!comissoes.ContainsKey(venda.Vendedor))
                {
                    comissoes[venda.Vendedor] = 0m;
                    totaisVendidos[venda.Vendedor] = 0m;
                    vendedores.Add(venda.Vendedor);
                }

                comissoes[venda.Vendedor] += CalcularComissao(venda.Valor);
                totaisVendidos[venda.Vendedor] += venda.Valor;
            }

            Console.WriteLine("========== RELATORIO DE COMISSOES ==========\n");
            Console.WriteLine($"{"Vendedor",-20} {"Total vendido",15} {"Comissao",12}");
            Console.WriteLine(new string('-', 50));

            foreach (string vendedor in vendedores)
            {
                Console.WriteLine($"{vendedor,-20} {totaisVendidos[vendedor],15:C} {comissoes[vendedor],12:C}");
            }
        }

        static decimal CalcularComissao(decimal valorVenda)
        {
            if (valorVenda < 100m)
                return 0m;

            if (valorVenda < 500m)
                return valorVenda * 0.01m;

            return valorVenda * 0.05m;
        }
    }

    class RegistroVendas
    {
        [JsonPropertyName("vendas")]
        public List<Venda> Vendas { get; set; } = new List<Venda>();
    }

    class Venda
    {
        [JsonPropertyName("vendedor")]
        public string Vendedor { get; set; } = string.Empty;

        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }
}
