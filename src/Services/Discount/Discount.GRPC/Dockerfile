#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Discount/Discount.GRPC/Dicount.GRPC.csproj", "Services/Discount/Discount.GRPC/"]
RUN dotnet restore "Services/Discount/Discount.GRPC/Dicount.GRPC.csproj"
COPY . .
WORKDIR "/src/Services/Discount/Discount.GRPC"
RUN dotnet build "Dicount.GRPC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dicount.GRPC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dicount.GRPC.dll"]