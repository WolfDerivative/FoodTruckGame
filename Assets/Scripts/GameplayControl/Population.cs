using UnityEngine;

[System.Serializable]
public class Population {

    public GameObject CustomerPrefab;
    [Range(0f, 1f)] public float SpawnChance = 0.5f;


    public GameObject GetPrefab() { return CustomerPrefab; }

}//class Population
