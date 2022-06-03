using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class TreeNode<T>
{
    private T data;
    public LinkedList<TreeNode<T>> children;

    public TreeNode(T data)
    {
        this.data = data;
        children = new LinkedList<TreeNode<T>>();
        
    }

    public TreeNode<T> Parent { get; private set; }

    public T value { get { return data; } }

    public void AddChild(T data)
    {
        children.AddLast(new TreeNode<T>(data));
    }

    public TreeNode<T> GetChild(int i)
    {
        foreach (TreeNode<T> n in children)
            if (--i == 0)
                return n;
        return null;
    }

    public void Traverse(Action<T> action){

        action(value);
        foreach (var child in children)
            child.Traverse(action);

    }

    public IEnumerable<T> Flatten(){

        return new[] {value}.Concat(children.SelectMany(x => x.Flatten()));

    }
}

delegate void TreeVisitor<T>(T nodeData);