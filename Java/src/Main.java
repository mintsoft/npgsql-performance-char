import java.sql.*;
import java.util.concurrent.TimeUnit;

import org.apache.commons.lang3.time.StopWatch;

public class Main {

    public static void main(String[] args) {
        try {
            String connectionString = args[0];

            Connection connection = DriverManager.getConnection(connectionString);

            //force connection
            Statement warmUp = connection.createStatement();
            ResultSet rs = warmUp.executeQuery("SELECT NULL;");
            while(rs.next()) {}
            rs.close();
            warmUp.close();

/* varCharQuery */
            PreparedStatement varCharStatement = connection.prepareStatement("SELECT * FROM fast_table WHERE name = ?");
            varCharStatement.setString(1, "rob");

            StopWatch varcharStopwatch = StopWatch.createStarted();

            ResultSet varCharResultSet = varCharStatement.executeQuery();
            while (varCharResultSet.next()) {}

            System.out.println("varcharCommmand :: " + varcharStopwatch.getTime(TimeUnit.MILLISECONDS));

            varCharResultSet.close();
            varCharStatement.close();

/* Slow query on C# */
            PreparedStatement charStatement = connection.prepareStatement("SELECT * FROM slow_table WHERE name = ?" );
            charStatement.setString(1, "rob");

            StopWatch charStopWatch = StopWatch.createStarted();

            ResultSet charResultSet = charStatement.executeQuery();
            while (charResultSet.next()) {}

            System.out.println("charCommand :: " + charStopWatch.getTime(TimeUnit.MILLISECONDS));

            charResultSet.close();
            charStatement.close();


            connection.close();
        }
        catch (Exception e )
        {
            System.out.println("-------------------------- Exception ¬_¬ ---------------------");
            System.out.print(e);
        }
    }
}
