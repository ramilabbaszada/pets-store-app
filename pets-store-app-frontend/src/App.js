import "./App.css";
import Login from "./Pages/Login";
import Register from "./Pages/Register";
import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import AdoptedPetContext from "./utilities/AdoptedPetContext";
import UserContext from "./utilities/UserContext";
import Details from "./Pages/Details";
import { useState } from "react";
import ForgetPassword from "./Pages/ForgetPassword";
import ForgetPasswordForm from "./Pages/ForgetPasswordForm";
import Notification from "./Pages/Notification";
import ConfirmRegistration from "./Pages/ConfirmRegistration";
import SearchParams from "./Pages/SearchParams";
import "bootstrap/dist/js/bootstrap.min.js";
import Navbar from "./Components/Navbar";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: Infinity,
      cacheTime: Infinity,
    },
  },
});

function App() {
  const [adoptedPets, setAdoptedPets] = useState([]);
  const [user, setUser] = useState(null);

  return (
    <AdoptedPetContext.Provider value={[adoptedPets, setAdoptedPets]}>
      <UserContext.Provider value={[user, setUser]}>
        <QueryClientProvider client={queryClient}>
          <BrowserRouter>
            {user ? (
              <Navbar />
            ) : (
              <header>
                <Link to="/">Adopt Me!</Link>
              </header>
            )}
            <Routes>
              <Route
                exact
                path="/details/:id"
                element={user ? <Details /> : <Login />}
              ></Route>
              <Route
                exact
                path="/"
                element={user ? <SearchParams /> : <Login />}
              ></Route>
              <Route exact path="/login" element={<Login />} />
              <Route exact path="/register" element={<Register />} />
              <Route
                exact
                path="/forget-password"
                element={<ForgetPassword />}
              />
              <Route
                exact
                path="/forget-password-form/:resetToken"
                element={<ForgetPasswordForm />}
              />
              <Route
                exact
                path="/notification/:message"
                element={<Notification />}
              ></Route>
              <Route
                exact
                path="/confirm/:token"
                element={<ConfirmRegistration />}
              ></Route>
            </Routes>
          </BrowserRouter>
        </QueryClientProvider>
      </UserContext.Provider>
    </AdoptedPetContext.Provider>
  );
}

export default App;
