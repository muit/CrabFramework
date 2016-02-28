using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Crab
{
    [RequireComponent(typeof(SphereCollider))]
    public class Item : MonoBehaviour
    {
        public ItemData attributes;

        public bool moveWhenDisappear = false;

        public bool autoPickUp = false;
        public float pickUpDistance = 5;
        public bool playersCanPick = true;
        public bool entitiesCanPick = false;

        public float radius = 0.3f;

        public UnityEvent OnPickUp;

        [SerializeField]
        new private SphereCollider collider;
        

        void Start()
        {
            OnGameStart(SceneScript.Instance);
            UpdateCollider();
        }

        public void UpdateCollider()
        {
            collider = collider? collider : collider = GetComponent<SphereCollider>();

            float scaleSize = Mathf.Max(new float[] { transform.localScale.x, transform.localScale.y, transform.localScale.z });
            if (autoPickUp)
            {
                collider.radius = pickUpDistance / scaleSize;
            }
            else
            {
                collider.radius = radius / scaleSize;
            }
            collider.isTrigger = true;
        }

        public virtual void Reset()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }

        //Events
        protected virtual void OnGameStart(SceneScript scene) { }

        void PickUp(Entity entity)
        {
            UnityEngine.Debug.Log("Picked Up an Item!");
        }

        void OnTriggerEnter(Collider col)
        {
            if (!autoPickUp)
                return;

            Entity entity = col.GetComponent<Entity>();
            if (entity)
            {
                if ((entitiesCanPick && entity.IsAI()) || (playersCanPick && entity.IsPlayer()))
                {
                    OnPickUp.Invoke();
                    SendMessage("PickUp", entity);
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(Crab.Item))]
public class ItemEditor : Editor
{
    Crab.Item t;
    SphereCollider collider;
    void Awake()
    {
        t = target as Crab.Item;
        collider = t.GetComponent<SphereCollider>();
    }

    public override void OnInspectorGUI()
    {
        t.UpdateCollider();
        serializedObject.Update();

        //EditorGUILayout.PropertyField(serializedObject.FindProperty("attributes"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("moveWhenDisappear"), new GUIContent("Fly"));
        t.autoPickUp = EditorGUILayout.Toggle("Auto Pick Up", t.autoPickUp);
        EditorGUI.indentLevel++;
        if (t.autoPickUp)
        {
            t.pickUpDistance = EditorGUILayout.FloatField("Pick Up Distance", t.pickUpDistance);
        }
        else
        {
            t.radius = EditorGUILayout.FloatField("Radius", t.radius);
        }
        EditorGUI.indentLevel--;

        GUILayout.Space(5);

        EditorGUILayout.LabelField("Can be picked By: ", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playersCanPick"), new GUIContent("  Players"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("entitiesCanPick"), new GUIContent("  Entities"));
        EditorGUI.indentLevel--;

        GUILayout.Space(5);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("OnPickUp"));

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            t.UpdateCollider();
        }
    }
}
#endif