using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace BaseUnit.Commands
{
    public class MoveCommand : Command
    {
        public override event Action<Command> OnStart;
        public override event Action<Command> OnComplete;
        public override event Action<Command> OnCancel;

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
            OnStart?.Invoke(this);

            agent.SetDestination(destination);

            completeExecutionCoroutine = agentMonoBehaviour.StartCoroutine(CompleteExecution());
        }

        private IEnumerator CompleteExecution()
        {
            yield return new WaitWhile(() => agent.pathPending);

            float minDistance = 0.1f;
            yield return new WaitUntil(() => agent.remainingDistance < minDistance);

            commandState = CommandState.Pending;
            OnComplete?.Invoke(this);
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
            OnCancel?.Invoke(this);
        }
    }
}