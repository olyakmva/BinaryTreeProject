﻿namespace BinTreeLib
{
    internal class Node<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public Node<TKey, TValue> Left, Right, Parent;
        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
