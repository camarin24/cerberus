using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Entities;
using Intecgra.Cerberus.Domain.Exceptions;
using Intecgra.Cerberus.Domain.Ports.Auth;
using Intecgra.Cerberus.Domain.Ports.Data;
using Intecgra.Cerberus.Domain.Utilities;

namespace Intecgra.Cerberus.Domain.Services.Auth
{
    [DomainService]
    public class UserService : BaseService<User, UserDto>, IUserService
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IMapper _mapper;
        private readonly IMessagesManager _messagesManager;
        private readonly IClientService _clientService;

        public UserService(IGenericRepository<User> repository, IMapper mapper, IMessagesManager messagesManager,
            IClientService clientService) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _messagesManager = messagesManager;
            _clientService = clientService;
        }

        public new async Task<UserDto> Create(UserDto dto)
        {
            dto.Salt = PasswordHash.CreateSalt();
            dto.Password = PasswordHash.Create(dto.Password, dto.Salt);
            return await Save(dto);
        }


        /// <summary>
        /// Login the user through the specific client database 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DomainException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<AuthorizationDto> Login(LoginDto request)
        {
            var users = (await GetFilter(u => u.Email.Equals(request.Email))).ToList();
            if (users == null || !users.Any()) throw new DomainException(_messagesManager.GetMessage("InvalidUser"));
            var user = users.FirstOrDefault(m => PasswordHash.Validate(request.Password, m.Password, m.Salt));
            if (user == null) throw new DomainException(_messagesManager.GetMessage("InvalidPassword"));

            var client = await _clientService.GetClientByAppIdAndId(request.AppId, user.ClientId);


            return null;
        }

        public async Task<IEnumerable<UserDto>> GetFilter(Expression<Func<User, bool>> filter)
        {
            return _mapper.Map<IEnumerable<UserDto>>(await _repository.Get(filter));
        }
    }
}