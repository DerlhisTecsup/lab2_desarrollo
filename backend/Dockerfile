# Fase de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establecer el directorio de trabajo
WORKDIR /app

# Crear un archivo de proyecto webapi y restaurar dependencias
RUN dotnet new webapi -o myapp --no-https

# Copiar el archivo Program.cs al directorio del proyecto
WORKDIR /app/myapp
COPY Program.cs ./

# Restaurar las dependencias y construir la aplicación
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Fase de producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar los archivos publicados desde la fase de construcción
COPY --from=build /app/publish .

# Exponer el puerto 80
EXPOSE 80

# Configurar el punto de entrada
ENTRYPOINT ["dotnet", "myapp.dll"]
