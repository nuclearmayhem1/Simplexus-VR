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

    public GameObject lastPole = null;
    public bool buildingWall = false;

    private bool held = false;
    public LineRenderer lineRenderer;
    public Transform rayOrigin;

    public GameObject polePreview;
    public GameObject wallPreveiw;

    Level currentLevel = null;

    private bool canPlace = false;

    public Material previewMaterial;
    public Color wrong, correct;
    public float maxLenght = 0.5f;


    private Quaternion rotationAwayFromSurface = Quaternion.identity;



    private void Update()
    {
        if (held)
        {
            lineRenderer.enabled = true;

            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hitInfo, 10, LayerMask.GetMask("Terrain")))
            {
                currentLevel = hitInfo.transform.GetComponentInParent<Level>();

                hitPoint.position = hitInfo.collider.ClosestPoint(hitInfo.point);
                rotationAwayFromSurface = Quaternion.LookRotation(hitPoint.position - hitInfo.transform.position) * Quaternion.Euler(90, 0, 0);
                hitPoint.rotation = rotationAwayFromSurface;
                hitPoint.Translate(new Vector3(0, polePrefab.transform.localScale.y, 0), Space.Self);

                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, rayOrigin.position);
                lineRenderer.SetPosition(1, hitPoint.position);

                BuildPreview();
            }
            else
            {
                lineRenderer.enabled = false;
                polePreview.SetActive(false);
                wallPreveiw.SetActive(false);
                currentLevel = null;
            }
        }
        else
        {
            lineRenderer.enabled = false;
            polePreview.SetActive(false);
            wallPreveiw.SetActive(false);
            currentLevel = null;
        }
    }

    public void BuildPreview()
    {
        if (!buildingWall)
        {
            previewMaterial.color = correct;
            polePreview.SetActive(true);
        }
        else if (lastPole != null)
        {
            Vector3 averagePos = (lastPole.transform.position + hitPoint.position) / 2;
            wallPreveiw.transform.position = averagePos;
            Vector3 wallSize = wallPrefab.transform.localScale;
            float distance = Vector3.Distance(lastPole.transform.position, hitPoint.position);
            wallSize.z = distance;
            wallPreveiw.transform.rotation = Quaternion.LookRotation(hitPoint.position - lastPole.transform.position);
            wallPreveiw.transform.localScale = wallSize;

            if (distance > maxLenght)
            {
                canPlace = false;
                previewMaterial.color = wrong;
            }
            else
            {
                canPlace = true;
                previewMaterial.color = correct;
            }

            polePreview.SetActive(true);
            wallPreveiw.SetActive(true);
        }
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);

        if (!buildingWall)
        {
            lastPole = Instantiate(polePrefab, hitPoint.position, hitPoint.rotation, currentLevel.PlacedObjects.transform);
            buildingWall = true;
        }

    }
    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        held = true;
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        held = false;
    }
    public void ResetTool()
    {
        buildingWall = false;
        Debug.Log("Reset");
    }



}
