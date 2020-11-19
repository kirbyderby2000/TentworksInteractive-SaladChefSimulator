using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] Customer customerPrefab;
    [SerializeField] CustomerGameLogicHandler logicHandler;

    private List<Customer> activeCustomers = new List<Customer>();

    [SerializeField] List<CustomerServingArea> customerServingAreas;

    [Min(3.0f)]
    [SerializeField] float spawnRate = 10.0f;



    Coroutine customerSpawnCoroutine = null;

    public void StartSpawningCustomers()
    {
        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers()
    {
        SpawnNextCustomer();
        while (true)
        {
            float secondsToWait = spawnRate;

            yield return new WaitForSeconds(secondsToWait);

            SpawnNextCustomer();
        }
        
    }

    private void SpawnNextCustomer()
    {
        int randomStartIndex = Random.Range(0, customerServingAreas.Count);

        CustomerServingArea nextCustomerServeArea = null;

        for (int i = 0; i < customerServingAreas.Count; i++)
        {
            if (randomStartIndex >= customerServingAreas.Count)
                randomStartIndex = 0;

            if(customerServingAreas[randomStartIndex].ServingAreaOccupied == false)
            {
                nextCustomerServeArea = customerServingAreas[randomStartIndex];
                break;
            }
            randomStartIndex++;
        }

        if (nextCustomerServeArea != null)
            SpawnCustomer(nextCustomerServeArea);
    }

    private void SpawnCustomer(CustomerServingArea servingArea)
    {
        if (servingArea.ServingAreaOccupied)
            return;

        Transform spawnEntranceRef = servingArea.GetComponent<ServeAreaPositions>().CustomerEntrance;
        Customer spawnedCustomer = Instantiate(customerPrefab, spawnEntranceRef.position, spawnEntranceRef.rotation, transform.parent);

        spawnedCustomer.StartCustomerCoroutine(servingArea, logicHandler);
        spawnedCustomer.OnCustomerLeaving.AddListener(OnCustomerLeaving);
        activeCustomers.Add(spawnedCustomer);
    }

    private void OnCustomerLeaving(Customer customerLeaving)
    {
        customerLeaving.OnCustomerLeaving.RemoveListener(OnCustomerLeaving);
        if (activeCustomers.Contains(customerLeaving))
        {
            activeCustomers.Remove(customerLeaving);
        }
    }

    public void StopSpawningCustomers()
    {
        if(customerSpawnCoroutine != null)
        {
            StopCoroutine(customerSpawnCoroutine);
        }
    }
}
