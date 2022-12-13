using System;
using UnityEditor;
using UnityEngine;

namespace Inworld.Triggers
{
    public class InworldEventTrigger : MonoBehaviour
    {
        [Header("Trigger Properties")]
        [SerializeField] private TriggerType _triggerType = TriggerType.Enter;
        [SerializeField] private string _eventName;
        
        [Header("Interaction Target")]
        [SerializeField] private InworldInteraction _interaction;

#if UNITY_EDITOR
        [MenuItem("GameObject/Inworld Lite/Triggers/Add Event Box Trigger", false, 10)]
        private static void AddEventBoxTrigger()
        {
            if (Selection.activeGameObject)
            {
                if (!HasTrigger(Selection.activeGameObject))
                {
                    var collider = Selection.activeGameObject.AddComponent<BoxCollider>();
                    collider.isTrigger = true;
                }
                var trigger = Selection.activeGameObject.AddComponent<InworldEventTrigger>();
                trigger._interaction = FindObjectOfType<InworldInteraction>();
            }
        }
        [MenuItem("GameObject/Inworld Lite/Triggers/Add Event Box Trigger", true, 10)]
        private static bool AddEventBoxTriggerValidate()
        {
            return Selection.activeGameObject;
        }
        [MenuItem("GameObject/Inworld Lite/Triggers/Add Event Sphere Trigger", false, 10)]
        private static void AddEventSphereTrigger()
        {
            if (Selection.activeGameObject)
            {
                if (!HasTrigger(Selection.activeGameObject))
                {
                    var collider = Selection.activeGameObject.AddComponent<SphereCollider>();
                    collider.isTrigger = true;
                }
                var trigger = Selection.activeGameObject.AddComponent<InworldEventTrigger>();
                trigger._interaction = FindObjectOfType<InworldInteraction>();
            }
        }
        [MenuItem("GameObject/Inworld Lite/Triggers/Add Event Sphere Trigger", true, 10)]
        private static bool AddEventSphereTriggerValidate()
        {
            return Selection.activeGameObject;
        }
        [MenuItem("GameObject/Inworld Lite/Triggers/Add Event Capsule Trigger", false, 10)]
        private static void AddEventCapsuleTrigger()
        {
            if (Selection.activeGameObject)
            {
                if (!HasTrigger(Selection.activeGameObject))
                {
                    var collider = Selection.activeGameObject.AddComponent<CapsuleCollider>();
                    collider.isTrigger = true;
                }
                var trigger = Selection.activeGameObject.AddComponent<InworldEventTrigger>();
                trigger._interaction = FindObjectOfType<InworldInteraction>();
            }
        }
        [MenuItem("GameObject/Inworld Lite/Triggers/Add Event Capsule Trigger", true, 10)]
        private static bool AddEventCapsuleTriggerValidate()
        {
            return Selection.activeGameObject;
        }
        
        
        [MenuItem("GameObject/Inworld Lite/Triggers/Event Box Trigger", false, 10)]
        private static void CreateBoxTrigger()
        {
            var handler = new GameObject("Trigger");
            var collider = handler.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            var trigger = handler.AddComponent<InworldEventTrigger>();
            trigger._interaction = FindObjectOfType<InworldInteraction>();
            if (Selection.activeGameObject)
            {
                handler.transform.parent = Selection.activeGameObject.transform;
            }
        }
        [MenuItem("GameObject/Inworld Lite/Triggers/Event Box Trigger", true, 10)]
        private static bool CreateBoxTriggerValidate()
        {
            return !Selection.activeGameObject;
        }
        [MenuItem("GameObject/Inworld Lite/Triggers/Event Sphere Trigger", false, 10)]
        private static void CreateSphereTrigger()
        {
            var handler = new GameObject("Trigger");
            var collider = handler.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            var trigger = handler.AddComponent<InworldEventTrigger>();
            trigger._interaction = FindObjectOfType<InworldInteraction>();
            if (Selection.activeGameObject)
            {
                handler.transform.parent = Selection.activeGameObject.transform;
            }
        }
        [MenuItem("GameObject/Inworld Lite/Triggers/Event Sphere Trigger", true, 10)]
        private static bool CreateSphereTriggerValidate()
        {
            return !Selection.activeGameObject;
        }
        [MenuItem("GameObject/Inworld Lite/Triggers/Event Capsule Trigger", false, 10)]
        private static void CreateCapsuleTrigger()
        {
            var handler = new GameObject("Trigger");
            var collider = handler.AddComponent<CapsuleCollider>();
            collider.isTrigger = true;
            var trigger = handler.AddComponent<InworldEventTrigger>();
            trigger._interaction = FindObjectOfType<InworldInteraction>();
            if (Selection.activeGameObject)
            {
                handler.transform.parent = Selection.activeGameObject.transform;
            }
        }
        [MenuItem("GameObject/Inworld Lite/Triggers/Event Capsule Trigger", true, 10)]
        private static bool CreateCapsuleTriggerValidate()
        {
            return !Selection.activeGameObject;
        }
#endif

        public static bool HasTrigger(GameObject gameObject)
        {
            
            var colliders = gameObject.GetComponents<Collider>();
            if (colliders.Length == 0) return false;

            bool hasValidTrigger = false;
            foreach(var c in colliders)
            {
                if (c.isTrigger)
                {
                    hasValidTrigger = true;
                    break;
                }
            }

            return hasValidTrigger;
        }
        
        private void OnValidate()
        {
            if (!HasTrigger(gameObject))
            {
                Debug.LogWarning("No trigger collider found on " + name + "!");
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_triggerType.HasFlag(TriggerType.Enter))
            {
                _interaction.SendEvent(_eventName);
            }
        }
    
        public void OnTriggerExit(Collider other)
        {
            if (_triggerType.HasFlag(TriggerType.Exit))
            {
                _interaction.SendEvent(_eventName);
            }
        }

        [Flags]
        public enum TriggerType
        {
            None,
            Enter,
            Exit
        }
    }
}