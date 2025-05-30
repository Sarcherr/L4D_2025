/// <summary>
/// 泛型单例类，使用时继承该类，然后使用Instance属性获取单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : Singleton<T>, new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.Init();
            }
            return _instance;
        }
    }

    protected virtual void Init()
    {

    }
}
