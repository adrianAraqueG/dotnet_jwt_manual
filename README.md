Sistema de Autenticación con Tokens Personalizados
==================================================

Este repositorio contiene un sistema de autenticación personalizado basado en tokens, cumpliendo con la restricción de no utilizar librerías externas como JWT.

Tabla de Contenido
------------------

* Descripción General
* TokenService
  * Generación de Tokens
  * Verificación de Tokens
* Endpoints de la API
* Instalación y Uso
* Contribuciones

Descripción General
-------------------

El sistema se basa en la generación y validación manual de tokens usando el algoritmo HMACSHA256. Además de validar la firma del token, se verifica la fecha de expiración y se proporciona funcionalidad para refrescar y revocar tokens.

TokenService
------------

### Generación de Tokens

La generación de tokens se lleva a cabo mediante el método `GenerateTokenSignature`. Este método toma como entrada información del usuario y genera un token firmado con una clave secreta.

```csharp
public string GenerateTokenSignature(DataTokenDto clientInfo, DateTime expiration);
```

### Verificación de Tokens

La verificación se realiza a través del método VerifyTokenSignature, que comprueba tanto la firma como la fecha de expiración del token.

```csharp
public bool VerifyTokenSignature(string token, DataTokenDto clientInfo, DateTime expiration);
```

### Endpoints de la API

    /register: Permite a los usuarios registrarse.
    /auth: Autentica a los usuarios y devuelve un token.
    /validate-token: Verifica la validez de un token.
    /refresh-token: Renueva un token si se proporciona un refresh token válido.
    /logout: Revoca un token, evitando su reutilización.