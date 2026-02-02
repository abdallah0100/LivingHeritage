using System;
using System.Threading;
using UnityEngine;

public class Npc
{
    private Transform t;
    private bool busy;
    private bool walking;
    public Vector3 velocity;

    public Npc(Transform t) {
        this.t = t;
        this.busy = false;
        this.walking = false;
    }
    public bool isBusy() {
        return busy;
    }

    public Transform getTransform() {
        return this.t;
    }

    public void setBusy(bool value) {
        this.busy = value;
    }
    public bool isWalking() {
        return this.walking;
    }
    public void setWalking(bool value) {
        this.walking = value;
    }
}
