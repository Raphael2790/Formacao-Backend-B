using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SHOP.Models
{   
    [Table("Products")]
    public class Product
    {
        [Key]
        [Column("Product_Id")]
        [DataType("int")]
        public int Id { get; set; }

        [Required(ErrorMessage="Este campo é obrigatório")]
        [MinLength(3, ErrorMessage="Esse campo deve conter entre 3 e 60 caracteres")]
        [MaxLength(60, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        [Column("Product_Title")]
        [DataType("nvarchar")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage="Este campo deve conter no máximo 1024 caracteres")]
        [DataType("nvarchar")]
        [Column("Product_Description")]
        public string Description { get; set; }

        [Required(ErrorMessage="Este compo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage="O preço deve ser maior que zero")]
        [DataType("real")]
        [Column("Product_Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage="Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage="Categoria inválida")]
        [Column("Product_CategoryId")]
        [DataType("int")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}