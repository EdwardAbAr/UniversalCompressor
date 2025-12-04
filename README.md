Read Me

# UniversalCompressor – Aplicación de compresión y descompresión

Esta aplicación de escritorio en C# permite comprimir y descomprimir archivos de texto utilizando distintos algoritmos de compresión (Huffman, LZ77 y LZ78).  
El resultado de la compresión se guarda en archivos con extensión ".myzip".

# 1. Requisitos de ejecución

- Windows con .NET instalado (version .NET 10.0).
- Visual Studio con soporte para aplicaciones de escritorio .NET (Windows Forms).


# 2. Descarga y ejecución del proyecto

1. Clonar o descargar el repositorio desde la rama correspondiente (master).
2. Abrir la solución "Compressor.sln" con Visual Studio (versión 2025 o similar, con el workload de “.NET desktop development” instalado).
3. Una vez abierto el proyecto:
      - Compilar el proyecto (Build) para verificar que todo esté correcto.
4. Ejecutar la aplicación desde Visual Studio con el botón de Iniciar (Start) o la tecla F5.


# 3. Uso de la aplicación

La ventana principal muestra:

- Un botón “Seleccionar archivos”.
- Una lista donde se muestran los archivos seleccionados.
- Un combo box “Seleccionar Algoritmo” para elegir el tipo de compresión o descompresión que desea hacer:
  - Huffman  
  - LZ77  
  - LZ78
- Botones:
  - “Comprimir”
  - “Descomprimir”
- Etiquetas con estadísticas:
  - Tiempo
  - Memoria
  - Tasa de compresión

## 3.1. Compresión de archivos

Para comprimir uno o varios archivos:

1. Presionar “Seleccionar archivos”.
2. En el cuadro de diálogo:
   - Elegir uno o varios archivos de texto (`.txt`).  
3. Verificar que los archivos aparezcan en la lista.
4. En el combo “Seleccionar Algoritmo”, seleccionar el algoritmo de compresión que se desea usar:
   - Huffman, LZ77 o LZ78.
5. Presionar el botón “Comprimir”.
   - Si no hay archivos seleccionados, la aplicación mostrará un mensaje de advertencia indicando que se debe seleccionar al menos un archivo.
6. Se abrirá un cuadro de Guardar archivo:
   - El nombre sugerido del archivo a comprimir sera "nombre del archivo original.myzip".
   - Si se seleccionaron varios archivos, se propone "nombre del archivo original_varios.myzip".
7. Elegir la carpeta de destino y confirmar.
8. Una vez finalizado el proceso, en la ventana principal se actualiza:
   - El tiempo que tardó la compresión.
   - La memoria estimada utilizada durante el proceso.
   - La tasa de compresión (tamaño comprimido / tamaño original).

El archivo comprimido ".myzip" quedará guardado en la ruta que el usuario haya elegido.


## 3.2. Descompresión de archivos ".myzip"

Para descomprimir un archivo generado por la aplicación:

> Importante: la descompresión **NO** utiliza el botón “Seleccionar archivos”.  
> Siempre debe iniciarse desde el botón “Descomprimir” para que se puedan seleccionar archivos ".myzip".

1. Presionar el botón “Descomprimir”.
2. En el cuadro de diálogo:
   - Elegir un archivo con extensión ".myzip" generado por esta aplicación.
   - Si se cancela el cuadro sin seleccionar nada, se mostrará una advertencia indicando que se debe elegir un archivo ".myzip".
3. Confirmar la selección.
4. La aplicación descomprimirá el contenido utilizando automáticamente el algoritmo que fue usado al comprimir.
5. Los archivos descomprimidos se guardan en una carpeta nueva creada junto al archivo de origen, con el siguiente formato:

   - Si el archivo de entrada es:  
     "C:\Ruta\nombre.myzip"  
   - La carpeta de salida será:  
     "C:\Ruta\nombre_out\"

   Dentro de esa carpeta se encontrarán los archivos originales restaurados.

6. Al finalizar, la aplicación mostrará un mensaje indicando que la descompresión fue exitosa y la ruta donde se guardaron los archivos.
7. En la ventana principal se actualizarán también las estadísticas de la *última descompresión* realizada:
   - El tiempo total de descompresión.
   - La memoria estimada utilizada.
   - La tasa calculada a partir del tamaño original y del tamaño comprimido almacenado en el ".myzip".


# 4. Comportamiento de validaciones

- Para comprimir:
   - Si se intenta comprimir sin haber seleccionado archivos, se mostrará un cuadro de diálogo de advertencia solicitando que se seleccione al menos un archivo antes de comprimir.

- Para descomprimir:
  - Es obligatorio iniciar desde el botón “Descomprimir”.
  - Si se cierra el cuadro de selección sin elegir un ".myzip", se mostrará una advertencia.

- La aplicación muestra estadísticas (tiempo, memoria y tasa) tanto al comprimir como al descomprimir, siempre sobre la última operación realizada.
