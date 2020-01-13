using Schoolman.Student.Infrastructure;

namespace Schoolman.Student.WenApi.Controllers.Identity.DTO.Response
{
    public class UserRegisteredModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public static explicit operator UserRegisteredModel(AppUser appUser)
        {
            return new UserRegisteredModel()
            {
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                PhoneNumber = appUser.PhoneNumber
            };
        }
    }
}
