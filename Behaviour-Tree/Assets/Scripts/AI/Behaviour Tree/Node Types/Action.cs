using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Action : Node
    {

        /*==============================================================
                                    Action.cs
        ================================================================

        The Action node will excute the action defined in the
        ActionDelegates, which are user defined. the user will also need 
        to define when and if the the action is successful or failed. 

        ==============================================================*/
        
        public bool conditional = false;

#region                             Delegates
        /*==============================================================
                                    Delegates
        ==============================================================*/

        //delegates are used to perform an activty in the behaviour tree.
        // e.g move to player.

        //there is also a option for delegates with more functionality 

#region                             Standard Delegates
        /*=================
        standard Delegates
        =================*/

        //the standard Delegates can be used for most scenario.

        public delegate void ActionDelegates();

        //this delegate is excuted once at the start of the Action.
        //if a action is completed and the Behaviour tree runs the
        //action again this will also be excuted again but only
        //once per full action.
        public ActionDelegates onEnter;

        //this delegate should be treated like the Update() function
        //it will run each frame if the action is running. 
        public ActionDelegates onLoop;
        
        //this delegate is like the OnEnter but will excute at the
        //end of a action node. 
        public ActionDelegates onExit;

#endregion
#region                             Advanced Delegates
        /*================
        advanced Delegates
        ================*/

        //some users might need Delegates with parameters if so
        //they should be put here.

        //if there has been advanced delegates assigned here it NEEDS to
        //be put into one of the delegate exctuion functions(Enter(), Loop(), Exit())
        //this can be found at the bottom of this script. 


#endregion
#endregion

        /*===============================================================
                                    Constructor
        ===============================================================*/
        /// <summary>
        /// Excute the loop
        /// </summary>
        /// <param name="parent">this node's parent</param>
        /// <returns></returns>
        public Action(Node parent)  :   base(parent) {   }  

        /*==============================================================
                                    Methods
        ==============================================================*/

        /// <summary>
        /// this method defines if the action needs 
        /// to meet a condition before it can run.
        /// </summary>
        /// <param name="conditional">if True no condition
        ///  is needed to run the action </param>
        public void conditionsOff(bool conditional)
        {
            
            if (conditional) this.conditional = false;
            else this.conditional = true;

        }

        /// <summary>
        /// check if there is a condition to the action
        /// </summary>
        /// <returns></returns>
        public bool IsConditional()
        {

            return conditional;

        }

        /// <summary>
        /// excute the the loop delegate 
        /// </summary>
        public override void Activity()
        {
            base.Activity(); 
            
            if (CheckCondition() == true || conditional == false)
            {
                Loop();              
            }
            else
            {
                //if the condition isn't met then the action fails
                ActionFailed();

            }

        }

        /// <summary>
        /// Set the Status to successful. indicating the action is complete
        /// </summary>
        public void ActionSuccessful()
        {

            status = Status.Successful;
            if (tag != null) Debug.Log( tag + " Successful");

        }

        /// <summary>
        /// Set the Status to failed. indicating the action is complete
        /// </summary>
        public void ActionFailed()
        {

            status = Status.Failed;
            if (tag != null) Debug.Log( tag + " Failed");

        }

#region                     Delegates excution
        /*=============================================================
                            delegates excution 
        =============================================================*/
        /// <summary>
        /// excute all Enter delegates 
        /// </summary>
        public void Enter()
        {

            if(tag != null) Debug.Log("in Enter() for " + tag + " Action");

            //delegates should be placed here if
            //they need to be done BEFORE the action.

            //basic on enter delegate
            if(onEnter != null) onEnter();

            //place any advanced enter Delegates below.

        }

        /// <summary>
        /// excute all Loop delegates
        /// </summary>
        public void Loop()
        {

            if(tag != null) Debug.Log("in Loop() for " + tag + " Action");

            //delegates should be placed here if
            //they need to be done each frame.

            //basic on loop delegate
            if(onLoop != null) onLoop();

            //place any advanced loop Delegates below.

        }

        /// <summary>
        /// excute all Exit delegates
        /// </summary>
        public void Exit()
        {

            if(tag != null) Debug.Log("in Exit() for " + tag + " Action");

            //delegates should be placed here if
            //they need to be done AFTER the action.

            //basic on exit delegate
            if(onExit != null) onExit();

            //place any advanced exit delegate below.

        }
#endregion
    }

}
