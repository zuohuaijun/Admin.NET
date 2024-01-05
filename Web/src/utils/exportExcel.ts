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
	let headerDepth = getMaxDepth(header);
	let headerColumns = getTotalColumns(header);

	//创建表头二维数组
	let headerArr = new Array(headerDepth);
	for (let i = 0; i < headerArr.length; i++) {
		headerArr[i] = new Array(headerColumns);
	}
	//计算列索引
	let colIndex = 0;
	for (let i = 0; i < header.length; i++) {
		let col = header[i];

		//获取列对应的长度
		let colNum = getTotalColumns([col]);
		let colDepth = getMaxDepth([col]);
		for (let y = 0; y < colNum; y++) {
			colIndex = colIndex + y;
			for (let z = 0; z < colDepth; z++) {
				headerRec(z, colIndex, y, col, headerArr);
				// headerArr[z][colIndex]=col;
			}
		}
		colIndex++;
	}

	//填充表头列为空的列，为空的列要和本列上一行保持一致，通过一致的单元格，来合并单元格
	for (let i = 0; i < headerArr.length; i++) {
		let row = headerArr[i];
		for (let j = 0; j < row.length; j++) {
			if (headerArr[i][j] == null) {
				headerArr[i][j] = headerArr[i - 1][j];
			}
		}
	}

	//递归header
	function headerRec(rowindex: number, colindex: number, childrenindex: number, col: any, arr: any) {
		if (rowindex > 0) {
			if (col.children) {
				headerRec(rowindex, colIndex, childrenindex, col.children, arr);
			} else {
				arr[rowindex][colindex] = col[childrenindex];
			}
		} else {
			arr[rowindex][colindex] = col;
		}
	}

	for (let i = 0; i < headerArr.length; i++) {
		var headrow = new Array();
		let ha = headerArr[i];
		for (let j = 0; j < ha.length; j++) {
			let item = ha[j];
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
		}
		data.push(headrow); //写入标题
	}

	// 计算合并单元格信息
	var mergedCells = [];

	var mergedflg = false;
	var mergedcell = { s: { r: 0, c: 0 }, e: { r: 0, c: 0 } };

	//列合并
	for (let i = 0; i < headerColumns; i++) {
		let rowcol = headerArr[0][i];
		for (let j = 0; j < headerDepth; j++) {
			let col = headerArr[j][i];
			if (col == rowcol && mergedflg == false) {
				mergedflg = true;
				mergedcell.s = { r: j, c: i };
			} else if (col != rowcol && mergedflg == true) {
				mergedcell.e = { r: j - 1, c: i };

				if (mergedcell.s.r != mergedcell.e.r) {
					mergedCells.push(JSON.parse(JSON.stringify(mergedcell)));
					mergedflg = false;
					mergedcell = { s: { r: 0, c: 0 }, e: { r: 0, c: 0 } };
				} else if (j == headerDepth - 1 && mergedflg == true) {
					mergedcell.e = { r: j, c: i };
					if (mergedcell.s.r != mergedcell.e.r && col == rowcol) {
						mergedCells.push(JSON.parse(JSON.stringify(mergedcell)));
					}
					mergedflg = false;
					mergedcell = { s: { r: 0, c: 0 }, e: { r: 0, c: 0 } };
				} else {
					rowcol = col;
					mergedflg = true;
					mergedcell.s = { r: j, c: i };
				}
				// rowcol=col;
				// mergedflg=false;
				// mergedcell={s:{r:0,c:0},e:{r:0,c:0}}
			} else if (j == headerDepth - 1 && mergedflg == true) {
				mergedcell.e = { r: j, c: i };
				if (mergedcell.s.r != mergedcell.e.r && col == rowcol) {
					mergedCells.push(JSON.parse(JSON.stringify(mergedcell)));
				}
				mergedflg = false;
				mergedcell = { s: { r: 0, c: 0 }, e: { r: 0, c: 0 } };
			}
		}
	}

	//行合并
	for (let i = 0; i < headerDepth; i++) {
		let rowcol = headerArr[i][0];
		for (let j = 0; j < headerColumns; j++) {
			let col = headerArr[i][j];
			if (col == rowcol && mergedflg == false) {
				mergedflg = true;
				mergedcell.s = { r: i, c: j };
			} else if (col != rowcol && mergedflg == true) {
				mergedcell.e = { r: i, c: j - 1 };

				if (mergedcell.s.c != mergedcell.e.c) {
					mergedCells.push(JSON.parse(JSON.stringify(mergedcell)));
					mergedflg = false;
					mergedcell = { s: { r: 0, c: 0 }, e: { r: 0, c: 0 } };
				} else if (j == headerColumns - 1 && mergedflg == true) {
					mergedcell.e = { r: i, c: j };
					if (mergedcell.s.r != mergedcell.e.r && col == rowcol) {
						mergedCells.push(JSON.parse(JSON.stringify(mergedcell)));
					}
					mergedflg = false;
					mergedcell = { s: { r: 0, c: 0 }, e: { r: 0, c: 0 } };
				} else {
					rowcol = col;
					mergedflg = true;
					mergedcell.s = { r: i, c: j };
				}
				// rowcol=col;
				// mergedflg=false;
				// mergedcell={s:{r:0,c:0},e:{r:0,c:0}}
			} else if (j == headerColumns - 1 && mergedflg == true) {
				mergedcell.e = { r: i, c: j };
				if (mergedcell.s.c != mergedcell.e.c && col == rowcol) {
					mergedCells.push(JSON.parse(JSON.stringify(mergedcell)));
				}
				mergedflg = false;
				mergedcell = { s: { r: 0, c: 0 }, e: { r: 0, c: 0 } };
			}
		}
	}

	jsonarr.forEach((json) => {
		var row = new Array();
		headerArr[headerArr.length - 1].forEach((item) => {
			if (json.hasOwnProperty(item.prop)) {
				let val = '';
				if (json[item.prop] != null) {
					if (item.formatter) {
						var itemf = item.formatter(json);
						val = formatterRec(itemf); //递归获取formatter信息
					} else {
						val = json[item.prop];
					}
				}
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
	ws['!merges'] = mergedCells; // 设置合并单元格信息
	XLSXS.utils.book_append_sheet(wb, ws, sheetName);
	/* generate file and send to client */
	XLSXS.writeFile(wb, name + '.xlsx');
}
//递归formatter
function formatterRec(itemf: any) {
	let r = '';
	if (itemf.children) {
		if (itemf.children.default) {
			r = itemf.children.default();
		} else {
			itemf.children.forEach((element: any) => {
				r = r + formatterRec(element);
			});
		}
	} else {
		r = itemf;
	}
	return r;
}
//获取深度
function getMaxDepth(data: any) {
	let maxDepth = 1;

	function traverse(obj, depth) {
		if (obj.children && obj.children.length > 0) {
			depth++;
			if (depth > maxDepth) {
				maxDepth = depth;
			}
			obj.children.forEach((child) => traverse(child, depth));
		}
	}

	data.forEach((obj) => traverse(obj, 1));

	return maxDepth;
}
//获取总列数
function getTotalColumns(data: any) {
	let totalColumns = 0;

	function traverse(obj) {
		if (obj.children && obj.children.length > 0) {
			obj.children.forEach((child) => traverse(child));
		} else {
			totalColumns++;
		}
	}

	data.forEach((obj) => traverse(obj));

	return totalColumns;
}
