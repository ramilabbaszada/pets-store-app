export default async function logOutUser() {
  const response = await fetch(`https://localhost:7123/api/Auth/logout`, {
    credentials: "include",
  });

  //if (!response.ok) throw new Error(JSON.stringify(await response.json()));

  return response.json();
}
