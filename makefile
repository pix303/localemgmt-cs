run:
	clear && dotnet run --project Localemgmt.Api
test:
	clear && dotnet test Localemgmt.Api.Test/Localemgmt.Api.Test.csproj && dotnet test Localemgmt.Application.Test/Localemgmt.Application.Test.csproj && dotnet test Localemgmt.Domain.Test/Localemgmt.Domain.Test.csproj
