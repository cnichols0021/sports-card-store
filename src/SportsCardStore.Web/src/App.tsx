import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { CardListPage } from "./pages/CardListPage";
import { CardDetailPage } from "./pages/CardDetailPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<CardListPage />} />
        <Route path="/cards/:id" element={<CardDetailPage />} />
      </Routes>
    </Router>
  );
}

export default App;
