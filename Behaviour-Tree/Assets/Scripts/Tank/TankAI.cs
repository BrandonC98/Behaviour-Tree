using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TankAI : Tank
{

    //the nodes for the Behaviour Tree
    private BehaviourTree.Tree stdTank;
    private Sequence seqToRange;
    private Sequence seqEngageInCombat;
    private Sequence seqGetAmmo;
    private Action actFindEnemy;
    private Action actToRange;
    private Sequence seqFire;
    private Action actIsVisible;
    private Action actAim;
    private Action actFire;
    private Action actMoveToAmmo;
    private Repeater repGetAmmo;
    private Succeeder sucGetAmmo;
    private Action actGetAmmo;

    private string AmmoTag;

    private int ammoCount;

    private int prevHealth;


    //the stucture of the Behaviour Tree
    private void BehaviourTree()
    {

        //create behvaiour tree
        stdTank = new BehaviourTree.Tree();

        //creates and add the root node to the Tree
        Root root = stdTank.AddRoot();

        //make root section
        stdTank.MakeSectionFor(root);
        seqEngageInCombat = stdTank.MakeSequence();
        seqGetAmmo = stdTank.MakeSequence();
        stdTank.AddToSection(seqEngageInCombat, seqGetAmmo);
        stdTank.EndSection();

        //engege in combat Sequence 
        stdTank.MakeSectionFor(seqEngageInCombat);
        seqToRange = stdTank.MakeSequence();
        seqFire = stdTank.MakeSequence();
        stdTank.AddToSection(seqToRange, seqFire);
        stdTank.EndSection();

        //move into range for comabat Sequence
        stdTank.MakeSectionFor(seqToRange);
        actFindEnemy = stdTank.MakeAction();
        actToRange = stdTank.MakeAction();
        stdTank.AddToSection(actFindEnemy, actToRange);
        stdTank.EndSection();

        //fire the turret Sequence
        stdTank.MakeSectionFor(seqFire);
        actAim = stdTank.MakeAction();
        actIsVisible = stdTank.MakeAction();
        actFire = stdTank.MakeAction();
        stdTank.AddToSection(actAim, actIsVisible, actFire);
        stdTank.EndSection();

        //get ammo Sequence
        stdTank.MakeSectionFor(seqGetAmmo);
        sucGetAmmo = stdTank.MakeSucceeder();
        actMoveToAmmo = stdTank.MakeAction();
        stdTank.AddToSection(actMoveToAmmo, sucGetAmmo);
        stdTank.EndSection();

        //get Ammo Succeeder
        stdTank.MakeSectionFor(sucGetAmmo);
        repGetAmmo = stdTank.MakeRepeater();
        stdTank.AddToSection(repGetAmmo);
        stdTank.EndSection();

        //get ammmo Repeater
        stdTank.MakeSectionFor(repGetAmmo);
        actGetAmmo = stdTank.MakeAction();
        stdTank.AddToSection(actGetAmmo);
        stdTank.EndSection();

        SetConditions();
        SetActions();
        BT_Debug();

    }

    private void SetConditions()
    {

        //turn off conditions for these nodes
        actFindEnemy.conditionsOff(true);
        actToRange.conditionsOff(true);
        actIsVisible.conditionsOff(true);
        actAim.conditionsOff(true);
        actFire.conditionsOff(true);
        actMoveToAmmo.conditionsOff(true);

        //give these conditions a inital value of true
        sucGetAmmo.ConditionTrue();
        repGetAmmo.ConditionTrue();
        sucGetAmmo.ConditionTrue();

    }

    private void SetActions()
    {

        //Allocate the delegates 
        actFindEnemy.onLoop += FindEnemy;
        actToRange.onLoop += ToRange;
        actToRange.onExit += ResetRange;
        actAim.onLoop += AimTank;
        actAim.onExit += ResetAim;
        actIsVisible.onLoop += IsVisible;
        actFire.onLoop += Fire; 
        actFire.onExit += ResetFireSeq;
        actMoveToAmmo.onLoop += MoveToAmmo;
        actGetAmmo.onEnter += GetAmmoCount; 
        actGetAmmo.onLoop += AmmoCheck;
        
    }

    private void RemoveActions()
    {

        //Deallocate the delegates 
        actFindEnemy.onLoop -= FindEnemy;
        actToRange.onLoop -= ToRange;
        actToRange.onExit -= ResetRange;
        actAim.onLoop -= AimTank;
        actAim.onExit -= ResetAim;
        actIsVisible.onLoop -= IsVisible;
        actFire.onLoop -= Fire; 
        actFire.onExit -= ResetFireSeq;
        actMoveToAmmo.onLoop -= MoveToAmmo;
        actGetAmmo.onEnter -= GetAmmoCount; 
        actGetAmmo.onLoop -= AmmoCheck;

    }

    private void BT_Debug()
    {

        //debug action nodes
        actFindEnemy.SetTag("actFindEnemy");
        actToRange.SetTag("actToRange");
        actAim.SetTag("actAim");
        actIsVisible.SetTag("actIsVisible");
        actFire.SetTag("actFire");
        actMoveToAmmo.SetTag("actMoveToAmmo");
        actGetAmmo.SetTag("actGetAmmo"); 
        
    }

    // Start is called before the first frame update
    void Start()
    {

        StartUp();
        BehaviourTree();

        repGetAmmo.SetAmount(ammo);
        stdTank.Start();
        seqEngageInCombat.ConditionTrue();
 
    }

    // Update is called once per frame
    void Update()
    {

        step = Utilities.MoveOverTime(moveSpeed);
        FireRateCheck();

        if (currentHealth <= 0)
        {

            RemoveActions();
            Death();

        }

        // begin go and get ammo behaviour
        if(currentAmmo <= 0)
        {

            seqGetAmmo.ConditionTrue();
            seqEngageInCombat.ConditionFalse();

        }

        // begin the combat behaviour
        if (currentAmmo >= ammo)
        {

            seqEngageInCombat.ConditionTrue();
            seqGetAmmo.ConditionFalse();

        }

        if(seqEngageInCombat.CheckCondition() == true)
        seqToRange.ConditionTrue();

        //if a tank is hit while resupplying ammo intantly being combat behaviour
        //as long as the tank has atleast 1 missile
        if(seqGetAmmo.StatusCheck() == Node.Status.Running)
        {
            
            if(currentAmmo > 0 && currentHealth != prevHealth)
            {

                seqEngageInCombat.ConditionTrue();
                seqGetAmmo.ConditionFalse();

            }

            prevHealth = currentHealth;

        }

        stdTank.Run();

    }

#region                 Action functions 
    private void ToRange()
    {
    
        //if another enemy is close it will fail so it can 
        //change targets to the closer one 
        if(GetNearestObject(EnemyTag()) != CurrentTarget)
        {

            actToRange.ActionFailed();

        }

        //move to firing range
        MoveToRange();
        
        //if in range the action fails
        if (InRange == true)
        {

            actToRange.ActionSuccessful();
            seqFire.ConditionTrue();

        }
        
        //enemy equals null so action fails
        if(enemyLost == true)
        {

            actToRange.ActionFailed();

        }

    }

    private void AmmoCheck()
    {

        if(ammoCount != currentAmmo) actGetAmmo.ActionSuccessful();

    }

    private void GetAmmoCount()
    {

        ammoCount = currentAmmo;

    }

    private void MoveToAmmo()
    {

        //set ammo tag
        if(this.tag == Utilities.Tags.BlueTeam)
        AmmoTag = Utilities.Tags.BlueAmmoBox;
        else AmmoTag = Utilities.Tags.RedAmmoBox;

        //get nearest ammo supply location
        Vector3 ammoSupply = GetNearestObject(AmmoTag).transform.position;
        
        //move tank to ammo
        Goto(ammoSupply);

        float dist = Vector3.Distance(ammoSupply, this.transform.position);

        //if close enough the action is successful
        if(dist <= distenceOffset)
        {

            actMoveToAmmo.ActionSuccessful();

        }

    }

    private void ResetFireSeq()
    {

        seqFire.ConditionFalse();

    }

    private void ResetRange()
    {
        InRange = false;
        enemyLost = false;
    }

    private void FindEnemy()
    {

        CurrentTarget = GetNearestObject(EnemyTag());

        //if a enemy is within the tanks view the action is successful
        if(Vector3.Distance(gameObject.transform.position, GetTargetPosition()) <= viewDistance)
        {

            actFindEnemy.ActionSuccessful();

        }
        else actFindEnemy.ActionFailed(); 

    }

    private void IsVisible()
    {
        
        if(IsTargetVisble()) actIsVisible.ActionSuccessful();
        else actIsVisible.ActionFailed();

    }

    private void Fire()
    {

        //if the tank has no ammo or the fire timer isn't ready fail the action 
        if(fireTimer < fireRate || currentAmmo <= 0)
        {

            actFire.ActionFailed();

        } 
        else
        { 

            miss = CreateProjectile();

            //reset firetimer so it will tick up again to create a fire rate
            fireTimer = 0;  
            actFire.ActionSuccessful();

        }

    }

    private void AimTank()
    {

        Aim(step);
        if(rotationSuccessful) actAim.ActionSuccessful();
        
    }

    private void ResetAim()
    {

        rotationSuccessful = false;

    }

#endregion

}
