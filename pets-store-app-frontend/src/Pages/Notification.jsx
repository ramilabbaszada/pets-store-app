import React from "react";
import { useParams } from "react-router-dom";

export default function Notification() {
  const params = useParams();
  return (
    <div className="notification-page">
      <h2>Notification</h2>
      <div className="notification">
        <p>{params.message}</p>
      </div>
    </div>
  );
}
