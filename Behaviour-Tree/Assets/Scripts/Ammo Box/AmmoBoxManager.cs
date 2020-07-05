using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxManager : MonoBehaviour
{

    [SerializeField]
    private int timeInterval;

    private List<GameObject> tanks = new List<GameObject>();

    private string friendlyTeam;

    [SerializeField]
    private int ammoPerInterval;

    //the intervals between the ammo is resupplied to the tank
    private IEnumerator Interval()
    {

        while (true)
        {

            foreach(GameObject tank in tanks)
            {

                //add ammo if the tank has the StdTankAI script attached
                if(tank != null)
                tank.GetComponent<TankAI>().AddAmmo(ammoPerInterval);

            }

            yield return new WaitForSeconds(timeInterval);

        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == friendlyTeam)
        {

            tanks.Add(other.gameObject);

        }

    }

    private void OnTriggerExit(Collider other)
    {

        if(other.gameObject.tag == friendlyTeam)
        {

            foreach(GameObject tank in tanks)
            {

                if(tank == other.gameObject)
                {

                    tanks.Remove(tank);
                    break;

                }

            }

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //sets tags so the right team's tanks are healed if in the collider 
        if (gameObject.tag == Utilities.Tags.BlueAmmoBox)
        {

            friendlyTeam = Utilities.Tags.BlueTeam;

        }
        else if (gameObject.tag == Utilities.Tags.RedAmmoBox)
        {

            friendlyTeam = Utilities.Tags.RedTeam;

        }

        StartCoroutine(Interval());

    }

}
