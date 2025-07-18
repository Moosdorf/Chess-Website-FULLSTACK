import * as signalR from "@microsoft/signalr";

let connection;

export const connectToHub = async (username, onMessageReceived) => {
  connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5000/gameHub", {
      withCredentials: true
    })
    .build();

  connection.on("ReceiveMessage", (user, message) => {
    console.log("message received:", message);
    console.log("username:", user);
    if (onMessageReceived) onMessageReceived(message);
  });

  await connection.start();
  console.log("SendMessage");
  await connection.invoke("SendMessage", username, "hello");
};

