namespace MeuApp
{
  public struct PessoaDTO
  {

    public PessoaDTO(string name, string sobrenome, int idade, EEstadoCivil estadoCivil)
    {
      Name = name;
      Sobrenome = sobrenome;
      Idade = idade;
      EstadoCivil = estadoCivil;
    }

    public string Name { get; set; }
    public string Sobrenome { get; set; }

    public EEstadoCivil EstadoCivil { get; set; }
    public int Idade { get; set; }
  }

  public enum EEstadoCivil
  {
    Casado = 1,
    Solteiro = 2,
    Viuvo = 3,
    Divorciado = 4
  }

}