import { Link } from "react-router-dom";
import UserContext from "../utilities/UserContext";
import AdoptedPetsContext from "../utilities/AdoptedPetContext";
import { useContext } from "react";
import { useNavigate } from "react-router-dom";
import forgetPasswordRequest from "../Services/forgetPasswordRequest";
import CartItem from "./CartItem";
import logOutUser from "../Services/logOutUser";

export default function Navbar() {
  const [user, setUser] = useContext(UserContext);
  const [adoptedPets] = useContext(AdoptedPetsContext);
  const navigate = useNavigate();

  const handleSignOutClick = (e) => {
    logOutUser().then(() => {
      setUser(null);
      navigate("/");
    });
  };

  const handlePasswordReset = () => {
    forgetPasswordRequest(user.email)
      .then((data) => {
        navigate("/notification/" + data?.message);
      })
      .catch(({ message }) => {
        message = JSON.parse(message);
        navigate("/notification/" + message.message);
      });
  };

  return (
    <nav className="navbar navbar-expand-lg bg-body-tertiary">
      <div className="container-fluid">
        <Link className="navbar-brand" to="/">
          Pets Store App
        </Link>
        <div className="collapse navbar-collapse" id="navbarSupportedContent">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0"></ul>
          {adoptedPets.length !== 0 && (
            <div className="nav-item dropdown" style={{ margin: "0 30px" }}>
              <div
                className="nav-link "
                role="button"
                data-bs-toggle="dropdown"
                aria-expanded="false"
              >
                Cart
                <i className="shopping cart icon"></i>
              </div>
              <ul className="dropdown-menu">
                {adoptedPets.map((pet) => {
                  return (
                    <li key={pet.id}>
                      <div role="button" className="dropdown-item">
                        <CartItem
                          animal={pet.animal}
                          key={pet.id}
                          name={pet.name}
                          breed={pet.breed}
                          images={pet.images}
                          location={`${pet.city}, ${pet.state}`}
                          id={pet.id}
                        />
                      </div>
                    </li>
                  );
                })}
                <li>
                  <div role="button" className="dropdown-item">
                    <h4>
                      <i className="check circle icon" />
                      Confirm
                    </h4>
                  </div>
                </li>
              </ul>
            </div>
          )}
          <div className="nav-item dropdown" style={{ margin: "0 0 0 200px" }}>
            <div
              className="nav-link "
              role="button"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              <i className="user circle icon"></i>
              {user.firstName + " " + user.lastName}
            </div>
            <ul className="dropdown-menu">
              <li>
                <div
                  role="button"
                  className=" dropdown-item"
                  onClick={handleSignOutClick}
                >
                  <i className="sign out alternate icon"></i>
                  Log Out
                </div>
              </li>
              <li>
                <div
                  role="button"
                  className=" dropdown-item"
                  onClick={handlePasswordReset}
                >
                  <i className="sync icon"></i>
                  Update Password
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </nav>
  );
}
