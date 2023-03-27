using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaceTool : XRGrabInteractable
{
    private static readonly string[] buildingLayers = { "Terrain", };
    private static readonly string[] destroyLayers = { "Terrain", "Pole", "Wall" };
    private static readonly string[] adjustLayers = { "Terrain" };


    public WallToolMode mode = WallToolMode.Build;
    private bool held = false;

    public Transform hitPoint;
    public GameObject previewObject;
    public Transform rayOrigin;

    public LineRenderer lineRenderer;

    private Quaternion rotationAwayFromSurface = Quaternion.identity;
    private Level currentLevel;

    public GameObject selectedPrefab;

    private bool isPlacing = false;
    private Quaternion placedAxis = Quaternion.identity;

    private void Update()
    {
        if (held)
        {
            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hitInfo, 10, LayerMask.GetMask(buildingLayers)))
            {
                if (hitInfo.collider.gameObject.layer == 6)
                {
                    currentLevel = hitInfo.transform.GetComponentInParent<Level>();

                    hitPoint.position = hitInfo.collider.ClosestPoint(hitInfo.point);
                    rotationAwayFromSurface = Quaternion.LookRotation(hitPoint.position - hitInfo.transform.position) * Quaternion.Euler(90, 0, 0);
                    hitPoint.rotation = rotationAwayFromSurface;
                    hitPoint.Translate(new Vector3(0, selectedPrefab.transform.localScale.y, 0), Space.Self);

                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, rayOrigin.position);
                    lineRenderer.SetPosition(1, hitPoint.position);

                    BuildPreview();

                    if (isPlacing)
                    {
                        Vector3 targetPos = new Vector3(hitPoint.position.x, previewObject.transform.position.y, hitPoint.position.z);

                        Quaternion newRotation = Quaternion.LookRotation(previewObject.transform.position - hitPoint.position, previewObject.transform.up);

                        previewObject.transform.rotation = newRotation;
                    }
                }
            }
            else
            {
                lineRenderer.enabled = false;
                DisableBuildPreview();
            }
        }
    }


    public void BuildPreview()
    {
        if (previewObject == null)
        {
            previewObject = Instantiate(selectedPrefab, hitPoint);
        }

    }

    public void DisableBuildPreview()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
            isPlacing = false;
        }
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

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);

        if (!isPlacing)
        {
            isPlacing = true;
            previewObject.transform.parent = currentLevel.PlacedObjects.transform;
            placedAxis = rotationAwayFromSurface;
        }
        else
        {
            isPlacing = false;
            previewObject = null;
        }
        BuildPreview();
    }

    public void SelectNewPrefab()
    {

    }

}
