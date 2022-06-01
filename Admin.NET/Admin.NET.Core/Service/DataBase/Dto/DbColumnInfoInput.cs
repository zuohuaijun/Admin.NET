namespace Admin.NET.Core.Service;

public class DbColumnInfoInput
{
    public string TableName { get; set; }

    public string DbColumnName { get; set; }

    public string DataType { get; set; }

    public int Length { get; set; }

    public string ColumnDescription { get; set; }

    public int IsNullable { get; set; }

    public int IsIdentity { get; set; }

    public int IsPrimarykey { get; set; }

    public int DecimalDigits { get; set; }
}