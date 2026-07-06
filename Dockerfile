FROM mcr.microsoft.com/dotnet/sdk:10.0 as builder
WORKDIR /app

COPY ./some-crud.webApi ./

RUN dotnet restore

RUN dotnet tool restore
RUN dotnet ef database update

RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 as runtime
WORKDIR /app

COPY --from=builder /app/out .

ENTRYPOINT [ "dotnet", "someCrud.dll" ]