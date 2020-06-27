using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.DAL.Models;
using api.DAL.Repositories;
using api.Services.Extensions;

namespace api.Services.Auth
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly GroupRepository _groupRepository;
        private readonly RoleRepository _roleRepository;
        private readonly TokenService _tokenService;

        public static readonly string[] DefaultRolesNames = new string[]
        {
            "group_m",
            "finan_w",
            "finan_r",
            "finan_a",
            "msg"
        };

        public UserService(UserRepository userRepository, GroupRepository
            groupRepository, RoleRepository roleRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _roleRepository = roleRepository;
            _tokenService = tokenService;
        }

        public async Task<Group> CreateNewGroup(string groupName)
        {
            return await _groupRepository.InsertAsync(new Group {Name = groupName});
        }

        public async Task<Role> CreateNewRole(string roleName, string roleDescription)
        {
            try
            {
                var newRole = await _roleRepository.InsertAsync(new Role {Name = roleName, Description = roleDescription});
                return newRole;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IList<Role>> ValidateRoles(string[] roleNames)
        {
            return await _roleRepository.GetByNamesAsync(roleNames);
        }

        public async Task<User> CreateNewUser(string email, string name, string password, string groupName, string[] roleNames)
        {
            User user = new User
            {
                Email = email,
                Name = name,
                Password = password.ToMD5Hash()
            };

            var roles = await ValidateRoles(roleNames);
            user.Roles = roles.Select(r => new UserRole {User = user, Role = r}).ToList();

            return await _userRepository.InsertAsync(user);
        }

        public async Task<User> GetUser(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<User> GetUser(ClaimsPrincipal user)
        {
            var idClaim = user.Claims.SingleOrDefault(c => c.Type == nameof(User.Id));
            if (idClaim != null)
            {
                int.TryParse(idClaim.Value, out int id);
                if (id > 0)
                {
                    return await GetUser(id);
                }
            }

            return null;
        }

        public async Task<bool> CheckUserExists(string email)
        {
            return await _userRepository.CheckEmailExists(email);
        }

        public async Task<string> AuthenticateUser(string email, string password)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user.Password == password.ToMD5Hash())
            {
                string token = _tokenService.GenerateUserToken(user);
                return token;
            }
            else
            {
                throw new Exception("desautorizado");
            }
        }

        public async Task AssignUserToGroup(User user, Group group)
        {
            await _userRepository.UpdateGroupAsync(user, group);
        }

    }
}
