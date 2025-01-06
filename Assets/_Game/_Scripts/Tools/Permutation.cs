using UnityEngine;
using System.Collections.Generic;

public class Permutation
{
    private List<int> list;

    public IReadOnlyList<int> List => list;
    public int GetElement(int index) => list[index];

    public Permutation(int n)
    {
        list = new List<int>();

        for(int i=0; i<n; i++)
        {
            list.Add(i);
        }
    }

    public Permutation(int from, int to)
    {
        list = new List<int>();

        for (int i = from; i < to; i++)
            list.Add(i);
    }

    public void Add(int from, int to)
    {
        for (int i = from; i < to; i++)
            list.Add(i);
    }

    public void RemoveLowerThen(int max)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i] < max)
                list.RemoveAt(i);
        }
    }

    public void Shuffle()
    {
        int m = list.Count;
        int i;
        int temp;

        while (m > 0)
        {
            i = Random.Range(0, 10000) * m--;
            i %= list.Count;

            temp = list[m];
            list[m] = list[i];
            list[i] = temp;
        }
    }

    public override string ToString()
    {
        string str = string.Empty;

        foreach(int el in list)
        {
            str += el.ToString();
            str += " ";
        }

        return str;
    }
}
