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
    public Material selectedMaterial;
    private Quaternion rotationAwayFromSurface = Quaternion.identity;
    public Dictionary<GameObject, WallConnection> wallConnections = null;
    private GameObject newPole = null;
    private GameObject hoveredPole = null;
    private GameObject hoveredWall = null;
    private static readonly string[] buildingLayers = { "Terrain", "Pole" };
    private static readonly string[] destroyLayers = { "Terrain", "Pole", "Wall" };
    private static readonly string[] adjustLayers = { "Terrain" };
    public Material destroyPole, destroyWall, destroyHovered;
    public Material adjustPole, adjustWall, adjustHovered;

    private List<GameObject> currentSelection = new List<GameObject>();

    private GameObject adjustedPole = null;

    public Material incorrectMat, correctMat;

    public WallToolMode mode = WallToolMode.Build;
    public void SetMode(WallToolMode newMode)
    {
        mode = newMode;
    }


    private void Update()
    {
        if (held && mode == WallToolMode.Build)
        {
            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hitInfo, 10, LayerMask.GetMask(buildingLayers)))
            {
                if (hitInfo.collider.gameObject.layer == 6)
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
                    if (hoveredPole != null)
                    {
                        Material[] newMaterials = new Material[1];

                        newMaterials[0] = hoveredPole.GetComponent<MeshRenderer>().materials[0];
                        hoveredPole.GetComponent<MeshRenderer>().materials = newMaterials;
                        hoveredPole = null;
                    }
                    BuildPreview();
                }
                else if (hitInfo.collider.gameObject.layer == 8)
                {
                    if (hoveredPole != null && hitInfo.collider.gameObject != hoveredPole)
                    {
                        Material[] newMaterials2 = new Material[1];

                        newMaterials2[0] = hoveredPole.GetComponent<MeshRenderer>().materials[0];
                        hoveredPole.GetComponent<MeshRenderer>().materials = newMaterials2;
                        hoveredPole = null;
                    }
                    currentLevel = hitInfo.transform.GetComponentInParent<Level>();
                    wallConnections = currentLevel.wallConnections;
                    lineRenderer.enabled = true;

                    hoveredPole = hitInfo.collider.gameObject;

                    lineRenderer.SetPosition(0, rayOrigin.position);
                    lineRenderer.SetPosition(1, hoveredPole.transform.position);

                    Material[] newMaterials = new Material[2];

                    newMaterials[0] = hoveredPole.GetComponent<MeshRenderer>().materials[0];
                    newMaterials[1] = selectedMaterial;
                    hoveredPole.GetComponent<MeshRenderer>().materials = newMaterials;

                    BuildPreview();
                }

            }
            else
            {
                if (hoveredPole != null)
                {
                    Material[] newMaterials = new Material[1];

                    newMaterials[0] = hoveredPole.GetComponent<MeshRenderer>().materials[0];
                    hoveredPole.GetComponent<MeshRenderer>().materials = newMaterials;
                    hoveredPole = null;
                }
                if (hoveredWall != null)
                {
                    Material[] newMaterials = new Material[1];

                    newMaterials[0] = hoveredPole.GetComponent<MeshRenderer>().materials[0];
                    hoveredWall.GetComponent<MeshRenderer>().materials = newMaterials;
                    hoveredWall = null;
                }
                lineRenderer.enabled = false;
                polePreview.SetActive(false);
                wallPreveiw.SetActive(false);
                currentLevel = null;
                wallConnections = null;
                canPlace = false;
            }
        }
        else if (mode == WallToolMode.Destroy)
        {
            polePreview.SetActive(false);
            wallPreveiw.SetActive(false);

            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hitInfo, 10, LayerMask.GetMask(destroyLayers)))
            {
                if (hoveredPole != null && hitInfo.collider.gameObject != hoveredPole)
                {
                    if (currentSelection.Count != 0)
                    {
                        hitInfo.transform.GetComponentInParent<Level>().ClearMaterials(currentSelection);
                        currentSelection = new List<GameObject>();
                        hoveredPole = null;
                    }
                }
                if (hoveredWall != null && hitInfo.collider.gameObject != hoveredWall)
                {
                    if (currentSelection.Count != 0)
                    {
                        hitInfo.transform.GetComponentInParent<Level>().ClearMaterials(currentSelection);
                        currentSelection = new List<GameObject>();
                        hoveredWall = null;
                    }
                }

                if (hitInfo.collider.gameObject.layer == 6)
                {
                    currentLevel = hitInfo.transform.GetComponentInParent<Level>();
                    wallConnections = currentLevel.wallConnections;
                    hitPoint.position = hitInfo.collider.ClosestPoint(hitInfo.point);
                    rotationAwayFromSurface = Quaternion.LookRotation(hitPoint.position - hitInfo.transform.position) * Quaternion.Euler(90, 0, 0);
                    hitPoint.rotation = rotationAwayFromSurface;
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, rayOrigin.position);
                    lineRenderer.SetPosition(1, hitPoint.position);
                }
                else if (hitInfo.collider.gameObject.layer == 8)
                {
                    currentLevel = hitInfo.transform.GetComponentInParent<Level>();
                    wallConnections = currentLevel.wallConnections;
                    lineRenderer.enabled = true;
                    hoveredPole = hitInfo.collider.gameObject;
                    lineRenderer.SetPosition(0, rayOrigin.position);
                    lineRenderer.SetPosition(1, hoveredPole.transform.position);
                    currentSelection = currentLevel.HighlightConnectedWalls(hoveredPole, destroyPole, destroyWall, destroyHovered);
                    hoveredWall = null;
                }
                else if (hitInfo.collider.gameObject.layer == 9)
                {
                    currentLevel = hitInfo.transform.GetComponentInParent<Level>();
                    wallConnections = currentLevel.wallConnections;
                    lineRenderer.enabled = true;
                    hoveredWall = hitInfo.collider.gameObject;
                    lineRenderer.SetPosition(0, rayOrigin.position);
                    lineRenderer.SetPosition(1, hoveredWall.transform.position);
                    hoveredPole = null;
                    MeshRenderer rend = hoveredWall.GetComponent<MeshRenderer>();
                    Material[] newMats = new Material[2];
                    newMats[0] = rend.materials[0];
                    newMats[1] = destroyWall;
                    rend.materials = newMats;
                    currentSelection.Add(hoveredWall);
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
                hoveredPole = null;
                hoveredWall = null;
                if (currentSelection.Count != 0)
                {
                    hitInfo.transform.GetComponentInParent<Level>().ClearMaterials(currentSelection);
                    currentSelection = new List<GameObject>();
                }
            }
        }
        else if (mode == WallToolMode.Adjust)
        {
            polePreview.SetActive(false);
            wallPreveiw.SetActive(false);

            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hitInfo, 10, LayerMask.GetMask(adjustedPole == null? buildingLayers : adjustLayers)))
            {
                if (adjustedPole == null)
                {
                    if (hoveredPole != null && hitInfo.collider.gameObject != hoveredPole)
                    {
                        if (currentSelection.Count != 0)
                        {
                            hitInfo.transform.GetComponentInParent<Level>().ClearMaterials(currentSelection);
                            currentSelection = new List<GameObject>();
                            hoveredPole = null;
                        }
                    }

                    if (hitInfo.collider.gameObject.layer == 6)
                    {
                        currentLevel = hitInfo.transform.GetComponentInParent<Level>();
                        wallConnections = currentLevel.wallConnections;
                        hitPoint.position = hitInfo.collider.ClosestPoint(hitInfo.point);
                        rotationAwayFromSurface = Quaternion.LookRotation(hitPoint.position - hitInfo.transform.position) * Quaternion.Euler(90, 0, 0);
                        hitPoint.rotation = rotationAwayFromSurface;
                        lineRenderer.enabled = true;
                        lineRenderer.SetPosition(0, rayOrigin.position);
                        lineRenderer.SetPosition(1, hitPoint.position);
                    }
                    else if (hitInfo.collider.gameObject.layer == 8)
                    {
                        currentLevel = hitInfo.transform.GetComponentInParent<Level>();
                        wallConnections = currentLevel.wallConnections;
                        lineRenderer.enabled = true;
                        hoveredPole = hitInfo.collider.gameObject;
                        lineRenderer.SetPosition(0, rayOrigin.position);
                        lineRenderer.SetPosition(1, hoveredPole.transform.position);
                        currentSelection = currentLevel.HighlightConnectedWalls(hoveredPole, adjustPole, adjustWall, adjustHovered);
                        hoveredWall = null;
                    }
                    else if (hitInfo.collider.gameObject.layer == 9)
                    {
                        currentLevel = hitInfo.transform.GetComponentInParent<Level>();
                        wallConnections = currentLevel.wallConnections;
                        lineRenderer.enabled = true;
                        hoveredPole = null;
                        lineRenderer.SetPosition(0, rayOrigin.position);
                        lineRenderer.SetPosition(1, hitPoint.position);
                    }
                }
                else
                {
                    if (hitInfo.collider.gameObject.layer == 6)
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
                        AdjustPreview();
                    }
                    else
                    {
                        lineRenderer.enabled = true;
                        lineRenderer.SetPosition(0, rayOrigin.position);
                        lineRenderer.SetPosition(1, hitPoint.position);
                    }
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
                hoveredPole = null;
                hoveredWall = null;
            }
        }
    }
    private bool canAdjust = false;
    public void AdjustPreview()
    {
        WallConnection connection = currentLevel.wallConnections[adjustedPole];

        canAdjust = true;

        adjustedPole.transform.position = hitPoint.position;
        adjustedPole.transform.rotation = hitPoint.rotation;

        for (int i = 0; i < connection.incomingPoles.Count; i++)
        {
            if (!PreviewWall(connection.incomingWalls[i], connection.incomingPoles[i]))
            {
                canAdjust = false;
            }
        }
        for (int i = 0; i < connection.outgoingPoles.Count; i++)
        {
            if (!PreviewWall(connection.outgoingWalls[i], connection.outgoingPoles[i]))
            {
                canAdjust = false;
            }
        }

    }

    public bool PreviewWall(GameObject wall, GameObject pole)
    {
        Vector3 averagePos = (adjustedPole.transform.position + pole.transform.position) / 2;
        wall.transform.position = averagePos;
        Vector3 wallSize = wallPrefab.transform.localScale;
        float distance = Vector3.Distance(adjustedPole.transform.position, pole.transform.position);
        wallSize.z = distance;

        wall.transform.rotation = Quaternion.LookRotation(pole.transform.position - adjustedPole.transform.position, adjustedPole.transform.up);
        wall.transform.localScale = wallSize;

        if (distance > maxLenght || distance < minLenght)
        {
            MeshRenderer rend = wall.GetComponent<MeshRenderer>();
            Material[] newMats = new Material[2];
            newMats[0] = rend.materials[0];
            newMats[1] = incorrectMat;
            rend.sharedMaterials = newMats;
            return false;
        }
        else
        {
            MeshRenderer rend = wall.GetComponent<MeshRenderer>();
            Material[] newMats = new Material[2];
            newMats[0] = rend.materials[0];
            newMats[1] = correctMat;
            rend.sharedMaterials = newMats;
            return true;
        }
    }

    public void BuildPreview()
    {
        if (currentLevel.DoesConnectionExist(lastPole, hoveredPole))
        {
            polePreview.SetActive(false);
            wallPreveiw.SetActive(false);
            canPlace = false;
        }
        else if (lastPole == hoveredPole && lastPole != null)
        {
            polePreview.SetActive(false);
            wallPreveiw.SetActive(false);
            canPlace = false;
        }
        else if (lastPole == null && hoveredPole == null)
        {
            previewMaterial.color = correct;
            polePreview.SetActive(true);
            wallPreveiw.SetActive(false);
            newPole = polePrefab;
            canPlace = true;
        }
        else if (lastPole == null && hoveredPole != null)
        {
            polePreview.SetActive(false);
            wallPreveiw.SetActive(false);
            canPlace = false;
        }
        else if (lastPole != null && hoveredPole == null)
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
        else if (lastPole != null && hoveredPole != null)
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

            polePreview.SetActive(false);
            wallPreveiw.SetActive(true);
        }
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        if (mode == WallToolMode.Build)
        {
            if (canPlace)
            {
                lastPole = currentLevel.PlacePole(lastPole, newPole, wallPrefab, hitPoint);
            }
            else
            {
                if (hoveredPole != null)
                {
                    lastPole = hoveredPole;
                    Material[] newMaterials = new Material[1];

                    newMaterials[0] = hoveredPole.GetComponent<MeshRenderer>().materials[0];
                    hoveredPole.GetComponent<MeshRenderer>().materials = newMaterials;
                    hoveredPole = null;
                }
                else
                {
                    lastPole = null;
                }
            }
        }
        else if (mode == WallToolMode.Destroy)
        {
            if (hoveredPole != null)
            {
                currentLevel.ClearAllMaterials();
                currentLevel.DestroySelected(hoveredPole);
                currentSelection = new List<GameObject>();
                hoveredPole = null;
            }
            else if (hoveredWall != null)
            {
                currentLevel.ClearAllMaterials();
                currentLevel.DestroySelected(hoveredWall);
                currentSelection = new List<GameObject>();
                hoveredWall = null;
            }
        }
        else if (mode == WallToolMode.Adjust)
        {
            if (adjustedPole == null)
            {
                if (hoveredPole != null)
                {
                    adjustedPole = hoveredPole;
                }
            }
            else
            {
                if (canAdjust)
                {
                    adjustedPole = null;
                    hoveredPole = null;
                }
                else
                {
                    currentLevel.ClearAllMaterials();
                    currentLevel.DestroySelected(hoveredPole);
                }
            }
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

public enum WallToolMode
{
    Build, Adjust, Destroy
}