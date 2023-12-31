using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] private HiveMovement _hiveMovement;
    public HiveMovement HiveMovement => _hiveMovement;
    [SerializeField] private HiveHurt _hiveHurt;
    public HiveHurt HiveHurt => _hiveHurt;

    [SerializeField] private Animator _animator;
    public Animator AnimatorComponent => _animator;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    public Rigidbody2D Rigidbody => _rigidbody2D;
    [SerializeField] private SpriteRenderer _spriteRendererComponent;
    public SpriteRenderer SpriteRendererComponent => _spriteRendererComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!_hiveMovement) _hiveMovement = GetComponent<HiveMovement>();
        if (!_hiveHurt) _hiveHurt = GetComponent<HiveHurt>();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, _hiveMovement.HiveDetectionRange);
    }
}
