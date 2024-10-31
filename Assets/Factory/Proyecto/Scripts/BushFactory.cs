using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushFactory : Factory
{
    public GameObject bushPrefab;

    public override GameObject CreateObject(Vector3 position)
    {
        return Instantiate(bushPrefab, position, Quaternion.identity);
    }
}
