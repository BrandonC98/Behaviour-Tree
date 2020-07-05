using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{

    /*====================================================================
                                Tree.cs
    ======================================================================

    The Tree class contains all the information needed for behaviour tree
    AI. It allows the user to create the different nodes needed for an AI.
    the user can then create sections and add the nodes to them. 

    =====================================================================*/

    public class Tree 
    {       

        //store all parent nodes
        Dictionary<int, Node> ParentNodes = new Dictionary<int, Node>();

        int parentCounter = 0;

        public Root root;

        List<Node> tempNodes = new List<Node>();

        public bool runAI;

        /*================================================
                            Methods
        ================================================*/
        /// <summary>
        /// Add a Root Node to the Behaviour Tree
        /// </summary>
        /// <returns></returns>
        public Root AddRoot()
        {

            root = new Root();
            
            ParentNodes.Add(parentCounter, root);
            parentCounter++;
            return root;

        }

#region                 Section Functions
        /*================================================
                        Section Functions
        ================================================*/

        /// <summary>
        /// make a section so children can be assigned 
        /// </summary>
        /// <param name="parentNode">the node that children will be added to</param>
        public void MakeSectionFor(Node parentNode)
        {            

            ParentNodes.Add(parentCounter, parentNode);
            
        }

        /// <summary>
        /// add these nodes to the section 
        /// </summary>
        /// <param name="nodes">the nodes to become children of the section node</param>
        public void AddToSection(params Node[] nodes)
        {

            foreach(Node node in nodes)
            {

                //the temp nodes are used to store all the children nodes of the section
                tempNodes.Add(node);

            }
            
        }

        /// <summary>
        /// ends the current section so no children can be added to a node
        /// </summary>
        public void EndSection()
        {

            //the parent node needs to be casted to a Composite, Decorator or Root
            //node. this is so the children can be assigned using the correct function
            //for the type of node. 

            if (ParentNodes[parentCounter] is Composite)
            {
                //cast the node to a Composite node to assign multiple children
                Composite comp = (Composite)ParentNodes[parentCounter];
                comp.SetChildren(tempNodes);

            }
            else if (ParentNodes[parentCounter] is Root)
            {

                //cast the node to a Root node to assign multiple children 
                Root rt = (Root)ParentNodes[parentCounter];
                rt.SetChildren(tempNodes);

            }
            else if (ParentNodes[parentCounter] is Decorator)
            {

                //cast the node to a Decorator node to assign a single node
                Decorator dec = (Decorator)ParentNodes[parentCounter];
                dec.GetChild(tempNodes[0]);

            }
            
            tempNodes.Clear();
            parentCounter++;

        }
#endregion
#region                 Maker Functions
        /*================================================
                        Maker Functions
        ================================================*/
        //these functions creates the nodes and returns them
        //it also assigns their parent node to that of the
        //dictonary ParentNodes using the parentCounter as
        //the index value. 

#region                 Composite Nodes
        /*=============
        Composite Nodes
        =============*/
        
        /// <summary>
        /// Make and return a Selector Node
        /// </summary>
        /// <returns></returns>
        public Selector MakeSelector()
        {

            Selector selector = new Selector(ParentNodes[parentCounter]);
            return selector;

        }

        /// <summary>
        /// Make and return a Sequence Node
        /// </summary>
        /// <returns></returns>
        public Sequence MakeSequence()
        {

            Sequence sequence = new Sequence(ParentNodes[parentCounter]);
            return sequence;

        }
#endregion
#region                 Decorator Nodes
        /*==============
        Decorator Nodes
        ==============*/
        /// <summary>
        /// make and return a Repeater Node
        /// </summary>
        /// <param name="RepeatAmount">how many times should this child node repeat</param>
        /// <returns></returns>
        public Repeater MakeRepeater(int RepeatAmount)
        {

            Repeater repeater = new Repeater(ParentNodes[parentCounter], RepeatAmount);
            return repeater;

        }

        /// <summary>
        /// make and return a Repeater Node without setting the amount of times to repeat
        /// </summary>
        /// <returns></returns>
        public Repeater MakeRepeater()
        {

            Repeater repeater = new Repeater(ParentNodes[parentCounter]);
            return repeater;

        }

        /// <summary>
        /// make and return a Succeeder Node
        /// </summary>
        /// <returns></returns>
        public Succeeder MakeSucceeder()
        {

            Succeeder succeeder = new Succeeder(ParentNodes[parentCounter]);
            return succeeder;

        }
#endregion

        /// <summary>
        /// Make and return a Action Node
        /// </summary>
        /// <returns></returns>
        public Action MakeAction()
        {

            Action action = new Action(ParentNodes[parentCounter]);
            return action;

        }
#endregion
#region             Behaviour Tree excution functions
        /*================================================
                    Behaviour Tree excution control
        ================================================*/

        /// <summary>
        /// put this function in the Update function. once Tree.Start() is called the AI will run
        /// </summary>
        public void Run()
        {

            if (runAI == true)  root.Activity();

        }

        /// <summary>
        /// Enables the AI to run
        /// </summary>
        public void Start()
        {

            runAI = true;

        }

        /// <summary>
        /// Disables the AI from running
        /// </summary>
        public void Stop()
        {

            runAI = false;

        }
#endregion
    }

}

