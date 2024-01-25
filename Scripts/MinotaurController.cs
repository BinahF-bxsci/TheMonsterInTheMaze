using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MinotaurController : MonoBehaviour
{
    public Transform Player;
    public NavMeshAgent agent;
    public Transform self;
    public Animator animator;
    public GameObject home;
    public Slider healthbar;
    public Text mission;

    private const string IsWalking = "IsWalking";
    private const string IsRunning = "IsRunning";
    private const string IsDead = "IsDead";
    private const string Speed = "Speed";
    private const string angle = "angle";
    private const string atk = "atk";

    private float prevAngle;
    private float fovAngle = 45f;
    private int health = 100;
    private int healthreshold = 50;
    private bool readyheal = false;
    private bool spain = false;
    private float spainCount = 0;
    private bool couldseeplayer;

    public void Start() //a bunch of stuff to reset scene on restart
    {
        mission.text = "Mission: Kill the Minotaur";
        prevAngle = home.transform.rotation.y;
        agent.enabled = false;
        animator.enabled = false; //need to briefly turn these off so they dont mess with moving the taur
        self.SetPositionAndRotation(home.transform.position, home.transform.rotation); //goes to start spot
        agent.enabled = true;
        animator.enabled = true;
        health = 100;
        healthbar.SetValueWithoutNotify(health); //reset health to full
    }
    void Update()
    {
        if (health > 0) //only send animator a bunch of info if alive
        {
            animator.SetBool(IsWalking, agent.velocity.magnitude > 0.01f && agent.velocity.magnitude < 1.5f);
            animator.SetBool(IsRunning, agent.velocity.magnitude >= 1.5f);
            animator.SetFloat(Speed, agent.velocity.magnitude);
            animator.SetFloat(angle, prevAngle - self.transform.eulerAngles.y);
            prevAngle = self.transform.eulerAngles.y;
        }
        if (health <= 0) //just prevents any other behaviors if dead
        { }
        else if (CanSeePlayer())
        {
            couldseeplayer = true;
            spain = false;
            spainCount = 0;
            if (Vector3.Distance(self.transform.position, Player.transform.position) < 2) //player is close & in front of minotaur - cue attack
            {
                animator.SetBool(atk, true); //start attack animation
                var targetRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);                            //look at player
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);   //but only rotate on one axis otherwise it gets weird
                agent.SetDestination(self.transform.position); //stop moving towards player
            }
            else
            {
                agent.SetDestination(Player.transform.position); //chase
                animator.SetBool(atk, false);
            }
        }
        else if (readyheal == true) //need to tweak so he can still do other stuff while readyheal = true - maybe scrap hp condition here and only set readyheal when threshold crossed on hit?
        {                       //UPDATE: DID THAT oops caps
            spain = false;
            spainCount = 0;
            animator.SetBool(atk, false);
            if (Vector3.Distance(self.transform.position, home.transform.position) < 3)  //at home
            {
                if (health >= healthreshold *2/3)
                {
                    readyheal = false;
                    healthreshold /= 2;
                }
                else
                {
                    health += Random.Range(0, 5);
                    healthbar.SetValueWithoutNotify(health);
                }
            }
            else if (health <= healthreshold) {
                agent.SetDestination(home.transform.position);
            }
        }
        else if (spain || couldseeplayer) //spain is looking around behavior, basically spins around in a circle
        {
            if(couldseeplayer)  //activate if lose sight of player
            {
                couldseeplayer = false;
                spain = true;
            }
            if (spainCount >= 360) //stop if you spin a full circle
            {
                spain = false;
                spainCount = 0;
            }
            else //spin boy
            {
                spainCount++;
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 1, 0);
            }
        }
        else //go to random nearby(ish) point
        {
            animator.SetBool(atk, false); //dont keep attacking if sight lost
                                          
            if (agent.remainingDistance <= agent.stoppingDistance) //done with path
            {
                Vector3 point;
                if (RandomPoint(self.position, 8, out point)) 
                {
                    agent.SetDestination(point);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) //if sword hits body
    {
        if (other.tag == "PlayerWeapon")
        {
            Hit(5);
        }
    }

    public void Hit(int damage)
    {
        health -= damage;
        healthbar.SetValueWithoutNotify(health);
        if (health <= 0) {
            animator.SetBool(IsDead, true);
            agent.enabled = false;
            mission.text = "Mission: Leave the Labyrinth";
        }
        else if (health < healthreshold)
        { readyheal = true; }
        else if (!CanSeePlayer()) //if you're not looking and you get hit u should start looking around
        {
            spain = true;
        }
    }
    public int GetHealth()
    { return health; }

    bool CanSeePlayer()
    {
        Vector3 direction = Player.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, direction, out hit) && hit.collider.gameObject.CompareTag(Player.tag) && angle < fovAngle)
        { return true; }
        return false;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}



/*
 Notes 4 later:
 ambient music: "Fantasy RPG Adventure 25 Tracks!" forbidden temple
whoops didn't do this. oops. later i guess sorry I'm kinda swamped rn

 */