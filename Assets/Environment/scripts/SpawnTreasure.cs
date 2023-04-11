using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = System.Random;

public class SpawnTreasure : MonoBehaviour
{
    public GameObject Treasure;
    public GameObject[] TreasureTargets;
    private Random _rand = new();

    public void spawnTreasure()
    {
        int target_num = _rand.Next(TreasureTargets.Length);
        Treasure.transform.position = TreasureTargets[target_num].transform.position;
    }

    private void Start()
    {
        spawnTreasure();
    }

}
