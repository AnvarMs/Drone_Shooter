
using UnityEngine;

public class DroneSelector : MonoBehaviour
{
    [SerializeField]
    GameObject[] Drones;
    public RoomManager roomManager;
    public Transform spownPos;
    public static int Index = 0;

    private void Start(){
        foreach(GameObject i in Drones){
    
            i.transform.position = spownPos.transform.position;
            i.SetActive(false);
        }
         SpownDrone();

    }

    public void NextDrone()
    {
       
        if (Index < Drones.Length-1)
        {
             SpownDrone();
            Index++;
        }
        else
        {
            Debug.Log("No next Drone");
            return;
        }
        SpownDrone();
    }

    public void PrevisasSrone()
    {
      
        if (Index > 0)
        {
         SpownDrone();
            Index--;
        }
        else
        {

            Debug.Log("No next Drone");
            return;
        }
        SpownDrone();
    }

    private void SpownDrone()
    {
        RoomManager.instance.PlayerIndex= Index;
       Drones[Index].SetActive(!Drones[Index].activeSelf);

            
    }
}
