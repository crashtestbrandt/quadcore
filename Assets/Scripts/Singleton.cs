using UnityEngine;
using System.Collections;

public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance>
{
    public static Instance instance;
    public bool isPersistent;

    public virtual void Awake()
    {
        if (isPersistent)
        {
            transform.parent = null;
            if (!instance)
            {
                instance = this as Instance;
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance = this as Instance;
        }
    }
}