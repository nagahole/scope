using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteClip : Clip {
    public override bool IsFull() {
        return true;
    }

    public override int Remaining() {
        return 999;
    }

    public override int Size() {
        return 999;
    }

    public override bool TryAddAmmo(int n) {
        return false;
    }

    public override int TryFillAmmo(int ammoInReserve) {
        return 0;
    }

    public override bool TryUseAmmo(int n) {
        return true;
    }
}
