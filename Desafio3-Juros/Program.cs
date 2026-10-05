using System;
using System.Globalization;
using System.Threading;

namespace Desafio3Juros
{
    class Program
    {
        // Multa/juros de 2,5% ao dia sobre o valor, conforme enunciado.
        const decimal TaxaJurosDiaria = 0.025m;

        static void Main(string[] args)
        {
            var cultura = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = cultura;

            Console.WriteLine("===== CALCULO DE JUROS POR ATRASO =====\n");

            Console.Write("Informe o valor: R$ ");
            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, cultura, out decimal valor) || valor < 0)
            {
                Console.WriteLine("Valor invalido.");
                return;
            }

            Console.Write("Informe a data de vencimento (dd/mm/aaaa): ");
            if (!DateTime.TryParse(Console.ReadLine(), cultura, DateTimeStyles.None, out DateTime vencimento))
            {
                Console.WriteLine("Data invalida. Use o formato dd/mm/aaaa.");
                return;
            }

            DateTime hoje = DateTime.Today;
            int diasAtraso = (hoje - vencimento.Date).Days;

            Console.WriteLine();
            Console.WriteLine($"Data de hoje:        {hoje:dd/MM/yyyy}");
            Console.WriteLine($"Data de vencimento:  {vencimento:dd/MM/yyyy}");

            if (diasAtraso <= 0)
            {
                Console.WriteLine("O titulo ainda nao venceu. Nao ha juros a cobrar.");
                Console.WriteLine($"Valor a pagar: {valor:C}");
                return;
            }

            decimal juros = valor * TaxaJurosDiaria * diasAtraso;
            decimal total = valor + juros;

            Console.WriteLine($"Dias em atraso:      {diasAtraso}");
            Console.WriteLine($"Valor original:      {valor:C}");
            Console.WriteLine($"Juros (2,5% a.d.):   {juros:C}");
            Console.WriteLine($"Valor total a pagar: {total:C}");
        }
    }
}
