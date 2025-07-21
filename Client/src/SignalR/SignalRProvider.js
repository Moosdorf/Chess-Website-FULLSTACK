import { createContext, useContext, useEffect, useRef, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { useAuth } from "../Data/AuthProvider";

const SignalRContext = createContext(null);

export function SignalRProvider({ children }) {
  const { user } = useAuth();
  const [connection, setConnection] = useState(null);
  const startedRef = useRef(false); // guard against double-start

  // Only build a connection when logged in; stop on logout.
  useEffect(() => {
    if (!user) {
      if (connection) connection.stop();
      setConnection(null);
      return;
    }

    const conn = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5000/gameHub") 
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    setConnection(conn);

    return () => {
      conn.stop();
    };
  }, [user]);


  useEffect(() => {
    if (!connection || startedRef.current) return;
    startedRef.current = true;

    connection
      .start()
      .then(() => {
        console.log("SignalR connected.");
      })
      .catch((err) => {
        console.error("SignalR start error:", err);
        startedRef.current = false; 
      });
  }, [connection]);

  return (
    <SignalRContext.Provider value={connection}>
      {children}
    </SignalRContext.Provider>
  );
}

export function useSignalRConnection() {
  return useContext(SignalRContext);
}
