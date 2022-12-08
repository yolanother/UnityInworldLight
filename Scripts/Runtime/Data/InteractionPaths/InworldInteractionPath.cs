using UnityEngine;

namespace Inworld.Data
{
    public abstract class InworldInteractionPath : ScriptableObject
    {
        public abstract string InteractionPath { get; }
    }
}