using UnityEngine;

public class RuntimeInitializer
{
    [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
    private static void InitializeBeforeSceneLoad()
    {
        // ゲーム中に常に存在するオブジェクトを読み込み、およびシーンの変更時にも破棄されないようにする。
        var audioManager = GameObject.Instantiate( Resources.Load( "AudioManager" ) );
        GameObject.DontDestroyOnLoad( audioManager );
    }

} // class RuntimeInitializer
