FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app/

COPY ./ManageMySpace.MailNotificationService/. ./ManageMySpace.MailNotificationService/.
COPY ./ManageMySpace.Common/. ./ManageMySpace.Common/.
COPY ./wait-for-it.sh ./.

RUN dotnet restore /app/ManageMySpace.MailNotificationService/ManageMySpace.MailNotificationService.csproj

RUN dotnet publish ManageMySpace.MailNotificationService/ManageMySpace.MailNotificationService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS mail-notification-service
WORKDIR /app
COPY --from=build /app/publish /app/wait-for-it.sh ./

ENTRYPOINT ["dotnet", "ManageMySpace.MailNotificationService.dll"]