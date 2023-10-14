using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    private CollectionManager collectionManager;

    // Start is called before the first frame update
    void Start()
    {
        collectionManager = this.transform.parent.gameObject.GetComponent<CollectionManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            collectionManager.UpdateCollectibleList(this.gameObject);
        }
    }
}
