import { Navigate, Outlet } from "react-router-dom";

export function AdminRoute() {
  // TODO: Replace with JWT role check when auth is implemented
  const isAdmin = localStorage.getItem("isAdmin") === "true";

  if (!isAdmin) {
    return <Navigate to="/" replace />;
  }

  return <Outlet />;
}
