import { createContext, useContext, useState, useEffect } from 'react';
import { GetCookies } from '../Functions/HelperMethods';
import { ClearCookies } from '../Functions/HelperMethods';

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);

  useEffect(() => {
    const cookies = GetCookies();
    setUser(cookies.user || null);
  }, []);

  const signin = () => {
    const cookies = GetCookies();
    setUser(cookies.user || null);
  };

  const signout = async () => {
    const request = new Request(`http://localhost:5000/api/user/sign_out`, {
      method: "POST",
      credentials: "include",
      headers: {
          "Content-Type": "application/json", // Correct Content-Type for JSON
        }
      });
    await fetch(request)
      .then(res => res.text())
      .then(data => JSON.parse(data))
      .then(results => { console.log(results) });
    ClearCookies();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, signin, signout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}
