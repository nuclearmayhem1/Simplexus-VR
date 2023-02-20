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
            };

            connection.outgoingPoles.Add(lastPole);
            wallConnections[lastPole].incomingPoles.Add(instantiatedPole);
            connection.outgoingWalls.Add(BuildWall(instantiatedPole.transform, lastPole.transform, wallPrefab));

            wallConnections[instantiatedPole] = connection;

            return instantiatedPole;
        }
    }

    public void ConnectPoles(GameObject newPole, GameObject lastPole, GameObject wallPrefab)
    {
        wallConnections[newPole].outgoingPoles.Add(lastPole);
        wallConnections[lastPole].incomingPoles.Add(newPole);
        wallConnections[newPole].outgoingWalls.Add(BuildWall(newPole.transform, lastPole.transform, wallPrefab));
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

}


[System.Serializable]
public struct WallConnection
{
    public List<GameObject> incomingPoles;
    public List<GameObject> outgoingPoles;
    public List<GameObject> outgoingWalls;
}