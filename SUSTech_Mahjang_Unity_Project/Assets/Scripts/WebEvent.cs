

public class WebEvent
{
    public readonly string gameObject;
    public readonly string component;
    public readonly string function;
    public readonly object param;
    public readonly bool isCoroutine;

    public WebEvent(string gameObject, string component, string function, object param = null, bool isCoroutine = false)
    {
        this.gameObject = gameObject;
        this.component = component;
        this.function = function;
        this.param = param;
        this.isCoroutine = isCoroutine;
    }
}
