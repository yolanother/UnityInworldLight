using System;
using System.Text;
using Inworld.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Inworld
{
    public class InworldDownloadHandler : DownloadHandlerScript
    {
        public event Action<JSONNode> OnResponse;
        // Standard scripted download handler - allocates memory on each ReceiveData callback

        public InworldDownloadHandler(): base() {
        }

        public InworldDownloadHandler(byte[] buffer): base(buffer) {
        }

        // Required by DownloadHandler base class. Called when you address the 'bytes' property.

        protected override byte[] GetData() { return null; }

        // Called once per frame when data has been received from the network.

        protected override bool ReceiveData(byte[] data, int dataLength) {
            if(data == null || data.Length < 1) {
                return false;
            }

            var response = Encoding.UTF8.GetString(data);
            if (!string.IsNullOrEmpty(response))
            {
                try
                {
                    OnResponse?.Invoke(JSON.Parse(response));
                }
                catch (JsonException e)
                {
                    Debug.LogError(e);
                }
            }
            return true;
        }
    }
}