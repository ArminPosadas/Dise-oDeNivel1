using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SergioHdz.Scripts
{
    public class QueueManager : MonoBehaviour
    {
        [SerializeField] private Transform[] queuePositions;
        [SerializeField] private Transform frontPosition;
        [SerializeField] private Transform boothPosition;
        [SerializeField] private Transform acceptPosition;
        [SerializeField] private Transform rejectPosition;
        [SerializeField] private GameObject npcPrefab;

        private Queue<NPCController> npcQueue = new Queue<NPCController>();
        private NPCController currentNpc;

        private void Start()
        {
            SpawnNpcs(5);
            AdvanceQueue();
        }

        private void SpawnNpcs(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject npcGO = Instantiate(npcPrefab, queuePositions[i].position, Quaternion.identity);
                NPCController npc = npcGO.GetComponent<NPCController>();
                npcQueue.Enqueue(npc);
            }
        }

        public void AdvanceQueue()
        {
            if (currentNpc != null)
            {
                Destroy(currentNpc.gameObject);
                currentNpc = null;
            }

            if (npcQueue.Count == 0)
            {
                Debug.Log("No hay más NPCs en la fila.");
                return;
            }

            currentNpc = npcQueue.Dequeue();
            currentNpc.MoveTo(frontPosition.position);

            int index = 0;
            foreach (var npc in npcQueue)
            {
                npc.MoveTo(queuePositions[index].position);
                index++;
            }
        }

        public void OnPlayClicked()
        {
            if (currentNpc != null)
            {
                currentNpc.MoveTo(boothPosition.position);
            }
        }

        public void AcceptNpc()
        {
            if (currentNpc != null)
            {
                currentNpc.MoveTo(acceptPosition.position);
                StartCoroutine(DestroyAfterDelay(currentNpc));
            }
        }

        public void RejectNpc()
        {
            if (currentNpc != null)
            {
                currentNpc.MoveTo(rejectPosition.position);
                StartCoroutine(DestroyAfterDelay(currentNpc));
            }
        }

        private IEnumerator DestroyAfterDelay(NPCController npc)
        {
            while (Vector3.Distance(npc.transform.position, acceptPosition.position) > 0.1f &&
                   Vector3.Distance(npc.transform.position, rejectPosition.position) > 0.1f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            if (npc == currentNpc)
            {
                currentNpc = null;
            }

            Destroy(npc.gameObject);

            AdvanceQueue();
        }
    }
}



