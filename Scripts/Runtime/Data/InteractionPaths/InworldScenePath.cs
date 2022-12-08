using UnityEngine;

namespace Inworld.Data.InteractionPaths
{
    [CreateAssetMenu(fileName = "InworldScenePath", menuName = "Inworld Lite/Interaction Paths/Scene", order = 0)]
    public class InworldScenePath : InworldInteractionPath
    {
        [SerializeField] public string workspace;
        [SerializeField] public string scene;
        
        public override string InteractionPath => $"workspaces/{workspace}/scenes/{scene}";
    }
}