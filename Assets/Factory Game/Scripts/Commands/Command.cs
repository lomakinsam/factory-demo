using System;

namespace BaseUnit.Commands
{
    public abstract class Command
    {
        public abstract event Action<Command> OnStart;
        public abstract event Action<Command> OnComplete;
        public abstract event Action<Command> OnCancel;

        public abstract CommandState CommandState { get; }
        public abstract void Execute();
        public abstract void Cancel();
    }

    public enum CommandState { Pending, Executing }
}