#
# Base
#
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 443
RUN useradd -u 1000 -M appuser

#
# Build
#
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["cocograph.csproj", "./"]
RUN dotnet restore "cocograph.csproj"
COPY . .
RUN dotnet build "cocograph.csproj" -c Release -o /app/build

#
# Publish
#
FROM build AS publish
RUN dotnet publish "cocograph.csproj" -c Release -o /app/publish

#
# Final
#
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configure certificate
ARG CERT_NAME
COPY ${CERT_NAME} .
RUN chmod 644 ${CERT_NAME}
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=${CERT_NAME}

USER 1000
ENTRYPOINT ["dotnet", "cocograph.dll"]