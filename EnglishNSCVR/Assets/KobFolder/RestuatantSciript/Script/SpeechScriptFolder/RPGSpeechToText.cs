using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.InputSystem;

public class SpeechToTextDebug : MonoBehaviour
{
    [SerializeField] ConverManager_New converManager;
    private DictationRecognizer dictation;
   
    private bool isListening = false;


    void Start()
    {
        // converManager = FindFirstObjectByType<ConverManager>();
        dictation = new DictationRecognizer();

        // Real-time (optional)
        dictation.DictationHypothesis += text =>
        {
            Debug.Log("Listening... " + text);
            
        };

        // Final speech result
        dictation.DictationResult += (text, confidence) =>
        {
            Debug.Log($"PLAYER SAID: \"{text}\" | Confidence: {confidence}");
            converManager.SentMessageToServer(text);
            

        };

        dictation.DictationComplete += cause =>
        {
            isListening = false;
            Debug.Log("Dictation stopped: " + cause);
        };

        dictation.DictationError += (error, hresult) =>
        {
            isListening = false;
            Debug.LogError("Speech error: " + error);
        };
    }

    void Update()
    {
        // Push-to-talk (hold T)
        if (Keyboard.current[Key.T].wasPressedThisFrame)
        {
            StartListening();
            
        }

        if (Keyboard.current[Key.T].wasReleasedThisFrame)
        {
            StopListening();
        }
    }

    void StartListening()
    {
        if (isListening) return;

        isListening = true;
        dictation.Start();
        Debug.Log("🎤 START LISTENING");
    }

    void StopListening()
    {
        if (!isListening) return;

        dictation.Stop();
        Debug.Log("⏹ STOP LISTENING");
    }

    void OnDestroy()
    {
        if (dictation != null)
            dictation.Dispose();
    }
}
