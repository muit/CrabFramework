using UnityEngine;
using UnityEditor;

namespace Crab
{
    [RequireComponent(typeof(SphereCollider))]
    public class Item : MonoBehaviour
    {
        public ItemType type;


        public bool autoPickUp = false;
        public float pickUpDistance = 5;
        public bool moveWhenDisappear = false;
        public bool playersCanPick = true;
        public bool entitiesCanPick = false;

        public float radius = 0.3f;

        
        private Entity disappearingTarget;
        private Rigidbody rigidbody;

        void Start()
        {
            OnGameStart(SceneScript.Instance);
            rigidbody = GetComponent<Rigidbody>();
            UpdateCollider();
        }

        void UpdateCollider()
        {
            SphereCollider collider = GetComponent<SphereCollider>();
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

        void Update()
        {
            if (disappearingTarget)
            {
                Vector3 direction = (disappearingTarget.transform.position - transform.position).normalized;
                rigidbody.AddForce(direction * 800 * Time.deltaTime);
                
                if (Vector3.Distance(disappearingTarget.transform.position, transform.position) <= radius)
                {
                    SendMessage("PickUp", disappearingTarget);
                    GameObject.Destroy(gameObject);
                }
            }
        }

        //Events
        protected virtual void OnGameStart(SceneScript scene) { }

        void PickUp(Entity entity)
        {
            UnityEngine.Debug.Log("Picked Up an Item!");
        }

        void OnTriggerEnter(Collider col)
        {

            Entity entity = col.GetComponent<Entity>();
            if (entity)
            {
                if ((entitiesCanPick && entity.IsAI()) || (playersCanPick && entity.IsPlayer()))
                {
                    if (!autoPickUp || (autoPickUp && !moveWhenDisappear) || !rigidbody)
                    {
                        SendMessage("PickUp", entity);
                        GameObject.Destroy(gameObject);
                    }
                    else
                    {
                        rigidbody.useGravity = false;

                        autoPickUp = false;
                        UpdateCollider();

                        if(!disappearingTarget)
                            disappearingTarget = entity;
                    }
                }
            }
        }
    }
}


[CustomEditor(typeof(Crab.Item))]
public class ItemEditor : Editor
{
    Crab.Item t;

    void Awake()
    {
        t = target as Crab.Item;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, new string[] {
            "autoPickUp",
            "pickUpDistance",
            "moveWhenDisappear",
            "playersCanPick",
            "entitiesCanPick",
            "radius"
        });


        t.autoPickUp = EditorGUILayout.Toggle("Auto Pick Up", t.autoPickUp);

        EditorGUI.indentLevel++;
        if (t.autoPickUp)
        {
            t.pickUpDistance = EditorGUILayout.FloatField("Pick Up Distance", t.pickUpDistance);

            t.moveWhenDisappear = EditorGUILayout.Toggle("Follow on pick up", t.moveWhenDisappear);

            EditorGUILayout.LabelField("Can be picked By: ", EditorStyles.boldLabel);
            t.playersCanPick = EditorGUILayout.Toggle("  Players", t.playersCanPick);
            t.entitiesCanPick = EditorGUILayout.Toggle("  Entities", t.entitiesCanPick);
        }
        else
        {
            t.radius = EditorGUILayout.FloatField("Radius", t.radius);
        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            SphereCollider collider = t.GetComponent<SphereCollider>();
            float scaleSize = Mathf.Max(new float[] { t.transform.localScale.x, t.transform.localScale.y, t.transform.localScale.z });
            if (t.autoPickUp)
            {
                collider.radius = t.pickUpDistance / scaleSize;
            }
            else
            {
                collider.radius = t.radius / scaleSize;
            }
            collider.isTrigger = true;
        }
    }
}