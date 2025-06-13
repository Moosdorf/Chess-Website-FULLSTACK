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

  const signout = () => {
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
