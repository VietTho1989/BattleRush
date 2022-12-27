using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Preload : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(preload());
    }

    IEnumerator preload()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}