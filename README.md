Sistema de Autenticación con Tokens Personalizados
==================================================

Este repositorio contiene un sistema de autenticación personalizado basado en tokens, cumpliendo con la restricción de no utilizar librerías externas como JWT.

Descripción General
-------------------

El sistema se basa en la generación y validación manual de tokens usando el algoritmo HMACSHA256. Además de validar la firma del token, se verifica la fecha de expiración y se proporciona funcionalidad para refrescar y revocar tokens.

TokenService
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

### Endpoints de la API

    /register: Permite a los usuarios registrarse.
    /auth: Autentica a los usuarios y devuelve un token.
    /validate-token: Verifica la validez de un token.
    /refresh-token: Renueva un token si se proporciona un refresh token válido.
    /logout: Revoca un token, evitando su reutilización.