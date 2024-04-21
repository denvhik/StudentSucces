using Microsoft.Data.SqlClient;
using Handling;
public class UserFriendlyException : Exception
{
    public UserFriendlyException() : base() { }
    public UserFriendlyException(Exception ex) : base(ex.Message, ex) { }
    public UserFriendlyException(string message) : base(message) { }

    public UserFriendlyException(string message, Exception innerException) : base(message, innerException) { }

    public static UserFriendlyException FromSqlException(SqlException ex)
    {

        return new UserFriendlyException(ex.Message, ex);
    }

    public static UserFriendlyException FromException(Exception ex)
    {
        return new UserFriendlyException(ex.Message, ex);
    }
}