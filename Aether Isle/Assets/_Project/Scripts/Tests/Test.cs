using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Test : MonoBehaviour
    {
        Stack<float> myStack = new Stack<float>();

        private void Awake()
        {
            myStack.Push(1);
            myStack.Push(2);
            print(myStack.Peek());
            print(myStack.Pop());
            print(myStack.Peek());
        }
    }
}
