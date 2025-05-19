namespace BlogV1.Models.ViewModels
{
    public class RegisterViewModel                              // register sayfamızadki verileri almak için viewmodel oluşturduk .
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
