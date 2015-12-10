using System.ComponentModel.DataAnnotations;
namespace SlarkInc.Models
{
    
    public class Simple
    {
        [Display(Name = "Name")]       
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}