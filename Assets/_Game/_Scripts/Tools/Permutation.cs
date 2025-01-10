using System.Collections.Generic;
using UnityEngine;

public class Permutation
{
    public IReadOnlyList<int> List => _list;
    public int GetElement(int index) => _list[index];

    List<int> _list;

    public Permutation(int n)
    {
        _list = new List<int>();

        for (int i = 0; i < n; i++)
        {
            _list.Add(i);
        }
    }

    public Permutation(int from, int to)
    {
        _list = new List<int>();

        for (int i = from; i < to; i++)
            _list.Add(i);
    }

    public void Add(int from, int to)
    {
        for (int i = from; i < to; i++)
            _list.Add(i);
    }

    public void RemoveLowerThen(int max)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i] < max)
                _list.RemoveAt(i);
        }
    }

    public void Shuffle()
    {
        int m = _list.Count;
        int i;
        int temp;

        while (m > 0)
        {
            i = Random.Range(0, 10000) * m--;
            i %= _list.Count;

            temp = _list[m];
            _list[m] = _list[i];
            _list[i] = temp;
        }
    }

    public override string ToString()
    {
        string str = string.Empty;

        foreach (int el in _list)
        {
            str += el.ToString();
            str += " ";
        }

        return str;
    }
}
