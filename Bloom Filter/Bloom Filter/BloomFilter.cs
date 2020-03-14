using System;
using System.Collections.Generic;
using System.Text;

namespace Bloom_Filter
{
    public class BloomFilter<T>
    {
        bool[] bitSet;
        int capacity;
        HashSet<Func<T, int>> hashFunction;
        public int Count { get; private set; }
        public int HashFunctionCount => hashFunction.Count;

        public BloomFilter(int cap)
        {
            capacity = cap;
            bitSet = new bool[capacity];
            hashFunction = new HashSet<Func<T, int>>();
        }

        public void LoadHashFunc(Func<T, int> hashFunc)
        {
            if (hashFunction != null)
            {
                hashFunction.Add(hashFunc);
            }
        }

        public void Insert(T item)
        {
            if (HashFunctionCount == 0)
            {
                UseProvidedFilters();
            }
            foreach (var func in hashFunction)
            {
                var index = Math.Abs(func(item)) % capacity;
                bitSet[index] = true;
            }

            Count++;
        }

        public bool ProbablyContains(T item)
        {
            foreach (var func in hashFunction)
            {
                int index = Math.Abs(func(item)) % capacity;
                if (bitSet[index] == false)
                {
                    return false;
                }
            }
            return true;
        }

        private void UseProvidedFilters()
        {
            hashFunction.Add(HashFuncOne);
            hashFunction.Add(HashFuncTwo);
            hashFunction.Add(HashFuncThree);
        }



        private int HashFuncOne(T item)
        {
            return item.GetHashCode();
        }

        private int HashFuncTwo(T item)
        {
            string dummyString = "dummystring";
            return (dummyString, item).GetHashCode();
        }

        private int HashFuncThree(T item)
        {
            int hash = 17;

            hash *= (HashFuncOne(item), HashFuncTwo(item)).GetHashCode();

            return hash;

        }
    }
}
