using Localemgmt.Application.Users.Commons;
using Localemgmt.Contracts.Users;
using Mapster;
using System.Reflection;

namespace Localemgmt.Api.Config
{

	public static class MapsterConfig
	{
		public static void RegisterMapsterConfiguration(this IServiceCollection collection)
		{
			TypeAdapterConfig<UserInfo, UserInfoResponse>.NewConfig()
			.Map(d => d.Id, s => s.Id)
			.Map(d => d, s => s.User);

			TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
		}
	}

}
