using Schoolman.Student.Infrastructure;

namespace Schoolman.Student.WenApi.Controllers.Identity.DTO.Response
{
    public class Register_ResponseModel_OnSuccess
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public static explicit operator Register_ResponseModel_OnSuccess(AppUser appUser)
        {
            return new Register_ResponseModel_OnSuccess()
            {
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                PhoneNumber = appUser.PhoneNumber
            };
        }
    }
}
