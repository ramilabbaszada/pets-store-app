import React from "react";
import ResetPassword from "../Services/resetPassword";
import { useNavigate } from "react-router-dom";
import { useParams } from "react-router-dom";

export default function ForgetPasswordForm() {
  const navigate = useNavigate();
  const params = useParams();
  const handleSubmit = (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const body = {
      password: formData.get("password"),
      resetToken: params.resetToken,
    };
    ResetPassword(body).then((data) => {
      navigate("/notification/" + data?.message);
    });
  };

  return (
    <div className="login-container">
      <h2>Forgot Password</h2>
      <form className="login-form" onSubmit={handleSubmit}>
        <div>
          <label>
            New Password:
            <input name="password" type="password" />
          </label>
        </div>

        <button type="submit">Reset Password</button>
      </form>
    </div>
  );
}
