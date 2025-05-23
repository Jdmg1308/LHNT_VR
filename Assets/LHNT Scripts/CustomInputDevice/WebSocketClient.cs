using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using NativeWebSocket;
using TMPro;

public class WebSocketClient : MonoBehaviour
{
    private static WebSocketClient _instance;
    public string IP_address;
    private WebSocket websocket;
    public static string buttonState = "00"; // This will be updated based on received data
    public TextMeshProUGUI status_descriptor;

    // Define a data structure that matches the incoming JSON
    [Serializable]
    public class WebSocketMessage
    {
        public float[] vector;
        public string text;
    }

    async void Start()
    {
        status_descriptor.text = "not connected";
        // Ensure the WebSocket URL is correct

        // websocket = new WebSocket("ws://172.20.10.3:8000/ws");
        // websocket = new WebSocket("ws://" + IP_address + "/ws"); // 10.150.52.215:8000
    }

    public async void ConnectToWebSocket(string ip)
    {
        IP_address = ip;
        websocket = new WebSocket("ws://" + IP_address + "/ws");
        status_descriptor.text = "Attempting to connect";
        websocket.OnOpen += () => {
            Debug.Log("Connected to WebSocket server");
            // Update UI to show checkmark
            status_descriptor.text = "Connected to WebSocket server";
        };

        websocket.OnError += (e) => {
            Debug.Log($"WebSocket Error: {e}");
            // Update UI to show error icon
            status_descriptor.text = $"WebSocket Error: {e}";
        };

        websocket.OnClose += (e) => {
            Debug.Log("WebSocket Closed");
            status_descriptor.text = "WebSocket Closed";
        };

        websocket.OnMessage += (bytes) =>
        {
            string rawMessage = Encoding.UTF8.GetString(bytes);
            //Debug.Log($" Received from WebSocket: {rawMessage}");  // Always log received messages

            try
            {
                if (rawMessage.TrimStart().StartsWith("{"))
                {
                    // Proper JSON object, parse normally
                    WebSocketMessage data = JsonUtility.FromJson<WebSocketMessage>(rawMessage);
                    Debug.Log($"Parsed Data: text={data.text}, vector={string.Join(",", data.vector)}");

                    if (data.vector.Length > 0)
                    {
                        buttonState = data.vector[UnityEngine.Random.Range(0, data.vector.Length)].ToString();
                        Debug.Log($"Updated buttonState: {buttonState}");
                    }
                }
                else
                {
                    buttonState = rawMessage.ToString();
                    // Otherwise it's just a simple number or string
                    //int simpleValue;
                    //if (int.TryParse(rawMessage, out simpleValue))
                    //{
                    //    buttonState = simpleValue.ToString();
                    //    //Debug.Log($"Parsed simple buttonState: {buttonState}");
                    //}
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

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Subscribe to scene change
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // If a duplicate somehow spawns, disable it
            gameObject.SetActive(false);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Deactivate any duplicates in the newly loaded scene
        WebSocketClient[] objects = FindObjectsOfType<WebSocketClient>();
        foreach (var obj in objects)
        {
            if (obj != this)
            {
                obj.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        // Always clean up event subscription
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}