using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Composite : Node
    {

        /*==================================================
                                Composite.cs
        ====================================================

        Composite nodes are nodes with mutiple children. 

        ==================================================*/

        protected List<Node> children = new List<Node>();

        /*==================================================
                                Constructor
        ==================================================*/
        /// <summary>
        /// Create a Composite Node
        /// </summary>
        /// <param name="parent">this nodes parent</param>
        /// <returns></returns>
        public Composite(Node parent)   :   base(parent)    {}

        /*==================================================
                                Methods
        ==================================================*/
        /// <summary>
        /// Set the children Nodes.
        /// </summary>
        /// <param name="childrenNode"></param>
        public void SetChildren(List<Node> childrenNode)
        {
            children.AddRange(childrenNode);
            
            foreach (Node nod in children)
            {

                Debug.Log(nod);

            }

        }

        /// <summary>
        /// Resets all the children's status to None.
        /// </summary>
        protected void RestChildrenStatus()
        {

            foreach(Node child in children)
            child.ResetStatus();

        }


    }
}
