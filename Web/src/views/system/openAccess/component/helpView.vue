<template>
	<div class="sys-notice-container">
		<el-dialog v-model="state.isShowDialog" draggable :close-on-click-modal="false" width="900px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-QuestionFilled /> </el-icon>
					<span> 说明 </span>
				</div>
			</template>
			<!-- <template #footer>
				<span class="dialog-footer">
					<el-button @click="close">关 闭</el-button>
				</span>
			</template> -->
			<div class="text-content">
				<h2>OpenAPI 使用</h2>
				<ul>
					<li>
						在需要使用 Signature 身份验证的 Api 中贴上
						<p><el-tag>[Authorize(AuthenticationSchemes = SignatureAuthenticationDefaults.AuthenticationScheme)]</el-tag></p>
					</li>
					<li>
						如果 Api 需要保留 Jwt 方式的身份验证，可贴上
						<p><el-tag>[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + SignatureAuthenticationDefaults.AuthenticationScheme)]</el-tag></p>
					</li>
					<li>
						通过对请求的签名，可以达到以下目的：
						<ul>
							<li>免登录识别访问接口用户的身份</li>
							<li>防止潜在的重放攻击</li>
						</ul>
					</li>
				</ul>
				<el-divider />
				<h2>OpenAPI 签名流程</h2>
				客户端在请求时，需要按照如下步骤生成签名 Signature，并添加公共参数：
				<h3>公共请求参数</h3>
				<p>在原始请求的基础上添加 Header 请求参数</p>
				<ul>
					<li><el-tag effect="plain">accessKey</el-tag>：身份标识</li>
					<li><el-tag effect="plain">timestamp</el-tag>：时间戳，精确到秒</li>
					<li><el-tag effect="plain">nonce</el-tag>：唯一随机数，建议为一个6位的随机数</li>
					<li><el-tag effect="plain">sign</el-tag>：签名数据（见“计算签名”部分）</li>
				</ul>
				<h3>计算签名</h3>
				<ul>
					<li>
						按照如下顺序对请求中的参数进行排序，各个参数通过&进行拼接（中间不含空格）：
						<p><el-tag>method & url & accessKey & timestamp & nonce</el-tag></p>
						<ul>
							<li><el-tag effect="plain">method</el-tag> 需要大写，如：GET</li>
							<li><el-tag effect="plain">url</el-tag> 去除协议、域名、参数，以 / 开头，如：/api/demo/helloWord</li>
						</ul>
					</li>
					<li>使用 HMAC-SHA256 协议创建基于哈希的消息身份验证代码 (HMAC)，以 <el-tag effect="plain">appSecret</el-tag> 作为密钥，对上面拼接的参数进行计算签名，所得签名进行 Base-64 编码</li>
				</ul>
			</div>
			<div class="el-alert el-alert--info is-light">
				HMAC-SHA256 在线计算：
				<el-link href="https://1024tools.com/hmac" target="_blank" type="primary">https://1024tools.com/hmac</el-link>
			</div>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup name="sysOpenAccessHelpView">
import { reactive } from 'vue';

const state = reactive({
	isShowDialog: false,
});

// 打开弹窗
const openDialog = () => {
	state.isShowDialog = true;
};

// // 关闭
// const close = () => {
// 	state.isShowDialog = false;
// };

// 导出对象
defineExpose({ openDialog });
</script>
<style scoped lang="scss">
.text-content {
	h1 {
		margin: 8px 0;
	}
	h2 {
		margin: 8px 0;
	}
	h3 {
		margin: 8px 0;
	}
	p {
		margin: 8px 0;
	}
	ul {
		padding: 0 0 0 30px;
		li {
			margin: 8px 0;
		}
	}
}
</style>
