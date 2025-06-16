1. 
El presente proyecto, titulado Pixel Wall-E, consiste en el desarrollo de una aplicación interactiva que combina el arte del pixel-art con un lenguaje de programación personalizado. Inspirado en el robot Wall-E, el objetivo principal es permitir que el usuario escriba y ejecute instrucciones sobre un canvas cuadrado, logrando que el robot "pinte" según las órdenes dadas. Este enfoque lúdico e interactivo fomenta el aprendizaje de estructuras básicas de lenguajes, control de flujo, operaciones gráficas y manipulación de variables.

2. Objetivos del Proyecto
Diseñar una interfaz gráfica intuitiva que incluya un editor de texto, un canvas y controles interactivos.

Implementar un lenguaje de comandos específico que permita crear figuras y composiciones en píxeles.

Permitir la lectura y escritura de archivos con extensión .pw.

Aplicar buenas prácticas de programación orientada a objetos, facilitando la extensibilidad del sistema.

3. Características del Lenguaje
El lenguaje diseñado para este proyecto es sencillo pero expresivo. Entre sus características destacan:

Inicialización obligatoria con el comando Spawn(x, y), que posiciona a Wall-E en el canvas.

Instrucciones gráficas como DrawLine, DrawCircle, DrawRectangle y Fill para modificar el canvas.

Control de color y tamaño mediante los comandos Color(string) y Size(int).

Expresiones aritméticas y booleanas, con soporte para operaciones básicas y funciones personalizadas.

Asignación de variables y estructura de control por etiquetas y saltos condicionales con GoTo.

Este lenguaje permite construir programas secuenciales y controlados que modifican visualmente el canvas.

4. Interfaz de Usuario
La aplicación cuenta con los siguientes elementos:

Editor de texto con numeración de líneas para escribir el código.

Canvas dinámico, cuadriculado, que representa cada píxel modificable por el código.

Entrada de tamaño del canvas para definir dimensiones personalizadas.

Botones funcionales:

Ejecutar código

Redimensionar canvas

Cargar archivo .pw

Guardar archivo .pw

Esta disposición facilita tanto la programación como la visualización inmediata del resultado.

5. Ejecución y Validación
La ejecución del código se realiza de forma controlada:

Se verifica la validez sintáctica y semántica del programa antes de ejecutarlo.

Si ocurre un error de ejecución, se detiene el proceso y se mantiene el estado alcanzado del canvas.

En caso de errores de sintaxis, se reportan con detalles, permitiendo su rápida corrección.

El sistema está diseñado para evitar comportamientos inesperados y fomentar el aprendizaje iterativo del lenguaje.