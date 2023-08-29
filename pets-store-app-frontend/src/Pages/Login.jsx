import React, { useEffect } from "react";
import { useContext } from "react";
import { Link } from "react-router-dom";
import UserContext from "../utilities/UserContext";
import logInUser from "../Services/logInUser";
import logInUserByCookies from "../Services/logInUserByCookies";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const [, setUser] = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    let mounted = true;
    if (mounted)
      logInUserByCookies()
        .then((data) => {
          setUser(data);
        })
        .catch(({ message }) => {
          message = JSON.parse(message);
          //navigate("/notification/" + message.message);
        });
    return () => (mounted = false);
  });

  const handleLogin = (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const body = {
      email: formData.get("email"),
      password: formData.get("password"),
    };
    logInUser(body)
      .then((data) => {
        setUser(data);
      })
      .catch(({ message }) => {
        message = JSON.parse(message);
        navigate("/notification/" + message.message);
      });
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      <form className="login-form" onSubmit={(e) => handleLogin(e)}>
        <label>
          Email:
          <input name="email" type="email" />
        </label>
        <br />
        <label>
          Password:
          <input name="password" type="password" />
        </label>
        <button type="submit">Login</button>
      </form>
      <div>
        <Link to="/forget-password">Forgot Password?</Link>
      </div>
      <div>
        <Link to="/register">Register</Link>
      </div>
    </div>
  );
}
