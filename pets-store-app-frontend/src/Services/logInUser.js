export default async function loginUser(body) {
  const response = await fetch(`https://localhost:7123/api/Auth/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(body),
    credentials: "include",
  });

  if (!response.ok) throw new Error(JSON.stringify(await response.json()));

  return response.json();
}
