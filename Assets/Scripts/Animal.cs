using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public struct AnimalState : IState
{
    public int   CurrentHealth;
    public float LastAttackTime;
    public bool  IsAttacking;
    public float RemainingCooldown;
}

public enum AnimalTypes
{
    Sheep,
    Wolf,
    Cow,
}

public class AnimalData
{
    public int Health;

    public float FollowDistance = 10f;
    public float AttackDistance;
    public float WindupCooldown;
    public float AttackCooldown;
    public int   Damage;
    
    public string DropPrefabPath;
}
public class Animal : MonoBehaviour, IComponent<AnimalState>, IDamagable, IDamager
{
    private Transform _transform;
    private Transform _head;
    
    [SerializeField] private AnimalTypes _animalType;
    private AnimalData _animalData;
    private AnimalState _animalState;

    private RichAI _aiPath;
    private Seeker _seeker;

    private RaycastHit[] _hits = new RaycastHit[5];
    private PhysicsScene _physicsScene;
    
    public void Awake()
    {
        _transform = GetComponent<Transform>();
        _head = transform.GetChild(0).Find("Head");
        _aiPath = GetComponent<RichAI>();
        _seeker = GetComponent<Seeker>();
    
        _physicsScene = gameObject.scene.GetPhysicsScene();
        
        _animalData = _animals[_animalType];
        _animalState.CurrentHealth = _animalData.Health;
    }

    public void Start()
    {
        _aiPath.endReachedDistance = _animalData.AttackDistance;
        // _aiPath.sl = _animalData.AttackDistance + 0.5f;
    }

    public void FixedUpdate()
    {
        ref var state = ref _animalState;
        // Find Closest Player
        var position1 = _transform.position;
        var closestPlayer = PlayerPool.GetClosestPlayer(position1);
        // Debug.Log($"Closest player is {closestPlayer.player}, {closestPlayer.distance}");
        // Move Towards Player

        if (closestPlayer.distance > _animalData.FollowDistance)
        {
            _aiPath.destination = _transform.position;
            return;
        }

        // // _aiPath.destination = position;
        // _seeker.StartPath(position1, playerPos, path =>
        // {
        //     if (path.error) Debug.Log($"{path.error}");
        // });
        // if (distanceToSteeringTarget + movementPlane.ToPlane(steeringTarget - richPath.Endpoint).magnitude + movementPlane.ToPlane(destination - richPath.Endpoint).magnitude > endReachedDistance) return false;
        // Debug.Log(closestPlayer.distance);
        if (closestPlayer.distance <= _animalData.AttackDistance)
        {
            _aiPath.destination = _transform.position;
            Debug.Log("Dest" + _aiPath.reachedDestination);
            Debug.Log("EndOfPath" + _aiPath.reachedEndOfPath);
            var playerPos = closestPlayer.player.Transform.position;
            if(!state.IsAttacking)
            {
                if (!(state.RemainingCooldown > 0))
                {
                    // Wind up attack
                    _transform.LookAt(new Vector3(playerPos.x, position1.y, playerPos.z), Vector3.up);
                    BeginAttack(ref state);
                }
                else
                {
                    Debug.Log("Waiting for cooldown");
                }
            }
            
            // Attack
            _transform.LookAt(new Vector3(playerPos.x, position1.y, playerPos.z), Vector3.up);
            Attack(ref state);
        }
        else
        {
            if (state.IsAttacking)
            {
                // Attack
                _transform.LookAt(closestPlayer.player.Transform.position, Vector3.up);
                Attack(ref state);
            }
            else
            {
                var playerPos = closestPlayer.player.Transform.position;
                _aiPath.destination = playerPos;
                Debug.Log("Move Towards Player");
            }
        }
        
        _animalState.RemainingCooldown -= Time.fixedDeltaTime;
    }

    private void BeginAttack(ref AnimalState state)
    {
        Debug.Log("Begin Attack windup");
        state.LastAttackTime    = (int) Time.time;
        state.RemainingCooldown = (int) _animalData.WindupCooldown;
        state.IsAttacking       = true;
    }

    private void Attack(ref AnimalState state)
    {
        if(state.RemainingCooldown > 0) return;


        // Do Damage to players within range.
        var hit = _physicsScene.Raycast(_head.position, _head.forward, _hits, 5, (1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Collide);
        Debug.Log($"Attack! {hit}");
        if (hit != 0)
        {
            Debug.Log(_hits[0].transform.root);
            _hits[0].transform.root.GetComponent<IDamagable>().TakeDamage(this, _head.forward, 10);
        }
        EndAttack(ref state);
    }
    
    private void EndAttack(ref AnimalState state)
    {
        Debug.Log("End Attack");
        state.RemainingCooldown = _animalData.AttackCooldown;
        state.IsAttacking       = false;
    }


    public void        TakeDamage(IDamager player, Vector3 hitDirection, int damage)
    {
        ref var animalState = ref _animalState;
        if(animalState.CurrentHealth <= 0) return;
    
        animalState.CurrentHealth -= damage;
        Debug.Log($"Hit {_animalType} for {damage} => {animalState.CurrentHealth} / {_animalData.Health}");
        if (animalState.CurrentHealth <= 0)
            Death(hitDirection);
    }

    public void Death(Vector3 hitDirection)
    {
        gameObject.SetActive(false);
        var dropPrefab = PrefabPool.Prefabs[_animalData.DropPrefabPath];
        var position = transform.position;
        var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
        var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
        
        drop.GetComponent<Rigidbody>().AddForce(hitDirection * 10f, ForceMode.Impulse);
    }

    // private void OnDrawGizmos()
    // {
    //     // ref var state = ref _animalState;
    //     // if (state.IsAttacking)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawRay(_head.position, _head.forward * 5f);
    //     }
    // }

    public AnimalState State => _animalState;

    private static readonly Dictionary<AnimalTypes, AnimalData> _animals = new Dictionary<AnimalTypes, AnimalData>()
    {
        { AnimalTypes.Sheep, new AnimalData()
        {
            Damage         = 10,
            Health         = 100,
            FollowDistance = 20,
            AttackDistance = 2.5f,
            WindupCooldown = 0.3f,
            AttackCooldown = 3f,
            
            DropPrefabPath = "Prefabs/Items/Item_Coin"
        }},
        { AnimalTypes.Cow, new AnimalData()
        {
            Damage         = 10,
            Health         = 100,
            FollowDistance = 20,
            AttackDistance = 2.5f,
            WindupCooldown = 0.3f,
            AttackCooldown = 3f,

            DropPrefabPath = "Prefabs/Items/Item_Meat"
        }},
        { AnimalTypes.Wolf, new AnimalData()
        {
            Damage         = 10,
            Health         = 100,
            FollowDistance = 20,
            AttackDistance = 2.5f,
            WindupCooldown = 0.3f,
            AttackCooldown = 3f,

            DropPrefabPath = "Prefabs/Items/Item_Wolf_Hide"
        }}
    };
}
