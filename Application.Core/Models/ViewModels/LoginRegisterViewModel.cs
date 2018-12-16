namespace Application.Core.Models.ViewModels
{
    public class LoginRegisterViewModel
    {
        public LoginViewModel loginViewModel { get; set; }
        
        public RegisterViewModel registerViewModel { get; set; }

        public LoginRegisterViewModel()
        {
            loginViewModel=new LoginViewModel();
            registerViewModel=new RegisterViewModel();
        }
        
        
    }
}