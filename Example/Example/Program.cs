using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = args[0];

            //force connection
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT NULL;", connection);
                using (var reader = command.ExecuteReader()) while(reader.Read()) {}
              
#region varCharQuery
                var varcharStopwatch = Stopwatch.StartNew();

                var varcharCommmand = new NpgsqlCommand("SELECT * FROM fast_table WHERE name = @name;", connection);
                varcharCommmand.Parameters.AddWithValue("@name", "rob");

                using (var reader = varcharCommmand.ExecuteReader()) while(reader.Read()) {}

                Console.WriteLine("varcharCommmand :: {0}", varcharStopwatch.Elapsed.TotalMilliseconds);
#endregion

#region SlowQuery
                var charStopwatch = Stopwatch.StartNew();

                var charCommand = new NpgsqlCommand("SELECT * FROM slow_table WHERE name = @name ;", connection);
                charCommand.Parameters.AddWithValue("@name", "rob");

                using (var reader = charCommand.ExecuteReader()) while (reader.Read()) {}

                Console.WriteLine("charCommand :: {0}", charStopwatch.Elapsed.TotalMilliseconds);
#endregion

#region SlowQueryWithExplicitType
                var slowStopwatchWithType = Stopwatch.StartNew();

                var slowWithTypeCommand = new NpgsqlCommand("SELECT * FROM slow_table WHERE name = @name ;", connection);
                slowWithTypeCommand.Parameters.AddWithValue("@name", NpgsqlDbType.Char, "rob");

                using (var reader = slowWithTypeCommand.ExecuteReader()) while (reader.Read()) {}

                Console.WriteLine("slowWithTypeCommand :: {0}", slowStopwatchWithType.Elapsed.TotalMilliseconds);
#endregion
            }
        }
    }
}
