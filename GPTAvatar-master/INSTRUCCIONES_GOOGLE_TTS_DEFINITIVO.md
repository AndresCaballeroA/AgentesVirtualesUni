# 🎯 SOLUCIÓN DEFINITIVA PARA GOOGLE TTS

## ⚠️ IMPORTANTE
El método `gcloud auth application-default` NO funciona porque vincula los tokens al proyecto interno de Google (`764086051850`) en lugar de tu proyecto.

## ✅ SOLUCIÓN DEFINITIVA: Service Account + Python

### Paso 1: Instalar dependencias de Python

Abre **CMD** o **PowerShell** y ejecuta:

```cmd
pip install google-auth
```

### Paso 2: Generar token con el script

Ejecuta el script Python que acabo de crear:

```cmd
python generar_token_definitivo.py
```

El script hará automáticamente:
1. ✅ Genera un token válido usando tu Service Account
2. ✅ Vincula el token al proyecto correcto (`cohesive-totem-478501-q0`)
3. ✅ Actualiza `config.txt` automáticamente
4. ✅ Guarda el token en `google_access_token.txt` como respaldo

### Paso 3: Reiniciar Unity

1. Cierra Unity **completamente**
2. Vuelve a abrir Unity
3. Ejecuta el proyecto y prueba Japanese Teacher

## 🔄 Cuando el token expire (cada ~1 hora)

Simplemente vuelve a ejecutar:

```cmd
python generar_token_definitivo.py
```

Y reinicia Unity.

## 🎯 Voces configuradas actualmente

- **Japanese Teacher**: Google TTS `es-US-Neural2-A` (femenina)
- **Seth**: ElevenLabs (masculina)
- **Burger Barn**: ElevenLabs (masculina)

## ❓ Si aún así falla

Verifica que el archivo `google-tts-credentials.json` está en el mismo directorio que el script.

## 💡 ALTERNATIVA SIMPLE

Si esto sigue sin funcionar, **usa ElevenLabs para todos los agentes**. Es más simple y no expira nunca. Solo di "activa ElevenLabs para Japanese Teacher" y listo.
