using UnityEngine;

namespace SergioHdz.Scripts
{
    public class NPCController : MonoBehaviour
    {
        [SerializeField] private Transform visualModel;
        [SerializeField] private float moveSpeed = 3f;

        private Vector3 targetPosition;
        private bool isMoving;

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
            }
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
