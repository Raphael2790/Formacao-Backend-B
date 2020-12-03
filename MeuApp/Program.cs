using System;
using System.Collections.Generic;
using static MeuApp.PessoaDTO;

namespace MeuApp
{
  class Program
  {
    static void Main(string[] args)
    {
      var pessoa1 = new PessoaDTO();
      pessoa1.Name = "Raphael";
      pessoa1.Sobrenome = "Silvestre";
      pessoa1.Idade = 29;
      pessoa1.EstadoCivil = EEstadoCivil.Solteiro;

      var pessoa2 = new PessoaDTO("Monica", "Silva", 26, EEstadoCivil.Casado);

      List<PessoaDTO> listaPessoas = new List<PessoaDTO>();

      listaPessoas.Add(pessoa1);
      listaPessoas.Add(pessoa2);

      foreach (var pessoa in listaPessoas)
      {
        System.Console.WriteLine($"{pessoa.Name} {pessoa.Sobrenome} tem {pessoa.Idade} anos e atualmente é {pessoa.EstadoCivil.ToString().ToLower()}");
      }
    }
  }
}
