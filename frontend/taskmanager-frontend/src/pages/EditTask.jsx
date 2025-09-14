import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import api from "../api/axios";
import TaskForm from "../components/TaskForm";

export default function EditTask() {
  const { id } = useParams();
  const [categories, setCategories] = useState([]);
  const [task, setTask] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const loadData = async () => {
      try {
        const [catsRes, tasksRes] = await Promise.all([
          api.get("/tasks/categories"),
          api.get("/tasks"),
        ]);
        setCategories(catsRes.data);

        const foundTask = tasksRes.data.find((t) => t.id === Number(id));
        if (!foundTask) {
          alert("Tarea no encontrada");
          navigate("/tasks");
          return;
        }
        setTask(foundTask);
      } catch {
        alert("Error al cargar la tarea");
        navigate("/tasks");
      }
    };

    loadData();
  }, [id, navigate]);

  const handleUpdate = async (data) => {
    await api.put(`/tasks/${id}`, data);
    navigate("/tasks");
  };

  return (
    <div className="flex items-center justify-center h-screen bg-gray-100">
      {task && (
        <TaskForm
          initialData={task}
          categories={categories}
          onSubmit={handleUpdate}
        />
      )}
    </div>
  );
}
