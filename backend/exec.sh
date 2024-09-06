#!/bin/bash

# Construye la imagen Docker para el backend
docker build -t back .

sleep 2
clear

sleep 1

# Ejecuta el contenedor Docker para el backend
docker run -d -it -p 5000:8080 --name apiend back

cd frontend

chmod +x ixic.sh 

./ixic.sh
