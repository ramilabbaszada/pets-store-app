import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router-dom";
import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import fetchPet from "../Services/fetchPet";
import Carousel from "../Components/Carousel";
import Modal from "../utilities/Modal";
import AdoptedPetContext from "../utilities/AdoptedPetContext";

export default function Details() {
  let params = useParams();
  const navigate = useNavigate();
  const [adoptedPets, setAdoptedPets] = useContext(AdoptedPetContext);
  const petData = useQuery(["details", params.id], fetchPet);
  const [showModal, setShowModal] = useState(false);
  const pet = petData?.data?.pets[0];

  if (petData.isLoading) {
    return (
      <div className="loading-pane">
        <h2 className="loader">🌀</h2>
      </div>
    );
  }

  return (
    <div className="details">
      <Carousel images={pet.images} />
      <div>
        <h1>{pet.name}</h1>
        <h2>{`${pet.animal} — ${pet.breed} — ${pet.city}, ${pet.state}`}</h2>
        <button onClick={() => setShowModal(true)}>Adopt {pet.name}</button>
        <p>{pet.description}</p>
      </div>
      {showModal ? (
        <Modal>
          <div>
            <h1>Would you like to adopt {pet.name}?</h1>
            <div className="buttons">
              <button
                onClick={() => {
                  setAdoptedPets([pet, ...adoptedPets]);
                  navigate("/");
                }}
              >
                Yes
              </button>
              <button onClick={() => setShowModal(false)}>No</button>
            </div>
          </div>
        </Modal>
      ) : null}
    </div>
  );
}
