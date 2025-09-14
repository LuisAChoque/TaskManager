import { BrowserRouter, Routes, Route } from "react-router-dom";
import Login from "./pages/Login";
import TasksBoard from "./pages/TasksBoard";
import NewTask from "./pages/NewTask";
import EditTask from "./pages/EditTask";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/tasks" element={<TasksBoard />} />
        <Route path="/new-task" element={<NewTask />} />
        <Route path="/edit-task/:id" element={<EditTask />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
