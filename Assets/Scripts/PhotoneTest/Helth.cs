

using Photon.Pun;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class Helth : MonoBehaviourPun
{
    public int _helth;
    public bool isLocalPlayer;
    [Header("UI")]
    public TextMeshProUGUI text;
    public Slider helthSlider;

    public Image fillAria;

    private void Start()
    {

        helthSlider.value = _helth;
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        _helth -= damage;
        helthSlider.value = _helth;
        text.text = _helth.ToString();

        if (_helth < 0)
        {

            Destroy(gameObject);
            if (isLocalPlayer)
            {
                RoomManager.instance.SpawnPlayer();
            }
        }
        else if (_helth <= 50)
        {

            fillAria.color = Color.blue;
        }
        else if (_helth >= 50)
        {
            fillAria.color = Color.green;
        }
    }
    [PunRPC]
    public void TakeHelth()
    {
        int heal = 100 - _helth;
        _helth += heal;

        helthSlider.value = _helth;
        text.text = _helth.ToString();
        fillAria.color = Color.green;

    }
   

   


}
