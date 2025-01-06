
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class WaitPlayers : MonoBehaviour
{

public GameObject[] slotes;
public TextMeshProUGUI[] playerNames;
    private void Start(){

        InvokeRepeating("Refresh",1f,1f);

    }

    void Refresh(){
        var PlayerList = PhotonNetwork.PlayerList.ToList();

        int i=0;
        foreach(var player in PlayerList){
            slotes[i].SetActive(true);
            playerNames[i].text = player.NickName;
            i++;
        }
    }


}
