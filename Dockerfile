# ------------------------------------------------------------------ build ---

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

ENV APP_NAME=Alpha.Weather

COPY . ./${APP_NAME}

ARG NUGET_USER
ENV NUGET_USER ${NUGET_USER}

ARG NUGET_TOKEN
ENV NUGET_TOKEN ${NUGET_TOKEN}

RUN echo ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> " ${NUGET_USER}

RUN dotnet restore ${APP_NAME}

# ----------------------------------------------------------------- publish ---

FROM build as publish

ENV APP_NAME=Alpha.Weather

RUN dotnet publish ./${APP_NAME} -c Release -o /${APP_NAME}/publish/

# --------------------------------------------------------------------- app ---

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS app
EXPOSE 8080

ARG APP_NAME=Alpha.Weather
ENV APP_NAME ${APP_NAME}

WORKDIR /app

COPY --from=publish /${APP_NAME}/publish/ .

ENTRYPOINT ["dotnet", "Alpha.Weather.dll"]

