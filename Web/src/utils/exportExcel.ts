import XLSXS from 'xlsx-js-style';
/**
* @description:
* @param {Object} json 服务端发过来的数据
* @param {String} name 导出Excel文件名字

* @param {String} titleArr 导出Excel表头

* @param {String} sheetName 导出sheetName名字
* @return:
**/
export function exportExcel(jsonarr: Array<EmptyObjectType>, name: string, header: Array<EmptyObjectType>, sheetName: string) {
	var data = new Array();
	var wpxArr = new Array(); //列宽度
	const borderStyle = {
		//边框样式
		top: {
			style: 'thin',
			color: {
				rgb: '000000',
			},
		},
		bottom: {
			style: 'thin',
			color: {
				rgb: '000000',
			},
		},
		left: {
			style: 'thin',
			color: {
				rgb: '000000',
			},
		},
		right: {
			style: 'thin',
			color: {
				rgb: '000000',
			},
		},
	};
	var headrow = new Array();
	header.forEach((item) => {
		let width = 200;
		if (item.width && !isNaN(item.width)) {
			width = parseInt(item.width) * 0.7;
		}
		wpxArr.push({ wpx: width });
		headrow.push({
			v: item.label,
			t: 's',
			s: {
				font: { bold: true },
				alignment: { wrapText: true, horizontal: item.headerAlign ? item.headerAlign : item.align ? item.align : '', vertical: 'center' },
				border: borderStyle,
			},
		});
	});
	data.push(headrow); //写入标题
	jsonarr.forEach((json) => {
		var row = new Array();
		header.forEach((item) => {
			if (json.hasOwnProperty(item.prop)) {
				let val = '';
				if (json[item.prop] != null) val = json[item.prop];
				row.push({
					v: val,
					t: 's',
					s: {
						alignment: { wrapText: true, horizontal: item.align ? item.align : '', vertical: 'center' },
						border: borderStyle,
					},
				});
			}
		});
		data.push(row);
	});
	const ws = XLSXS.utils.aoa_to_sheet(data);
	const wb = XLSXS.utils.book_new();
	ws['!cols'] = wpxArr;
	XLSXS.utils.book_append_sheet(wb, ws, sheetName);
	/* generate file and send to client */
	XLSXS.writeFile(wb, name + '.xlsx');
}
