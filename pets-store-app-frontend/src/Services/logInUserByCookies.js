export default async function loginUserByCookies() {
  const response = await fetch(
    `https://localhost:7123/api/Auth/login-by-cookies`,
    { credentials: "include" }
  );

  if (!response.ok) throw new Error(JSON.stringify(await response.json()));

  return response.json();
}
