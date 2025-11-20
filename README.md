Te dejo un README nuevo, estructurado siguiendo las buenas prácticas del tutorial: responde al **qué, por qué y cómo**, tiene descripción, tabla de contenido, instalación, uso, créditos, licencia, contribución, etc.

Cópialo tal cual en `README.md` y luego ajusta detalles como tu usuario de GitHub o la imagen.

````markdown
# HoloTalk 3D – Avatares Conversacionales para Pruebas con Usuarios

> Un avatar 3D que habla, escucha y conversa con las personas, pensado para hacer **pruebas rápidas con usuarios** en escritorio (y fácilmente portable a VR porque está hecho en Unity).

HoloTalk 3D es un pequeño laboratorio de **agentes virtuales**: toma como base el proyecto [GPTAvatar](https://github.com/SethRobinson/GPTAvatar) y lo adapta para:
- funcionar en **español**,
- simplificar la configuración para pruebas con participantes,
- documentar claramente cómo instalarlo, ejecutarlo y usarlo en estudios de experiencia de usuario.

---

## Tabla de contenidos

1. [Motivación y descripción](#motivación-y-descripción)
2. [Características principales](#características-principales)
3. [Tecnologías usadas](#tecnologías-usadas)
4. [Requisitos previos](#requisitos-previos)
5. [Instalación para desarrollo](#instalación-para-desarrollo)
6. [Configuración de `config.txt`](#configuración-de-configtxt)
7. [Uso del ejecutable](#uso-del-ejecutable)
8. [Capturas / imagen representativa](#capturas--imagen-representativa)
9. [Contribuir al proyecto](#contribuir-al-proyecto)
10. [Créditos](#créditos)
11. [Licencia](#licencia)
12. [Trabajo futuro](#trabajo-futuro)

---

## Motivación y descripción

### ¿Qué problema intenta resolver?

En muchos cursos y proyectos de investigación sobre **agentes virtuales y realidad mixta/virtual** se necesita:

- Un agente conversacional 3D “creíble”.
- Que hable y escuche (voz + micrófono).
- Que se pueda configurar rápido para distintos **escenarios de prueba con usuarios** (pedir comida, tutor de idiomas, etc.).
- Y que no requiera implementar desde cero toda la integración con modelos de lenguaje, TTS y STT.

HoloTalk 3D nace justamente de esa necesidad: tener un **prototipo listo para usar**, fácil de explicar a otras personas y fácil de adaptar a diferentes experimentos.

### ¿Qué es HoloTalk 3D?

Es una aplicación de Unity que muestra uno o varios **avatares 3D conversacionales**. Cada avatar:

- Tiene una **personalidad** definida por *prompts* en un archivo de configuración.
- Escucha al usuario por micrófono (Whisper).
- Envía el texto a un modelo de OpenAI (chat completions).
- Responde por texto y (opcionalmente) por voz usando Google TTS o ElevenLabs.
- Puede mover labios y ojos si se instala el plugin **SALSA LipSync Suite** (opcional, no obligatorio).

---

## Características principales

- 👤 **Avatares 3D configurables**  
  - Distintos personajes (por ejemplo, “Profesora de japonés”, “Cajero de comida rápida”, etc.).
  - Cada uno con su propio prompt en español.

- 🗣️ **Interacción por voz**  
  - El usuario habla por micrófono.
  - OpenAI Whisper transcribe lo que dijo.
  - El agente responde.

- 🔊 **Síntesis de voz (opcional pero recomendada)**  
  - Google Cloud Text-to-Speech (español y otros idiomas).
  - ElevenLabs para voces más expresivas (si se quiere).

- 💬 **Soporte completo en español**  
  - Prompts en español.
  - Instrucciones y comentarios pensados para experimentos con usuarios hispanohablantes.

- 🎭 **Lipsync opcional con SALSA**  
  - Si instalas SALSA LipSync Suite, el avatar moverá la boca y ojos de forma automática.
  - Si no lo instalas, el proyecto **igualmente funciona** (el avatar habla por audio pero sin animación de labios).

- 🧪 **Pensado para pruebas con usuarios**  
  - Escenarios simples (por ejemplo: “pide un combo y confírmalo”).
  - Fácil de explicar en una sesión de laboratorio o en clase.

---

## Tecnologías usadas

- **Unity 2022.2+**
- **C#** para scripts.
- **OpenAI API**  
  - Modelos de chat (por ejemplo: `gpt-3.5-turbo`, `gpt-4o-mini`, `gpt-4.1-mini` según tu cuenta).
  - Whisper (speech-to-text).
- **Google Cloud Text-to-Speech** (opcional, recomendado para voz en español).
- **ElevenLabs Text-to-Speech** (opcional).
- **SALSA LipSync Suite v2** (opcional, para lipsync y animaciones faciales).

---

## Requisitos previos

### Software

- **Windows 10/11**
- **Unity Hub** instalado.
- **Unity Editor 2022.2.x o superior** (idealmente la misma rama que el proyecto original GPTAvatar).
- Un editor de texto (VS Code, Notepad++, etc.) para editar `config.txt`.

### Cuentas y API keys

1. **OpenAI**
   - Crear cuenta en <https://platform.openai.com>
   - Generar una API key en **API keys**.
   - Esa misma clave se usa tanto para:
     - Whisper (voz → texto).
     - Chat completions (texto → respuesta).

2. **Google Cloud (opcional, recomendado)**
   - Crear proyecto en <https://console.cloud.google.com>
   - Habilitar **Text-to-Speech API**.
   - Crear una API key de tipo “API key” en *APIs & Services → Credentials*.

3. **ElevenLabs (opcional)**
   - Crear cuenta en <https://elevenlabs.io>
   - Generar una API key en el panel de usuario.

### SALSA LipSync Suite (opcional)

- **No es necesaria** para que el proyecto funcione.
- Solo se necesita si quieres:
  - Animación de labios sincronizada con la voz.
  - Movimiento de ojos/parpadeo más avanzado.

Si no tienes SALSA:
- Deja comentada la integración en `AIManager.cs` (o usa la versión del script sin SALSA).
- El agente seguirá escuchando y hablando por audio.

---

## Instalación para desarrollo

### 1. Clonar el repositorio

```bash
git clone https://github.com/TU-USUARIO/holotalk-3d.git
cd holotalk-3d
````

(Sustituye `TU-USUARIO` y el nombre del repo por los tuyos reales.)

### 2. Abrir el proyecto en Unity

1. Abrir **Unity Hub**.
2. Clic en **Add → Add project from disk**.
3. Seleccionar la carpeta raíz clonada (deberías ver `Assets`, `ProjectSettings`, etc.).
4. Abrir con **Unity 2022.2.x o superior**.

### 3. Verificar dependencias

* Espera a que Unity importe todos los assets.
* Si ves errores de SALSA y **no tienes SALSA**, asegúrate de estar usando la versión de `AIManager.cs` **sin** la línea:

  ```csharp
  #define CRAZY_MINNOW_PRESENT
  ```
* Revisa la consola de Unity; si solo hay warnings de audio o mensajes informativos, puedes ignorarlos por ahora.

### 4. Preparar `config.txt`

En la carpeta raíz del proyecto:

1. Localiza `config_template.txt`.

2. Haz una copia y renómbrala a **`config.txt`**.

3. Abre `config.txt` con tu editor de texto favorito.

4. Rellena las claves mínimas:

   ```txt
   set_openai_api_key|TU_API_KEY_DE_OPENAI
   set_google_api_key|TU_API_KEY_DE_GOOGLE_TTS   # opcional (puede quedarse vacío)
   set_elevenlabs_api_key|TU_API_KEY_DE_ELEVENLABS  # opcional (puede quedarse vacío)

   # Modelo de OpenAI
   set_openai_model|gpt-3.5-turbo
   ```

5. (Opcional) Ajusta los prompts y voces de cada “amigo” (friend) según tus necesidades de prueba.

### 5. Abrir la escena principal

* En la ventana **Project**, busca la escena `Main` (normalmente en la raíz de `Assets`).
* Haz doble clic para abrirla.
* Pulsa **Play** para ejecutar el prototipo dentro del Editor.

---

## Configuración de `config.txt`

El archivo `config.txt` controla:

* La API key de OpenAI, Google, ElevenLabs.
* El modelo de OpenAI a usar.
* Los distintos avatares (“friends”):

  * Idioma.
  * Voz.
  * Prompt base.
  * Prompt de dirección (estilo de respuesta).
  * Prompt de introducción (para el botón de “Advice” / “Start”).

Ejemplo (resumido) de un “friend” pensado para español:

```txt
add_friend|Profesora de japonés
set_friend_language|spanish
set_friend_token_memory|800
set_friend_max_tokens_to_generate|200
set_friend_temperature|0.9
set_friend_google_voice|es-ES-Neural2-C
set_friend_voice_pitch|0
set_friend_voice_speed|1.0
set_friend_visual|japanese_teacher

set_friend_base_prompt
Eres una profesora de japonés llamada Atsuko. Hablas con el estudiante siempre en español claro,
y usas ejemplos en japonés con su traducción al español. Tu objetivo es ayudarle a practicar
saludos, presentaciones y situaciones cotidianas sencillas.
No uses HTML ni formato raro, solo texto plano.
<END_TEXT>

set_friend_direction_prompt
Responde con menos de 60 palabras. Siempre incluye alguna frase corta en japonés con su traducción.
<END_TEXT>

set_friend_advice_prompt
El estudiante se sienta a comenzar su clase de japonés.
Preséntate brevemente en español, explica que vas a usar ejemplos en japonés, pero siempre traducidos,
y pregúntale qué aspecto del japonés le gustaría trabajar hoy (saludos, pedir comida, presentarse, etc.).
<END_TEXT>
```

Puedes añadir tantos “friends” como necesites para tus experimentos.

---

## Uso del ejecutable

### 1. Generar el ejecutable (build)

En Unity:

1. Ve a **File → Build Settings…**
2. Plataforma: **PC, Mac & Linux Standalone**.
3. Target: **Windows**.
4. Añade la escena `Main` a la lista de escenas del build (botón **Add Open Scenes**).
5. Clic en **Build** y elige una carpeta de salida (por ejemplo `Build/`).

Esto generará algo como:

* `HoloTalk3D.exe`
* `HoloTalk3D_Data/`
* (Copias de otros archivos necesarios)

Asegúrate de copiar también tu `config.txt` a la **misma carpeta donde está el `.exe`**.

### 2. Configurar `config.txt` junto al ejecutable

En la carpeta del ejecutable:

* Debe existir un archivo `config.txt` con tus claves y configuración.
* Si no está, copia el `config.txt` desde el proyecto de Unity.

### 3. Ejecutar

1. Haz doble clic en `HoloTalk3D.exe`.
2. Debe abrirse una ventana con:

   * Un avatar 3D.
   * Botones de elección de personaje (según tu UI).
   * Botón de micrófono, botón de “Advice/Start”, etc.

### 4. Flujo típico de uso en pruebas con usuarios

1. Escoger el avatar / escenario (por ejemplo, “Burger Barn”).
2. Pulsar el botón de **Advice/Start** para que el agente se presente.
3. Pulsar el botón del **micrófono**:

   * Hablar una frase corta (3–5 segundos).
   * Esperar a que el sistema:

     * Transcriba la voz (Whisper).
     * Genere respuesta (modelo de OpenAI).
     * Reproduzca la respuesta TTS (si está configurada).
4. Repetir la interacción según el protocolo de la sesión (tareas, preguntas, etc.).

> ⚠️ Si ves errores como `HTTP 400/401/429` en la consola integrada del juego, revisa:
>
> * Que tu API key de OpenAI sea válida y tenga saldo.
> * Que el modelo configurado exista en tu cuenta.
> * Que no estés superando límites de uso.

---

## Capturas / imagen representativa

Añade una imagen en tu repositorio, por ejemplo en `docs/img/holotalk_hero.png`, con:

* El avatar 3D en primer plano.
* El cuadro de diálogo visible.
* Un entorno que sugiera “laboratorio de interacción” o “escenario de restaurante / clase”.

Y referencia la imagen así:

```markdown
![HoloTalk 3D – Avatar conversacional en acción](docs/img/holotalk_hero.png)
```

---

## Contribuir al proyecto

Este proyecto nació como parte de un trabajo académico, pero se puede extender para:

* Añadir nuevos escenarios de interacción.
* Integrar directamente con VR (XR Interaction Toolkit).
* Mejorar la interfaz para estudios más complejos.

Si quieres contribuir:

1. Haz un fork del repositorio.
2. Crea una rama descriptiva:

   ```bash
   git checkout -b feature/nueva-funcionalidad
   ```
3. Realiza tus cambios y haz commits claros.
4. Abre un Pull Request describiendo:

   * Qué problema resuelves.
   * Qué cambios hiciste.
   * Cómo probarlos.

---

## Créditos

* **Implementación original**:

  * [Seth A. Robinson – GPTAvatar](https://github.com/SethRobinson/GPTAvatar)

* **Adaptación, prompts en español y guía para pruebas con usuarios**:

  * (Tu nombre aquí) – diseño de escenarios, configuración en español, documentación y README.

* **Tecnologías y servicios externos**:

  * [OpenAI](https://platform.openai.com)
  * [Google Cloud Text-to-Speech](https://cloud.google.com/text-to-speech)
  * [ElevenLabs](https://elevenlabs.io)
  * [SALSA LipSync Suite](https://crazyminnowstudio.com/unity-3d/lip-sync-salsa/)

---

## Licencia

Este proyecto está licenciado bajo la licencia **MIT**.

Puedes usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias del software, siempre que se incluya el aviso de copyright y esta nota de permiso en todas las copias o partes sustanciales del software.

Para más detalles, consulta el archivo `LICENSE` incluido en este repositorio.

---

## Trabajo futuro

Algunas direcciones interesantes:

* 🎧 **Integración VR nativa**

  * Sustituir la cámara estándar por un rig XR (OpenXR + XR Interaction Toolkit).
  * Colocar el avatar frente al usuario en un entorno VR.
  * Mantener el mismo backend de IA.

* 📊 **Instrumentación para estudios**

  * Registro de tiempos de interacción.
  * Registro automático de turnos de diálogo.
  * Exportación de logs a CSV/JSON.

* 🧠 **Perfiles de agente más ricos**

  * Diferentes estilos de tutor (más estricto, más lúdico).
  * Escenarios guiados para entrenamiento de habilidades específicas.

