export default async function forgetPasswordRequest(email) {
  const response = await fetch(
    `https://localhost:7123/api/Auth/forget-password?email=` + email,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
    }
  );

  if (!response.ok) throw new Error(JSON.stringify(await response.json()));

  return response.json();
}
