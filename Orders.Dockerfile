FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src

# copy csproj and restore as distinct layers
COPY ./Pizza.sln .
COPY ./src/Frontend/Frontend.csproj ./src/Frontend/
COPY ./src/JaegerTracing/JaegerTracing.csproj ./src/JaegerTracing/
COPY ./src/Orders/Orders.csproj ./src/Orders/
COPY ./src/Orders.PubSub/Orders.PubSub.csproj ./src/Orders.PubSub/
COPY ./src/Orders.ShopConsole/Orders.ShopConsole.csproj ./src/Orders.ShopConsole/
COPY ./src/Toppings/Toppings.csproj ./src/Toppings/
COPY ./src/Toppings.Data/Toppings.Data.csproj ./src/Toppings.Data/
COPY ./test/TestHelpers/TestHelpers.csproj ./test/TestHelpers/
COPY ./test/Toppings.Tests/Toppings.Tests.csproj ./test/Toppings.Tests/

RUN dotnet restore

# copy and publish app and libraries
COPY . .

RUN dotnet build -c Release --no-restore
RUN dotnet publish src/Orders -c Release -o /app --no-build

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["./Orders"]
