using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StewLogic : MonoBehaviour
{

    [SerializeField] private List<GameObject> stewComponents;

    [SerializeField] private GameObject toSpawn;
    [SerializeField] private Transform spawnLocation;

    public bool forceSpawn;

    private int numItemsToGather;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numItemsToGather = stewComponents.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (numItemsToGather == 0 || forceSpawn) {
            Instantiate(toSpawn, spawnLocation);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(stewComponents.Contains(other.gameObject)) {
            numItemsToGather -= 1;
        }
    }

    void OnTriggerExit(Collider other) {
        numItemsToGather += 1;
    }

}
