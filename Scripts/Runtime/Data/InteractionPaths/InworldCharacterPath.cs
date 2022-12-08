using UnityEngine;

namespace Inworld.Data
{
    [CreateAssetMenu(fileName = "InworldCharacterPath", menuName = "Inworld Lite/Interaction Paths/Character", order = 0)]
    public class InworldCharacterPath : InworldInteractionPath
    {
        [SerializeField] public string workspace;
        [SerializeField] public string character;
        
        public override string InteractionPath => $"workspaces/{workspace}/characters/{character}";
    }
}