using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("COLLIDE");
        if(other.tag == "Player")
        {
            foreach(Transform child in gameObject.transform)
            {
                if (child.GetComponent<Enemy>() != null) 
                {
                    child.GetComponent<Enemy>().state = "Active";
                } else {
                    child.GetComponent<TurretScript>().state = "Active";
                }
	            
            }
        }
    }

}
