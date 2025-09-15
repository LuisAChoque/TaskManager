#!/bin/bash

# Detener el script si hay un error
set -e

# Verificar si Docker está instalado
if ! command -v docker &> /dev/null; then
    echo "Instalando Docker..."
    sudo apt update && sudo apt install -y docker.io
fi

# Asegurar que Docker esté corriendo
if ! pgrep -x "dockerd" > /dev/null; then
    echo "Iniciando servicio de Docker..."
    sudo systemctl start docker
    sudo systemctl enable docker
fi

# Agregar usuario actual al grupo Docker para evitar uso de sudo
if ! groups | grep -q "docker"; then
    echo "Agregando usuario al grupo Docker..."
    sudo usermod -aG docker $USER
    exec sg docker "$0"
fi

# Iniciar SQL Server en Docker
echo "Iniciando SQL Server en Docker..."
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrong!Passw0rd' \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest || echo "El contenedor ya está en ejecución."

# Verificar si .NET SDK está instalado
if ! command -v dotnet &> /dev/null; then
    echo "Instalando .NET SDK..."
    echo "Detectando versión de Ubuntu..."
    UBUNTU_VERSION=$(lsb_release -rs)
    echo "Agregando repositorio de Microsoft para Ubuntu $UBUNTU_VERSION..."
    wget https://packages.microsoft.com/config/ubuntu/$UBUNTU_VERSION/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    sudo apt update 
    # Instalar libicu manualmente si es necesario
    if ! dpkg -s libicu-dev &> /dev/null; then
        echo "Intentando instalar libicu-dev..."
        sudo apt install -y libicu-dev || echo "libicu no disponible"
    fi
    sudo apt install -y dotnet-sdk-8.0
fi

# Instalar Entity Framework CLI si no está instalado
if ! command -v dotnet-ef &> /dev/null; then
    echo "Instalando Entity Framework CLI..."
    dotnet tool install --global dotnet-ef
    export PATH="$HOME/.dotnet/tools:$PATH"
fi

# Verificar si curl está instalado
if ! command -v curl &> /dev/null; then
    echo "Instalando curl..."
    sudo apt install -y curl
fi

# Verificar si NVM (Node Version Manager) está instalado
if ! command -v nvm &> /dev/null; then
    echo "Instalando NVM (Node Version Manager)..."
    # Instalar NVM
    curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.3/install.sh | bash
    # Cargar nvm en el shell actual
    export NVM_DIR="$HOME/.nvm"
    [ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"  # This loads nvm
    echo 'export NVM_DIR="$HOME/.nvm"' >> ~/.bashrc
    echo '[ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"' >> ~/.bashrc
    source ~/.bashrc
fi

# Instalar Node.js v18.20.8 usando NVM
echo "Instalando Node.js v18.20.8..."
nvm install 18.20.8
nvm use 18.20.8

# Verificar si npm está instalado
if ! command -v npm &> /dev/null; then
    echo "Error: npm no se instaló correctamente. Inténtalo manualmente."
    exit 1
fi

# Verificar existencia de directorios del backend y frontend
BACKEND_DIR="backend/TaskManager/TaskManager.API"
if [ ! -d "$BACKEND_DIR" ]; then
    echo "Error: El directorio del backend no existe: $BACKEND_DIR"
    exit 1
fi

FRONTEND_DIR="frontend/taskmanager-frontend"
if [ ! -d "$FRONTEND_DIR" ]; then
    echo "Error: El directorio del frontend no existe: $FRONTEND_DIR"
    exit 1
fi

# Moverse al directorio del backend
cd $BACKEND_DIR

# Asegurar que ~/.dotnet/tools esté en el PATH
if [[ ":$PATH:" != *":$HOME/.dotnet/tools:"* ]]; then
    echo "Agregando ~/.dotnet/tools al PATH..."
    export PATH="$HOME/.dotnet/tools:$PATH"
    echo 'export PATH="$HOME/.dotnet/tools:$PATH"' >> ~/.bashrc
fi

# Restaurar paquetes de .NET
echo "Restaurando dependencias..."
dotnet restore

# Verificar si existe la migración 'InitialCreate' antes de crearla
if dotnet ef migrations list | grep -q "InitialCreate"; then
    echo "La migración 'InitialCreate' ya existe. Eliminándola..."
    dotnet ef migrations remove -f
fi

# Crear migración inicial
echo "Creando migración inicial..."
dotnet ef migrations add InitialCreate

# Aplicar migraciones de la base de datos (si existen)
echo "Aplicando migraciones..."
dotnet ef database update || echo "No hay migraciones para aplicar."

# Ejecutar el backend en el puerto 5000
echo "Iniciando backend en http://localhost:5000..."
dotnet run --urls="http://localhost:5000" &

# Esperar unos segundos a que el backend arranque
sleep 5



# Volver al directorio raíz y ejecutar el frontend
cd ../../..  # Volver a la raíz de TaskManager

# Moverse a la carpeta del frontend
cd $FRONTEND_DIR

# Hacer npm install para instalar dependencias del frontend
echo "Instalando dependencias del frontend..."
npm install

# Ejecutar el frontend en el puerto 3000
echo "Iniciando frontend en http://localhost:3000..."
npm start -- --port 3000 &

# Esperar a que los procesos terminen
wait

