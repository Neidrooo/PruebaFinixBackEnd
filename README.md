    # API REST de Bancos

    Este proyecto implementa una API REST para la gestión de bancos,
    desarrollada con .NET 7 y Swagger. Proporciona operaciones CRUD (Crear, Leer, Actualizar, Eliminar) para la gestión de bancos,
    así como autenticación de usuarios y
    protección de endpoints mediante tokens JWT.

    ## Características

    - **CRUD de Bancos**: Permite crear, leer, actualizar y eliminar información de bancos.
    - **Autenticación de Usuarios**: Soporte para el registro y autenticación de usuarios, y verificacion de tokens devolviendo un token JWT para el acceso a endpoints protegidos.
    - **Swagger UI**: Documentación de la API a través de Swagger UI.

    ## Tecnologías Utilizadas

    - .NET 7
    - Entity Framework Core para la persistencia de datos.
    - Swagger para la documentación de la API.
    - JWT (Json Web Tokens) para la autenticación y autorización.

    ### Prerrequisitos

    - .NET 7 SDK
    - Un sistema gestor de bases de datos compatible (SQL Server, SQLite, etc.)

    ## Endpoints Disponibles

    La API ofrece los siguientes endpoints principales para la gestión de bancos y usuarios:

    - **Bancos**
    - `GET /api/banks`: Obtiene una lista paginada de todos los bancos.
    - `GET /api/banks/{uid}`: Obtiene los detalles de un banco específico por su UID.
    - `POST /api/banks/createBank`: Crea un nuevo banco.
    - `DELETE /api/banks/{uid}`: Elimina un banco por su UID.
    - `PATCH /api/banks/UpdateBank/{uid}`: Actualiza el nombre de un banco.

    - **Auth**
    - `POST /api/auth/login`: Autentica a un usuario, devuelve un token JWT y datos del usuario.
    - `POST /api/auth/register`: Registra un nuevo usuario en el sistema.
    - `POST /api/auth/verifyToken`: Verifica si el token esta caducado.


    Todos los endpoints, excepto el de login, requieren autenticación mediante el token JWT proporcionado tras el inicio de sesión exitoso.

    Para una descripción detallada de cada endpoint, incluyendo parámetros y formatos de respuesta, por favor consulta la documentación de Swagger UI accesible ejecutando la API.

    ## Inserción Automática de Datos

    Este proyecto está configurado para realizar una inserción automática de 200 registros bancarios al iniciar la aplicación. Esta operación se lleva a cabo a través de llamadas a una API externa que proporciona datos de bancos de manera aleatoria. La lógica para esta inserción de datos se encuentra en el proyecto `BusinessLogic`, dentro del directorio `Data`, en el archivo `DataBanks.cs`.

    ### Detalles de la Implementación

    La inserción automática de datos se ejecuta cada vez que el programa inicia, verificando primero la cantidad actual de registros en la base de datos. Si hay menos de 200 bancos registrados, el sistema realizará una o más llamadas a la API externa para obtener los datos faltantes hasta alcanzar los 200 registros. Esta funcionalidad asegura que la base de datos siempre inicie con una cantidad mínima de datos para pruebas y demostraciones.

    Para más detalles sobre la implementación y configuración de esta funcionalidad, por favor revisa el archivo `BusinessLogic/Data/DataBanks.cs` en el código fuente del proyecto.

    ## Configuración

    Este proyecto utiliza Entity Framework Core y sigue la metodología Code First para la gestión de la base de datos. Esto significa que el esquema de la base de datos se genera a partir de las clases modelo definidas en el código, lo que facilita las actualizaciones y mantenimientos del esquema de la base de datos mediante migraciones.

    ### Base de Datos

    La configuración inicial y las migraciones para la base de datos se encuentran dentro del proyecto `BusinessLogic`, específicamente en la carpeta `Data/Migrations`. Para aplicar las migraciones y generar la base de datos, utiliza los siguientes comandos desde la terminal en el directorio raíz del proyecto:

    ```bash
    dotnet ef migrations add NombreMigracion -p BusinessLogic -s WebApi -o Data/Migrations
