#!/bin/bash

# === CONFIGURACIÓN ===
BACKEND_DIR="backend/TaskManager/TaskManager.API" 
FRONTEND_DIR="frontend/taskmanager-frontend"
BACKEND_PORT=5000
FRONTEND_PORT=3000

# === FUNCIONES ===

start_backend() {
  echo "🚀 Iniciando backend en $BACKEND_DIR ..."
  cd "$BACKEND_DIR" || exit
  dotnet restore
  dotnet build
  # ejecuta backend en background con logs
  dotnet run --urls "http://localhost:$BACKEND_PORT" > ../backend.log 2>&1 &
  BACK_PID=$!
  cd - > /dev/null || exit
  echo "✅ Backend levantado en http://localhost:$BACKEND_PORT (PID: $BACK_PID, log: backend.log)"
}

start_frontend() {
  echo "🚀 Iniciando frontend en $FRONTEND_DIR ..."
  cd "$FRONTEND_DIR" || exit
  npm install
  # ejecuta frontend en background con logs
  PORT=$FRONTEND_PORT npm start > ../../frontend.log 2>&1 &
  FRONT_PID=$!
  cd - > /dev/null || exit
  echo "✅ Frontend levantado en http://localhost:$FRONTEND_PORT (PID: $FRONT_PID, log: frontend.log)"
}

stop_all() {
  echo "🛑 Deteniendo procesos..."
  kill $BACK_PID $FRONT_PID 2>/dev/null
  exit 0
}

# === MAIN ===
echo "==============================="
echo "  Levantando Task Manager 🚀"
echo "==============================="

start_backend
start_frontend

# atrapar Ctrl+C para detener todo
trap stop_all SIGINT

# mantener script vivo
wait

