using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{

    /*====================================================================
                                Root.cs
    ======================================================================

    The Root Node is the first node in any behaviour tree. the behaviour
    tree's excution will begin here and go to the root node's children.

    =====================================================================*/

    public  class Root : Node
    {

        protected List<Node> children = new List<Node>();

        /*===============================================
                            Methods
        ===============================================*/
        /// <summary>
        /// run the children nodes and check what status they return
        /// </summary>
        public override void Activity()
        {
            base.Activity();

            foreach(Node child in children)
            {
#region                     Pre-activty Checks
                /*==========================================
                            pre-activity checks
                ==========================================*/
                if(child.StatusCheck() == Status.Running)
                {
                    //if the child node is already running it should 
                    //run again to attempt to complete the child activity

                    //it then should break out of the loop as it shouldn't
                    //continue until this child's activity is completed and 
                    //set to successful or failed.
                    child.Activity();

                }
                else if (child.StatusCheck() == Status.None)
                {

                    //if the child node is already set to None it
                    //should run the child activity as this will be the 
                    //first time it runs.

                    //it then should run the post activity checks to 
                    //see the outcome of the child's activity function

                    ActionEnter(child);

                    child.Activity();
                    

                }
#endregion
#region                     Post-activity Checks
                 /*==========================================
                            post-activity checks
                ==========================================*/
                if(child.StatusCheck() == Status.Failed)
                {
                    //reset children's status node as the child node is 
                    //completed. 
                    child.ResetStatus();

                    ActionExit(child);

                    //skip to the next child
                    continue;

                }
                else if(child.StatusCheck() == Status.Successful)
                {

                    child.ResetStatus();

                    ActionExit(child);

                    continue;


                }
                else if(child.StatusCheck() == Status.Running)
                {

                    //if the child is still running then the loop needs to
                    //break as it shouldn't continue until this node is complete.
                    break;

                }
#endregion
            }           

        }

        /// <summary>
        /// set the children nodes
        /// </summary>
        /// <param name="childrenNode"></param>
        public void SetChildren(List<Node> childrenNode)
        {

            children.AddRange(childrenNode);
            
            foreach (Node nod in children)
            { Debug.Log(nod); }

        }

    }

}

