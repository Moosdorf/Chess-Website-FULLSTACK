import { Button, Card, CardBody, Form, InputGroup } from "react-bootstrap";
import {  useState } from "react";
import { useAuth } from "../Data/AuthProvider";
import { useSignalRGame } from "../SignalR/SingalRGameProvider";





function Chat( {sessionId} ) {
    const { user }  = useAuth();
    const { connection, messages } = useSignalRGame();

    const [inputValue, setInputValue] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
        if (inputValue.trim()) {

            connection.invoke("SendMessageToGroup", user, inputValue, sessionId);
            setInputValue("");
        }
    };

    return (
        <Card className="chat mb-1">
            <CardBody style={{ padding: 10 }}>
                <div className="chess-chat-content">
                    {messages.map((message) => (
                        <div 
                            key={message.id}
                            className={`chat-message-${message.sender === "System" ? "system" : message.isOwn ? 'own' : 'opponent'}`}
                        >
                            <div className="chat-message-sender">{message.sender +": "+ message.text}</div>
                        </div>
                    ))}
                </div>
            </CardBody>
            <Form onSubmit={handleSubmit}>
                <InputGroup className="igrp">
                    <Form.Control 
                        className="chat-area" 
                        placeholder="Enter message"
                        value={inputValue}
                        onChange={(e) => setInputValue(e.target.value)}
                    /> 
                    <Button type="submit">Send</Button>
                </InputGroup>
            </Form>
        </Card>
    );
}

export default Chat;