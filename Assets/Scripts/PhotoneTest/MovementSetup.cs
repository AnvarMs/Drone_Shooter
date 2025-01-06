using UnityEngine;
using Photon.Pun;
using TMPro;

public class MovementSetup : MonoBehaviour
{
    public DroneController droneController;
    public GameObject mainCameraPrefab;
    
        
   public TextMeshPro Niknametext;
  
  
    public void SetPlayer()
    {
        droneController.enabled = true;

        // Only instantiate the camera for the local player
        if (PhotonNetwork.LocalPlayer.IsLocal)
        {
            mainCameraPrefab.SetActive(true);
             // Attach camera to player
        }
    }
  
  [PunRPC]
public void AssignNicknameToPlayer(string _name){
    
       
       Niknametext.text=_name;
       
}


}
