using System;
using Inworld.Data;
using UnityEngine;

namespace Inworld
{
    public class InworldInteraction : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private InworldInteractionPath _interactionPath;
        [SerializeField] private InworldConfig _inworldConfig;
        [SerializeField] private InworldServerConfig _inworldServerConfig;
        
        [Header("Events")]
        [SerializeField] private InworldEvents _inworldEvents = new InworldEvents();
        
        public InworldEvents InworldEvents => _inworldEvents;

        private InworldRequest _inworldRequest;

        public InworldRequest InworldRequest
        {
            get
            {
                if (null == _inworldRequest)
                {
                    _inworldRequest = new InworldRequest(_inworldServerConfig, _inworldConfig, _interactionPath);
                }
                
                return _inworldRequest;
            }
        }

        public void SendText(string text)
        {
            SendText(text, null);
        }

        public void SendText(string text, Action<JSONNode> onComplete)
        {
            string transcription = "";
            var interactionId = Guid.NewGuid().ToString();
            SendMessage("OnInteractionStart", interactionId, SendMessageOptions.DontRequireReceiver);
            InworldEvents.OnInteractionStart.Invoke(interactionId);
            InworldRequest.Message(text, (response) =>
            {
                if (response.HasKey("text"))
                {
                    var text = response["text"]["text"].Value;
                    if (transcription.Length > 0) transcription += "\n";
                    transcription += text;
                    SendMessage("OnSpeak", text, SendMessageOptions.DontRequireReceiver);
                    InworldEvents.OnSpeak.Invoke(text);
                    SendMessage("OnPartialTranscription", transcription, SendMessageOptions.DontRequireReceiver);
                    InworldEvents.OnPartialTranscription.Invoke(transcription);
                }
                
                if (response.IsInteractionEnd())
                {
                    response["text"]["text"] = transcription;
                    response["text"]["isFinal"] = true;
                    InworldEvents.OnFullTranscription.Invoke(transcription);
                        
                    SendMessage("OnInteractionEnd", interactionId, SendMessageOptions.DontRequireReceiver);
                    InworldEvents.OnInteractionEnd.Invoke(interactionId);
                    onComplete?.Invoke(response);
                }

                SendMessage("OnResponse", response, SendMessageOptions.DontRequireReceiver);
                InworldEvents.OnResponse.Invoke(response);
            });
        }
    }
    
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(InworldInteraction))]
    public class InworldCharacterEditor : UnityEditor.Editor
    {
        private InworldInteraction _inworldInteraction;
        private string _text;

        private void OnEnable()
        {
            _inworldInteraction = (InworldInteraction) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying)
            {
                _text = UnityEditor.EditorGUILayout.TextField("Text", _text);
                if (GUILayout.Button("Send Text"))
                {
                    _inworldInteraction.SendText(_text);
                }
            }
        }
    }
    #endif
}