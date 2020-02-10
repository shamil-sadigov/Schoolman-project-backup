﻿using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.UserRegistration
{
    public class ClientRegistrationRequestMapper:Profile
    {
        /// <summary>
        /// Mapper from UserRegistrationRequest to User
        /// Doesnt map password because password shoulb be hashed
        /// </summary>
        public ClientRegistrationRequestMapper()
        {
            CreateMap<ClientRegistraionRequest, User>()
                    .ForMember(user => user.FirstName,
                               ops => ops.MapFrom(request => request.FirstName))
                    .ForMember(user => user.LastName,
                               ops => ops.MapFrom(request => request.LastName))
                    .ForMember(user => user.Email,
                               ops => ops.MapFrom(request => request.Email))
                    .ForMember(user => user.PhoneNumber,
                               ops => ops.MapFrom(request => request.PhoneNumber));
        }
    }
}