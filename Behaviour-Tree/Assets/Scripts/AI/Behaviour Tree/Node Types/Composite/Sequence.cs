using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{

    /*====================================================================
                                Sequence.cs
    ======================================================================

    The Sequence node will run its child node if any of them fail the 
    sequence fails and it returns to the parent node excution with a fail
    status. 

    If all node in the Sequence are successful then it returns successful 
    to its parent node. 

    =====================================================================*/

    public class Sequence : Composite
    {
        /*================================================
                            Constructor
        ================================================*/
        /// <summary>
        /// Create a Sequence node for a behaviour tree.
        /// </summary>
        /// <param name="parent">this nodes parent</param>
        /// <returns></returns>
        public  Sequence(Node parent)   : base(parent)  {  }

        /*===============================================
                            Methods
        ===============================================*/
        /// <summary>
        /// Run this nodes children and assess their status
        /// </summary>
        public override void Activity()
        {
            
            base.Activity();

            if(CheckCondition() == true)
            {
                
                foreach (Node child in children)
                {
#region                             Pre-activty Checks
                    /*==========================================
                                    pre-activity checks
                    ==========================================*/
                    if (child.StatusCheck() == Status.None)
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

                        //it then should break out of the loop as it shouldn't
                        //continue until this child's activity is completed and 
                        //set to successful or failed.
                        child.Activity();

                    }
                    else if (child.StatusCheck() == Status.Successful)
                    {

                        //if the child node is already successful it should skip 
                        //this iteration 
                        continue;
                        
                    }
                    else if (child.StatusCheck() == Status.Failed)
                    {

                        //if the child node is already failed it should exit
                        //the sequence node as it failed 
                        status = Status.Failed;
                        break;

                    }
#endregion
#region                             Post-activity Checks
                    /*==========================================
                                    post-activity checks
                    ==========================================*/

                    if (child.StatusCheck() == Status.Successful)
                    {

                        ActionExit(child);

#region                         Successful Completion of Sequence
                        /*=============================================
                                Successful Completion of Sequence 
                        =============================================*/

                        // the behaviour below will be excuted once the
                        // last child node is successful therfore completing
                        // the Sequence node.  

                        if(children[children.Count -1].StatusCheck() == Status.Successful)
                        {

                            //if this is last node in the sequence then all
                            //the child nodes need to be reset. 
                            RestChildrenStatus();

                            status = Status.Successful;
                            break;

                        }
#endregion
                        //if the child is successful but isn't the last child
                        //then the iteration can be skipped to move on to the
                        //next child
                        continue;

                    }
#region                         Failed Completion of Sequence
                    /*=============================================
                                Failed Completion of Sequence 
                    =============================================*/

                    // the behaviour below will be excuted once any
                    // child node is Failed therfore completing
                    // the Sequence node.  

                    else if (child.StatusCheck() == Status.Failed)
                    {
                        
                        //reset children's status node as the Sequence node is 
                        //completed. 
                        RestChildrenStatus();

                        ActionExit(child);

                        //once a single node fails the entire sequence fails 
                        status = Status.Failed;

                        break;

                    }
#endregion
                    else if (child.StatusCheck() == Status.Running)
                    {

                        //if the child is still running then the loop needs to
                        //break as it shouldn't continue until this node is complete.
                        break;
                        
                    }
#endregion
                }
              
            }
            else if(CheckCondition() == false)
            {
                
                //if the condition to begin the sequence isn't true then then
                //the sequence will be classes as failed. 
                status = Status.Failed;
                                        
                RestChildrenStatus();


            }  

        }

    }

}