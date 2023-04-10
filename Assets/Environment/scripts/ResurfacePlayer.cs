using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class ResurfacePlayer : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;

    [FormerlySerializedAs("Speed1")] public float speed1 = 10;
    [FormerlySerializedAs("Speed2")] public float speed2 = 20;
    [FormerlySerializedAs("Speed3")] public float speed3 = 20;

    public int shootingSceneNum = 3;
    private float _speed;
    private int _triggerNum;
    private bool _isSurfacing;
    // private GameObject _player;
    private Rigidbody _rb;
    private GrappleController grappleController;
    private Vector3 _targetPosition;
    [SerializeField] private GameObject _player;

    //private void FixedUpdate()
    //{
    //    if (!_isSurfacing) return;
    //    if (_player.transform.position == target1.transform.position)
    //    {
    //        // Debug.Log("approaching first target!");
    //        _targetPosition = target2.transform.position;
    //        _speed = speed2;
    //    }
    //    else if (_player.transform.position == target2.transform.position)
    //    {
    //        _targetPosition = target3.transform.position;
    //        _speed = speed3;
    //    }
    //    else if (_player.transform.position == target3.transform.position)
    //    {
    //        SpawnControl.LoadFreeFall();
    //        SceneManager.LoadSceneAsync(shootingSceneNum, LoadSceneMode.Additive);
    //        _isSurfacing = false;
    //        _rb.useGravity = true;
    //        _speed = speed1;
    //        _triggerNum = 0;
    //    }
    //    else
    //    {
    //        _player.transform.position =
    //            Vector3.MoveTowards(_player.transform.position, _targetPosition, _speed * Time.deltaTime);
    //    }
    //}
    private void teleport(GameObject player)
    {
        player.transform.position = target3.transform.position;
    }

    private void loadShootingScene()
    {
        SpawnControl.LoadFreeFall();  // move enemys to air
        SceneManager.LoadSceneAsync(shootingSceneNum, LoadSceneMode.Additive);

    }

    private void OnTriggerEnter(Collider collision)
    { 
        if (_isSurfacing) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        GameObject player = collision.gameObject.transform.parent.parent.gameObject;
        grappleController = player.GetComponent<GrappleController>();
        if (! _player.Equals(player)) return;
        if (PlayerData.HasTreasure())
        {
            Debug.Log("player running aways");
            _isSurfacing = true;
            // _speed = speed1;
            // _targetPosition = target1.transform.position;
            // _rb = _player.GetComponent<Rigidbody>();
            // _rb.useGravity = false;
            grappleController.SetGrappleActive(Grappleable.Hand.Right, false);
            grappleController.SetGrappleActive(Grappleable.Hand.Left, false);

            loadShootingScene();
            teleport(player);
            _isSurfacing = false;
        }
    }
}