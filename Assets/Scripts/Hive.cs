using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] private HiveMovement _hiveMovement;
    public HiveMovement HiveMovement => _hiveMovement;

    [SerializeField] private Animator _animator;
    public Animator AnimatorComponent => _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!_hiveMovement) _hiveMovement = GetComponent<HiveMovement>();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, _hiveMovement.HiveDetectionRange);
    }
}
