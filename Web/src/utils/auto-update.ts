/**
 *通过监听当前页面的JS的SRC来判断当前网页是否有更新
 *使用方法   main.js   import checkUpdate from "/@/utils/auto-update";
 */

let lastSrcs: any[] | null; //上次js地址集合
// const scriptReg = /(?<=<script.*src=["']).*?(?=["'])/gm; //IOS 不支持断言匹配
const scriptReg = /<script.*?src=['"](.*?)['"]/gm;

/**
 * 获取最新的js集合
 * @returns
 */
async function extractNewScripts() {
	const html = await fetch('/?_t=' + Date.now()).then((res) => res.text());
	scriptReg.lastIndex = 0;
	const result = html.match(scriptReg);
	return result;
}
/**
 * 判断是否有更新
 * @returns
 */
async function checkUpdate() {
	const newScripts = await extractNewScripts();
	if (!lastSrcs) {
		lastSrcs = newScripts;
		return false;
	}
	if (newScripts == null) return false;
	let result = false;
	if (lastSrcs.length !== newScripts.length) {
		result = true;
	}
	for (let i = 0; i < lastSrcs.length; i++) {
		if (lastSrcs[i] !== newScripts[i]) {
			result = true;
			break;
		}
	}
	lastSrcs = newScripts;
	return result;
}

/**
 * 定时器定时检测是否更新，有更新则执行回调函数
 * @param callbackFn
 * @param interval
 */
function checkUpdateInterval(callbackFn: any, interval: number = 60000) {
	setTimeout(async () => {
		const willUpdate = await checkUpdate();
		if (willUpdate) {
			callbackFn();
		}
		checkUpdateInterval(callbackFn, interval);
	}, interval);
}

export default checkUpdateInterval;
