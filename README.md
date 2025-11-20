# HoloTalk 3D: Agentes Conversacionales para Pruebas con Usuarios

Proyecto de investigación en el que se usa un avatar 3D conversacional, integrado con modelos de lenguaje (OpenAI) y síntesis de voz, para realizar **pruebas sencillas con usuarios** en escenarios controlados (por ejemplo, pedir comida en un restaurante o practicar japonés con una profesora virtual).

El proyecto está construido sobre Unity y se basa en el repositorio original **GPTAvatar** de Seth A. Robinson, adaptado para:
- funcionar en español,
- simplificar la configuración para pruebas,
- documentar un flujo básico de experimentación con participantes.

---

## Tecnologías usadas

- **Unity 2022.2+** (motor principal del proyecto).
- **C#** (scripts de lógica del agente y la integración).
- **OpenAI API**  
  - Modelo de conversación (por ejemplo `gpt-3.5-turbo` / `gpt-4o-mini`, según tu cuenta).  
  - Modelo de Speech-to-Text (Whisper) para transcribir el audio del usuario.
- **Google Cloud Text-to-Speech (TTS)** (opcional, para voz sintética en español).
- **ElevenLabs TTS** (opcional, voces de alta calidad para otros personajes).
- **SALSA LipSync Suite** (opcional, para animación de labios y ojos del avatar).

---

## Pre-requisitos

### Software

- **Sistema operativo**: Windows 10/11 (para desarrollo y ejecutable).
- **Unity Hub** instalado.
- **Unity Editor 2022.2.x o superior** (versión recomendada similar a la del repositorio original).
- **.NET / Visual Studio** (opcional, pero útil para depurar scripts C#).

### Cuentas y API keys

Para usar el agente conversacional con voz:

1. **Cuenta de OpenAI**  
   - Crear una API key en: <https://platform.openai.com/api-keys>  
   - La clave se usa tanto para:
     - Chat (respuesta del agente).
     - Whisper (reconocimiento de voz del usuario).

2. **Cuenta de Google Cloud (opcional, recomendado para voz en español)**  
   - Crear proyecto en Google Cloud.
   - Habilitar **Cloud Text-to-Speech API**.
   - Crear una API key en “APIs & Services → Credentials”.

3. **Cuenta de ElevenLabs (opcional)**  
   - Solo si deseas usar voces de ElevenLabs para algún avatar.
   - Generar API key en el panel de ElevenLabs.

### SALSA LipSync Suite (OPCIONAL – mejora visual)

SALSA NO es obligatorio para que el proyecto funcione.  
Sin SALSA:
- El avatar sigue respondiendo por texto y voz.
- La cara no moverá labios/ojos de forma automática.

Si quieres activar lipsync y movimientos faciales:

- Comprar/instalar **SALSA LipSync Suite v2** desde la Unity Asset Store.
- (Opcional) Instalar los **OneClick** apropiados para tu tipo de modelo (por ejemplo Reallusion CC3/CC4).
- Asegurarte de que la línea `#define CRAZY_MINNOW_PRESENT` en `AIManager.cs` esté **descomentada** cuando SALSA esté instalado, o **comentada** si no lo usas.

---

## Imagen representativa

Para “vender” la aplicación o mostrarla en entregas/pósters, se recomienda una imagen tipo:

- El avatar 3D en primer plano (profesora o cajero del restaurante).
- Un globo de texto o caja de diálogo con una frase en español.
- El usuario (o una silueta) frente a la pantalla o al visor VR.

Ejemplo de ruta (ajusta según dónde guardes tu imagen):

```markdown
![HoloTalk 3D – Avatar conversacional en acción](docs/img/holotalk_hero.png)
