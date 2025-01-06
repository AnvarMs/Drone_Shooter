using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Unity.VisualScripting;

public class Shoot : MonoBehaviourPun
{
    public Rigidbody rb;
    public Camera _camera;
    string myName;
   [Space]
    public int damage;


    public int amoLimit;
    public float recoilStrength = 10f;
    public Vector3 Recoilamount;
    public bool shootdilay = true;
    private int Bcount;
    private Vector3 currentRecoil; // Tracks the current recoil
    private Vector3 recoilVelocity;

    [Space]
    public TextMeshProUGUI Btext;
    public GameObject Particle;
    public GameObject _missileObj;
    public Transform missilePos;
    private bool isRealoading = false;
    [Space]
    public float aimAngle = 45f; // Half of the cone angle
    public float aimRange = 50f; // Maximum distance of detection
    [Space]
    public GameObject reloadingPnel, CrosaireUi;


    GameObject hitTarget;

    public void Start() {
        Bcount = amoLimit;
        isRealoading = false;
        myName = PhotonNetwork.LocalPlayer.NickName;
    }
    public void Update()
    {

        if (Input.GetButtonDown("Fire1") && photonView.IsMine && shootdilay && !isRealoading) {

            OnFire();
        }

    }
    //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

    void OnFire()
    {
        

        RaycastHit hit;
        bool isHit = Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 100f);
        if (isHit)
        {
            var newMissile = PhotonNetwork.Instantiate(_missileObj.name, missilePos.position, missilePos.rotation);

            // PhotonNetwork.Instantiate(Particle.name, hit.point, Quaternion.identity);
            Missile missile = newMissile.GetComponent<Missile>();

            missile.ShareNikname(myName);


            // Camera used for aiming



            // Calculate dynamic aim radius based on distance to simulate FOV effect

            // Perform the sphere cast with the dynamic radius
            Collider[] hits = Physics.OverlapSphere(transform.position, aimRange);

            foreach (Collider hitc in hits)
            {
                if (hit.transform.GetComponent<Helth>() && !photonView.IsMine)
                {
                    // Calculate direction to the enemy
                    Vector3 directionToEnemy = (hitc.transform.position - transform.position).normalized;

                    // Check if the enemy is within the aim angle
                    float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                    if (angleToEnemy < aimAngle)
                    {

                        hitTarget = hitc.gameObject;
                        // Optional: Rotate towards the enemy
                        missile.SetTarget(hitTarget.transform, photonView.ViewID);

                    }

                }
                else
                {
                    missile.SetTarget(hit.point, photonView.ViewID);
                }
            }



            if (Bcount > 0)
            {
                Bcount -= 1;
                Btext.text = "/ " + Bcount.ToString();
            }
            if (!isRealoading && Bcount == 0)
            {
                StartCoroutine(Reaload());

            }

        }

        Recoil();
        StartCoroutine(ResetDelay());
    }
    private IEnumerator ResetDelay()
    {
        shootdilay = false; // Disable shooting
        float recoveryDuration = 1f; // Duration of recoil recovery
        float elapsedTime = 0.1f; // Timer for the recovery process

        // Get the current rotation of the camera
        Quaternion initialRotation = _camera.transform.localRotation;
        Quaternion targetRotation = Quaternion.identity; // The default "zero" rotation

        // Gradually reset the camera rotation to zero over the recovery duration
        while (elapsedTime < recoveryDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / recoveryDuration; // Normalized time for interpolation
            _camera.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t); // Smoothly interpolate rotation
            yield return null; // Wait for the next frame
        }

        // Ensure the rotation is exactly zero at the end
        _camera.transform.localRotation = targetRotation;

        // Re-enable shooting after the delay
        shootdilay = true;
    }

    IEnumerator Reaload()
    {
        CrosaireUi.SetActive(false);
        reloadingPnel.SetActive(true);
        Btext.color = Color.red;
        yield return new WaitForSeconds(3);
        Bcount = amoLimit;
        Btext.text = "/ " + Bcount.ToString();
        reloadingPnel.SetActive(false);
        CrosaireUi.SetActive(true);
        isRealoading = false;
        Btext.color = Color.white;
    }

    // SmoothDamp velocity tracker
    private void Recoil()
    {
        // Recoil settings (adjust as needed)
        float recoilSmoothTime = 0.5f; // SmoothDamp time for recoil recovery

        // Calculate new rotation for recoil
        Vector3 targetRecoil = new Vector3(
             Recoilamount.x, // Random X recoil
            Random.Range(-Recoilamount.y, Recoilamount.y), // Random Y recoil
            Random.Range(-Recoilamount.z, Recoilamount.z)  // Random Z recoil (optional for roll)
        );

        // Smoothly apply recoil over time
        currentRecoil.x = Mathf.SmoothDamp(currentRecoil.x, targetRecoil.x, ref recoilVelocity.x, recoilSmoothTime);
        currentRecoil.y = Mathf.SmoothDamp(currentRecoil.y, targetRecoil.y, ref recoilVelocity.y, recoilSmoothTime);
        currentRecoil.z = Mathf.SmoothDamp(currentRecoil.z, targetRecoil.z, ref recoilVelocity.z, recoilSmoothTime);

        // Apply the calculated recoil to the camera rotation
        _camera.transform.localRotation = Quaternion.Euler(currentRecoil);
    }
    void OnDrawGizmos() {

        if (hitTarget != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitTarget.transform.position, 10f);
        }


        Gizmos.color = Color.red;

    }

    [PunRPC]
    public void AddScor()
    {
        PhotonNetwork.LocalPlayer.AddScore(1);
    }

}


