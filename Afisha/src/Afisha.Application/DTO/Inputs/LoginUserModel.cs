using System.ComponentModel.DataAnnotations;

namespace Afisha.Application.DTO.Inputs
{
    public record LoginUserModel(    
        [Required] string Email,
        [Required] string Password);    
}
