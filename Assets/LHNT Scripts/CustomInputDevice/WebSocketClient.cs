using System;
using System.Text;
using UnityEngine;
using NativeWebSocket;

public class WebSocketClient : MonoBehaviour
{
    private WebSocket websocket;
    public static int buttonState = 0; // This will be updated based on received data

    // Define a data structure that matches the incoming JSON
    [Serializable]
    public class WebSocketMessage
    {
        public float[] vector;
        public string text;
    }

    async void Start()
    {
        // Ensure the WebSocket URL is correct
        websocket = new WebSocket("ws://10.150.52.215:8000/ws");

        websocket.OnOpen += () => Debug.Log("Connected to WebSocket server");
        websocket.OnError += (e) => Debug.LogError($"WebSocket Error: {e}");
        websocket.OnClose += (e) => Debug.Log("WebSocket Closed");

        websocket.OnMessage += (bytes) =>
        {
            string rawMessage = Encoding.UTF8.GetString(bytes);
            Debug.Log($" Received from WebSocket: {rawMessage}");  // Always log received messages

            try
            {
                WebSocketMessage data = JsonUtility.FromJson<WebSocketMessage>(rawMessage);
                Debug.Log($"Parsed Data: text={data.text}, vector={string.Join(",", data.vector)}");

                if (data.vector.Length > 0)
                {
                    buttonState = (Mathf.RoundToInt(data.vector[UnityEngine.Random.Range(0, data.vector.Length)])) - 1;
                    Debug.Log($" Updated buttonState: {buttonState}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($" JSON Parse Error: {e.Message}");
            }
        };


        await websocket.Connect();
    }

    void Update()
    {
        if (websocket != null)
        {
            websocket.DispatchMessageQueue(); // Required to process received messages!
        }
    }


    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}