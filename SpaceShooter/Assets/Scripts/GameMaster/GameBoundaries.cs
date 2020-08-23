using UnityEngine;

public class GameBoundaries : MonoBehaviour
{
    private static GameBoundaries instance = null;

    public static GameBoundaries Instance { get { return instance; } }

    [SerializeField] private Transform lowerLeftPoint = null;
    [SerializeField] private Transform upperRightPoint = null;

    private float upperX;
    private float lowerX;
    private float upperZ;
    private float lowerZ;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        lowerX = lowerLeftPoint.position.x;
        upperX = upperRightPoint.position.x;
        lowerZ = lowerLeftPoint.position.z;
        upperZ = upperRightPoint.position.z;
    }

    public bool InsideBoundaries(Vector3 objectPosition)
    {
        return InsideX(objectPosition) && InsideZ(objectPosition);
    }

    public bool InsideX(Vector3 objectPosition)
    {
        return objectPosition.x < upperX && objectPosition.x > lowerX;
    }

    public bool InsideZ(Vector3 objectPosition)
    {
        return objectPosition.z < upperZ && objectPosition.z > lowerZ;
    }

    public Vector3 GetLocationInBoundary(Vector3 location)
    {
        Vector3 vectorInArea = location;
        if (InsideBoundaries(location))
        {
            return location;
        }
        else
        {
            if(location.x > upperX)
            {
                vectorInArea.x = upperX;
            }
            else if(location.x < lowerX)
            {
                vectorInArea.x = lowerX;
            }

            if(location.z > upperZ)
            {
                vectorInArea.z = upperZ;
            }
            else if(location.z < lowerZ)
            {
                vectorInArea.z = lowerZ;
            }
        }

        return vectorInArea;
    }


}
