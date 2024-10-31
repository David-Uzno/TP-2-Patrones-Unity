using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFactory : Factory
{
    public GameObject treePrefab;

    public override GameObject CreateObject(Vector3 position)
    {
        return Instantiate(treePrefab, position, Quaternion.identity);
    }
}
