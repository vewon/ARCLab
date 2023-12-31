using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenAIController : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;

    private OpenAIAPI api;
    private List<ChatMessage> messages;

    public ShowDetailsOnTouch showDetailsOnTouch;
    private Dictionary<string, string> imageDetails;

    // Start is called before the first frame update
    void Start()
    {
        imageDetails = showDetailsOnTouch.ImageDetails;

        // Add a debug log to check if imageDetails are correctly fetched
        Debug.Log("Number of image details fetched: " + imageDetails.Count);

        // This line gets your API key (and could be slightly different on Mac/Linux)
        //api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        api = new OpenAIAPI(""
);

        StartConversation();
        okButton.onClick.AddListener(() => GetResponse());
    }

    public void StartConversation()
    {
        string currentImageName = showDetailsOnTouch.CurrentImageName;

        string detailString = "";

        if (imageDetails.TryGetValue(currentImageName, out detailString))
        {
            messages = new List<ChatMessage> {
                new ChatMessage(ChatMessageRole.System, "You are an assistant to the user. The purpose of the application is to identify electronic components. You will respond to any of the users inquiry about the electronic component. You will keep your responses short and to the point." + detailString)
            };

            inputField.text = "";
            string startString = "What would you like to know about the " + detailString + "?";
            textField.text = startString;
            Debug.Log(startString);
        }
        else
        {
            Debug.LogError("No details found for image: " + currentImageName);
        }

        // string detailString = GenerateDetailsString(imageDetails);
        // messages = new List<ChatMessage> {
        //     new ChatMessage(ChatMessageRole.System, "You are an assistant to the user. The purpose of the application is to identify electronic components. You will respond to any of the users inquiry about the electronic component. You will keep your responses short and to the point." + detailString)
        // };

        // inputField.text = "";
        // string startString = "What would you like to know? " + detailString;
        // textField.text = startString;
        // Debug.Log(startString);
    }

    string GenerateDetailsString(Dictionary<string, string> imageDetails)
    {
        string details = "";
        foreach (KeyValuePair<string, string> pair in imageDetails)
        {
            details += pair.Value + "\n";
        }

        // Add a debug log to check the generated detailString
        Debug.Log("Generated detail string: " + details);

        return details;
    }

    private async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }

        // Disable the OK button
        okButton.enabled = false;

        // Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        if (userMessage.Content.Length > 100)
        {
            // Limit messages to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 100);
        }
        Debug.Log(string.Format("{0}: {1}", userMessage.rawRole, userMessage.Content));

        // Add the message to the list
        messages.Add(userMessage);

        // Update the text field with the user message
        textField.text = string.Format("User: {0}", userMessage.Content);

        // Clear the input field
        inputField.text = "";

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            MaxTokens = 50,
            Messages = messages
        });

        // Get the response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));

        // Add the response to the list of messages
        messages.Add(responseMessage);

        // Update the text field with the response
        textField.text = string.Format("User: {0}\n\nARC: {1}", userMessage.Content, responseMessage.Content);

        // Re-enable the OK button
        okButton.enabled = true;
    }
}