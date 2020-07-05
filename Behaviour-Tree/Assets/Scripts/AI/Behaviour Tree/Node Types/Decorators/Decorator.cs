using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Decorator : Node
    {

        /*==================================================
                                Decorator.cs
        ====================================================

        Decorator nodes are nodes with only a single child.
        they will usually modify some functionality. 

        ==================================================*/

        protected Node child;

        /*==================================================
                                Constructor
        ==================================================*/
        /// <summary>
        /// Create a Decorator Node
        /// </summary>
        /// <param name="parent">this nodes parent</param>
        /// <returns></returns>
        public Decorator(Node parent)  :   base(parent) {}

        /*==================================================
                                Methods
        ==================================================*/
        /// <summary>
        /// Set the child Node.
        /// </summary>
        /// <param name="child"></param>
        public void GetChild(Node child)
        {

            this.child = child;
            Debug.Log(child);
        }

        /// <summary>
        /// Set child status to None
        /// </summary>
        public void ResetChild()
        {

            child.ResetStatus();

        }

    }
}


