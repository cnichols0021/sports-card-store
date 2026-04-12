import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { CardListPage } from "./pages/CardListPage";
import { CardDetailPage } from "./pages/CardDetailPage";
import { AdminRoute } from "./components/AdminRoute";
import { AdminLayout } from "./components/AdminLayout";
import { AdminDashboardPage } from "./pages/admin/AdminDashboardPage";
import { ManageCardsPage } from "./pages/admin/ManageCardsPage";
import { CardFormPage } from "./pages/admin/CardFormPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<CardListPage />} />
        <Route path="/cards/:id" element={<CardDetailPage />} />

        {/* Admin Routes */}
        {/* AdminRoute guards access, AdminLayout provides the nav shell.
            Layout routes must be pathless in React Router v6 so they don't
            consume a URL segment — the path matching is handled by the parent. */}
        <Route path="/admin" element={<AdminRoute />}>
          <Route element={<AdminLayout />}>
            <Route index element={<AdminDashboardPage />} />
            <Route path="cards" element={<ManageCardsPage />} />
            <Route path="cards/new" element={<CardFormPage />} />
            <Route path="cards/:id/edit" element={<CardFormPage />} />
          </Route>
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
