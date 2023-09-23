FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app/

COPY ./ManageMySpace.UserService/. ./ManageMySpace.UserService/.
COPY ./ManageMySpace.Common/. ./ManageMySpace.Common/.
COPY ./wait-for-it.sh ./.

RUN dotnet restore /app/ManageMySpace.UserService/ManageMySpace.UserService.csproj

RUN dotnet publish ManageMySpace.UserService/ManageMySpace.UserService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS user-service
WORKDIR /app
COPY --from=build /app/publish /app/wait-for-it.sh ./
EXPOSE 8080

ENTRYPOINT ["dotnet", "ManageMySpace.UserService.dll"]