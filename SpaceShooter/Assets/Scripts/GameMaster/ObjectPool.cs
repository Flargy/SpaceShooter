using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance = null;

    public static ObjectPool Instance { get { return instance; } }

    [SerializeField] private List<GameObject> LazerPool;
    private List<GameObject> MissilePool;
    private List<GameObject> HomingMissilePool;
    
    public GameObject LazerPoolObject;
    public GameObject MissilePoolObject;
    public GameObject HomingMissilePoolObject;
    public int LazerPoolAmmount;
    public int MissilePoolAmmount;
    public int HomingMissilePoolAmmount;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        LazerPool = new List<GameObject>();
        MissilePool = new List<GameObject>();
        HomingMissilePool = new List<GameObject>();

        for(int i = 0; i < LazerPoolAmmount; i++)
        {
            GameObject obj = Instantiate(LazerPoolObject);
            obj.SetActive(false);
            LazerPool.Add(obj);
        }
        for (int i = 0; i < MissilePoolAmmount; i++)
        {
            GameObject obj = Instantiate(MissilePoolObject);
            obj.SetActive(false);
            MissilePool.Add(obj);
        }
        for (int i = 0; i < HomingMissilePoolAmmount; i++)
        {
            GameObject obj = Instantiate(HomingMissilePoolObject);
            obj.SetActive(false);
            HomingMissilePool.Add(obj);
        }

    }

    public GameObject GetPooledLazer()
    {
        GameObject obj;
        if(LazerPool.Count > 0)
        {
            obj = LazerPool[0];
            RemoveFromList(ref LazerPool ,obj);
        }
       else
        {

            obj = Instantiate(LazerPoolObject);
        }
        

        return obj;
    }

    public GameObject GetPooledMisslie()
    {
        GameObject obj;
        if (MissilePool.Count > 0)
        {
            obj = MissilePool[0];
            RemoveFromList(ref MissilePool, obj);
            return obj;
        }
        else
        {
            obj = Instantiate(MissilePoolObject);
        }

        return obj;
    }

    public GameObject GetPooledHomingMissile()
    {

        GameObject obj;
        if (HomingMissilePool.Count > 0)
        {
            obj = HomingMissilePool[0];
            RemoveFromList(ref HomingMissilePool, obj);
            return obj;
        }
        else
        {
            obj = Instantiate(HomingMissilePoolObject);
        }

        return obj;
    }

    private void RemoveFromList(ref List<GameObject> list ,GameObject obj)
    {
        list.Remove(obj);
    }

    public void AddToList(GameObject obj)
    {
        obj.SetActive(false);

        switch (obj.GetComponent<ProjectileBase>().ProjectileType)
        {
            case ProjectileBase.Type.Lazer:
                LazerPool.Add(obj);
                break;
            case ProjectileBase.Type.Missile:
                MissilePool.Add(obj);
                break;
            case ProjectileBase.Type.HomingMisslie:
                HomingMissilePool.Add(obj);
                break;

        }
    }
}
