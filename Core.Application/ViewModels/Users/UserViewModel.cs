
using Core.Application.ViewModels.Beneficiarios;
using Core.Application.ViewModels.Products;

namespace Core.Application.ViewModels.Users
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public int? Status { get; set; }

        public string? ImagePath { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string? ActivationKey { get; set; }

        public int? IdCard { get; set; }

        public int UserType { get; set; }

        //Navigation Properties
        public List<ProductViewModel>? Products { get; set; }
        public List<BeneficiarioViewModel>? Beneficiarios { get; set; }
    }
}
