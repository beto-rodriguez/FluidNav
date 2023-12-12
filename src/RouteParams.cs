namespace FluidNav;

public class RouteParams
{
    private readonly Dictionary<string, string> _params = [];

    /// <summary>
    /// Gets the value of the specified query parameter.
    /// </summary>
    /// <param name="parameterName">The parameter name</param>
    /// <returns></returns>
    public string this[string parameterName] => _params[parameterName];

    internal void SetParams(string queryParams)
    {
        var pairs = queryParams.Split('&');
        foreach (var pair in pairs)
        {
            var keyValue = pair.Split('=');
            _params[keyValue[0]] = keyValue[1];
        }
    }

    internal void Clear()
    {
        _params.Clear();
    }
}
