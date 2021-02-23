FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . ./
RUN dotnet restore "Campbelltech.PostalCodeTax.Api/Campbelltech.PostalCodeTax.Api.csproj"
WORKDIR "/src/Campbelltech.PostalCodeTax.Api"
COPY . .
RUN dotnet build "Campbelltech.PostalCodeTax.Api/Campbelltech.PostalCodeTax.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Campbelltech.PostalCodeTax.Api/Campbelltech.PostalCodeTax.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Campbelltech.PostalCodeTax.Api.dll"]
