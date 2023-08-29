export default async function fetchPetList({ queryKey }) {
  const { animal, location, breed } = queryKey[1];

  const response = await fetch(
    `https://pets-v2.dev-apis.com/pets?animal=${animal}&location=${location}&breed=${breed}`
  );

  if (!response.ok) {
    throw new Error(`Pets/${animal},${location},${breed} fetch not ok`);
  }

  return response.json();
}
