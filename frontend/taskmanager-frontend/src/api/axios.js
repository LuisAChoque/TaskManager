import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5000/api",
  withCredentials: true, // para que viaje la cookie de sesión
});

export default api;
