using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using TrainRecord.Application.RegisterUser;
using TrainRecord.Core.Entities;

namespace TrainRecord.Application.Mapping
{
    public class AuthMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<(User User, string NewPassword), User>()
                .Map(dest => dest.Password, src => src.NewPassword)
                .Map(dest => dest, src => src.User);
        }
    }
}
