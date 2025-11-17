#!/usr/bin/env python3
"""
Script definitivo para generar tokens de Google Cloud Text-to-Speech
que funcionan correctamente con el proyecto cohesive-totem-478501-q0
"""

import json
import time
import sys
from datetime import datetime, timedelta

try:
    from google.oauth2 import service_account
    from google.auth.transport.requests import Request
except ImportError:
    print("ERROR: Necesitas instalar google-auth")
    print("Ejecuta: pip install google-auth")
    sys.exit(1)

# Configuración
SERVICE_ACCOUNT_FILE = 'google-tts-credentials.json'
SCOPES = ['https://www.googleapis.com/auth/cloud-platform']
PROJECT_ID = 'cohesive-totem-478501-q0'

def generar_token():
    """Genera un access token válido usando Service Account"""
    try:
        # Cargar credenciales del Service Account
        credentials = service_account.Credentials.from_service_account_file(
            SERVICE_ACCOUNT_FILE,
            scopes=SCOPES
        )
        
        # Agregar quota_project_id
        credentials = credentials.with_quota_project(PROJECT_ID)
        
        # Obtener el token
        credentials.refresh(Request())
        
        token = credentials.token
        expiry = credentials.expiry
        
        # Mostrar información
        print("=" * 70)
        print("TOKEN GENERADO EXITOSAMENTE")
        print("=" * 70)
        print(f"\nProyecto: {PROJECT_ID}")
        print(f"Service Account: {credentials.service_account_email}")
        print(f"\nToken (primeros 50 caracteres):")
        print(f"{token[:50]}...")
        print(f"\nExpira: {expiry.strftime('%Y-%m-%d %H:%M:%S')}")
        print(f"Tiempo restante: {(expiry - datetime.now()).seconds // 60} minutos")
        print("\n" + "=" * 70)
        print("INSTRUCCIONES:")
        print("=" * 70)
        print("1. Copia el token completo de abajo")
        print("2. Ábrelo en config.txt")
        print("3. Reemplaza la línea set_google_api_key|... con:")
        print(f"   set_google_api_key|{token[:30]}...")
        print("\n" + "=" * 70)
        print("\nTOKEN COMPLETO:")
        print("=" * 70)
        print(token)
        print("=" * 70)
        
        # Guardar en archivo
        with open('google_access_token.txt', 'w') as f:
            f.write(token)
        
        print("\n✅ Token también guardado en: google_access_token.txt")
        
        # Actualizar config.txt automáticamente
        try:
            with open('config.txt', 'r', encoding='utf-8') as f:
                config = f.read()
            
            # Buscar y reemplazar el token
            import re
            pattern = r'(set_google_api_key\|)ya29\.[^\n]+'
            
            if re.search(pattern, config):
                nuevo_config = re.sub(pattern, f'\\1{token}', config)
                
                with open('config.txt', 'w', encoding='utf-8') as f:
                    f.write(nuevo_config)
                
                print("✅ config.txt actualizado automáticamente")
            else:
                print("⚠️  No se encontró set_google_api_key en config.txt")
                print("   Copia el token manualmente")
        except Exception as e:
            print(f"⚠️  No se pudo actualizar config.txt automáticamente: {e}")
            print("   Copia el token manualmente")
        
        print("\n" + "=" * 70)
        print("🎉 LISTO! Reinicia Unity y prueba de nuevo")
        print("=" * 70)
        
        return token
        
    except FileNotFoundError:
        print(f"ERROR: No se encontró el archivo '{SERVICE_ACCOUNT_FILE}'")
        print("\nAsegúrate de que el archivo google-tts-credentials.json")
        print("esté en el mismo directorio que este script.")
        sys.exit(1)
    except Exception as e:
        print(f"ERROR al generar token: {e}")
        sys.exit(1)

if __name__ == "__main__":
    print("\n🚀 Generador de Tokens de Google Cloud TTS")
    print("   Proyecto: cohesive-totem-478501-q0\n")
    
    generar_token()
    
    input("\nPresiona ENTER para cerrar...")
