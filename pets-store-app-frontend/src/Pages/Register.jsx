import React from "react";
import { useNavigate } from "react-router-dom";
import RegisterUser from "../Services/registerUser.js";

export default function Register() {
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const body = {
      Email: formData.get("email"),
      FirstName: formData.get("firstName"),
      LastName: formData.get("lastName"),
      Password: formData.get("password"),
    };
    RegisterUser(body).then((Data) => {
      navigate("/notification/" + Data?.message);
    });
  };

  return (
    <div className="login-container">
      <h2>Register</h2>
      <form className="login-form" onSubmit={handleSubmit}>
        <div>
          <label>
            Email:
            <input name="email" type="email" />
          </label>
        </div>
        <div>
          <label>
            First Name:
            <input name="firstName" type="text" />
          </label>
        </div>
        <div>
          <label>
            Last Name:
            <input name="lastName" type="text" />
          </label>
        </div>
        <div>
          <label>
            Password:
            <input name="password" type="password" />
          </label>
        </div>
        <button type="submit">Register</button>
      </form>
    </div>
  );
}
