import { useQuery } from "@tanstack/react-query";
import Results from "../Components/Results";
import { useState } from "react";
import fetchBreedList from "../Services/fetchBreedList";
import fetchPetList from "../Services/fetchPetList";
import ErrorBoundary from "../utilities/ErrorBoundry";

const ANIMALS = ["bird", "cat", "dog", "rabbit", "reptile"];

export default function SearchParams() {
  const [requestParams, setRequestParams] = useState({
    animal: "",
    location: "",
    breed: "",
  });

  const [animal, setAnimal] = useState(ANIMALS[0]);
  const breedsData = useQuery(["breeds", animal], fetchBreedList);
  const petsData = useQuery(["pets", requestParams], fetchPetList);

  return (
    <>
      <div className="search-params">
        <form
          onSubmit={(e) => {
            e.preventDefault();
            const formData = new FormData(e.target);
            setRequestParams({
              animal: formData.get("animal") ?? "",
              location: formData.get("location") ?? "",
              breed: formData.get("breed" ?? ""),
            });
          }}
        >
          <label htmlFor="location">
            Location
            <input name="location" id="location" placeholder="Location" />
          </label>
          <label htmlFor="animal">
            Animal
            <select
              name="animal"
              id="animal"
              value={animal}
              onChange={(e) => {
                setAnimal(e.target.value);
              }}
            >
              {ANIMALS.map((animal) => (
                <option key={animal} value={animal}>
                  {animal}
                </option>
              ))}
            </select>
          </label>
          <label htmlFor="breed">
            Breed
            <select id="breed" name="breed">
              {breedsData?.data?.breeds?.map((breed) => (
                <option key={breed} value={breed}>
                  {breed}
                </option>
              ))}
            </select>
          </label>
          <button>Submit</button>
        </form>
        <ErrorBoundary>
          <Results pets={petsData?.data?.pets} />
        </ErrorBoundary>
      </div>
    </>
  );
}
