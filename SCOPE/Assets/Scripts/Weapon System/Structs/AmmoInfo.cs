using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class AmmoInfo
{
    public abstract string MainText();
    public abstract string SecondaryText();
    public abstract string TertiaryText();
}

public class ClipBasedAmmoInfo : AmmoInfo {
    private (int remaining, int size) clip;
    private int reserve;

    public ClipBasedAmmoInfo() { }

    public void SetAmmoInfo((int remaining, int size) clip, int reserve) {
        this.clip = clip;
        this.reserve = reserve;
    }

    public void SetAmmoInfo(int clipRemaining, int clipSize, int reserve) {
        this.clip.remaining = clipRemaining;
        this.clip.size = clipSize;
        this.reserve = reserve;
    }

    public override string MainText() {
        return clip.remaining.ToString();
    }

    public override string SecondaryText() {
        return reserve.ToString();
    }

    public override string TertiaryText() {
        return string.Empty;
    }
}
