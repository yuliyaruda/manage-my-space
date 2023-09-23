FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app/

COPY ./ManageMySpace.ActivityService/. ./ManageMySpace.ActivityService/.
COPY ./ManageMySpace.Common/. ./ManageMySpace.Common/.
COPY ./wait-for-it.sh ./.

RUN dotnet restore /app/ManageMySpace.ActivityService/ManageMySpace.ActivityService.csproj

RUN dotnet publish ManageMySpace.ActivityService/ManageMySpace.ActivityService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS activity-service
WORKDIR /app
COPY --from=build /app/publish /app/wait-for-it.sh ./
EXPOSE 8080

ENTRYPOINT ["dotnet", "ManageMySpace.ActivityService.dll"]