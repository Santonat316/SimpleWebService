#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see http://aka.ms/containercompat 

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY src/MyWebApplication.csproj src/
RUN dotnet restore src/MyWebApplication.csproj
COPY . .
WORKDIR /src/src
RUN dotnet build MyWebApplication.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MyWebApplication.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyWebApplication.dll"]
