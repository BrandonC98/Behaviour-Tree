using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{

    /*====================================================================
                                Succeeder.cs
    ======================================================================

    The Succeeder Node runs its child node and no matter if it fails or
    succeeds this node will return successful to it's parent.

    note if this node's condition isn't met it will return fail.

    =====================================================================*/

    public class Succeeder : Decorator
    {

        /*================================================
                            Constructor
        ================================================*/
        /// <summary>
        /// Create a Succeeder Node.
        /// </summary>
        /// <param name="parent">this node's parent</param>
        /// <returns></returns>
        public Succeeder(Node parent)   :   base(parent)
        {

            this.parent = parent;

        }

        /*===============================================
                            Methods
        ===============================================*/
        /// <summary>
        /// Run the child node if the condition is true and then run the child node
        /// </summary>
        public override void Activity()
        {

            base.Activity();

            if(CheckCondition() == true)
            {
#region                         Pre-activty Checks
                /*==========================================
                                pre-activity checks
                ==========================================*/
                if(child.StatusCheck() == Status.None)
                {

                    //if the child node is already set to None it
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
                    child.Activity();

                }
#endregion
#region                     Post-activity Checks
                /*==========================================
                            post-activity checks
                ==========================================*/
                if(child.StatusCheck() == Status.Successful)
                {

                    //if the child is successful this node is successful

                    ActionExit(child);

                    status = Status.Successful;
                    ResetChild();

                }
                else if(child.StatusCheck() == Status.Failed)
                {

                    //if the child is Failed this node is still successful

                    ActionExit(child);

                    status = Status.Successful;
                    ResetChild();

                }
#endregion
            }
            else
            {

                status = Status.Failed;
                ResetChild();

            }

        }

    }
    
}
