using DbUp;
using DbUp.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbUp.Engine;
using System.Text;

public class Program
{
	private const string JournalSchemaName = "dbup";

	public static int Main(string[] args)
	{
		var configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.Build();

		var connectionString = configuration.GetConnectionString("defaultConnection");

		EnsureDatabase.For.PostgresqlDatabase(connectionString);

		// Workaround to manually create the Journal Schema to store the migration control table.
		var createJournalSchema = DeployChanges
			.To
			.PostgresqlDatabase(connectionString)
			.JournalTo(new NullJournal())
			.WithScript("CreateJournalSchema", $"CREATE SCHEMA IF NOT EXISTS {JournalSchemaName}")
			.LogToConsole()
			.LogScriptOutput()
			.Build();

		var journalSchemaResult = createJournalSchema.PerformUpgrade();
		if (journalSchemaResult.Successful == false)
		{
			Console.WriteLine($"Error creating journal schema '{JournalSchemaName}'.");
			Console.WriteLine($"Error: {journalSchemaResult.Error}");
			return -2;
		}

		UpgradeEngine upgradeEngine = DeployChanges.To
			.PostgresqlDatabase(connectionString)
			.JournalToPostgresqlTable(JournalSchemaName, "schema_versions")
			.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
			//.WithVariables(userPasswords)
			.WithTransactionPerScript()
			.LogToConsole()
			.LogScriptOutput()
			.Build();

		IEnumerable<SqlScript> pendingScriptList = upgradeEngine.GetScriptsToExecute();
		if (pendingScriptList.Any() == false)
		{
			Console.WriteLine("\nThere are no pending scripts to execute\n");
			return -3;
		}

		PrintReport(pendingScriptList);

		DatabaseUpgradeResult result = upgradeEngine.PerformUpgrade();
		if (result.Successful == false)
		{
			Console.WriteLine("\nError applying update in database.\n");
			return -4;
		}

		Console.WriteLine("\nScripts successfully applied.");

		return 0;
	}

	private static void PrintReport(IEnumerable<SqlScript> pendingScriptList)
	{
		Console.WriteLine();

		foreach (SqlScript sqlScript in pendingScriptList)
		{
			string header = BuildReportHeader(sqlScript.Name);

			Console.WriteLine(header);
			Console.WriteLine();
			Console.WriteLine(sqlScript.Contents);
			Console.WriteLine();
		}
	}

	private static string BuildReportHeader(string scriptName)
	{
		var headerMain = $"----- {scriptName} -----";

		var headerStringBuilder = new StringBuilder();
		headerStringBuilder.AppendLine(new string('-', headerMain.Length));
		headerStringBuilder.AppendLine(headerMain);
		headerStringBuilder.AppendLine(new string('-', headerMain.Length));

		return headerStringBuilder.ToString();
	}
}
