import { useEffect } from "react";
import { useParams } from "react-router-dom";
import ConfirmUserRegistration from "../Services/confirmUserRegistration";
import { useNavigate } from "react-router-dom";

export default function ConfirmRegistration() {
  const params = useParams();
  const navigate = useNavigate();
  useEffect(() => {
    let mounted = true;
    if (mounted)
      ConfirmUserRegistration(params.token)
        .then((data) => {
          navigate("/notification/" + data?.message);
        })
        .catch((data) => {
          navigate("/notification/" + data?.message);
        });
    return () => (mounted = false);
  });
  return;
}
