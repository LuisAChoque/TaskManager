import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../api/axios";

export default function TasksBoard() {
  const [tasks, setTasks] = useState([]);
  const [categories, setCategories] = useState([]);
  const navigate = useNavigate();

  const loadData = async () => {
    try {
      const [tasksRes, catsRes] = await Promise.all([
        api.get("/tasks"),
        api.get("/tasks/categories"),
      ]);
      setTasks(tasksRes.data);
      setCategories(catsRes.data);
    } catch (err) {
      if (err.response?.status === 401) {
        alert("Sesión expirada. Inicia sesión nuevamente.");
        navigate("/");
      }
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const handleDelete = async (id) => {
    if (window.confirm("¿Seguro que deseas eliminar esta tarea?")) {
      await api.delete(`/tasks/${id}`);
      loadData();
    }
  };

  const handleEdit = (task) => {
    navigate(`/edit-task/${task.id}`);
  };

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">Mis Tareas</h1>
      <div className="flex gap-6">
        {categories.map((cat) => (
          <div key={cat.id} className="flex-1 bg-gray-100 p-4 rounded-xl">
            <h2 className="text-lg font-semibold mb-2">{cat.name}</h2>
            {tasks
              .filter((t) => t.categoryId === cat.id)
              .map((t) => (
                <div
                  key={t.id}
                  className="group bg-white p-3 rounded shadow mb-2 relative"
                >
                  <h3 className="font-medium">{t.title}</h3>
                  <p className="text-sm text-gray-600">{t.description}</p>

                  {/* Botones ocultos al pasar el mouse */}
                  <div className="absolute top-2 right-2 opacity-0 group-hover:opacity-100 transition-opacity flex gap-2">
                    <button
                      onClick={() => handleEdit(t)}
                      className="text-blue-500 hover:text-blue-700 text-sm"
                    >
                      Editar
                    </button>
                    <button
                      onClick={() => handleDelete(t.id)}
                      className="text-red-500 hover:text-red-700 text-sm"
                    >
                      Eliminar
                    </button>
                  </div>
                </div>
              ))}
          </div>
        ))}
      </div>
      <Link
        to="/new-task"
        className="inline-block mt-4 bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
      >
        Nueva Tarea
      </Link>
    </div>
  );
}
