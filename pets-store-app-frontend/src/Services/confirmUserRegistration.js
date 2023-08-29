export default async function confirmUserRegistration(token) {
  const response = await fetch(
    `https://localhost:7123/api/Auth/confirm-registeration?token=` + token,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json;charset=UTF-8",
      },
    }
  );

  if (!response.ok) throw new Error(JSON.stringify(await response.json()));

  return response.json();
}
