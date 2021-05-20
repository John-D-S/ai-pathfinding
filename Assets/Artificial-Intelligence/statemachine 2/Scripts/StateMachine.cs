
using System.Collections.Generic;
using UnityEngine;

namespace StateMachines
{
    public enum State
    {
        Translate,
        Rotate,
        Scale
    }

    // the delegate that dictates what the functions for each state will look like
    public delegate void StateDelegate();

    public class StateMachine : MonoBehaviour
    {
        private Dictionary<State, StateDelegate> states = new Dictionary<State, StateDelegate>();

        [SerializeField] private State currentState = State.Translate;
        [SerializeField] private Transform controlled; // the thing that will be effected by our state
        [SerializeField] private float speed = 1f; // this isn't really in a state machine, this is just for testing

        public void ChangeState(State _newState)
        {
            if (_newState != currentState)
            {
                currentState = _newState;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // This is the same as checking if he variable is null, then setting it, otherwise
            // retain the value
            controlled ??= transform;

            states.Add(State.Translate, Translate);
            states.Add(State.Rotate, Rotate);
            states.Add(State.Scale, Scale);
        }

        // Update is called once per frame
        void Update()
        {
            //these two lines are what actually runs the state machine.
            // it works by attempting to retrieve the relevant function for the current state,
            // then running the function if it successfully found it
            if (states.TryGetValue(currentState, out StateDelegate state))
                state.Invoke();
            else
                Debug.LogError($"No state funciton set for state {currentState}");
        }

        private void Translate() => controlled.position += controlled.forward * Time.deltaTime * speed;
        private void Rotate() => controlled.Rotate(Vector3.up, speed * 0.5f);
        private void Scale() => controlled.localScale = Vector3.one * Mathf.PingPong(Time.deltaTime, 1);
    }
}
