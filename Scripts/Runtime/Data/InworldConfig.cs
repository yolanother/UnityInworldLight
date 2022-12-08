using UnityEditor;
using UnityEngine;

namespace Inworld.Data
{
    [HelpURL("https://studio.inworld.ai/workspaces/ai_art_gallery/integrations")]
    [CreateAssetMenu(fileName = "InworldConfig", menuName = "Inworld Lite/Integration Config", order = 0)]
    public class InworldConfig : ScriptableObject
    {
        [SerializeField] public string key;
        [SerializeField] public string secret;
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(InworldConfig))]
    public class InworldConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var inworldConfig = (InworldConfig)target;
            var key = EditorGUILayout.PasswordField("Key", inworldConfig.key);
            if (key != inworldConfig.key)
            {
                inworldConfig.key = key;
                EditorUtility.SetDirty(inworldConfig);
            }
            var secret = EditorGUILayout.PasswordField("Secret", inworldConfig.secret);
            if (secret != inworldConfig.secret)
            {
                inworldConfig.secret = secret;
                EditorUtility.SetDirty(inworldConfig);
            }
        }
    }
    #endif
}