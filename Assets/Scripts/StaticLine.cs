using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StaticLine : MonoBehaviour {

    public abstract List<Vector2> GetPath();

    //public abstract void GetHealth();

    //public abstract void Damage(Vector2 _location, float _amount);
}
