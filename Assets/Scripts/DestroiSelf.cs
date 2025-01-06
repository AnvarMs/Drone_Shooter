using System.Collections;
using Photon.Pun;
using UnityEngine;

public class DestroiSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(DestroyMayseif());
    }
IEnumerator DestroyMayseif(){

        yield return new WaitForSeconds(5);
   Destroy(gameObject);
}



}
