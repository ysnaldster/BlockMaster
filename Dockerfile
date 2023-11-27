FROM public.ecr.aws/lambda/dotnet:6 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlockMaster.Domain/BlockMaster.Domain.csproj", "BlockMaster.Domain/"]
COPY ["BlockMaster.Infrastructure/BlockMaster.Infrastructure.csproj", "BlockMaster.Infrastructure/"]
COPY ["BlockMaster.Business/BlockMaster.Business.csproj", "BlockMaster.Business/"]
COPY ["BlockMaster.Api/BlockMaster.Api.csproj", "BlockMaster.Api/"]
RUN dotnet restore "BlockMaster.Api/BlockMaster.Api.csproj"
COPY . .
WORKDIR "/src/BlockMaster.Api"
RUN dotnet build "BlockMaster.Api.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "BlockMaster.Api.csproj" -c Release -o /app/publish
FROM base as final
WORKDIR /app
COPY --from=publish /app/publish /var/task                                 
CMD [ "BlockMaster.Api::BlockMaster.Api.LambdaEntryPoint::FunctionHandlerAsync" ]