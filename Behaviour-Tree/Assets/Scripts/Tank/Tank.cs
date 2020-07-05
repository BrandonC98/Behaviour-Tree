using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Tank : MonoBehaviour
{

    [Header("Movement system")]
    protected NavMeshAgent agent;

    protected float step;

    protected float moveSpeed;

    [Header("Weapon system")]

    [SerializeField]
    protected GameObject turret;

    [SerializeField]
    protected Transform barrelEnd;

    [SerializeField]
    protected GameObject missile;

    protected int ammo;

    protected int currentAmmo;

    protected float fireRate;

    protected float fireTimer;

    protected float firingDistance;

    protected bool InRange;

    [SerializeField]
    private LayerMask AmmoLayer;

    [SerializeField]
    private GameObject expolsion;

    [SerializeField]
    private RandomAudio FireAudio;

    [Header("General system")]

    [SerializeField]
    protected float viewDistance;

    [SerializeField]
    protected int health;

    public int currentHealth;

    protected GameObject CurrentTarget;

    protected bool rotationSuccessful;

    protected bool enemyLost;

    protected float distenceOffset;

    protected GameObject miss;

    private AudioSource source;

    protected void StartUp()
    {

        //initalise variables 

        agent = GetComponent<NavMeshAgent>();
        
        ammo = Random.Range(3, 20);
        moveSpeed = Random.Range(2, 8);
        fireRate = Random.Range(0.2f, 1.0f);
        firingDistance = Random.Range(5, 50);
        
        fireTimer = 0;
        distenceOffset = 5.0f;

        InRange = false;
        rotationSuccessful = false;
        enemyLost = false;
        
        agent.speed = moveSpeed;

        currentAmmo = ammo;
        currentHealth = health;

        source = gameObject.GetComponent<AudioSource>();
        
    }

    public void FireRateCheck()
    {

        //calcualtes the time between firing 
        if (fireTimer < fireRate) fireTimer += Time.deltaTime;

    }

    protected GameObject CreateProjectile()
    {

        currentAmmo--;

        //get the direction to fire the missile
        Vector3 targetDir = CurrentTarget.transform.position - turret.transform.position;

        GameObject projectile = Instantiate(missile, barrelEnd.position, barrelEnd.rotation);
        
        //play audio
        source.PlayOneShot(FireAudio.RandomAudioClip());

        //set the direction the missle should move in
        projectile.gameObject.GetComponent<MissileManager>().direction = targetDir;

        return projectile;

    }

    protected void Goto(Vector3 destination)
    {
        agent.isStopped = false;

        agent.destination = destination;

        float dist = Vector3.Distance(destination, gameObject.transform.position);

        //stop movement when within the set distance 
        if (dist <= distenceOffset)
        {

            agent.isStopped = true;

        }

    }

    public void AddAmmo(int ammo)
    {

        currentAmmo += ammo;

        if(currentAmmo >= this.ammo)
        currentAmmo = this.ammo;

    }

    protected void MoveToRange()
    {
        //if the target is still exists move to firing range
        if (CurrentTarget != null)
        {
             agent.isStopped = false;
            agent.destination = CurrentTarget.transform.position;
           
            float dist = Vector3.Distance(CurrentTarget.transform.position, gameObject.transform.position);
           
            if (dist <= firingDistance)
            {
                agent.isStopped = true;
                agent.ResetPath();
                InRange = true;
                
            }
            
        }
        else
        {

            enemyLost = true;

        }

    }

    protected Vector3 GetTargetPosition()
    {

        if(CurrentTarget != null)
        {

           Vector3 position = CurrentTarget.transform.position;
           
            return position;

        }
        else return new Vector3(0,-1,0);

    }

    protected bool IsTargetVisble()
    {

        if (CurrentTarget == null) CurrentTarget = GetNearestObject(EnemyTag());

        Vector3 targetDir = CurrentTarget.transform.position - turret.transform.position;

        RaycastHit hit;
        
        //shoot ray to see if the enemy is visible
        if (Physics.Raycast(turret.transform.position, targetDir, out hit, viewDistance, ~AmmoLayer))
        {

            if (hit.transform.gameObject.tag == EnemyTag())
            {

                return true;

            }
            else if (hit.transform.gameObject.tag == Utilities.Tags.Obsticle)
            {

                return false;

            }

        }
        else return false;

       return false;

    }
    

    protected void Aim(float step)
    {
        //if within firing distance aim at the target
        if (Vector3.Distance(CurrentTarget.transform.position, gameObject.transform.position) <= firingDistance)
        {

            Vector3 targetDir = CurrentTarget.transform.position - turret.transform.position;

            Vector3 newDir = Vector3.RotateTowards(turret.transform.forward, targetDir, step, 0);

            turret.transform.rotation = Quaternion.LookRotation(newDir);

            //check if the rotation is looking in the correct direction
            if(turret.transform.rotation == Quaternion.LookRotation(newDir))
            rotationSuccessful = true;
            else
            rotationSuccessful = false;

        }

    }


    protected GameObject GetNearestObject(string tag)
    {

        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        GameObject closestObject = null;
        float oldDist = 0;

        //this loops through all GameObjects of the tag and compares the distance until the closest is found 
        foreach (GameObject _object in objects)
        {

            float dist = Vector3.Distance(_object.transform.position, this.gameObject.transform.position);

            if (dist <= oldDist || oldDist == 0)
            {

                oldDist = dist;
                closestObject = _object;

            }

        }

        if (closestObject == null)
        {

            return null;

        }

        return closestObject;

    }

    protected void Death()
    {

        //expolsion particle
        GameObject exp = Instantiate(expolsion, this.gameObject.transform.position, Quaternion.identity);

        Destroy(exp, 5.5f);
        Destroy(gameObject);

    }

    public string EnemyTag()
    {

        //gets the enemy tag
        if (gameObject.tag == Utilities.Tags.BlueTeam) return Utilities.Tags.RedTeam;
        else return Utilities.Tags.BlueTeam;

    }

}
