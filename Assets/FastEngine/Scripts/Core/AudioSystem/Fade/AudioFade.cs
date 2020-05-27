namespace FastEngine.Core
{
    public delegate void AudioFadeCallback(AudioAsset audioAsset);

    /// <summary>
    /// 音频过度类型
    /// </summary>
    public enum AudioFadeType
    {
        FadeIn,
        FadeOut,
        FadeOut2In,
    }

    /// <summary>
    /// 音频过度状态
    /// </summary>
    public enum AudioFadeStatus
    {
        FadeIn,
        FadeOut,
        Delay,
        Complete,
    }

    /// <summary>
    /// 音频过度
    /// </summary>
    public class AudioFade
    {
        public AudioAsset audioAsset;
        public AudioFadeType fadeType;
        public AudioFadeStatus fadeStatus;
        public float fadeTime;
        public float delayTime;
        public float tempVolume;

        // 过度完成回调
        public AudioFadeCallback fadeCompleteCallback;

        // 用于过度类型为 FadeOut2In时，当fadeOut完成时回调
        public AudioFadeCallback fadeOutCompleteCallback;

        /// <summary>
        /// 初始化
        /// </summary>
        public void initialize()
        {
            if (fadeType == AudioFadeType.FadeIn) fadeStatus = AudioFadeStatus.FadeIn;
            else if (fadeType == AudioFadeType.FadeOut) fadeStatus = AudioFadeStatus.FadeOut;
            else if (fadeType == AudioFadeType.FadeOut2In) fadeStatus = AudioFadeStatus.FadeOut;

            tempVolume = audioAsset.volume;
        }
    }
}