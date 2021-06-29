using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class ZombieCtrl : MonoBehaviour
{
    public enum State
    {
        Idle,
        Walk,
        Run,
        Attack,
        Die
    }

    public State state = State.Idle;
    public float fullHp = 100.0f;
    public float Hp = 0.0f;
    private NavMeshAgent nav;
    private Animator _animator;
    private int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashAttack = Animator.StringToHash("Attack");
    private readonly int hashDie = Animator.StringToHash("Die");
    public bool isDie;
    private float stateTime = 0.3f;
    private WaitForSeconds ws;
    public float attackDistance = 1.0f;
    private Transform target = null;
    private float walkDistance = 5.0f;
    private int gold = 0;
    public bool isAttack = false;
    public float damage = 10.0f;
    public Heal heal;
    
    public float speed
    {
        get { return nav.velocity.magnitude; }
    }

    public float walkingSpeed = 4.0f;
    public float runningSpeed = 7.0f;
    
    void Start()
    {
        Hp = fullHp;
        nav = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        StartCoroutine(StateChanger());
        ws = new WaitForSeconds(stateTime);
        gold = UnityEngine.Random.Range(50,100);
    }

    void Update()
    {
        if (!isDie)
        {
            if (Hp <= 0)
            {
                state = State.Die;
            }
            SetRotation();
            _animator.SetFloat(hashSpeed, speed);
            if (target&&Vector3.Distance(transform.position, target.position) <= attackDistance)
            {
                state = State.Attack;
            }
        }
    }

    IEnumerator StateChanger()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.Idle :
                    nav.speed = 0.0f;
                    RandomState();
                    yield return ws;
                    break;
                case State.Walk:
                    nav.speed = walkingSpeed;
                    nav.SetDestination(SetRandomPoint(transform,walkDistance));
                    RandomState();
                    yield return ws;
                    break;
                case State.Run:
                    nav.speed = runningSpeed;
                    nav.SetDestination(target.position);
                    yield return ws;
                    break;
                case State.Attack:
                    nav.speed = 0.0f;
                    nav.velocity = Vector3.zero;
                    nav.isStopped = true;
                    if (!isAttack&&Vector3.Distance(transform.position, target.position) <= attackDistance)
                    {
                        isAttack = true;
                        _animator.SetBool(hashAttack,true);
                    }
                    else if(Vector3.Distance(transform.position, target.position) > attackDistance)
                    {
                        isAttack = false;
                        nav.isStopped = false;
                        _animator.SetBool(hashAttack,false);
                        state = State.Run;
                    }
                    yield return ws;
                    break;
                case State.Die:
                    nav.speed = 0.0f;
                    _animator.SetTrigger(hashDie);
                    ZombieSpawner.Instance.zombies.Remove(this);
                    GameManager.Instance.gold += gold;
                    RandomHealPack();
                    Destroy(gameObject,5.0f);
                    isDie = true;
                    yield return ws;
                    break;
            }
        }
    }

    private void SetRotation()
    {
        if (state == State.Attack)
        {
            Vector3 vec = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(vec);
        }
        else
        {
            transform.rotation = nav.desiredVelocity.normalized != Vector3.zero?Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(nav.desiredVelocity.normalized), 3.0f):transform.rotation;
        }
    }
    
    private bool RandomPoint(Vector3 center, float range, out Vector3 result, bool normalized = false)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = (normalized ? center + UnityEngine.Random.insideUnitSphere.normalized * range : center + UnityEngine.Random.insideUnitSphere * range);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private Vector3 SetRandomPoint(Transform point = null, float radius = 0, bool normalized = false)
    {
        Vector3 _point;

        if (RandomPoint(point.position, radius, out _point, normalized))
        {
            return _point;
        }
        return Vector3.zero;
    }
    private void RandomState()
    {
        if (state == State.Idle || state == State.Walk)
        {
            var dice = UnityEngine.Random.Range(0, 5);
            if (dice > 3)
            {
                state = State.Idle;
            }
            else
            {
                state = State.Walk;
            }
        }
    }

    public void Hit(float damage)
    {
        Hp -= damage;
        if (state != State.Attack || state != State.Run)
        {
            target = GameManager.Instance.player.transform;
            state = State.Run;
        }
    }

    public void RandomHealPack()
    {
        var dice = UnityEngine.Random.Range(0, 10);
        if (dice > 7)
        {
            var healPack = Instantiate(heal, transform.position + new Vector3(0,0.5f,0), transform.rotation);
            Destroy(healPack.gameObject,15.0f);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && (state != State.Attack || state != State.Run))
        {
            target = other.transform;
            state = State.Run;
        }
    }
}
