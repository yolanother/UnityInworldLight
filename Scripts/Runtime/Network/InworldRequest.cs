using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inworld.Data;
using UnityEngine.Networking;

namespace Inworld
{
    public class InworldRequest
    {
        private InworldServerConfig _serverConfig;
        private InworldConfig _config;
        private InworldInteractionPath _interactionPath;
        private string _clientId;
        
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

        public async void Get(string endpoint, Dictionary<string, string> queryParameters, Action<JSONNode> onResponse, Action<long> onError = null)
        {
            if (!queryParameters.ContainsKey("clientId"))
            {
                queryParameters.Add("clientId", _clientId);
            }
            UriBuilder uriBuilder = new UriBuilder(_serverConfig.scheme, _serverConfig.host, _serverConfig.port, endpoint);
            uriBuilder.Query = CreateQueryString(queryParameters);
            using var www = UnityWebRequest.Get(uriBuilder.Uri);
            www.SetRequestHeader("Authorization", $"Bearer {_config.key}:{_config.secret}:{_serverConfig.apikey}");
            var downloadHandler = new InworldDownloadHandler();
            www.downloadHandler = downloadHandler;
            if (null != onResponse)
            {
                downloadHandler.OnResponse += onResponse;
            }

            var operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(www.responseCode);
            }

            downloadHandler.OnResponse -= onResponse;
        }

        public void Message(string message, Action<JSONNode> onResponse, Action<long> onError = null)

        {
            var query = new Dictionary<string, string>()
            {
                { "m", message },
                {"scene", _interactionPath.InteractionPath }
            };
            Get("message", query, onResponse, onError);
        }
        
        public void StartSession(Action<JSONNode> onResponse, Action<long> onError = null)
        {
            var query = new Dictionary<string, string>()
            {
                {"scene", _interactionPath.InteractionPath }
            };
            Get("start-session", query, onResponse, onError);
        }

        public void StartSession(string sessionId, Action<JSONNode> onResponse, Action<long> onError = null)
        {
            var query = new Dictionary<string, string>()
            {
                {"sessionId", sessionId},
                {"scene", _interactionPath.InteractionPath }
            };
            Get("start-session", query, onResponse, onError);
        }
        
        public bool EndSession(Action<JSONNode> onResponse = null, Action<long> onError = null)
        {
            var query = new Dictionary<string, string>()
            {
                {"scene", _interactionPath.InteractionPath }
            };
            Get("end-session", query, onResponse, onError);
            return true;
        }
    }
}