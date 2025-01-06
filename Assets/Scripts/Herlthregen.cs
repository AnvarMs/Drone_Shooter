
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class Herlthregen : MonoBehaviourPun
{

public  ParticleSystem HelthParticls; 
    void OnTriggerEnter(Collider collider){
       collider.transform.gameObject.GetComponent<PhotonView>().RPC("TakeHelth", RpcTarget.All);
        gameObject.GetComponent<Collider>().enabled =false;
        gameObject.GetComponent<MeshRenderer>().enabled =false;
        HelthParticls.Play();
        StartCoroutine(ResponeHelth());
    }


    IEnumerator ResponeHelth(){

        yield return new WaitForSeconds(5f);

        gameObject.GetComponent<Collider>().enabled =true;
        gameObject.GetComponent<MeshRenderer>().enabled =true;

    }
}
