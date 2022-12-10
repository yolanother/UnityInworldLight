using System;
using Inworld.Data;
using UnityEditor;
using UnityEngine;

namespace Inworld
{
    public class InworldInteraction : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private InworldInteractionPath _interactionPath;
        [SerializeField] private InworldConfig _inworldConfig;
        [SerializeField] private InworldServerConfig _inworldServerConfig;

        [Header("Session")]
        [SerializeField] private bool _startSessionOnEnable = true;
        [SerializeField] private bool _endSessionOnDisable = true;
        
        [Header("Events")]
        [SerializeField] private InworldEvents _inworldEvents = new InworldEvents();
        
        public InworldEvents InworldEvents => _inworldEvents;

        private InworldRequest _inworldRequest;
        private string _session;

        public string Session => _session;

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

        private void OnEnable()
        {
            if (_startSessionOnEnable)
            {
                StartSession();
            }
        }
        
        private void OnDisable()
        {
            if (_endSessionOnDisable)
            {
                EndSession();
            }
        }

        public void StartSession()
        {
            if(!string.IsNullOrEmpty(_session)) StartSession(_session);
            else InworldRequest.StartSession((result) =>
            {
                _session = result.GetSessionId();
                InworldRequest.sessionId = _session;
            });
        }
        
        public void StartSession(string sessionId)
        {
            InworldRequest.sessionId = sessionId;
            InworldRequest.StartSession(sessionId, (result) =>
            {
                _session = result.GetSessionId();
                InworldRequest.sessionId = _session;
            });
        }

        public void EndSession()
        {
            InworldRequest.sessionId = null;
            InworldRequest.EndSession();
        }

        public void SendText(string text)
        {
            SendText(text, null);
        }

        public void SendText(string text, Action<JSONNode> onComplete, Action<JSONNode> onError = null)
        {
            if (string.IsNullOrEmpty(text)) return;
            
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
            }, error =>
            {
                onError?.Invoke(error);
            });
        }
        
#if UNITY_EDITOR
        [MenuItem("GameObject/Inworld Lite/Character", false, 10)]
        private static void CreateIntentHandler()
        {
            var handler = new GameObject("Character");
            var interaction = handler.AddComponent<InworldInteraction>();
            if (Selection.activeGameObject)
            {
                handler.transform.parent = Selection.activeGameObject.transform;
            }
        }
#endif
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
                GUILayout.Label(_inworldInteraction.Session, EditorStyles.miniLabel);
            }
        }
    }
    #endif
}