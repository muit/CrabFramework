using Crab.Components;
using UnityEngine;

namespace Crab {
    
    public class ControlledCamera : MonoBehaviour {
        public Transform target;

        public float targetHeight = 2.0f;
        [Header("Distance")]
        public float distance = 5.0f;
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


        private float x = 0.0f;
        private float y = 0.0f;

        private CMovement targetMovement;

        void Start() {
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            // Make the rigid body not change rotation
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().freezeRotation = true;

            targetMovement = target.GetComponent<CMovement>();
        }

        void LateUpdate() {
            if (!target)
                return;

            // If either mouse buttons are down, let them govern camera position
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                targetMovement.viewRotation = Quaternion.Euler(0,x,0);
                
                // otherwise, ease behind the target if any of the directional keys are pressed
            }
            else if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
            {
                float targetRotationAngle = target.eulerAngles.y;
                float currentRotationAngle = transform.eulerAngles.y;
                x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
            }

            distance -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = target.position - (rotation * Vector3.forward * distance + new Vector3(0, -targetHeight, 0));

            transform.rotation = rotation;
            transform.position = position;
        }

        static float ClampAngle(float angle, float min, float max) {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    }
}
