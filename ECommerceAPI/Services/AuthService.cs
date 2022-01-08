using ECommerceAPI.Helper;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Identity;
using ECommerceAPI.Constants;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerceAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;    
            this._jwt = jwt.Value;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already Registerd !" };
            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return new AuthModel { Message = "Username is already registerd" };

            var CustomerUser = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(CustomerUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error},";
                return new AuthModel { Message = errors };
            }
            if(!_roleManager.RoleExistsAsync(Roles.User.ToString()).Result)
                await _roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));    

            await _userManager.AddToRoleAsync(CustomerUser, Roles.User.ToString());
            var JwtToken = await CreateJwtTokenAsync(CustomerUser);


            return new AuthModel 
            { 
                IsAuthenticated=true,
                Roles=  _userManager.GetRolesAsync(CustomerUser).Result.ToList(),
                Username= model.Username,
                Email= model.Email, 
                Token= new JwtSecurityTokenHandler().WriteToken(JwtToken),
                ExpiresOn=JwtToken.ValidTo
            };

        }
        
        public async Task<AuthModel> RegisterAdminAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already Registerd !" };
            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return new AuthModel { Message = "Username is already registerd" };

            var AdminUser = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(AdminUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error},";
                return new AuthModel { Message = errors };
            }
            if (!_roleManager.RoleExistsAsync(Roles.Admin.ToString()).Result)
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

            await _userManager.AddToRoleAsync(AdminUser, Roles.Admin.ToString());
            var JwtToken = await CreateJwtTokenAsync(AdminUser);


            return new AuthModel
            {
                IsAuthenticated = true,
                Roles = _userManager.GetRolesAsync(AdminUser).Result.ToList(),
                Username = model.Username,
                Email = model.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                ExpiresOn = JwtToken.ValidTo
            };
        }
        public async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                 roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("username",user.UserName),
                new Claim("uid",user.Id),
                new Claim("guid",Guid.NewGuid().ToString()),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signinCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer:_jwt.Issuer,
                audience:_jwt.Audience,
                claims:claims,
                expires:DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials:signinCredentials);

            return jwtToken;
        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return new AuthModel { Message = "Invalid Email or Password" };

            var JwtToken = await CreateJwtTokenAsync(user);

            return new AuthModel
            {
                Email = model.Email,
                Username = user.UserName,
                IsAuthenticated = true,
                Roles = _userManager.GetRolesAsync(user).Result.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                ExpiresOn = JwtToken.ValidTo 
            };


        }
    }
}
