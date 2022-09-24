using Dapper;
using Microsoft.Data.Sqlite;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class PostOpenShiftQuery : IPostOpenShiftQuery
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public PostOpenShiftQuery(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
    
        public async void ExecuteAsync(Shift shift)
        {
            await using var sqlConnection = new SqliteConnection(_connectionStringProvider.GetConnectionString());

            try
            {
                var sqlString = string.Format(
                    "INSERT INTO Shift (EmployeeId, Start, End) VALUES (NULL, '{0:yyyy-MM-dd HH:mm:ss.fff}', '{1:yyyy-MM-dd HH:mm:ss.fff}');",
                    shift.Start, shift.End);
                await sqlConnection.ExecuteAsync(sqlString);
            }
            catch(SqliteException e)
            {
                throw e;
            }
        }
    }    
}

