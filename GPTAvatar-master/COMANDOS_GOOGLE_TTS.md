# Comandos para configurar Google Cloud Text-to-Speech manualmente

## Paso 0: Verificar y habilitar la API

Verificar si la API está habilitada:

```bash
gcloud services list --enabled --project=cohesive-totem-478501-q0 | grep texttospeech
```

Si NO aparece nada, habilítala con este comando:

```bash
gcloud services enable texttospeech.googleapis.com --project=cohesive-totem-478501-q0
```

Espera 30-60 segundos después de habilitar.

## Paso 1: Configurar el proyecto

```bash
gcloud config set project cohesive-totem-478501-q0
```

## Paso 2: Autenticar con Application Default Credentials

```bash
gcloud auth application-default login --project=cohesive-totem-478501-q0
```

Esto abrirá tu navegador para que autorices el acceso.

## Paso 3: Generar el access token

```bash
gcloud auth application-default print-access-token --project=cohesive-totem-478501-q0
```

Este comando te mostrará el token que empieza con `ya29.`

## Paso 4: Copiar el token

Copia el token completo (todo el texto que empieza con `ya29.`) y pégalo en tu archivo `config.txt`:

```
set_google_api_key=ya29.tu_token_aqui
```

## Paso 5: Verificar que funciona (OPCIONAL)

Prueba la API directamente con curl:

```bash
curl -X POST \
  -H "Authorization: Bearer $(gcloud auth application-default print-access-token --project=cohesive-totem-478501-q0)" \
  -H "Content-Type: application/json; charset=utf-8" \
  -d "{\"input\":{\"text\":\"Hola mundo\"},\"voice\":{\"languageCode\":\"es-US\",\"name\":\"es-US-Neural2-A\"},\"audioConfig\":{\"audioEncoding\":\"LINEAR16\"}}" \
  "https://texttospeech.googleapis.com/v1/text:synthesize"
```

Si funciona, verás una respuesta JSON con `audioContent`. Si da error 403, la API no está habilitada correctamente.

## Nota importante

- El token expira después de 1 hora
- Cuando expire, solo necesitas volver a ejecutar el Paso 3
- Si ves error 403 "SERVICE_DISABLED", ejecuta el comando de habilitación del Paso 0
