using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class InteractableObj
{
    public string name;
    public string descr;
    public string AnimationPath;

    public InteractableObj(string _name, string _descr, string _animationPath)
    {
        this.name = _name;
        this.descr = _descr;
        this.AnimationPath = _animationPath;
    }
}
