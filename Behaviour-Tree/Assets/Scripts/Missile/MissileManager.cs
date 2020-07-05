using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MissileManager : MonoBehaviour
{

    private Rigidbody rb;

    [Header("Missile")]

    [SerializeField]
    float speed;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float timeToLive;

    [HideInInspector]
    public Vector3 direction;

    [Header("explosion")]

    [SerializeField]
    private ParticleSystem expolsion;

    [SerializeField]
    private float explosionDuration;

    [SerializeField]
    private float explositionRadius;

    // Start is called before the first frame update
    void Start()
    {

        //destory the missile after a set time
        Destroy(gameObject, timeToLive);
        rb = gameObject.GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {

        //move the missile
        rb.velocity = direction * speed;

    }

    private void OnCollisionEnter(Collision collision)
    {

        Explode();

    }

    private void Explode()
    {

        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, explositionRadius);

        foreach(Collider coll in colliders)
        {

            //apply damage to all tanks within the explosition radius
            if (coll.tag == Utilities.Tags.BlueTeam || coll.tag == Utilities.Tags.RedTeam)
            coll.gameObject.GetComponent<Tank>().currentHealth -= damage;

        }

        //create exposion particle
        GameObject exp = Instantiate(expolsion.gameObject, this.gameObject.transform.position, Quaternion.identity);
        
        Destroy(exp, explosionDuration);
        Destroy(gameObject);
        
    }

}
