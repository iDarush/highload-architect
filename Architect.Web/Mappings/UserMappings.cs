using Architect.Web.BLL.Models;
using Architect.Web.DAL.Models;
using Architect.Web.Dto.Requests;
using Architect.Web.Dto.Responses;
using Mapster;

namespace Architect.Web.Mappings;

public class UserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserModel>()
            .ConstructUsing(
                user => new UserModel(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Age,
                    (Gender)user.Gender,
                    user.Hobby,
                    user.City));

        config.NewConfig<UserRegisterRequest, User>()
            .ConstructUsing(
                request => new User
                {
                    FirstName = request.FirstName,
                    LastName = request.SecondName,
                    Age = request.Age,
                    Gender = request.Gender,
                    Hobby = request.Biography ?? string.Empty,
                    City = request.City
                });

        config.NewConfig<UserModel, UserResponse>()
            .ConstructUsing(
                src => new UserResponse(
                    src.Id.ToString(),
                    src.FirstName,
                    src.LastName,
                    src.Age,
                    (short)src.Gender,
                    src.Hobby,
                    src.City));
    }
}
