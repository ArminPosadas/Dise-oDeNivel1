using UnityEngine;
using System;

namespace SergioHdz.Scripts
{
    public class NpcController : MonoBehaviour
    {
        [SerializeField] private Transform visualModel;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private Sprite portrait;

        private Vector3 targetPosition;
        private bool isMoving;

        public Action<NpcController> OnReachedDestination;
        public Vector3 Destination => targetPosition; 

        public void MoveTo(Vector3 destination)
        {
            targetPosition = destination;
            isMoving = true;
        }

        private void Update()
        {
            if (!isMoving) return;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;

                OnReachedDestination?.Invoke(this);
            }
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public Sprite GetPortraitSprite()
        {
            return portrait;
        }
    }
}

