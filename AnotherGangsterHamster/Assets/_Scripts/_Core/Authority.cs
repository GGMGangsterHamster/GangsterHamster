using System.Collections.Generic;

public class Authority : Singleton<Authority>
{
    private Dictionary<string, int> _keyDictionary;

    public Authority()
    {
        _keyDictionary = new Dictionary<string, int>();
    }

    public int GetOwnership(string type)
    {
        int key = 0;

        if (_keyDictionary.ContainsKey(type))
            key = ++_keyDictionary[type];
        else
            _keyDictionary.Add(type, key);

        return key;
    }

    public bool CheckOwnership(string type, int key)
    {
        if (!_keyDictionary.ContainsKey(type))
            return true;
        return _keyDictionary[type] == key;
    }
}