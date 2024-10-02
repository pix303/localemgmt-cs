using Localemgmt.Application.LocaleItem.Commands.Add;
using Localemgmt.Application.LocaleItem.Commands.Update;
using Localemgmt.Application.Users.Commons;
using Localemgmt.Contracts.Users;
using Localemgmt.Domain.LocaleItems.Events;
using Mapster;
using System.Reflection;

namespace Localemgmt.Api.Config
{
	public static class MapsterConfig
	{
		public static void RegisterMapsterConfiguration()
		{
			TypeAdapterConfig<UserInfo, UserInfoResponse>.NewConfig()
			.Map(d => d.Id, s => s.Id)
			.Map(d => d, s => s.User);

			TypeAdapterConfig<AddLocaleItemCommand, LocaleItemCreationEvent>.NewConfig()
			.ConstructUsing(s =>
				new LocaleItemCreationEvent(
					s.Lang,
					s.Content,
					s.UserId,
					s.Context
				));

			TypeAdapterConfig<UpdateLocaleItemCommand, LocaleItemUpdateEvent>.NewConfig()
			.ConstructUsing(s =>
				new LocaleItemUpdateEvent(
					s.Lang,
					s.Content,
					s.UserId,
					s.Context,
					s.AggregateId
				));

			TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
		}
	}

}
