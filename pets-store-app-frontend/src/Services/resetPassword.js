export default async function resetPassword(body) {
  const response = await fetch(
    `https://localhost:7123/api/Auth/forget-password-handler`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    }
  );

  if (!response.ok) throw new Error(JSON.stringify(await response.json()));

  return response.json();
}
