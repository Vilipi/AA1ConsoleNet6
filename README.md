## APPConsoleNet6

Esta aplicación trata de el manejo de citas en un centro médico.
Podremos crear un nuevo usuario donde seremos el paciente y podremo pedir, listar y cancelar las citas que queramos.
Desde el punto de vista del médico, podemos logearnos y tratar con las citas que tengamos. Podremos modificarlas añadiendo
comentarios, precio y cancelando la cita. Tambien contaremos con un buscador para buscars las citas de los pacientes filtrando por nombre.
Tambien hay un area pública donde cualquier usuario puede ver los especialistas sin necesidad de logearsa.

###### Docker
- docker build -t aa1net6:1.7 .
- docker run -it  -v /logs aa1net6:1.7
- docker exec -it CONTAINER /bin/bash
- docker pull vicelp/aa1net6console:aa1consoleusa
- docker run -it --rm -v /logs vicelp/aa1net6console:aa1consoleusa

###### Especificaciones
- Menú principal y secundarios para navegar entre las funcionalidades interactuando a través de la consola de la aplicación.
- Gestión de alta y selección de usuarios y/o productos y/o servicios.
- Zona privada de información.
- Zona pública de información.
- El modelo de datos estará compuesto de 3 clases. Patient, Specilaist y Apointment.
- Funcionalidad de búsqueda en las funcionalidades del especialista.
- App contenerizada y subida a docker Hub.(volumes y puertos).
- Utilizavión de Git y metodología Gitflow.
- Almacenamiento de datos en ficheros Json
- Gestión de logs con Nlog.
- Variables de entorno que modifican la app (currency).
- Interfaz mejorada con Spectre.Console
