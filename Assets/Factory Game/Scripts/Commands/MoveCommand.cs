using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace BaseUnit.Commands
{
    public class MoveCommand : Command
    {
        public override event Action OnStart;
        public override event Action OnComlete;
        public override event Action OnCancel;

        public override CommandState CommandState => commandState;
        private CommandState commandState;

        private readonly Vector3 destination;
        private readonly NavMeshAgent agent;
        private readonly MonoBehaviour agentMonoBehaviour;

        private Coroutine completeExecutionCoroutine;

        public MoveCommand(NavMeshAgent agent, Vector3 destination)
        {
            this.agent = agent;
            this.destination = destination;
            commandState = CommandState.Pending;

            agentMonoBehaviour = agent.GetComponent<MonoBehaviour>();
        }

        public override void Execute()
        {
            commandState = CommandState.Executing;
            OnStart?.Invoke();

            agent.SetDestination(destination);

            completeExecutionCoroutine = agentMonoBehaviour.StartCoroutine(CompleteExecution());
        }

        private IEnumerator CompleteExecution()
        {
            float minDistance = 0.1f;
            yield return new WaitUntil(() => agent.remainingDistance < minDistance);

            commandState = CommandState.Pending;
            OnComlete?.Invoke();
        }

        public override void Cancel()
        {
            if (commandState == CommandState.Executing)
            {
                if (completeExecutionCoroutine != null)
                {
                    agentMonoBehaviour.StopCoroutine(completeExecutionCoroutine);
                    completeExecutionCoroutine = null;
                }

                agent.ResetPath();
            }

            commandState = CommandState.Pending;
            OnCancel?.Invoke();
        }
    }
}