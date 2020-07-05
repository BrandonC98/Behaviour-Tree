using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    
    public abstract class Node 
    {

    /*============================================================
                                Node.cs
    ==============================================================

    this class is the base class for all the different types 
    of nodes, as it's abstract. the functionality for conditions 
    and Status is here as they need to be inherited by all nodes.   

    ============================================================*/

        //the Status enum is used to identify what state a node is
        //currently in. 
        public enum Status { Successful, Failed, Running, None }
        protected Status status;      

        public Node parent;

        protected bool condition;

        protected string tag;

        /*=========================================================
                                Constructor
        =========================================================*/

        /// <summary>
        /// create a node
        /// </summary>
        /// <param name="parent">this nodes parent</param>
        public Node(Node parent)
        {
            
            this.parent = parent;
            this.status = Status.None;
            condition = false;

        }

        /// <summary>
        /// create a Node
        /// </summary>
        protected Node()    {   }


        /*========================================================
                                Methods
        ========================================================*/
        
#region                         Status fuctions 
        /// <summary>
        /// set Status to None 
        /// </summary>
        public void ResetStatus()
        {

            status = Status.None;

        }

        /// <summary>
        /// returns the current status
        /// </summary>
        /// <returns></returns>
        public Status StatusCheck()
        {

            return status;

        }
#endregion
#region                         Condition functions
        /// <summary>
        /// Sets the condition to true 
        /// </summary>
        public void ConditionTrue()
        {

            condition = true;

        }

        /// <summary>
        /// checks if the conditon is true or false.
        /// </summary>
        /// <returns></returns>
        public bool CheckCondition()
        {

            return condition;

        }

        /// <summary>
        /// set the condition to false
        /// </summary>
        public void ConditionFalse()
        {

            condition = false;

        }
#endregion

    /// <summary>
    /// set debug tags so action delegates will be outputed to the console
    /// </summary>
    /// <param name="tag"></param>
        public void SetTag(string tag)
        {

            this.tag = tag;

        }

        /// <summary>
        /// run the Action node's enter delegate if applicable
        /// </summary>
        /// <param name="child"></param>
        protected void ActionEnter(Node child)
        {

            if(child is Action) 
            {

                Action tempAct = (Action)child;
                tempAct.Enter();

            }

        }

        /// <summary>
        /// run the Action node's exit delegate if applicable
        /// </summary>
        /// <param name="child"></param>
        protected void ActionExit(Node child)
        {

            if(child is Action) 
            {

                Action tempAct = (Action)child;
                tempAct.Exit();

            }

        }

        /// <summary>
        /// set the status to running
        /// </summary>
        public virtual void Activity()
        {

            //this function will be the main source of logic 
            //for all the different type of nodes. it's virtual 
            //and will be inherited. 

            //once this function is called the first thing it needs
            //to do is set the status to running. all child class should
            //call the base activty to implment this. 

            status = Status.Running;    

        }

    }
}
