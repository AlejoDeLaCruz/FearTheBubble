using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Trigger_Crujir_Madera_1 : MonoBehaviour
{
    // Start is called before the first frame update
   

    // Update is called once per frame
    void OnTriggerEnter(Collider collision)
    {

        {
            GetComponent<AudioSource>().Play();
        }

    }

}
