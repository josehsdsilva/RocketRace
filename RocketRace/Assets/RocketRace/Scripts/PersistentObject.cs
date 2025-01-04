using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static PersistentObject instance;

    private void Awake()
    {
        // If an instance already exists, destroy this one
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Make this the instance
        instance = this;
        
        // Keep the object across scenes
        DontDestroyOnLoad(gameObject);
    }
}