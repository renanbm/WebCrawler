FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY src/WebCrawler.Spider.Web/WebCrawler.Spider.Web.csproj src/WebCrawler.Spider.Web/
COPY src/WebCrawler.CrossCutting.IoC/WebCrawler.CrossCutting.IoC.csproj src/WebCrawler.CrossCutting.IoC/
COPY src/WebCrawler.CrossCutting/WebCrawler.CrossCutting.csproj src/WebCrawler.CrossCutting/
COPY src/WebCrawler.Spider.Application/WebCrawler.Spider.Application.csproj src/WebCrawler.Spider.Application/
COPY src/WebCrawler.Domain/WebCrawler.Domain.csproj src/WebCrawler.Domain/
COPY src/WebCrawler.Repository/WebCrawler.Repository.csproj src/WebCrawler.Repository/
RUN dotnet restore
COPY . .
WORKDIR /src/src/WebCrawler.Spider.Web
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebCrawler.Spider.Web.dll"]