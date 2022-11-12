using AutoMapper;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;


namespace Core.Application.Services
{
    public class UserService : GenericService<SaveUserViewModel, UserViewModel, User>, IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserViewModel>> GetAllViewModelWithInclude()
        {

            var userList = await _repository.GetAllWithInclude(new List<string> { "Users" });
            return userList.Select(user => new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Status = user.Status,
                ImagePath = user.ImagePath,
                Username = user.Username,
                Email = user.Email,
                ActivationKey = user.ActivationKey,
                IdCard = user.IdCard,
                UserType = user.UserType,
            }).ToList();
        }
    }
}
