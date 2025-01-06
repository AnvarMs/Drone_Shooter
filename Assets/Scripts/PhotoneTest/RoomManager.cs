using UnityEngine;
using Photon.Pun;
using System.Linq;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public GameObject[] playerPrefab;
    public int PlayerIndex;
    public Transform[] spawnPos;
    
    [Space]
    [Header("Panel Section")]
    public GameObject startupUI;
     public GameObject nikNamePanel;
     public GameObject joinPanel;

      public GameObject LoadingPanel;
      public GameObject startPanel;
    public GameObject killPanel;
    public GameObject timerPnel;
      public GameObject startBoutten;
    public GameObject MatchResultPanel;
    public GameObject DroneShowPos;
    [Space]
    public TextMeshProUGUI killMessageText;
    [Space]

    public string nikname;
    public string roomNameText;

    void Awake(){
        instance =this;
    }

    void Start()
    {
        Debug.Log("Connecting to Photon Server...");
        PhotonNetwork.ConnectUsingSettings();
        LoadingPanel.SetActive(true);
        Debug.Log(playerPrefab.GetType());
        PlayerIndex=0;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server.");
        PhotonNetwork.JoinLobby();
    }



     public override void OnJoinedLobby()
     {
        nikNamePanel.SetActive(true);
        LoadingPanel.SetActive(false);
        Debug.Log("in loby");
     }

    public void settRoomName(string _room){
        Debug.Log(_room+"room name asigned");
        roomNameText=_room;
    }
    public void settNikName(string _name){
        Debug.Log(_name+" nik name asigned");
        nikname =_name;
     
    }
   
    public void JoinRoom(){
        int currentPlayers = PhotonNetwork.PlayerList.Length;
       
        Debug.Log("Joined Photon Lobby. Attempting to join or create room.");
        if(currentPlayers < 5){
    
        PhotonNetwork.JoinOrCreateRoom(roomNameText,null,null);
        }else{
            Debug.Log("Room is Full");
        }
        
    }





    public override void OnJoinedRoom()
    {
         if (PhotonNetwork.IsMasterClient)
        {
            startBoutten.SetActive(true);
            startPanel.SetActive(true);   
        }
        else
        {
             startBoutten.SetActive(false);        
        }
         LoadingPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName =nikname; 
       // SpawnPlayer();
      
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room: " + message);
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Photon. Cause: " + cause.ToString());
    }

public ProceduralGeneration generation;
 public GameObject player;
    [PunRPC]
    public void SpawnPlayer(){
        // if(!ProceduralGeneration.isMapCreated){

        //      // generation.GenerateTheMap();
        // }
        DroneShowPos.SetActive(false);
        LoadingPanel.SetActive(false);
        startPanel.SetActive(false);
        startupUI.SetActive(false);
        timerPnel.SetActive(true);


        Debug.Log("Successfully joined the room. Spawning player...");
        int randomPoint = Random.Range(0,spawnPos.Length);

         player = PhotonNetwork.Instantiate(playerPrefab[PlayerIndex].name,spawnPos[randomPoint].position, Quaternion.identity);

        // Check if the player was successfully instantiated
        if (player != null)
        {
            Debug.Log("Player instantiated successfully: " + player.name);
            player.GetComponent<MovementSetup>().SetPlayer();
            player.GetComponent<Helth>().isLocalPlayer=true;
            // Get the Photon nickname
            player.GetComponent<PhotonView>().RPC("AssignNicknameToPlayer",RpcTarget.AllBuffered,nikname);
           
        }
        else
        {
            Debug.LogError("Failed to instantiate player.");
        }

    }

        
    public  void StartGame()
    {
      photonView.RPC("SpawnPlayer", RpcTarget.All);
      Timer timer = GetComponent<Timer>();
      timer.StartTimer();
    }

[PunRPC]
    public void GameEndShow(){
        timerPnel.SetActive(false);
        startupUI.SetActive(true);
        MatchResultPanel.SetActive(true);
        if(player!=null){
        player.GetComponentInChildren<Canvas>().enabled =false;
        }
        Cursor.lockState = CursorLockMode.None;
        StopAllCoroutines();
                     
    }
    [PunRPC]
    public void ShowKillMessage(string killer,string aceust)
    {
        killPanel.SetActive(true);
        killMessageText.text = killer + " Killed " + aceust;
        StartCoroutine(KillPanelActiveFalse());

    }

    IEnumerator KillPanelActiveFalse()
    {
        yield return new WaitForSeconds(3);

        killPanel.SetActive(false);
    }





}
