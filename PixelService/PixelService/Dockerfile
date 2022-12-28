#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PixelService.csproj", "PixelService/"]
RUN dotnet restore "PixelService/PixelService.csproj"

WORKDIR "/src/PixelService"
COPY . .
RUN dotnet build "PixelService.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "PixelService.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir /app/Source
COPY /Source/pixer.gif /app/Source
ENTRYPOINT ["dotnet", "PixelService.dll"]