using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : BaseManager<MusicManager> {
    //唯一的背景音乐组件
    private AudioSource bgMusic = null;
    //音乐大小
    private float bgValue = 1f;

    //音效依附对象
    public GameObject soundObj = null;
    //音效列表
    private List<AudioSource> soundList = new List<AudioSource>();
    //音效大小
    private float soundValue = 1f;

    protected MusicManager() {
        MonoManager.GetInstance().AddUpdateListener(MyUpdate);
    }
    void MyUpdate() {
        for (int i = soundList.Count - 1; i >= 0; i--) {
            if (!soundList[i].isPlaying) {

                GameObject.Destroy(soundList[i].gameObject);
                soundList.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBackgroundMusic(string name) {
        if (bgMusic == null) {
            GameObject obj = new GameObject();
            obj.name = "BackgroundMusic";
            bgMusic = obj.AddComponent<AudioSource>();
        }

        //异步加载背景音乐 加载完成后播放
        ResourceManager.GetInstance().LoadAsync<AudioClip>("Music/BackGround/" + name, (clip) => {
            bgMusic.clip = clip;
            bgMusic.loop = true;
            bgMusic.volume = bgValue;
            bgMusic.Play();
        });
    }
    /// <summary>
    /// 改变背景音乐音量大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeBackgroundMusicValue(float value) {
        bgValue = value;
        if (bgMusic == null)
            return;
        bgMusic.volume = bgValue;
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBackgroundMusic() {
        if (bgMusic == null)
            return;
        bgMusic.Pause();
    }
    /// <summary>
    /// 停止播放背景音乐
    /// </summary>
    public void StopBackgroundMusic() {
        if (bgMusic == null)
            return;
        bgMusic.Stop();
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callback = null) {
        if (soundObj == null) {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }

        //当音效异步加载结束后 再添加一个音效
        ResourceManager.GetInstance().LoadAsync<AudioClip>("Music/Sound/" + name, (clip) => {

            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();
            soundList.Add(source);
            callback?.Invoke(source);
        });
    }
    /// <summary>
    /// 改变所有音效音量大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundValue(float value) {
        soundValue = value;
        for (int i = 0; i < soundList.Count; i++) {
            soundList[i].volume = soundValue;
        }
    }
    /// <summary>
    /// 停止音效
    /// </summary>
    public void StopSound(AudioSource audioSource) {
        if (soundList.Contains(audioSource)) {
            soundList.Remove(audioSource);
            audioSource.Stop();
            GameObject.Destroy(audioSource);
        }
    }
}
