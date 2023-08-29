import React from "react";
import { useNavigate } from "react-router-dom";
import ForgetPasswordRequest from "../Services/forgetPasswordRequest";

export default function ForgetPassword() {
  const navigate = useNavigate();
  const handleSubmit = (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const email = formData.get("email");
    ForgetPasswordRequest(email).then((data) => {
      navigate("/notification/" + data?.message);
    });
  };

  return (
    <div className="login-container">
      <h2>Forgot Password</h2>
      <form className="login-form" onSubmit={handleSubmit}>
        <div>
          <label>
            Email:
            <input name="email" type="email" />
          </label>
        </div>
        <button type="submit">Reset Password</button>
      </form>
    </div>
  );
}
