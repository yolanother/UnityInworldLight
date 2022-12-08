using Inworld.Data;
using UnityEngine;

namespace DefaultNamespace.EventHandlers
{
    public class InworldEventLogger : MonoBehaviour
    {
        [TextArea]
        [SerializeField] public string lastSpokenText;
        [TextArea]
        [SerializeField] public string lastResponse;

        [TextArea] public string lastTranscription;
        
        public void OnPartialTranscription(string transcription)
        {
            lastTranscription = transcription;
            Debug.Log("Partial transcription: " + transcription);
        }
        
        public void OnFullTranscription(string transcription)
        {
            lastTranscription = transcription;
            Debug.Log("Full transcription: " + transcription);
        }
        
        public void OnSpeak(string text)
        {
            lastSpokenText = text;
            Debug.Log("OnSpeak: " + text);
        }

        public void OnInteractionStart(string interactionId)
        {
            Debug.Log("Interaction Started: " + interactionId);
        }

        public void OnInteractionEnd(string interactionId)
        {
            Debug.Log("Interaction Ended " + interactionId);
        }

        public void OnResponse(JSONNode response)
        {
            lastResponse = response.ToString();
            Debug.Log("OnResponse: " + lastResponse);
        }
    }
}