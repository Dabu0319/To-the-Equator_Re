using UnityEngine;
/// <summary>
/// 网上抄的一个很健壮的unity单例基类
/// https://blog.csdn.net/ycl295644/article/details/49487361
/// 1实现泛型，使用时public class Manager : UnitySingleton<Manager>
/// 2首先捕获场景中的目标单例，如果没有，就创建隐藏object，承载目标单例
/// 3切换场景不会删除
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnitySingleton<T> : MonoBehaviour 
    where T:Component {
    private static T _instance;
    public static T Instance        {
        get{
            if(_instance == null){
                _instance = FindObjectOfType(typeof(T)) as T;
                if(_instance == null){
                    GameObject obj = new GameObject ();
                    //obj.hideFlags = HideFlags.DontSave;
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance =(T) obj.AddComponent(typeof(T));
                }
            }
            return _instance;
        }
    }
    public virtual void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        if(_instance == null){
            _instance = this as T;
        }
        else{
            Destroy(gameObject);
        }
    }
}