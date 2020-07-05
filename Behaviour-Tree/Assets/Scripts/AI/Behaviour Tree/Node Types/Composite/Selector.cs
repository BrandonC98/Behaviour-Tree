using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{

    /*====================================================================
                                Selector.cs
    ======================================================================

    The Selector node will run its child node if any of them succeed the 
    Selector will be successful and it returns to the parent node excution with
    a successful status. 

    If all node in the Selector are Failed then it returns failure to its 
    parent node. 

    =====================================================================*/

    public class Selector : Composite
    {

        /*================================================
                            Constructor
        ================================================*/
        /// <summary>
        /// Create a Selector Node
        /// </summary>
        /// <param name="parent">this node's parent</param>
        /// <returns></returns>
        public Selector(Node parent)    :   base(parent)    {   }

        /*===============================================
                            Methods
        ===============================================*/
        /// <summary>
        /// Run the this Node's children and assess the Status of this node
        /// </summary>
        public override void Activity()
        {

            base.Activity();

            if (CheckCondition() == true)
            {

                foreach (Node child in children)
                {
#region                             Pre-activty Checks
                    /*==========================================
                                    pre-activity check
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
                    else if (child.StatusCheck() == Status.Successful)
                    {

                        //if the child node is already successful it end the loop
                        //as the selector will be completed.
                        status = Status.Successful;
                        break;

                    }
                    else if (child.StatusCheck() == Status.Failed)
                    {

                        //if the child node is already failed it should continue with 
                        //the other children nodes.
                        continue;

                    }
#endregion
#region                             Post-activity Checks
                    /*==========================================
                                    post-activity check
                    ==========================================*/
                    if (child.StatusCheck() == Status.Failed)
                    {
                        ActionExit(child);

#region                             Failed Completion of Selector
                        /*=============================================
                                    Failed Completion of Selector 
                        =============================================*/

                        //the behaviour below will be excuted once all
                        //child node have Failed therfore completing
                        //the Sequence node.  
                        if(children[children.Count -1].StatusCheck() == Status.Failed)
                        {

                            //if this is the last node in the Selector then all
                            //the child nodes need to be reset. 
                            RestChildrenStatus();

                            status = Status.Failed;
                            
                            break;

                        }
#endregion
                        //if a child node fails then it needs to
                        //skip this child and continue to the 
                        //next child.

                        continue;

                    }
#region                     Successful Completion of Selector
                    /*========================================
                            Successful Completion of Selector 
                    =========================================*/

                        //the behaviour below will be excuted
                        //once any child is successful therefore
                        //completing the selector node.  

                    else if (child.StatusCheck() == Status.Successful)
                    {

                        ActionExit(child);

                        //if a single node is successful then the 
                        //Selector node will be successful.

                        status = Status.Successful;
                    
                        RestChildrenStatus();

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
            else if (CheckCondition()== false)
            {

                //if the condition to begin the Selector isn't true then then
                //the Selector will be classed as failed. 
                status = Status.Failed;
                RestChildrenStatus();

            }

        }

    }

}