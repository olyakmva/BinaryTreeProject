using System.Collections;

namespace BinTreeLib
{
    public class BinaryTree<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> 
    {
        public int Count { get; private set; }
        private IComparer<TKey> _comparer;
        private Node<TKey, TValue> _root; //корень
        public BinaryTree(): this(null, Comparer<TKey>.Default)
        { }
        public BinaryTree(IComparer<TKey> comparer):this(null, comparer)
        { }
        public BinaryTree(IDictionary<TKey,TValue> dictionary):this(dictionary, Comparer<TKey>.Default)
        { }
        public BinaryTree(IDictionary<TKey,TValue> dictionary, IComparer<TKey> comparer)
        {
            _comparer = comparer;
            Count = 0;
            _root = null;
            if (dictionary != null && dictionary.Count > 0)
            {
                foreach (var pair in dictionary)
                {
                    Add(pair.Key, pair.Value);
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            var node = new Node<TKey, TValue>(key, value);
            if (_root == null)
            {
                _root = node;
                Count++;
                return;
            }
            var current = _root;
            var parent = _root;
            while (current != null)
            {
                parent = current;
                if (_comparer.Compare( current.Key,node.Key) == 0)
                {
                    throw new ArgumentException("Such key is already added");
                }
                if (_comparer.Compare(current.Key, node.Key) > 0)
                {
                    current = current.Left;
                }
                else if (_comparer.Compare(current.Key, node.Key) < 0)
                {
                    current = current.Right;
                }
            }
            if (_comparer.Compare(parent.Key, node.Key) > 0)
            {
                parent.Left = node;
            }
            if (_comparer.Compare(parent.Key, node.Key) < 0)
            {
                parent.Right = node;
            }
            node.Parent = parent;
            Count++;
        }

        public bool ContainsKey(TKey key)
        {
            // Поиск узла осуществляется другим методом.
            return Find(key) != null;
        }
        private Node<TKey, TValue> Find(TKey findKey)
        {
            // Попробуем найти значение в дереве.
            var current = _root;

            // До тех пор, пока не нашли...
            while (current != null)
            {
                int result = _comparer.Compare(current.Key, findKey);
                if (result > 0)
                {
                    // Если искомое значение меньше, идем налево.
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // Если искомое значение больше, идем направо.
                    current = current.Right;
                }
                else
                {
                    // Если равны, то останавливаемся
                    break;
                }
            }
            return current;
        }
        public TValue this[TKey key]
        {
            get 
            {
                if (key == null)
                    throw new ArgumentNullException();
                var node = Find(key);
                return node == null ? throw new KeyNotFoundException() : node.Value;
            }
            set 
            {
                if (key == null)
                    throw new ArgumentNullException();
                var node = Find(key);
                if (node == null)
                    Add(key, value);
                else node.Value = value;
            } 
        }
        public void Clear()
        {
            _root = null;
            Count = 0;
        }

        public bool ContainsValue( TValue value)
        {
            var comparer = EqualityComparer<TValue>.Default;
            foreach( var keyValuePair in Traverse())
            {
                if(comparer.Equals(value, keyValuePair.Value))
                    return true;
            }
            return false;
        }
        IEnumerable<KeyValuePair<TKey, TValue>> Traverse(Node<TKey, TValue> node)
        {
            var nodes = new List<KeyValuePair<TKey, TValue>>();
            if (node != null)
            {
                nodes.AddRange(Traverse(node.Left));
                nodes.Add(new KeyValuePair<TKey, TValue>(node.Key, node.Value));
                nodes.AddRange(Traverse(node.Right));
            }
            return nodes;
        }
        public IEnumerable<KeyValuePair<TKey, TValue>> Traverse()
        {
            return Traverse(_root);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Traverse().GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Traverse().GetEnumerator();
        }

        
       
    }
}
