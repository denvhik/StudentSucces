using Microsoft.Data.SqlClient;
using DAL.SystemExeptionHandling;
public class UserFriendlyException : Exception
{
    public UserFriendlyException() : base() { }

    public UserFriendlyException(string message) : base(message) { }

    public UserFriendlyException(string message, Exception innerException) : base(message, innerException) { }

    public static UserFriendlyException FromSqlException(SqlException ex)
    {
        string errorMessage = "Error SQL:";

        errorMessage += $" Error Code: {ex.Number}, Message: {ex.Message}";

        return new UserFriendlyException(errorMessage, ex);
    }

    public static SystemExeptionHandle FromException(Exception ex)
    {
        return SystemExeptionHandle.FromSystemException(ex);
    }
}