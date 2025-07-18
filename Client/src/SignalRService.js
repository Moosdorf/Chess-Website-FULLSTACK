import { useCallback, useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { useAuth } from "./Data/AuthProvider";



export default function useSignalR() {
  const { user } = useAuth();

  const [connection, setConnection] = useState(null);
  const [messages, setMessages] = useState([
      { id: 0, sender: 'System', text: 'Game started!', isOwn: false }
  ]);

 useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5000/gameHub")
      .configureLogging(signalR.LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    return () => {
      if (newConnection) newConnection.stop();
    };
  }, []);

  
  // Start connection
  const startConnection = useCallback(async () => {
    if (connection) {
      try {
        await connection.start();
        console.log("SignalR Connected.");
      } catch (err) {
        console.log("Connection failed: ", err);
      }
    }
  }, [connection]);
  
  useEffect(() => {
    if (connection) {
      connection.on("ReceiveMessage", (username, message) => {
        setMessages(prev => [...prev, 
          { id: prev.length , sender: username === user ? "You" : username, text: message, isOwn: username === user ? true : false }
        ]);
      });

    }
  }, [connection]);

  return { connection, startConnection, messages };
};

