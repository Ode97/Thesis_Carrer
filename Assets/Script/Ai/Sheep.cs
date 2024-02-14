using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Sheep : MonoBehaviour
{
    private FSM fsm;
    private bool detected = false;
    private Animator animator;
    public float health = 1;
    private Character character;
    public Element lowElement;
    private NavMeshAgent agent;

    private bool enigma;
    [SerializeField]
    private EarthPlant target;
    private Vector3 initPos;
    private Quaternion initRot;

    // Start is called before the first frame update
    void Awake()
    {
        EventManager.StartListening("Reset", Reset);
        initPos = transform.localPosition;
        initRot = transform.localRotation;
        animator = GetComponent<Animator>();

        FSMState idle = new FSMState("idle");
        FSMState foodChase = new FSMState("chase");
        FSMState eating = new FSMState("eat");

        idle.enterActions.Add(ResetEat);

        foodChase.enterActions.Add(MoveAnim);

        eating.enterActions.Add(ResetMoveAnim);
        eating.enterActions.Add(Eat);
        //foodChase
        //idle.stayActions.Add(Move);


        
        FSMTransition nearFood = new FSMTransition(NearFood);
        FSMTransition endEat = new FSMTransition(EndEating);
        FSMTransition startChase = new FSMTransition(DetectFood);
        


          
        idle.AddTransition(startChase, foodChase);
        foodChase.AddTransition(nearFood, eating);
        eating.AddTransition(endEat, idle);


        fsm = new FSM(idle);
        fsm.StartFSM();
    }

    private void Reset()
    {
        transform.localPosition = initPos;
        transform.localRotation = initRot;
    }

    private void Start()
    {
        character = FindObjectOfType<Character>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void MoveAnim()
    {
        
        animator.SetTrigger("Walk");
    }

    private void Eat()
    {
        eat = true;
        animator.SetTrigger("Eat");
    }
    private void ResetEat()
    {
        animator.ResetTrigger("Eat");
    }

    private void ResetMoveAnim()
    {
        agent.isStopped = true;
        animator.ResetTrigger("Walk");
    }

    private GameObject plant;
    private bool DetectFood()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 40);

        //Debug.Log("detect food");
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                
                if (collider.GetComponent<EarthPlant>())
                {
                    if (collider.gameObject.GetComponent<EarthPlant>().GetIndex() == target.GetIndex())
                    {
                        plant = collider.gameObject;
                        
                        agent.SetDestination(plant.transform.position);
                        agent.isStopped = false;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool NearFood()
    {
        //Debug.Log("near food");
        if (Vector3.Distance(transform.position, plant.transform.position) < 8)
            return true;

        return false;
    }

    private bool EndEating()
    {
        //Debug.Log("stop eating?");
        if (!eat)
            return true;

        return false;
    }

    private bool eat = false;
    private bool startEat = false;
    private void Update()
    {
        if (eat && !startEat)
        {
            startEat = true;
            StartCoroutine(WaitEat());
        }

        //Debug.Log(fsm.currentState.name);
        fsm.UpdateFSM();
        
    }

    private IEnumerator WaitEat()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("sit_to_stand"));
        eat = false;
        startEat = false;
        Destroy(plant);
        
    }

    public void SetPosAndRot(Vector3 pos, Quaternion rot) { 
    
        transform.position = pos;
        transform.rotation = rot;
    }

}
