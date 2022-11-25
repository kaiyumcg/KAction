using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    [System.Serializable]
    public class FOrderedDictionaryPair<TKey, TValue>
    {
        [SerializeField] TKey m_key;
        [SerializeField] TValue m_value;
        public TKey _Key { get { return m_key; } internal set { m_key = value; } }
        public TValue _Value { get { return m_value; } set { m_value = value; } }
        internal FOrderedDictionaryPair()
        {
            m_key = default;
            m_value = default;
        }
    }

    [System.Serializable]
    public class FOrderedDictionary<Key, Value> : IEnumerator, IEnumerable
    {
        [SerializeField] List<FOrderedDictionaryPair<Key, Value>> content;
        bool isKeyValueType;
        public FOrderedDictionary()
        {
            content = new List<FOrderedDictionaryPair<Key, Value>>();
            this.isKeyValueType = typeof(Key).IsValueType;
        }

        public FOrderedDictionaryPair<Key, Value> Get(int id)
        {
            return content[id];
        }

        /// <summary>
        /// Adds a value against a key. If the collection is configured to not have any null value then this is also checked
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddValue(Key key, Value value, bool useOptimization = false)
        {
            bool success = false;
            if (DSUtil.IsNull(key, isKeyValueType, useOptimization) == false && ContainsKey(key, useOptimization) == false)
            {
                content.Add(new FOrderedDictionaryPair<Key, Value> { _Key = key, _Value = value });
                success = true;
            }
            return success;
        }

        /// <summary>
        /// Removes an entry which have the specified 'Not null' key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveKeyedPair(Key key, bool useOptimization = false)
        {
            bool success = false;
            if (DSUtil.IsNull(key, isKeyValueType, useOptimization) == false && ContainsKey(key, useOptimization))
            {
                bool pairGetSuccess = false;
                FOrderedDictionaryPair<Key, Value> pair = null;
                pairGetSuccess = TryGetPair(key, ref pair, useOptimization);
                if (pairGetSuccess)
                {
                    content.Remove(pair);
                    success = true;
                }
            }
            return success;
        }

        /// <summary>
        /// Check whether a 'Not null' key is present in the collection
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(Key key, bool useOptimization = false)
        {
            bool exist = false;
            if (DSUtil.IsNull(key, isKeyValueType, useOptimization) == false && content != null && content.Count > 0)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    if (DSUtil.IsEqual(content[i]._Key, key, isKeyValueType, useOptimization))
                    {
                        exist = true;
                        break;
                    }
                }
            }
            return exist;
        }

        bool TryGetPair(Key key, ref FOrderedDictionaryPair<Key, Value> pair, bool useOptimization = false)
        {
            bool exist = false;
            pair = null;
            if (content != null && content.Count > 0)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    if (DSUtil.IsEqual(content[i]._Key, key, isKeyValueType, useOptimization))
                    {
                        exist = true;
                        pair = content[i];
                        break;
                    }
                }
            }
            return exist;
        }

        /// <summary>
        /// Output the value of a given 'Not null' key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(Key key, out Value value, bool useOptimization = false)
        {
            value = default(Value);
            bool success = false;
            if (DSUtil.IsNull(key, isKeyValueType, useOptimization) == false)
            {
                FOrderedDictionaryPair<Key, Value> pair = null;
                success = TryGetPair(key, ref pair, useOptimization);
                value = success ? pair._Value : default(Value);   
            }
            return success;
        }

        void SetData(Key key, Value value, bool useOptimization = false)
        {
#if GF_DEBUG
            bool success = false;
#endif
            if (DSUtil.IsNull(key, isKeyValueType, useOptimization) == false && content != null && content.Count > 0)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    if (DSUtil.IsEqual(content[i]._Key, key, isKeyValueType, useOptimization))
                    {
                        content[i]._Value = value;
#if GF_DEBUG
                        success = true;
#endif
                        break;
                    }
                }
            }

#if GF_DEBUG
            if (success == false)
            {
                KLog.PrintWarning("You were trying to set value against a key in the dictionary but the key is not present! Default value will be returned.");
            }
#endif
        }

        Value GetData(Key key, bool useOptimization = false)
        {
            Value result = default(Value);
            if (DSUtil.IsNull(key, isKeyValueType, useOptimization) == false)
            {
                bool success = TryGetValue(key, out result, useOptimization);
#if GF_DEBUG
                if (success == false)
                {
                    KLog.PrintWarning("You were trying to get value against a key in the dictionary but the key is not present! Default value will be returned.");
                }
#endif
            }
            return result;
        }

        /// <summary>
        /// Access the elements of the collection. If we can not get or set values due to many reasons(i.e. key is null), 
        /// Then return value is the default(Value)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Value this[Key key, bool useOptimization = false]
        {
            get => GetData(key, useOptimization);
            set => SetData(key, value, useOptimization);
        }

        /// <summary>
        /// Element count of the collection
        /// </summary>
        public int Count { get { return content == null ? 0 : content.Count; } }

        #region DotNetIterator
        object IEnumerator.Current { get { return content[position]; } }
        int position = -1;
        bool IEnumerator.MoveNext()
        {
            position++;
            return (position < content.Count);
        }

        void IEnumerator.Reset()
        {
            position = 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this;
        }
        #endregion
    }
}