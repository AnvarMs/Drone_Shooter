using System.Collections;
using UnityEngine;
using System.Linq;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class ResultSelecter : MonoBehaviour
{

    public GameObject ResultPanel,MaineMenu,SatrtMenu;
   
    [Header("Option")]

    [Space]

    public GameObject[] sloates;
    public TextMeshProUGUI[] nameText;
    public TextMeshProUGUI[] scorText;

    // Start is called before the first frame update
    void Start()
    {
       Refresh();
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

        StartCoroutine(ReturnMenu());
    }
    IEnumerator ReturnMenu(){

        yield return new WaitForSeconds(10f);
        GoToManu();
    }

    public void GoToManu(){
        MaineMenu.SetActive(true);
        SatrtMenu.SetActive(true);
        ResultPanel.SetActive(false);
        Destroy(RoomManager.instance.player);
    }

    
}
