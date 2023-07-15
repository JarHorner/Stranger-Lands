using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//My own created Class that operated like a regular List, except the Key and Value of each pair can be changed.
public class MutableKeyValPair<Key, Value>
{
    public Key key { get; set; }
    public Value value { get; set; }

    public MutableKeyValPair() { }

    public MutableKeyValPair(Key key, Value value)
    {
        this.key = key;
        this.value = value;
    }
}
