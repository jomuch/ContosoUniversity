using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContosoUniversity.Migrations
{
    public partial class AddRowVersionToDepartment : Migration
    {
        // This method is used for applying changes (e.g., adding a column) to the database
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the 'RowVersion' column to the 'Department' table
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",          // The name of the new column
                table: "Department",        // The table we're adding it to
                type: "rowversion",         // The type of the column (SQL Server rowversion)
                rowVersion: true,           // This makes it a 'timestamp' (rowversion) column
                nullable: false);          // This column should not be nullable

            // Optionally, you could also add more migration steps here, such as altering other tables or columns
        }

        // This method is used to undo changes made in the 'Up' method (e.g., drop a column)
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the 'RowVersion' column from the 'Department' table (undoes the Up method)
            migrationBuilder.DropColumn(
                name: "RowVersion",         // The name of the column to drop
                table: "Department");       // The table from which we are dropping the column

            // Optionally, you could add more undo steps here, such as restoring altered columns or tables
        }
    }
}
