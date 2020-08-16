using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using MEC;

/// <summary>
/// A reload service that may be used by the AmmoService. Is separated because some weapons may not need to reload
/// </summary>
public interface IReloadService {

    /// <summary>
    /// Called once reload is initiated. Should be a field-property pair with a private setter
    /// </summary>
    UnityEvent onStartReload { get; }

    /// <summary>
    /// Called once reload is finished. Should be a field-property pair with a private setter
    /// </summary>
    UnityEvent onFinishReload { get; }

    /// <summary>
    /// Reloads an ammo container with a given amount of ammo left
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="callback">Calls callback after reload is success, with ammo used to refill as its parameter</param>
    void Reload(IClip clip, System.Func<int> getReserveAmmo, System.Action<int> callback = null);

    /// <summary>
    /// Returns bool on whether is reloading or not 
    /// </summary>
    /// <returns></returns>
    bool IsReloading(); //May need to change this because something like pump-action shotguns are able to be interrupted in their reloads

    void CancelReload();
}

public abstract class ReloadService : MonoBehaviour, IReloadService {

    [SerializeField] private UnityEvent _onStartReload;
    [SerializeField] private UnityEvent _onFinishReload;

    private bool isReloading;

    private CoroutineHandle reloadCoroutine;
    private CoroutineHandle nestedReloadCoroutine;

    public UnityEvent onStartReload {
        get {
            return _onStartReload;
        }
        private set {
            _onStartReload = value;
        }
    }

    public UnityEvent onFinishReload {
        get {
            return _onFinishReload;
        }
        private set {
            _onFinishReload = value;
        }
    }

    public virtual bool IsReloading() {
        return isReloading;
    }

    public virtual void Reload(IClip clip, System.Func<int> getReserveAmmo, System.Action<int> callback = null) {
        if (getReserveAmmo() <= 0 || IsReloading()) {
            return;
        }
        reloadCoroutine = Timing.RunCoroutine(_HandleReload(clip, getReserveAmmo, callback));
    }

    public virtual void CancelReload() {
        if (IsReloading()) {
            Timing.KillCoroutines(reloadCoroutine);
            Timing.KillCoroutines(nestedReloadCoroutine);
            isReloading = false;
        }
    }

    private IEnumerator<float> _HandleReload(IClip clip, System.Func<int> getReserveAmmo, System.Action<int> callback = null) {
        Debug.Log("RELOADING...");
        onStartReload.Invoke();
        isReloading = true;

        yield return Timing.WaitUntilDone(
            (nestedReloadCoroutine = Timing.RunCoroutine(_Reload(clip, getReserveAmmo, callback)))
        );

        Debug.Log("FINISHED RELOADING");
        onFinishReload.Invoke();
        isReloading = false;
    }

    protected abstract IEnumerator<float> _Reload(IClip clip, System.Func<int> getReserveAmmo, System.Action<int> callback = null);
}