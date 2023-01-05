using AdaCredit.UI;
using AdaCredit.Utils;
using Sharprompt;
using System;

namespace AdaCredit
{
    class AdaCreditApplication
    {
        static void Main(string[] args)
        {
            Console.WriteLine("O sistema já possui alguns dados registrados nos " +
                "sub-diretórios \"Resources\" para simular um funcionamento real. No entanto," +
                "se desejar, você pode gerar dados aleatórios (sempre haverá um user \"paulo\" para fazer o login) " +
                "para o sistema confimando na opção abaixo,ou mesmo " +
                "pode começar o uso \"do zero\" apagando os sub-diretórios supracitados.");
            var answer = Prompt.Confirm($"Deseja gerar dados aleatórios?", defaultValue: true);
            Console.Clear();
            UserInterface.Login(args, answer);
        }
    }
}