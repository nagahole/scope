using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions 
{
    public static (int minutes, float seconds) ToMinutesAndSeconds(this float seconds) {
        int minutes = (int) (seconds / 60f);
        seconds -= minutes * 60;
        return (minutes, seconds);
    }

    public static string ToMinutesAndSecondsFormatted(this float seconds, int dp) {
        var s = seconds.ToMinutesAndSeconds();
        if(s.minutes > 0) {
            return string.Join(":", s.minutes, s.seconds.ToString("00." + new string('0', dp)));
        } else {
            return s.seconds.ToString("0." + new string('0', dp));
        }
    }
}
