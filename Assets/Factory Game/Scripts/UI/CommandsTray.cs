using System.Collections.Generic;
using UnityEngine;
using BaseUnit.Commands;

public class CommandsTray : MonoBehaviour
{
    [SerializeField]
    private CommandIcon commandIconPrefab;
    [SerializeField]
    private CommandVisualisationInfo visualisationInfo;

    private List<CommandIcon> commandIconsPool = new();

    private bool interactable;

    public bool Interactable
    {
        get { return interactable; }

        set
        {
            interactable = value;

            if (interactable)
            {
                foreach (var item in commandIconsPool)
                    item.AllowInteraction();
            }
            else
            {
                foreach (var item in commandIconsPool)
                    item.ProhibitInteraction();
            }
        }
    }

    private void Awake() => Init();

    private void Init() => Interactable = true;

    public void AddCommand<T>(T displayableCommand) where T : Command, IDisplayable
    {
        CommandIcon commandIcon = GetCommandIcon();
        commandIcon.SetActionIcon(displayableCommand.GetActionSprite(visualisationInfo));
        commandIcon.OnClick += displayableCommand.Cancel;

        displayableCommand.OnCancel += delegate
        {
            commandIcon.OnClick -= displayableCommand.Cancel;
            ResetCommandIcon(commandIcon);
        };

        displayableCommand.OnComplete += delegate
        {
            commandIcon.OnClick -= displayableCommand.Cancel;
            ResetCommandIcon(commandIcon);
        };
    }

    private void ResetCommandIcon(CommandIcon commandIcon)
    {
        commandIcon.SetDefaultVisual();
        commandIcon.gameObject.SetActive(false);
        commandIcon.transform.SetAsLastSibling();
    }

    private CommandIcon GetCommandIcon()
    {
        foreach (var item in commandIconsPool)
        {
            if (!item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(true);
                return item;
            }
                
        }

        CommandIcon commandIcon = Instantiate(commandIconPrefab, transform);
        commandIconsPool.Add(commandIcon);

        return commandIcon;
    }
}