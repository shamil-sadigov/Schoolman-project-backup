﻿namespace Schoolman.Student.WenApi.Controllers
{
    public partial class AuthenticationController
    {
        public class ConfirmationDTO
        {
            public string UserId { get; set; }
            public string ConfirmationToken { get; set; }
        }
    }
}
