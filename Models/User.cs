using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SHOP.Models
{
    [Table("Users")]
    public class User
    {   
        [Key]
        [Column("User_Id")]
        [DataType("int")]
        public int Id { get; set; }

        [Required(ErrorMessage="Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage="Este campo deve conter entre 3 e 20 caracteres")]
        [MinLength(3, ErrorMessage="Este campo deve conter entre 3 e 20 caracteres")]
        [Column("UserName")]
        [DataType("nvarchar")]
        public string UserName { get; set; }

        [Required(ErrorMessage="Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage="Este campo deve conter entre 3 e 20 caracteres")]
        [MinLength(3, ErrorMessage="Este campo deve conter entre 3 e 20 caracteres")]
        [Column("Password")]
        [DataType("nvarchar")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}