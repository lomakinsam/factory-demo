namespace BaseUnit.Commands
{
    public class Inventory<T>
    {
        public T StoredItem { get; private set; }

        public void Put(T item) => StoredItem = item;

        public void Remove() => StoredItem = default(T);
    }
}