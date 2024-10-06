using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float dieTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(die());
    }

    private IEnumerator die(){
        yield return new WaitForSeconds(dieTime);
        Destroy(this.gameObject);
    }
}
