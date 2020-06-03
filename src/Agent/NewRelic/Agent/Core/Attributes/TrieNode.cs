using System.Collections.Generic;

namespace NewRelic.Agent.Core.Attributes
{
    public class TrieNode<T>
    {
        public readonly T Data;

        public readonly ICollection<TrieNode<T>> Children = new List<TrieNode<T>>();

        public TrieNode(T metaData)
        {
            Data = metaData;
        }
    }
}
