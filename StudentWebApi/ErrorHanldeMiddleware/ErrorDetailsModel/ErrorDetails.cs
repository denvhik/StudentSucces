using Newtonsoft.Json;

namespace StudentWebApi.ErrorHanldeMiddleware.ErrorDetailsModel;

public class ErrorMessageLoader
{
    private readonly IConfiguration _configuration;
    private Dictionary<string, ErrorMessage> _errorMessages;

    public ErrorMessageLoader(IConfiguration configuration)
    {
        _configuration = configuration;
        LoadErrorMessages();
    }

    private void LoadErrorMessages()
    {
        var jsonFilePath = _configuration.GetValue<string>("ErrorMessagesPath");
        var json = File.ReadAllText(jsonFilePath);
        _errorMessages = JsonConvert.DeserializeObject<Dictionary<string, ErrorMessage>>(json);
    }

    public ErrorMessage GetErrorMessage(string key)
    {
        if (_errorMessages.ContainsKey(key))
            return _errorMessages[key];

        return null; // or throw an exception if preferred
    }
}

public class ErrorMessage
{
    public int Status { get; set; }
    public string Error { get; set; }
    public string Message { get; set; }
}
