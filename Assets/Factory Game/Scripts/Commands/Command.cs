using System;

namespace BaseUnit.Commands
{
    public abstract class Command
    {
        public abstract event Action OnStart;
        public abstract event Action OnComlete;
        public abstract event Action OnCancel;

        public abstract CommandState CommandState { get; }
        public abstract void Execute();
        public abstract void Cancel();
    }

    public enum CommandState { Pending, Executing }
}