using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue>
{

    // We save the keys and values in two lists because Unity does understand those.
    [SerializeField]
    private List<TKey> m_keys = new List<TKey>();
    [SerializeField]
    private List<TValue> m_values = new List<TValue>();

    public int Count
    {
        get { return m_keys.Count; }
    }
    
    public TKey[] Keys
    {
        get
        {
            TKey[] keys = new TKey[Count];
            m_keys.CopyTo(keys);
            return keys;
        }
    }

    public TValue[] Values
    {
        get
        {
            TValue[] values = new TValue[Count];
            m_values.CopyTo(values);
            return values;
        }
    }

    public void Add(TKey key, TValue value)
    {
        if (!m_keys.Contains(key))
        {
            m_keys.Add(key);
            m_values.Add(value);
            return;
        }

        int key_i = m_keys.IndexOf(key);

        m_values.Insert(key_i, value);
    }

    public void AddOrUpdate(TKey key, TValue value)
    {
        if (!m_keys.Contains(key))
        {
            m_keys.Add(key);
        }

        int key_i = m_keys.IndexOf(key);

        m_values.RemoveAt(key_i);
        m_values.Insert(key_i, value);
    }

    public bool Remove(TKey key)
    {
        int i = m_keys.IndexOf(key);
        if (i < 0)
            return false;

        m_values.RemoveAt(i);
        return m_keys.Remove(key);
    }

    public bool Contains(TKey key)
    {
        return m_keys.Contains(key);
    }

    public bool Contains(TValue value)
    {
        return m_values.Contains(value);
    }

    public void Clear() {
        m_keys.Clear();
        m_values.Clear();
    }

    public TValue this[TKey index]    // Indexer declaration
    {
        get
        {
            int i = m_keys.IndexOf(index);
            if (i > -1)
                return m_values[i];
            return default(TValue);
        }

        set
        {
            int i = m_keys.IndexOf(index);
            if (i > -1 && m_values[i] != null)
            {
                m_values[i] = value;
            }
        }
    }

    public void RenameKey(TKey fromKey, TKey toKey)
    {
        TValue value = this[fromKey];
        Remove(fromKey);
        Add(toKey, value);
    }
}
