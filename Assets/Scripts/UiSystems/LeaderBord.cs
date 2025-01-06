using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class LeaderBord : MonoBehaviour
{

    public GameObject leaderbordPanel;
    [Header("Option")]

    public float refreshRate=1;

    [Space]

    public GameObject[] sloates;
    public TextMeshProUGUI[] nameText;
    public TextMeshProUGUI[] scorText;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Refresh",1f,refreshRate);
    }

    private void Refresh(){

        foreach(var sloat in sloates){
            sloat.SetActive(false);
        }

        var pleyerSortedList = (from Player in PhotonNetwork.PlayerList orderby Player.GetScore() descending select Player).ToList();
        int i=0;
        foreach(var Player in pleyerSortedList){
            sloates[i].SetActive(true);

            if(Player.NickName ==""){
                Player.NickName="Unnamed";
            }
            nameText[i].text = Player.NickName;
            scorText[i].text = Player.GetScore().ToString();
            i++; 
        }
    }

    private void Update(){

        leaderbordPanel.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
