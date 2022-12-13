using System;
using Inworld.Data;
using UnityEngine.Events;

namespace Inworld
{
    [Serializable]
    public class InworldEvents
    {
        public UnityEvent<JSONNode> OnResponse;
        public UnityEvent<string> OnSpeak;
        public UnityEvent<string> OnPartialTranscription;
        public UnityEvent<string> OnFullTranscription;
        public UnityEvent<string> OnInteractionStart;
        public UnityEvent<string> OnInteractionEnd;
        public UnityEvent OnSessionStarted;
        public UnityEvent<JSONNode> OnFirstResponse;
    }
}