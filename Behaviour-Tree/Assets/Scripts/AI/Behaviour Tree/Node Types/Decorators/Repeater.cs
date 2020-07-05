using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{

    /*====================================================================
                                Repeater.cs
    ======================================================================

    the Repeater node will run it's child node a set amount of times. 
    this node will return successful as long as it has repeated the set
    amount of times, the childs status has no effect on this node succeeding.

    =====================================================================*/

    public class Repeater : Decorator
    {

        private int repeatAmount;
        private int successCount; 

        /*================================================
                            Constructor
        ================================================*/
        /// <summary>
        /// Create a Repeater Node.
        /// </summary>
        /// <param name="parent">this node's parent</param>
        /// <param name="repeatAmount">amount of times to repeat child</param>
        /// <returns></returns>
        public Repeater(Node parent, int repeatAmount) :   base(parent)
        {
            this.repeatAmount = repeatAmount;
            successCount = 0;

        }

        /// <summary>
        /// Create a Repeater node without defining the repeat amount
        /// </summary>
        /// <param name="parent">this node's parent</param>
        /// <returns></returns>
        public Repeater(Node parent) :   base(parent)
        {

            successCount = 0;

        }

        /*===============================================
                            Methods
        ===============================================*/

        /// <summary>
        /// assign the repeat amount
        /// </summary>
        /// <param name="repeatAmount"></param>
        public void SetAmount(int repeatAmount)
        {
            this.repeatAmount = repeatAmount;
        }

        /// <summary>
        /// run the child node the set amount of times and assess the status of this node
        /// </summary>
        public override void Activity()
        {

            base.Activity();

            if(CheckCondition() == true && repeatAmount > 0)
            {

#region                             Pre-activty Checks
                    /*==========================================
                                    pre-activity checks
                    ==========================================*/
                    if (child.StatusCheck() == Status.None)
                    {

                        //if the child node is already set to none it
                        //should run the child activity as this will be the 
                        //first time it runs.

                        //it then should run the post activity checks to 
                        //see the outcome of the child's activity function 

                        ActionEnter(child);

                        child.Activity();   

                    }
                    else if (child.StatusCheck() == Status.Running)
                    {

                        //if the child node is already running it should 
                        //run again to attempt to complete the child activity

                        //it then should break out of the loop as it shouldn't
                        //continue until this child's activity is completed and 
                        //set to successful or failed.


                        child.Activity();

                    }
#endregion
#region                             Post-activity Checks
                    /*==========================================
                                    post-activity checks
                    ==========================================*/

                    if (child.StatusCheck() == Status.Successful)
                    {

                        //as the child node was successful increase the success count 
                        //then reset the child's status so it can be run again

                        successCount++;
                        child.ResetStatus();

#region                         Successful Completion of Sequence
                        /*=============================================
                                Successful Completion of Sequence 
                        =============================================*/
                        if(successCount >= repeatAmount)
                        {

                            ActionExit(child);

                            status = Status.Successful;
                            successCount = 0;
                            child.ResetStatus();
                            Debug.Log("Repeater is complete");

                        }
#endregion
                    }

                        else if (child.StatusCheck() == Status.Failed)
                        {
                            
                            successCount++;
                            child.ResetStatus();

#region                                 Failed Completion of Sequence
                            /*=============================================
                                        Failed Completion of Sequence 
                            =============================================*/
                            if(successCount >= repeatAmount)
                            {

                                ActionExit(child);

                                status = Status.Failed;
                                successCount = 0;
                                child.ResetStatus();

                            }

                        }
#endregion
#endregion
            }
            else
            {

                status = Status.Failed;
                child.ResetStatus();
                successCount = 0;

            }

        }

    }
}

