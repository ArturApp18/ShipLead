using UnityEngine;

namespace CodeBase.Logic
{
    public class SaveTrigger: MonoBehaviour
    {
       // private ISaveLoadService _saveLoadService;
        public BoxCollider Collider;
        
        private void Awake()
        {
            //_saveLoadService = AllServices.Container.Signle<ISaveLoadService>();
        }

        private void OnTriggerEnter(Collider other)
        {
           // _saveLoadService.SaveProgress();
            Debug.Log("Progress Saved");
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if(!Collider) return;
            
            Gizmos.color = new Color(0.17f, 0.75f, 0.02f, 0.42f);
            Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
        }
    }
}