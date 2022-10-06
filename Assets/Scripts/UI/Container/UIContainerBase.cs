using System.Collections.Generic;
using UnityEngine;

namespace UI.Container{
    public abstract class UIContainerBase : MonoBehaviour{
        public readonly List<MonoBehaviour> children = new();
        public abstract void UpdateLayout();

        /// <summary>
        ///     Add an object to the container. DO NOT update layout;
        /// </summary>
        /// <param name="obj">The Object to be added</param>
        public void AddChild(MonoBehaviour obj){
            obj.transform.parent = transform;
            children.Add(obj);
        }

        /// <summary>
        ///     Remove an object from the container. If the object is not a child to the container, nothing would be done.
        ///     Otherwise, the object would be child to nothing.
        /// </summary>
        /// <param name="obj">The object to be removed</param>
        public void RemoveChild(MonoBehaviour obj){
            if (children.Contains(obj)){
                obj.transform.parent = null;
                children.Remove(obj);
            }
        }

        public void UntachChild(int index){
            if (index >= 0 && index < children.Count) children[index].transform.parent = null;
        }

        public void AttachChild(int index){
            if (index >= 0 && index < children.Count) children[index].transform.parent = transform;
        }
    }
}