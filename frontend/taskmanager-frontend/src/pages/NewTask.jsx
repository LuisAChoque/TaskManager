import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import api from "../api/axios";
import TaskForm from "../components/TaskForm";

export default function NewTask() {
  const [categories, setCategories] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchCategories = async () => {
      const res = await api.get("/tasks/categories");
      setCategories(res.data);
    };
    fetchCategories();
  }, []);

  const handleCreate = async (data) => {
    await api.post("/tasks", data);
    navigate("/tasks");
  };

  return (
    <div className="flex items-center justify-center h-screen bg-gray-100">
      <TaskForm categories={categories} onSubmit={handleCreate} />
    </div>
  );
}
