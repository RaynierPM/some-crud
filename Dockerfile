FROM mcr.microsoft.com/dotnet/sdk:10.0 as builder
WORKDIR /app

COPY . ./

RUN dotnet restore

RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 as runtime
WORKDIR /app

COPY --from=builder /app/out .

ENTRYPOINT [ "dotnet", "someCrud.dll" ]