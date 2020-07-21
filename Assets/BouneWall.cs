using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouneWall : MonoBehaviour
{
    public GameObject BounceEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collider) {
        Instantiate(BounceEffectPrefab, collider.gameObject.transform.position, Quaternion.identity);
    }
}
