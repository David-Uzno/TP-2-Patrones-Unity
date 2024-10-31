using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [Header("Buttons for Object Creation")]
    public Button bushButton;
    public Button rockButton;
    public Button treeButton;

    [Header("Object Factories")]
    public Factory bushFactory;
    public Factory rockFactory;
    public Factory treeFactory;

    private GameObject currentObject;

    void Start()
    {
        bushButton.onClick.AddListener(CreateBush);
        rockButton.onClick.AddListener(CreateRock);
        treeButton.onClick.AddListener(CreateTree);
    }

    void CreateBush()
    {
        Vector3 position = bushFactory.transform.position;
        CreateSingleObject(bushFactory, position);
    }

    void CreateRock()
    {
        Vector3 position = rockFactory.transform.position;
        CreateSingleObject(rockFactory, position);
    }

    void CreateTree()
    {
        Vector3 position = treeFactory.transform.position; 
        CreateSingleObject(treeFactory, position);
    }

    void CreateSingleObject(Factory factory, Vector3 position)
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }
        
        currentObject = factory.CreateObject(position);
    }
}
