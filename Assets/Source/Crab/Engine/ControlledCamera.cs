using UnityEngine;
using Crab.Entities;

namespace Crab {
    [RequireComponent(typeof(Camera))]
    public class ControlledCamera : MonoBehaviour {
        public Transform target;

        public Vector3 targetOffset = new Vector3(0, 2.0f, 0);
        [Header("Distance")]
        public float zoom = 5.0f;
        public float maxDistance = 20;
        public float minDistance = 2.5f;
        [Space(5)]
        public float zoomRate = 20;
        [Header("Speed")]
        public float xSpeed = 250.0f;
        public float ySpeed = 120.0f;
        [Space(5)]
        public float rotationDampening = 3.0f;
        [Header("Limit")]
        public float yMinLimit = -20;
        public float yMaxLimit = 80;

        [Header("Collision & Occlusion")]
        public LayerMask collidesWith;
        public float smoothCollisionRate = 15f;


        private float x = 0.0f;
        private float y = 0.0f;
        private float expectedDistance = 5.0f;
        private float realDistance = 5.0f;
        private bool mouseLocked = true;

        private CMovement targetMovement;
        new private Camera camera;


        public void LockMouse(bool value)
        {
            mouseLocked = value;
            if (value)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }


        void Start() {
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            // Make the rigid body not change rotation
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().freezeRotation = true;

            targetMovement = target.GetComponent<CMovement>();
            camera = GetComponent<Camera>();
        }

        void LateUpdate() {
            if (!target)
                return;

            if (!mouseLocked)
                return;

            //if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            //{
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                //Set Movement Direction
                Vector3 direction = transform.forward;
                direction.y = 0;
                targetMovement.viewDirection = direction.normalized;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            /*
            }
            else
            {
                targetMovement.viewDirection = Vector3.zero;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                if (targetMovement.IsMoving())
                {
                    float targetRotationAngle = target.eulerAngles.y;
                    float currentRotationAngle = transform.eulerAngles.y;
                    x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
                }
            }*/


            y = ClampAngle(y, yMinLimit, yMaxLimit);
            transform.rotation = Quaternion.Euler(y, x, 0);


            zoom -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(zoom);
            zoom = Mathf.Clamp(zoom, minDistance, maxDistance);
            if (zoom != 0)
            {
                expectedDistance = GetRayCastDistance(expectedDistance);
            }
            expectedDistance = Mathf.Clamp(expectedDistance, 0.0f, zoom);

            if (realDistance < expectedDistance)
                realDistance = Mathf.Lerp(realDistance, expectedDistance, smoothCollisionRate * Time.deltaTime);
            else
                realDistance = expectedDistance;

            transform.position = target.position - (transform.rotation * Vector3.forward * (realDistance) - targetOffset);
        }

        static float ClampAngle(float angle, float min, float max) {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

        private float GetRayCastDistance(float distance) {
            RaycastHit hit;
            Vector3 targetPos = target.position + targetOffset;

            Vector3[] corners = new Vector3[4];
            corners[0] = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane)) - transform.position;
            corners[1] = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane)) - transform.position;
            corners[2] = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane)) - transform.position;
            corners[3] = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane)) - transform.position;

            float HitDistance = zoom;

            foreach (Vector3 corner in corners)
            {
                //UnityEngine.Debug.DrawRay(targetPos + corner, transform.position - targetPos);
                if (Physics.Linecast(targetPos + corner, transform.position + corner, out hit, collidesWith))
                {
                    //Find the closer collision
                    if (hit.distance < HitDistance)
                        HitDistance = hit.distance;
                }
            }

            if (HitDistance != zoom)
                return HitDistance;
            else if (collisionCount > 0)
                return distance;
            else
                return zoom;
        }


        int collisionCount = 0;

        void OnTriggerEnter(Collider col) {
            if(IsInLayerMask(col.gameObject, collidesWith))
                collisionCount++;
        }

        void OnTriggerExit(Collider col) {
            if (IsInLayerMask(col.gameObject, collidesWith))
                collisionCount = collisionCount > 0 ? collisionCount - 1 : 0;
        }

        public static bool IsInLayerMask(GameObject obj, LayerMask mask) {
            return ((mask.value & (1 << obj.layer)) > 0);
        }
    }
}
