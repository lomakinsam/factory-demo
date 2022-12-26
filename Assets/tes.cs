using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUnit.Commands;
using UnityEngine.AI;

public class tes : MonoBehaviour
{
    Command command;

    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent agent = gameObject.AddComponent<NavMeshAgent>();
        command = new MoveCommand(agent, Vector3.zero);

        command.OnCancel += delegate { Print("+"); };

        command.Cancel();
    }

    private void Print(string text) => Debug.Log(text);
}
