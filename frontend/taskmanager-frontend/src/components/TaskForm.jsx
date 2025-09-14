import { useState, useEffect } from "react";

export default function TaskForm({ initialData, categories, onSubmit }) {
  const [title, setTitle] = useState(initialData?.title || "");
  const [description, setDescription] = useState(initialData?.description || "");
  const [categoryId, setCategoryId] = useState(initialData?.categoryId || 1);

  useEffect(() => {
    if (initialData) {
      setTitle(initialData.title);
      setDescription(initialData.description);
      setCategoryId(initialData.categoryId);
    }
  }, [initialData]);

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ title, description, categoryId });
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="bg-white p-6 rounded-xl shadow-lg w-96"
    >
      <h2 className="text-xl font-bold mb-4">
        {initialData ? "Editar Tarea" : "Nueva Tarea"}
      </h2>

      <input
        type="text"
        placeholder="Título"
        className="w-full p-2 border rounded mb-3"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
      />

      <textarea
        placeholder="Descripción"
        className="w-full p-2 border rounded mb-3"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
      />

      <select
        className="w-full p-2 border rounded mb-3"
        value={categoryId}
        onChange={(e) => setCategoryId(Number(e.target.value))}
      >
        {categories.map((c) => (
          <option key={c.id} value={c.id}>
            {c.name}
          </option>
        ))}
      </select>

      <button
        type="submit"
        className="w-full bg-blue-500 text-white py-2 rounded hover:bg-blue-600"
      >
        {initialData ? "Guardar cambios" : "Crear"}
      </button>
    </form>
  );
}
