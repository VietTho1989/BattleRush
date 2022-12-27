using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CommonLibrary
{
    public class AutoRotate : MonoBehaviour
    {

        public Vector3 startAngle;

        private float currentAngleX;
        public float speedX;

        private float currentAngleY;
        public float speedY;

        private float currentAngleZ;
        public float speedZ;

        private void Awake()
        {
            reset();
        }

        public void reset()
        {
            currentAngleX = startAngle.x;
            currentAngleY = startAngle.y;
            currentAngleZ = startAngle.z;
        }

        public bool isPause = false;

        private void Update()
        {
            if (!isPause)
            {
                this.transform.localRotation = Quaternion.Euler(currentAngleX, currentAngleY, currentAngleZ);
                currentAngleX += speedX * Time.deltaTime;
                currentAngleY += speedY * Time.deltaTime;
                currentAngleZ += speedZ * Time.deltaTime;
            }
        }

    }
}