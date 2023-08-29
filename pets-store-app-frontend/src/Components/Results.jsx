import Pet from "./Pet";
import { useContext } from "react";
import AdoptedPetsContext from "../utilities/AdoptedPetContext";

export default function Results({ pets }) {
  const [adoptedPets] = useContext(AdoptedPetsContext);

  console.log(pets);
  return (
    <div className="search">
      {!pets?.filter((pet) => {
        return !adoptedPets.some((adoptedPet) => adoptedPet.id === pet.id);
      })?.length ? (
        <h1>No Pets Found</h1>
      ) : (
        pets
          ?.filter((pet) => {
            return !adoptedPets.some((adoptedPet) => adoptedPet.id === pet.id);
          })
          ?.map((pet) => {
            return (
              <Pet
                animal={pet.animal}
                key={pet.id}
                name={pet.name}
                breed={pet.breed}
                images={pet.images}
                location={`${pet.city}, ${pet.state}`}
                id={pet.id}
              />
            );
          })
      )}
    </div>
  );
}
