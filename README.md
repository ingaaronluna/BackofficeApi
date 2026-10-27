# BackofficeApi
mini proyecto con controladores

## Primer paso es tener MySQL corriendo.
Con el usuario root podemos crear la base de datos y el usuario

```
CREATE DATABASE backoffice_db CHARACTER SET utf8mb4;

CREATE USER 'backoffice_user'@'localhost' IDENTIFIED BY 'ElijanUnaClaveSegura';

GRANT ALL PRIVILEGES ON backoffice_db.* TO 'backoffice_user'@'localhost';
FLUSH PRIVILEGES;
```

### Debemos cambiar de usuario y acceder la base de datos.

`mysql -u backoffice_user -p backoffice_db`

*Una vez dentro debemos revisar que no existan tablas*
`SHOW TABLES;`

## Clonar el proyecto

- Clonar este proyecto
- Correr `dotnet restore` de esta manera se instalan las librerias.

### En caso de que no se hayan instalado correctamente las librerías te dejo los comandos

Instalar los paquetes
```
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
dotnet build
```
Guardar la cadena de conexión con Secret Manager
```
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=backoffice_db;User=backoffice_user;Password=SU_CLAVE"
dotnet user-secrets list
```
## Debemos crear la base de datos con EF

Instalar: `dotnet tool install --global dotnet-ef --version 9.0.0`
Revisar la versión: `dotnet ef --version`

### Realizar la primera migración

```
dotnet ef migrations add Inicial
dotnet ef database update
```

Si falla usar:
`dotnet ef database update --connection "Server=127.0.0.1;Port=3306;Database=backoffice_db;Uid=backoffice_user;Password=TUCLAVE;SslMode=none"`

### Para revisar la base de datos por linea de comando 

```
SHOW TABLES;
DESCRIBE Productos;
SHOW CREATE TABLE Productos;
```

### En caso de tener problemas con la cadena de conexión

Listar `dotnet user-secrets list`

Vuelve a guardarlo en Mac
```
dotnet user-secrets remove "ConnectionStrings:DefaultConnection"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=backoffice_db;User=backoffice_user;Password=TuClave"
```

