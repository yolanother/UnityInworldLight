using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inworld.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace Inworld
{
    public class InworldRequest
    {
        private InworldServerConfig _serverConfig;
        private InworldConfig _config;
        private InworldInteractionPath _interactionPath;
        public string sessionId;
        
        private static string _clientId;

        public static string ClientId
        {
            get
            {
                if (string.IsNullOrEmpty(_clientId))
                {
                    _clientId = Guid.NewGuid().ToString();
                }

                return _clientId;
            }
        }
        
        public InworldRequest(InworldServerConfig serverConfig, InworldConfig config, InworldInteractionPath interactionPath)
        {
            _serverConfig = serverConfig;
            _config = config;
            _interactionPath = interactionPath;
            _clientId = Guid.NewGuid().ToString();
        }
        
        public static string CreateQueryString(Dictionary<string, string> parameters)
        {
            return string.Join("&", parameters.Select(kvp => 
                string.Format("{0}={1}", kvp.Key, UnityWebRequest.EscapeURL(kvp.Value))));
        }

        public async void Get(string endpoint, Dictionary<string, string> queryParameters, Action<JSONNode> onResponse, Action<JSONNode> onError = null)
        {
            if (!queryParameters.ContainsKey("clientId"))
            {
                queryParameters.Add("clientId", ClientId);
            }
            if(!string.IsNullOrEmpty(sessionId) && !queryParameters.ContainsKey("sessionId"))
            {
                queryParameters.Add("sessionId", sessionId);
            }
            var auth = $"{_config.key}:{_config.secret}:{_serverConfig.apikey}";
            UriBuilder uriBuilder = new UriBuilder(_serverConfig.scheme, _serverConfig.host, _serverConfig.port, endpoint);
            uriBuilder.Query = CreateQueryString(queryParameters);
            #if UNITY_WEBGL
            var body = JsonUtility.ToJson(new RequestBody() { auth = auth });
            using var request = UnityWebRequest.Post(uriBuilder.Uri, body);
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
            request.SetRequestHeader("Content-Type", "application/json");
            #else
            using var request = UnityWebRequest.Get(uriBuilder.Uri);
            #endif
            request.SetRequestHeader("Authorization", $"Bearer {auth}");
            request.SetRequestHeader("Access-Control-Allow-Origin", "*");
            var downloadHandler = new InworldDownloadHandler(request);
            request.downloadHandler = downloadHandler;
            
            if (null != onResponse) downloadHandler.OnResponse += onResponse;

            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success || request.responseCode != 200)
            {
                onError?.Invoke(downloadHandler.Response);
            }

            downloadHandler.OnResponse -= onResponse;
        }

        public void SendEvent(string eventName, Action<JSONNode> onResponse, Action<JSONNode> onError = null)

        {
            var query = new Dictionary<string, string>()
            {
                { "e", eventName },
                {"scene", _interactionPath.InteractionPath }
            };
            Get(_serverConfig.customEvent, query, onResponse, onError);
        }

        public void Message(string message, Action<JSONNode> onResponse, Action<JSONNode> onError = null)

        {
            var query = new Dictionary<string, string>()
            {
                { "m", message },
                {"scene", _interactionPath.InteractionPath }
            };
            Get(_serverConfig.message, query, onResponse, onError);
        }
        
        public void StartSession(Action<JSONNode> onResponse, Action<JSONNode> onError = null)
        {
            var query = new Dictionary<string, string>()
            {
                {"scene", _interactionPath.InteractionPath }
            };
            Get(_serverConfig.startSession, query, onResponse, onError);
        }

        public void StartSession(string sessionId, Action<JSONNode> onResponse, Action<JSONNode> onError = null)
        {
            var query = new Dictionary<string, string>()
            {
                {"sessionId", sessionId},
                {"scene", _interactionPath.InteractionPath }
            };
            Get(_serverConfig.startSession, query, onResponse, onError);
        }
        
        public bool EndSession(Action<JSONNode> onResponse = null, Action<JSONNode> onError = null)
        {
            var query = new Dictionary<string, string>()
            {
                {"scene", _interactionPath.InteractionPath }
            };
            Get(_serverConfig.endSession, query, onResponse, onError);
            return true;
        }

        private class RequestBody
        {
            public string auth;
        }
    }
}