using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : MonoBehaviour
{
    public string name = "Unnamed";
    public GameObject Terrain;
    public GameObject PlacedObjects;
    public GameObject wallObjects;

    public Dictionary<GameObject, WallConnection> wallConnections = new Dictionary<GameObject, WallConnection>();

    public bool DoesConnectionExist(GameObject lastPole, GameObject newPole)
    {
        if (lastPole == null || newPole == null)
        {
            return false;
        }
        else if (wallConnections.ContainsKey(lastPole))
        {
            WallConnection connection = wallConnections[lastPole];
            if (connection.incomingPoles.Contains(newPole))
            {
                return true;
            }
            else if (connection.outgoingPoles.Contains(newPole))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    
    //Walls
    public GameObject PlacePole(GameObject lastPole, GameObject newPole, GameObject wallPrefab, Transform hitPoint)
    {
        if (lastPole == null)
        {
            GameObject instantiatedPole = Instantiate(newPole, hitPoint.position, hitPoint.rotation, wallObjects.transform);

            WallConnection connection = new WallConnection
            {
                incomingPoles = new List<GameObject>(),
                outgoingPoles = new List<GameObject>(),
                outgoingWalls = new List<GameObject>(),
                incomingWalls = new List<GameObject>(),
            };

            wallConnections[instantiatedPole] = connection;

            return instantiatedPole;
        }
        else
        {
            if (wallConnections.ContainsKey(newPole))
            {
                ConnectPoles(lastPole, newPole, wallPrefab);
                return newPole;
            }

            GameObject instantiatedPole = Instantiate(newPole, hitPoint.position, hitPoint.rotation, wallObjects.transform);

            WallConnection connection = new WallConnection
            {
                incomingPoles = new List<GameObject>(),
                outgoingPoles = new List<GameObject>(),
                outgoingWalls = new List<GameObject>(),
                incomingWalls = new List<GameObject>(),
            };

            connection.outgoingPoles.Add(lastPole);
            wallConnections[lastPole].incomingPoles.Add(instantiatedPole);
            GameObject newWall = BuildWall(instantiatedPole.transform, lastPole.transform, wallPrefab);
            connection.outgoingWalls.Add(newWall);
            wallConnections[lastPole].incomingWalls.Add(newWall);
            wallConnections[instantiatedPole] = connection;

            return instantiatedPole;
        }
    }

    public List<GameObject> HighlightConnectedWalls(GameObject hoveredPole, Material poleMaterial, Material wallMaterial, Material hoveredMaterial)
    {
        if (wallMaterial == null)
        {
            wallMaterial = poleMaterial;
        }

        List<GameObject> selectedWalls = new List<GameObject>();
        List<GameObject> selectedPoles = new List<GameObject>();

        foreach (GameObject wall in wallConnections[hoveredPole].outgoingWalls)
        {
            selectedWalls.Add(wall);
        }

        foreach (GameObject wall in wallConnections[hoveredPole].incomingWalls)
        {
            selectedWalls.Add(wall);
        }

        foreach (GameObject pole in wallConnections[hoveredPole].outgoingPoles)
        {
            selectedPoles.Add(pole);
        }

        foreach (GameObject pole in wallConnections[hoveredPole].incomingPoles)
        {
            selectedPoles.Add(pole);
        }

        foreach (GameObject wall in selectedWalls)
        {
            MeshRenderer rend = wall.GetComponent<MeshRenderer>();
            Material[] newMats = new Material[2];
            newMats[0] = rend.materials[0];
            newMats[1] = wallMaterial;
            rend.materials = newMats;
        }
        foreach (GameObject pole in selectedPoles)
        {
            MeshRenderer rend = pole.GetComponent<MeshRenderer>();
            Material[] newMats = new Material[2];
            newMats[0] = rend.materials[0];
            newMats[1] = poleMaterial;
            rend.materials = newMats;
        }
        List<GameObject> returnedSelection = new List<GameObject>();
        returnedSelection.AddRange(selectedPoles);
        returnedSelection.AddRange(selectedWalls);

        MeshRenderer rend2 = hoveredPole.GetComponent<MeshRenderer>();
        Material[] newMats2 = new Material[2];
        newMats2[0] = rend2.materials[0];
        newMats2[1] = hoveredMaterial;
        rend2.materials = newMats2;

        returnedSelection.Add(hoveredPole);
        return returnedSelection;
    }

    public void ClearAllMaterials()
    {
        List<GameObject> all = new List<GameObject>();
        foreach (WallConnection connection in wallConnections.Values)
        {
            foreach (GameObject gameObject in connection.incomingPoles)
            {
                all.Add(gameObject);
            }
            foreach (GameObject gameObject in connection.incomingWalls)
            {
                all.Add(gameObject);
            }
            foreach (GameObject gameObject in connection.outgoingPoles)
            {
                all.Add(gameObject);
            }
            foreach (GameObject gameObject in connection.outgoingWalls)
            {
                all.Add(gameObject);
            }
        }
        ClearMaterials(all);
    }

    public void ClearMaterials(List<GameObject> selection)
    {
        foreach (GameObject selected in selection)
        {
            if (selected != null)
            {
                MeshRenderer rend = selected.GetComponent<MeshRenderer>();
                Material[] newMats = new Material[1];
                newMats[0] = rend.materials[0];
                rend.materials = newMats;
            }
        }
    }

    public void ConnectPoles(GameObject newPole, GameObject lastPole, GameObject wallPrefab)
    {
        wallConnections[newPole].outgoingPoles.Add(lastPole);
        wallConnections[lastPole].incomingPoles.Add(newPole);
        GameObject newWall = BuildWall(newPole.transform, lastPole.transform, wallPrefab);
        wallConnections[newPole].outgoingWalls.Add(newWall);
        wallConnections[lastPole].incomingWalls.Add(newWall);
    }

    public GameObject BuildWall(Transform a, Transform b, GameObject wallPrefab)
    {
        GameObject newWall = Instantiate(wallPrefab, wallObjects.transform);

        Vector3 averagePos = (b.position + a.position) / 2;
        newWall.transform.position = averagePos;
        Vector3 wallSize = wallPrefab.transform.localScale;
        float distance = Vector3.Distance(b.position, a.position);
        wallSize.z = distance;

        newWall.transform.rotation = Quaternion.LookRotation(a.position - b.position, b.up);
        newWall.transform.localScale = wallSize;

        return newWall;
    }

    public void DestroySelected(GameObject selected)
    {
        if (selected.layer == 8)
        {
            WallConnection connection = wallConnections[selected];
            foreach (GameObject wall in connection.outgoingWalls)
            {
                Destroy(wall);
            }
            foreach (GameObject wall in connection.incomingWalls)
            {
                Destroy(wall);
            }
            foreach (GameObject pole in connection.incomingPoles)
            {
                for (int i = 0; i < wallConnections[pole].outgoingPoles.Count; i++)
                {
                    if (wallConnections[pole].outgoingPoles[i] == selected)
                    {
                        wallConnections[pole].outgoingPoles.RemoveAt(i);
                        wallConnections[pole].outgoingWalls.RemoveAt(i);
                        break;
                    }
                }
            }
            foreach (GameObject pole in connection.outgoingPoles)
            {
                for (int i = 0; i < wallConnections[pole].incomingPoles.Count; i++)
                {
                    if (wallConnections[pole].incomingPoles[i] == selected)
                    {
                        wallConnections[pole].incomingPoles.RemoveAt(i);
                        wallConnections[pole].incomingWalls.RemoveAt(i);
                        break;
                    }
                }
            }
            Destroy(selected);
        }
        else if (selected.layer == 9)
        {
            foreach (WallConnection connection in wallConnections.Values)
            {
                for (int i = 0; i < connection.incomingWalls.Count; i++)
                {
                    if (connection.incomingWalls[i] == selected)
                    {
                        connection.incomingWalls.RemoveAt(i);
                        connection.incomingPoles.RemoveAt(i);
                        break;
                    }
                }
                for (int i = 0; i < connection.outgoingWalls.Count; i++)
                {
                    if (connection.outgoingWalls[i] == selected)
                    {
                        connection.outgoingWalls.RemoveAt(i);
                        connection.outgoingPoles.RemoveAt(i);
                        break;
                    }
                }
            }
            Destroy(selected);
        }
    }

}


[System.Serializable]
public struct WallConnection
{
    public List<GameObject> incomingPoles;
    public List<GameObject> outgoingPoles;
    public List<GameObject> outgoingWalls;
    public List<GameObject> incomingWalls;
}