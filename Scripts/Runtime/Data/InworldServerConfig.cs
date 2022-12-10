using System;
using UnityEditor;
using UnityEngine;

namespace Inworld.Data
{
    [CreateAssetMenu(fileName = "InworldServerConfig", menuName = "Inworld Lite/Server Config", order = 0)]
    [Serializable]
    public class InworldServerConfig : ScriptableObject
    {
        [Header("Server")]
        public string scheme = "https";
        public string host;
        public int port = 443;
        public string apikey;
        
        [Header("Endpoints")]
        public string message = "message";
        public string startSession = "start-session";
        public string endSession = "end-session";

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
            var scheme = EditorGUILayout.TextField("Scheme", config.scheme);
            if (scheme != config.scheme)
            {
                config.scheme = scheme;
                EditorUtility.SetDirty(config);
            }
            var host = EditorGUILayout.TextField("Host", config.host);
            if (host != config.host)
            {
                config.host = host;
                EditorUtility.SetDirty(config);
            }
            var port = EditorGUILayout.IntField("Port", config.port);
            if (port != config.port)
            {
                config.port = port;
                EditorUtility.SetDirty(config);
            }
            var apikey = EditorGUILayout.PasswordField("API Key", config.apikey);
            if (apikey != config.apikey)
            {
                config.apikey = apikey;
                EditorUtility.SetDirty(config);
            }
    
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Endpoints", EditorStyles.boldLabel);
            var message = EditorGUILayout.TextField("Message", config.message);
            if (message != config.message)
            {
                config.message = message;
                EditorUtility.SetDirty(config);
            }
            var startSession = EditorGUILayout.TextField("Start Session", config.startSession);
            if (startSession != config.startSession)
            {
                config.startSession = startSession;
                EditorUtility.SetDirty(config);
            }
            var endSession = EditorGUILayout.TextField("End Session", config.endSession);
            if (endSession != config.endSession)
            {
                config.endSession = endSession;
                EditorUtility.SetDirty(config);
            }
        }
    }
    #endif
}