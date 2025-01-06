
using Photon.Pun;
using UnityEngine;

public class Missile : MonoBehaviourPun
{
    [SerializeField]
    float _speed = 40;
    [SerializeField]
    int damage = 30;
    [SerializeField]
    float _rotationSpeed = 20;
    [SerializeField]
    Rigidbody _rb;
    [SerializeField]
    Vector3 _divatePrediction;
    [SerializeField]
    Transform _transformTarget;
    [SerializeField]
    GameObject _blastParticleefect;
    int parentViwId;
    string _nickName;




    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = transform.forward * _speed;
       
        if (_divatePrediction != null)
        {
            RootateRocket();
        }
        else if (_transformTarget != null)
        {
           _divatePrediction = _transformTarget.position;
            RootateRocket();
        }
        
    }

    public void RootateRocket()
    {
        var heading = _divatePrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.deltaTime));
    }

    public void SetTarget(Vector3 target, int parent)
    {

        _divatePrediction = target;
        parentViwId = parent;

    }
    public void SetTarget(Transform target, int parent)
    {

        _transformTarget = target;
        parentViwId = parent;

    }


    public void ShareNikname(string _name){
          _nickName = _name;
    }
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.gameObject.GetComponent<PhotonView>()&&parentViwId!= collision.transform.gameObject.GetComponent<PhotonView>().ViewID)
        {
            if (collision.transform.gameObject.GetComponent<Helth>())
            {
                collision.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
                if (collision.transform.gameObject.GetComponent<Helth>()._helth < 0)
                {
                    PhotonView parentView = PhotonNetwork.GetPhotonView(parentViwId);
                    if (!parentView.transform.GetComponent<Shoot>())
                    {
                        Debug.Log($"Shoo is not found for ID: {parentViwId}. Ensure the object is instantiated and synced.");
                        return;
                    }

                    parentView.RPC("AddScor", RpcTarget.All);
                    PhotonView roomMangerView = GameObject.Find("RoomManager").GetPhotonView();
                    roomMangerView.RPC("ShowKillMessage", RpcTarget.All, _nickName,parentView.Owner.NickName);
                }
            }

            
          
        }

        if(collision!=null){
              Destroy(gameObject);
              PhotonNetwork.Instantiate(_blastParticleefect.name, transform.position, Quaternion.identity);

        }

    }
}
