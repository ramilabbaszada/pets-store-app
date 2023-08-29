import { Link } from "react-router-dom";
import AdoptedPetsContext from "../utilities/AdoptedPetContext";
import { useContext } from "react";

export default function CartItem({
  name,
  animal,
  breed,
  images,
  location,
  id,
}) {
  let hero = "http://pets-images.dev-apis.com/pets/none.jpg";
  if (images.length) {
    hero = images[0];
  }

  const [adoptedPets, setAdoptedPets] = useContext(AdoptedPetsContext);

  function handleDeletePet(e) {
    setAdoptedPets(adoptedPets.filter((pet) => pet.id !== id));
  }

  return (
    <div style={{ display: "flex", alignItems: "center" }}>
      <Link to={`/details/${id}`} className="cart-item">
        <div className="cart-item-image-container">
          <img src={hero} alt={name} />
        </div>
        <div className="cart-item-info">
          <h4>{name}</h4>
          <h6>{`${animal} — ${breed} — ${location}`}</h6>
        </div>
      </Link>
      <i className="times icon" onClick={handleDeletePet} />
    </div>
  );
}
