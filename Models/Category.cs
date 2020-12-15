using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SHOP.Models
{   
    //Data Annotation Schema
    [Table("Categories")]
    public class Category
    {   
        //Data Annotation 
        [Key]
        [Column("Cat_id")]
        [DataType("int")]
        public int Id { get; set; }
        [Required(ErrorMessage= "Esse campo é obrigatório")]
        [MaxLength(60, ErrorMessage= "Esse campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage= "Esse campo deve conter entre 3 e 60 caracteres")]
        [DataType("nvarchar")]
        [Column("Category_name")]
        public string Title { get; set; }
    }   
    
}