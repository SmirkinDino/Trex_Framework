using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SmirkinDino 2017.09.05
/// </summary>
public class DAudioManager : MonoBehaviour
{
    #region SINGLETON
    protected static DAudioManager _managerInstance = null;
    protected static Transform _root = null;
    public static DAudioManager Instance
    {
        get
        {
            if (_root == null || _managerInstance == null)
            {
                _managerInstance = null;
                _root = new GameObject("Dino_Core-" + typeof(DAudioManager).ToString()).transform;
                _managerInstance = _root.gameObject.AddComponent<DAudioManager>();

                DontDestroyOnLoad(_root.gameObject);
            }

            if (_root == null || _managerInstance == null)
            {
#if DINO_DEBUG
                //DScreemLogger.Instance.LogToScreen("Instance Error");
#endif
            }
            return _managerInstance;
        }
    }
    protected DAudioManager() { }
    #endregion

    protected static readonly string ASSETBUNDLE_PATH = Application.streamingAssetsPath + "/..";

    protected static readonly int MAX_AUDIOSOURCES_COUNT = 5;

    /// <summary>
    /// the router, store all audio clips here
    /// </summary>
    protected Dictionary<string, AudioClip> _theRouter;

    /// <summary>
    /// ready audiosource queue, all ready audiosources are stored here
    /// </summary>
    protected Queue<DAudio> _readyQueue;

    /// <summary>
    /// current playing clips group
    /// </summary>
    protected List<DAudio> _mixedGroup;

    /// <summary>
    /// currentAudioListener
    /// </summary>
    protected AudioListener _registeredAudioListener;

    /// <summary>
    /// this will be the listener if registered listener is null
    /// </summary>
    protected AudioListener _selfAudioListener;

    /// <summary>
    /// assetbundle load request entity
    /// </summary>
    protected AssetBundleCreateRequest _soundTrackBundle;

    protected AssetBundleRequest _loadRequest;

    protected bool _loadFinsh = false;

    /// <summary>
    /// load progress of the resources
    /// </summary>
    public float loadProgress { get { return _soundTrackBundle.progress; } }

    /// <summary>
    /// load finish ?
    /// </summary>
    public bool loadFinish { get { return _loadFinsh; } }

    private void Update()
    {
        if (!loadFinish)
        {
            return;
        }

        UpdateListener();

        for (int i = 0; i < _mixedGroup.Count; i++)
        {
            if(!_mixedGroup[i].Tick())
            {
                _mixedGroup[i].Dispose();
                _readyQueue.Enqueue(_mixedGroup[i]);
                _mixedGroup.RemoveAt(i);
            }
        }
    }

    protected void UpdateListener()
    {
        if (_registeredAudioListener != null)
        {
            _root.position = _registeredAudioListener.transform.position;
        }
    }

    /// <summary>
    /// init router and load sources
    /// </summary>
    protected void InitRouterAndLoad()
    {
        if (_theRouter == null)
        {
            _theRouter = new Dictionary<string, AudioClip>();
        }
        else
        {
            _theRouter.Clear();
        }

        StartCoroutine(_excuteAssetBundleLoadingAsync());
    }
    protected IEnumerator _excuteAssetBundleLoadingAsync()
    {
        try
        {
            _soundTrackBundle = AssetBundle.LoadFromFileAsync(ASSETBUNDLE_PATH);
        }
        catch (Exception)
        {
#if DINO_DEBUG
            DScreemLogger.Instance.LogToScreen("Load sound AssetBundle Error");
#endif
            yield break;
        }

        yield return _soundTrackBundle;

        if (_soundTrackBundle.assetBundle == null)
        {
#if DINO_DEBUG
            DScreemLogger.Instance.LogToScreen("Load AssetBundle failed or none assets in bundle");
#endif
            yield break;
        }

        string[] _assetNames = _soundTrackBundle.assetBundle.GetAllAssetNames();

        for (int i = 0; i < _assetNames.Length; i++)
        {
            _loadRequest = _soundTrackBundle.assetBundle.LoadAssetAsync(_assetNames[i]);

            yield return _loadRequest;

            if (_loadRequest == null || _loadRequest.asset)
            {
#if DINO_DEBUG
                DScreemLogger.Instance.LogToScreen("Load AssetBundle failed or none assets in bundle");
#endif
                yield break;
            }

            AudioClip _audioClip = _loadRequest.asset as AudioClip;

            if (_audioClip == null)
            {
#if DINO_DEBUG
                DScreemLogger.Instance.LogToScreen("assert audio clip failed");
#endif
                yield break;
            }

            if (_theRouter.ContainsKey(_audioClip.name))
            {
#if DINO_DEBUG
                DScreemLogger.Instance.LogToScreen("already had clip:" + _audioClip.name);
#endif
                continue;
            }

            _theRouter.Add(_audioClip.name, _audioClip);
        }

        _loadFinsh = true;
    }

    /// <summary>
    /// init ready queue
    /// </summary>
    protected void InitReadyQueue()
    {
        if (_readyQueue != null)
        {
            _readyQueue.Clear();
        }
        else
        {
            _readyQueue = new Queue<DAudio>();
        }

        for (int i = 0; i < MAX_AUDIOSOURCES_COUNT; i++)
        {
            GameObject _obj = new GameObject("DAudioMixer-" + (i + 1));

            _obj.transform.SetParent(_root);
            _obj.transform.position = Vector3.zero;
            _obj.transform.rotation = Quaternion.identity;

            _readyQueue.Enqueue(DAudio.CreateAudio(_obj.AddComponent<AudioSource>()));
        }
    }

    /// <summary>
    /// init mixed sources group
    /// </summary>
    protected void InitMixedGroup()
    {
        if (_mixedGroup == null)
        {
            _mixedGroup = new List<DAudio>();
        }
        else
        {
            _mixedGroup.Clear();
        }
    }

    /// <summary>
    /// init audio listener
    /// </summary>
    protected void InitListener()
    {
        _selfAudioListener = gameObject.AddComponent<AudioListener>();
    }

    /// <summary>
    /// find audio in mixed group
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    protected DAudio FindSoundInMixedGroup(string _name)
    {
        for (int i = 0; i < _mixedGroup.Count; i++)
        {
            if (_mixedGroup[i].clip.name == _name)
            {
                return _mixedGroup[i];
            }
        }
        return null;
    }

    public void Init()
    {
        _loadFinsh = false;

        InitRouterAndLoad();

        InitReadyQueue();

        InitListener();

        InitMixedGroup();
    }
    public void Dispose()
    {
        _soundTrackBundle.assetBundle.Unload(true);
        _managerInstance = null;
        _root = null;
        Destroy(gameObject);
    }

    /// <summary>
    /// Register current AudioListener
    /// </summary>
    /// <param name="_listener"></param>
    public void RegisterAudioListener(AudioListener _listener)
    {
        _registeredAudioListener = _listener;
        _selfAudioListener.enabled = false;
    }

    /// <summary>
    /// unregister current AudioListener
    /// </summary>
    /// <param name="_listener"></param>
    public void UnregisterAudioListener()
    {
        _registeredAudioListener = null;
        _selfAudioListener.enabled = true;
    }
    
    public void PlaySound(string _name, float _volume = DAudio.MAX_VOLUME)
    {
        PlaySound(_name, _volume, DAudio.NO_DURING, DAudio.NO_DURING);
    }
    public void PlaySound(string _name, float _volume = DAudio.MAX_VOLUME, float _during = DAudio.NO_DURING)
    {
        PlaySound(_name, _volume, _during, DAudio.NO_DURING);
    }
    /// <summary>
    /// Play sound, first search key in mixed group, if not found, dequeue from ready queue
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_volume"></param>
    /// <param name="_during"></param>
    /// <param name="_delay"></param>
    public void PlaySound(string _name, float _volume = DAudio.MAX_VOLUME, float _during = DAudio.NO_DURING, float _delay = DAudio.NO_DELAY)
    {
        // if in mixed group, handle audio in mixed group
        DAudio _dAudio = FindSoundInMixedGroup(_name);

        if (_dAudio == null)
        {
            _dAudio = _readyQueue.Dequeue();
        }

        if (_dAudio == null)
        {
#if DINO_DEBUG
            DScreemLogger.Instance.LogToScreen("there is no AudioSource Availible");
#endif
            return;
        }

        _dAudio.ExcuteOperation(_volume, _during, _delay);
    }

    public void StopSound(string _name, float _during = DAudio.NO_DURING)
    {
        StopSound(_name, _during, DAudio.NO_DELAY);
    }
    /// <summary>
    /// Stop sound, first search key in mixed group, if found, handle sound and remove from mixed group and enqueue ready queue
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_during"></param>
    /// <param name="_delay"></param>
    public void StopSound(string _name, float _during = DAudio.NO_DURING, float _delay = DAudio.NO_DELAY)
    {
        // if in mixed group, handle audio in mixed group
        DAudio _dAudio = FindSoundInMixedGroup(_name);

        if (_dAudio == null)
        {
#if DINO_DEBUG
            DScreemLogger.Instance.LogToScreen("the name " + _name + "is not currently playing");
#endif
            return;
        }

        _dAudio.ExcuteOperation(DAudio.MIN_VOLUME, _during, _delay, false);
    }
}

public class DAudio
{
    public const float MAX_VOLUME = 1.0f;
    public const float MIN_VOLUME = 0.0f;
    public const float NO_DELAY = 0;
    public const float NO_DURING = 0;
    public const float NO_STEP = 0;

    public float volume { get { return _volume; } private set { _volume = Mathf.Clamp(value, MIN_VOLUME, MAX_VOLUME); if(source != null) source.volume = value; } }
    public AudioSource source { get; private set; }
    public AudioClip clip { get; private set; }

    private bool _valid = true;
    private bool _isOperating = false;
    private bool _validOnOperationFinish = true;

    private float _volume = MIN_VOLUME;
    private float _operationEndTime = float.NegativeInfinity;
    private float _operationTo = MIN_VOLUME;
    private float _operationStep = NO_STEP;
    private float _operationDelay = NO_DELAY;
    private float _operationDuring = NO_DURING;

    public static DAudio CreateAudio(AudioSource _source)
    {
        return new DAudio(_source);
    }

    public DAudio(AudioSource _source)
    {
        source = _source;
    }

    public void SetAudio(AudioClip _clip)
    {
        clip = _clip;

        if (source != null)
        {
            source.clip = _clip;
        }
    }

    public void ExcuteOperation(float _to, bool _validOnOperationFinish = true)
    {
        ExcuteOperation(_to, NO_DURING, NO_DELAY, _validOnOperationFinish);
    }

    public void ExcuteOperation(float _to, float _during, bool _validOnOperationFinish = true)
    {
        ExcuteOperation(_to, _during, NO_DELAY, _validOnOperationFinish);
    }

    public void ExcuteOperation(float _to, float _during, float _delay, bool _validOnOperationFinish = true)
    {
        _operationEndTime = Time.time + _during;
        _operationTo = _to;
        _operationStep = _to - volume;
        _operationDelay = _delay;

        _operationDuring = _during;

        this._validOnOperationFinish = _validOnOperationFinish;
        _isOperating = true;
    }

    public bool Tick()
    {
        if (_isOperating)
        {
            _isOperating = _excuteOperationLoop();
        }
        return _valid;
    }

    public void Dispose()
    {
        clip = null;

        source.clip = null;

        volume = MIN_VOLUME;

        _validOnOperationFinish = true;

        _valid = true;

        _isOperating = false;
    }

    private bool _excuteOperationLoop()
    {
        if (Time.time > _operationEndTime)
        {
            _operationEndTime = float.NegativeInfinity;
            _operationDuring = NO_DURING;

            volume = _operationTo;

            _valid = _validOnOperationFinish;

            return false;
        }

        if (_operationDuring != 0)
        {
            volume += _operationStep / _operationDuring * Time.deltaTime;
        }

        return true;
    }
}
