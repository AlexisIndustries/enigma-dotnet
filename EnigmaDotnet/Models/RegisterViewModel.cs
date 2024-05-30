namespace EnigmaDotnet.Models;

using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required] [Display(Name = "Email")] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string PasswordConfirm { get; set; }

    [EnumDataType(typeof(Role))] 
    [Display(Name = "Роль пользователя")]
    public Role Role { get; set; }
}