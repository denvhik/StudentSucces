using Microsoft.Data.SqlClient;

public static class ExceptionTranslator
{
    public static Exception Translate(Exception ex)
    {
        if (ex is SqlException sqlEx)
        {
            return UserFriendlyException.FromSqlException(sqlEx);
        }
        else
        {
            return UserFriendlyException.FromException(ex);
        }
    }
}