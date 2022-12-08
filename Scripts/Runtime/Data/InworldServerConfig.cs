using UnityEditor;
using UnityEngine;

namespace Inworld.Data
{
    [CreateAssetMenu(fileName = "InworldServerConfig", menuName = "Inworld Lite/Server Config", order = 0)]
    public class InworldServerConfig : ScriptableObject
    {
        [Header("Server")]
        public string scheme = "https";
        public string host;
        public int port = 443;
        public string apikey;
        
        [Header("Endpoints")]
        public string message = "message";

        public string GetEndpoint(Endpoints endpoint)
        {
            switch (endpoint)
            {
                case Endpoints.Message:
                    return message;
                default:
                    return null;
            }
        }
    }
    
    public enum Endpoints
    {
        Message
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(InworldServerConfig))]
    public class InworldServerConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            InworldServerConfig config = (InworldServerConfig) target;
            EditorGUILayout.LabelField("Server", EditorStyles.boldLabel);
            config.scheme = EditorGUILayout.TextField("Scheme", config.scheme);
            config.host = EditorGUILayout.TextField("Host", config.host);
            config.port = EditorGUILayout.IntField("Port", config.port);
            config.apikey = EditorGUILayout.PasswordField("API Key", config.apikey);
    
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Endpoints", EditorStyles.boldLabel);
            config.message = EditorGUILayout.TextField("Message", config.message);
        }
    }
    #endif
}