namespace BaseUnit.Commands
{
    public class Inventory<T>
    {
        public T StoredItem { get; private set; }

        public bool IsEmpty => StoredItem == null;

        public void Put(T item) => StoredItem = item;

        public void Clear() => StoredItem = default(T);
    }
}