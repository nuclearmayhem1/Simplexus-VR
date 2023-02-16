using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class WallTool : XRGrabInteractable
{
    public GameObject polePrefab;
    public GameObject wallPrefab;
    public Transform hitPoint;

    public Mesh poleMesh, wallMesh;

    public GameObject lastPole = null;
    public bool buildingWall = false;

    private void Awake()
    {
        selectEntered.AddListener(OnSelect);
    }

    private void Update()
    {
        
        



    }

    public void OnSelect(SelectEnterEventArgs args)
    {
        
    }
    

}
