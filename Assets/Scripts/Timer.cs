
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviourPun
{
    public int minit = 4;
    public int secont = 60;
      public TextMeshProUGUI TimeText;





    public void StartTimer()
    {
        minit = 4;
        secont = 60;
        UpdateUi();
        InvokeRepeating("UpdateTimer", 1f, 1f);
    }

    private void UpdateTimer()
    {
        if (secont > 0)
        {
            secont -= 1;
        }
        else if (minit > 0)
        {
            minit -= 1;
            secont = 60;
        }
        else
        {
            photonView.RPC("GameEndShow", RpcTarget.All);
            CancelInvoke("UpdateTimer");
        }
        UpdateUi();
    }
    public void UpdateUi()
    {
        string newText = $"{minit:D2} : {secont:D2}";




        if (photonView != null)
        {
            photonView.RPC("AssignLocalPlayerText", RpcTarget.All, newText);
        }
        else
        {
            Debug.Log("view is null");
        }
        

    }
      [PunRPC]
    void AssignLocalPlayerText(string timeText)
    {
        TimeText.text = timeText;
    }

}