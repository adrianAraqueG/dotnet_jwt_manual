:shipit:🔐 Sistema de Autenticación con Tokens Personalizados 
==================================================

Este repositorio contiene un sistema de autenticación personalizado basado en tokens, cumpliendo con la restricción de no utilizar librerías externas como JWT.

🎱 Descripción General
-------------------

El sistema se basa en la generación y validación manual de tokens usando el algoritmo **HMACSHA256**. Además de validar la firma del token, se verifica la fecha de expiración y se proporciona funcionalidad para refrescar y revocar tokens.

🔧 Instalación
-------------------
+ Configurar las conexión a la base de datos en **appsettins.json**
    ```
    ...
    "ConnectionStrings":{
            "DefaultConnection": "server=localhost;user=root;password=;database=jwtmanual"
        }
    ...
    ```
+ Aplicar la migración a la base de datos:
    ```
    dotnet ef database update InitialCreate --project .\Persistence\
    ```
    **NOTA** En caso de que quieras crear y correr una nueva migración:
    ```
    dotnet ef database update YourMigration --project .\Persistence\ --startup-project .\API\ --output-dir .\Data\Migrations
    dotnet ef database update YourMigration --project .\Persistence\
    ```
+ **OPCIONAL** Configurar la clave secreta de encriptación en el constructor de **UserService**:
     ```
    ...
    public UserService(IUnitOfWork unitOfWork){
        _unitOfWork = unitOfWork;
        _tokenService = new TokenService("YOUR_SECRET_KEY");
    }
    ...
    ```
+ 🚀Lanzar la aplicación
    ```
    cd .\API\
    dotnet run
    ```

## API Reference 🛰️

#### Permite a los usuarios registrarse.

```http
  POST api/user/register
  {
      "username": "yourusername",
      "password": "yourpassword",
      "email": "email@domain.com"
  }
```

#### Autentica a los usuarios y devuelve un token.

```http
  POST api/user/auth
  {
      "username": "yourusername",
      "password": "yourpassword"
  }
```

#### Verifica la validez de un token.

```http
  POST api/user/validate-token
  {
      "message": "statusmessage",
      "isAuthenticated": bool,
      "username": "yourusername",
      "email": "email@domain.com",
      "token": "yourtoken",
      "refreshToken": "yourrefreshtoken",
      "expiration": "TimeDate"
  }
```

#### Renueva un token si se proporciona un refresh token válido.

```http
  POST api/user/refresh-token
  "Authorization": "Bearer @token"
```

#### Revoca un token, evitando su reutilización.

```http
  POST api/user/logout
  "Authorization": "Bearer @token"
```


🦬 TokenService | How it works
------------

### Generación de Tokens

La generación de tokens se lleva a cabo mediante el método `GenerateTokenSignature`. Este método toma como entrada información del usuario y genera un token firmado con una clave secreta.

```csharp
public string GenerateTokenSignature(PayloadTokenDto clientInfo, DateTime expiration);
```

### Verificación de Tokens

La verificación se realiza a través del método VerifyTokenSignature, que comprueba tanto la firma como la fecha de expiración del token.

```csharp
public bool VerifyTokenSignature(string token, PayloadTokenDto clientInfo, DateTime expiration);
```

### Generación de Refresh Token

El refresh token al no guardar metadatos no tiene porqué ser un hash fuerte, es más como un identificador así que se genera un GUID.

```csharp
public string GenerateRefreshToken();
```

### Implementación de encriptación HMACSHA256

Recibimos un string por parámetro (en este caso se espera que sea un JSON).
```csharp
private string ComputeSignature(string data);
```

Creamos una nueva instancia del objeto HMACSHA256 y pasamos por parámetro la _secretKey covertida a un arreglo de bytes (necesario para la instancia).
```csharp
using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
```

Convertimos a un arreglo de bytes la información del usuario.
```csharp
byte[] dataBytes = Encoding.UTF8.GetBytes(data);
```

Usamos el método propio nuestra instancia HMACSHA256 para 'computar' el hash final.
```csharp
byte[] hashBytes = hmac.ComputeHash(dataBytes);
```

Por último retornamos el hash convertido a texto (Base64) para poder utilizarlo en un cotexto normal
```csharp
return Convert.ToBase64String(hashBytes);
```
