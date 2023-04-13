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

    private void teleport(GameObject player)
    {
        player.transform.position = target3.transform.position;
        player.transform.rotation = target3.transform.rotation;
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