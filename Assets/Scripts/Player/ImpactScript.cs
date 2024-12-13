using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    public IEnumerator Destroy() {
        yield return new WaitForSeconds(.2f);
        Destroy(this.gameObject);
    }
}    
