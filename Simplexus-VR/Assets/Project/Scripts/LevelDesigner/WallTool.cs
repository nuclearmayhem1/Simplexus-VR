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

    private bool held = false;
    public LineRenderer lineRenderer;
    public Transform rayOrigin;

    public GameObject polePreview;
    public GameObject wallPreveiw;

    Level currentLevel = null;

    private bool canPlace = false;

    public Material previewMaterial;
    public Color wrong, correct;
    public float maxLenght = 1f, minLenght = 0.25f;


    private Quaternion rotationAwayFromSurface = Quaternion.identity;

    public Dictionary<GameObject, WallConnection> wallConnections = null;

    private GameObject newPole = null;

    private GameObject hoveredPole = null;

    private void Update()
    {
        if (held)
        {
            lineRenderer.enabled = true;

            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hitInfo, 10, LayerMask.GetMask("Terrain")))
            {
                currentLevel = hitInfo.transform.GetComponentInParent<Level>();
                wallConnections = currentLevel.wallConnections;

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
                wallConnections = null;
                canPlace = false;
            }
        }
        else
        {
            lineRenderer.enabled = false;
            polePreview.SetActive(false);
            wallPreveiw.SetActive(false);
            currentLevel = null;
            wallConnections = null;
            canPlace = false;
        }
    }

    public void BuildPreview()
    {
        if (lastPole == null)
        {
            previewMaterial.color = correct;
            polePreview.SetActive(true);
            newPole = polePrefab;
            canPlace = true;
        }
        else if (hoveredPole == null)
        {
            newPole = polePrefab;
            Vector3 averagePos = (lastPole.transform.position + hitPoint.position) / 2;
            wallPreveiw.transform.position = averagePos;
            Vector3 wallSize = wallPrefab.transform.localScale;
            float distance = Vector3.Distance(lastPole.transform.position, hitPoint.position);
            wallSize.z = distance;

            wallPreveiw.transform.rotation = Quaternion.LookRotation(hitPoint.position - lastPole.transform.position, lastPole.transform.up);
            wallPreveiw.transform.localScale = wallSize;

            if (distance > maxLenght || distance < minLenght)
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
        else
        {
            newPole = hoveredPole;
            Vector3 averagePos = (lastPole.transform.position + hoveredPole.transform.position) / 2;
            wallPreveiw.transform.position = averagePos;
            Vector3 wallSize = wallPrefab.transform.localScale;
            float distance = Vector3.Distance(lastPole.transform.position, hoveredPole.transform.position);
            wallSize.z = distance;

            wallPreveiw.transform.rotation = Quaternion.LookRotation(hoveredPole.transform.position - lastPole.transform.position, lastPole.transform.up);
            wallPreveiw.transform.localScale = wallSize;

            if (distance > maxLenght || distance < minLenght)
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

        if (canPlace)
        {
            lastPole = currentLevel.PlacePole(lastPole, newPole, wallPrefab, hitPoint);
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
        currentLevel = null;
        wallConnections = null;
        lastPole = null;
        Debug.Log("Reset");
    }



}
