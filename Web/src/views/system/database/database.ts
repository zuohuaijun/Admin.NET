export interface EditRecordRow {
	columnDescription?: string | null;
	dataType?: string | null;
	dbColumnName?: string | null;
	decimalDigits: number;
	isIdentity: number;
	isNullable: number;
	isPrimarykey: number;
	length: number;
	key?: number;
	editable?: boolean;
	isNew: boolean;
}

export const yesNoSelect = [
	{
		label: '是',
		value: 1,
	},
	{
		label: '否',
		value: 0,
	},
];

export const dataTypeList = [
	{
		value: 'text',
		hasLength: false,
		hasDecimalDigits: false,
	},
	{
		value: 'varchar',
		hasLength: true,
		hasDecimalDigits: false,
	},
	{
		value: 'nvarchar',
		hasLength: true,
		hasDecimalDigits: false,
	},
	{
		value: 'char',
		hasLength: true,
		hasDecimalDigits: false,
	},
	{
		value: 'nchar',
		hasLength: true,
		hasDecimalDigits: false,
	},
	{
		value: 'timestamp',
		hasLength: false,
		hasDecimalDigits: false,
	},
	{
		value: 'int',
		hasLength: false,
		hasDecimalDigits: false,
	},
	{
		value: 'smallint',
		hasLength: false,
		hasDecimalDigits: false,
	},
	{
		value: 'tinyint',
		hasLength: false,
		hasDecimalDigits: false,
	},
	{
		value: 'bigint',
		hasLength: false,
		hasDecimalDigits: false,
	},
	{
		value: 'bit',
		hasLength: false,
		hasDecimalDigits: false,
	},
	{
		value: 'decimal',
		hasLength: true,
		hasDecimalDigits: true,
	},
	{
		value: 'datetime',
		hasLength: false,
		hasDecimalDigits: false,
	},
	{
		value: 'date',
		hasLength: false,
		hasDecimalDigits: false,
	},
	{
		value: 'blob',
		hasLength: false,
		hasDecimalDigits: false,
	},
	{
		value: 'clob',
		hasLength: false,
		hasDecimalDigits: false,
	},
];
